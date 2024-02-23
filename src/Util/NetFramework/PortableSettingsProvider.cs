using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace VectronsLibrary.NetFramework;

/// <summary>
/// A settings provider that stores the settings in the executable directory.
/// </summary>
public class PortableSettingsProvider : SettingsProvider, IApplicationSettingsProvider
{
    private const string ClassName = "PortableSettingsProvider";
    private const string SettingsFolder = "Settings";
    private const string SETTINGSROOT = "Settings";
    private SettingsContext? context;
    private ILogger logger;
    private XmlDocument? settingsXML;

    /// <summary>
    /// Initializes a new instance of the <see cref="PortableSettingsProvider"/> class.
    /// </summary>
    public PortableSettingsProvider()
        : this(NullLogger<PortableSettingsProvider>.Instance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PortableSettingsProvider"/> class.
    /// </summary>
    /// <param name="logger">An <see cref="ILogger"/> instance used for logging.</param>
    public PortableSettingsProvider(ILogger<PortableSettingsProvider> logger)
        => this.logger = logger;

    /// <inheritdoc/>
    public override string ApplicationName
    {
        get
        {
            var assembly = Assembly.GetEntryAssembly();

            if (assembly != null)
            {
                var customAttributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);

                if ((customAttributes != null) && (customAttributes.Length > 0))
                {
                    return ((AssemblyProductAttribute)customAttributes[0]).Product;
                }

                var fi = new FileInfo(assembly.CodeBase);
                return fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
            }

            return string.Empty;
        }

        set
        {
        }
    }

    /// <inheritdoc/>
    public override string Name => ClassName;

    private XmlDocument SettingsXML
    {
        get
        {
            // If we don't hold an xml document, try opening one.
            // If it doesn't exist then create a new one ready.
            if (settingsXML == null)
            {
                settingsXML = new XmlDocument();

                try
                {
                    var file = Path.Combine(GetAppSettingsPath(), GetAppSettingsFilename());

                    if (!File.Exists(file))
                    {
                        // Create new document
                        var dec = settingsXML.CreateXmlDeclaration("1.0", "utf-8", string.Empty);
                        _ = settingsXML.AppendChild(dec);

                        var nodeRoot = default(XmlNode);

                        nodeRoot = settingsXML.CreateNode(XmlNodeType.Element, SETTINGSROOT, string.Empty);
                        _ = settingsXML.AppendChild(nodeRoot);
                    }
                    else
                    {
                        var fileStream = File.OpenRead(file);
                        var reader = XmlReader.Create(fileStream, new XmlReaderSettings() { XmlResolver = null });
                        settingsXML.Load(reader);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogCritical(ex, "Failed to create settings file");
                }
            }

            return settingsXML;
        }
    }

    /// <summary>
    /// Create a unique name from the <see cref="Environment.MachineName"/>.
    /// </summary>
    /// <returns>Returns a valid XML machine name.</returns>
    public static string AlteredMachineName()
    {
        var machineName = "M" + Environment.MachineName;

        if (IsValidXmlString(machineName))
        {
            machineName = RemoveInvalidXmlChars(machineName);
        }

        return machineName;
    }

    /// <summary>
    /// Used to determine the filename to store the settings.
    /// </summary>
    /// <returns>File name.</returns>
    public virtual string GetAppSettingsFilename()
        => context == null
            ? "default.settings"
            : context["GroupName"].ToString().Substring(0, context["GroupName"].ToString().IndexOf(".", StringComparison.OrdinalIgnoreCase)) + ".settings";

    /// <summary>
    /// Get the storage path for the settings file.
    /// </summary>
    /// <returns>Storage path for the settings.</returns>
    public virtual string GetAppSettingsPath()
    {
        var codebaseLocation = Assembly.GetEntryAssembly().CodeBase;
        var location = codebaseLocation.Contains("://")
            ? new Uri(codebaseLocation).LocalPath
            : codebaseLocation;

        // Used to determine where to store the settings
        var fi = new FileInfo(location);
        var productName = ApplicationName;

        var settingsDir = Path.Combine(fi.DirectoryName, SettingsFolder);
        if (!Directory.Exists(settingsDir))
        {
            _ = Directory.CreateDirectory(settingsDir);
            var oldFile = Path.Combine(fi.DirectoryName, productName + ".settings");
            if (File.Exists(oldFile))
            {
                File.Move(oldFile, settingsDir + "\\" + productName + ".settings");
            }
        }

        return settingsDir;
    }

    /// <inheritdoc/>
    public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
    {
        this.context = context;

        // do nothing
        return new SettingsPropertyValue(property);
    }

    /// <inheritdoc/>
    public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
    {
        this.context = context;

        // Create new collection of values
        var values = new SettingsPropertyValueCollection();

        // Iterate through the settings to be retrieved
        foreach (SettingsProperty setting in collection)
        {
            var value = new SettingsPropertyValue(setting)
            {
                IsDirty = false,
                SerializedValue = GetValue(setting),
            };
            values.Add(value);
        }

        return values;
    }

    /// <inheritdoc/>
    public override void Initialize(string name, NameValueCollection config)
        => base.Initialize(ApplicationName, config);

    /// <inheritdoc/>
    public void Reset(SettingsContext context)
    {
        this.context = context;
        ((XmlElement)SettingsXML.SelectSingleNode(SETTINGSROOT)).RemoveAll();

        SettingsXML.Save(Path.Combine(GetAppSettingsPath(), GetAppSettingsFilename()));
    }

    /// <summary>
    /// Set the <see cref="ILogger"/> instance to use.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> instance.</param>
    public void SetLogger(ILogger logger)
        => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc/>
    public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
    {
        this.context = context;

        // Iterate through the settings to be stored
        // Only dirty settings are included in property values, and only ones relevant to this provider
        foreach (SettingsPropertyValue propertyValue in collection)
        {
            SetValue(propertyValue);
        }

        try
        {
            SettingsXML.Save(Path.Combine(GetAppSettingsPath(), GetAppSettingsFilename()));
        }
        catch (Exception ex)
        {
            // Ignore if cant save, device been ejected
            logger.LogCritical(ex, "Failed to write property");
        }
    }

    /// <inheritdoc/>
    public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
        => this.context = context;

    private static bool IsRoaming(SettingsProperty prop)
    {
        // Determine if the setting is marked as Roaming
        foreach (DictionaryEntry d in prop.Attributes)
        {
            var a = (Attribute)d.Value;
            if (a is SettingsManageabilityAttribute)
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsValidXmlString(string text)
    {
        try
        {
            _ = XmlConvert.VerifyXmlChars(text);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string RemoveInvalidXmlChars(string text)
    {
        var validXmlChars = text.Where(XmlConvert.IsXmlChar).ToArray();
        return new string(validXmlChars);
    }

    private string GetValue(SettingsProperty setting)
    {
        string ret;
        try
        {
            var node = string.Empty;
            node = IsRoaming(setting)
                ? SETTINGSROOT + "/" + setting.Name
                : SETTINGSROOT + "/" + AlteredMachineName() + "/" + setting.Name;

            ret = SettingsXML.SelectSingleNode(node) != null
                ? SettingsXML.SelectSingleNode(node).InnerText
                : setting.DefaultValue != null
                    ? setting.DefaultValue.ToString()
                    : string.Empty;
        }
        catch (Exception ex)
        {
            ret = setting.DefaultValue != null
                ? setting.DefaultValue.ToString()
                : string.Empty;
            logger.LogError(ex, "Failed to get value");
        }

        return ret;
    }

    private void SetValue(SettingsPropertyValue propVal)
    {
        XmlElement? settingNode;

        // Determine if the setting is roaming.
        // If roaming then the value is stored as an element under the root
        // Otherwise it is stored under a machine name node
        try
        {
            settingNode = IsRoaming(propVal.Property)
                ? (XmlElement)SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + propVal.Name)
                : (XmlElement)SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + AlteredMachineName() + "/" + propVal.Name);
        }
        catch (Exception ex)
        {
            settingNode = null;
            logger.LogError(ex, "Failed to set value");
        }

        // Check to see if the node exists, if so then set its new value
        if (settingNode != null)
        {
            settingNode.InnerText = propVal.SerializedValue.ToString();
        }
        else
        {
            if (IsRoaming(propVal.Property))
            {
                // Store the value as an element of the Settings Root Node
                settingNode = SettingsXML.CreateElement(propVal.Name);
                settingNode.InnerText = propVal.SerializedValue.ToString();
                _ = SettingsXML.SelectSingleNode(SETTINGSROOT).AppendChild(settingNode);
            }
            else
            {
                XmlElement machineNode;

                // Its machine specific, store as an element of the machine name node,
                // creating a new machine name node if one doesn't exist.
                try
                {
                    machineNode = (XmlElement)SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + AlteredMachineName());
                }
                catch (Exception ex)
                {
                    machineNode = SettingsXML.CreateElement(AlteredMachineName());
                    _ = SettingsXML.SelectSingleNode(SETTINGSROOT).AppendChild(machineNode);
                    logger.LogError(ex, "Failed to set value");
                }

                if (machineNode == null)
                {
                    machineNode = SettingsXML.CreateElement(AlteredMachineName());
                    _ = SettingsXML.SelectSingleNode(SETTINGSROOT).AppendChild(machineNode);
                }

                settingNode = SettingsXML.CreateElement(propVal.Name);
                settingNode.InnerText = propVal.SerializedValue.ToString();
                _ = machineNode.AppendChild(settingNode);
            }
        }
    }
}

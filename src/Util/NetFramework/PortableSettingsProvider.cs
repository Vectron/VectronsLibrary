using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace VectronsLibrary.NetFramework
{
    public class PortableSettingsProvider : SettingsProvider, IApplicationSettingsProvider
    {
        private const string ClassName = "PortableSettingsProvider";
        private const string SettingsFolder = "Settings";
        private const string SETTINGSROOT = "Settings";

        // XML Root Node
        private SettingsContext context;

        private ILogger logger;
        private XmlDocument settingsXML = null;

        public PortableSettingsProvider()
            : this(NullLogger<PortableSettingsProvider>.Instance)
        {
        }

        public PortableSettingsProvider(ILogger<PortableSettingsProvider> logger)
        {
            this.logger = logger;
        }

        public override string ApplicationName
        {
            get
            {
                var assembly = Assembly.GetEntryAssembly();

                if (assembly != null)
                {
                    object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);

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

        public override string Name => ClassName;

        private XmlDocument SettingsXML
        {
            get
            {
                // If we dont hold an xml document, try opening one.
                // If it doesnt exist then create a new one ready.
                if (settingsXML == null)
                {
                    settingsXML = new XmlDocument();

                    try
                    {
                        string file = Path.Combine(GetAppSettingsPath(), GetAppSettingsFilename());

                        if (!File.Exists(file))
                        {
                            // Create new document
                            XmlDeclaration dec = settingsXML.CreateXmlDeclaration("1.0", "utf-8", string.Empty);
                            _ = settingsXML.AppendChild(dec);

                            var nodeRoot = default(XmlNode);

                            nodeRoot = settingsXML.CreateNode(XmlNodeType.Element, SETTINGSROOT, string.Empty);
                            _ = settingsXML.AppendChild(nodeRoot);
                        }
                        else
                        {
                            settingsXML.Load(file);
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

        public string AlteredMachineName()
        {
            string machinename = "M" + Environment.MachineName;

            if (IsValidXmlString(machinename))
            {
                machinename = RemoveInvalidXmlChars(machinename);
            }

            return machinename;
        }

        public virtual string GetAppSettingsFilename() =>
            // Used to determine the filename to store the settings
            context["GroupName"].ToString().Substring(0, context["GroupName"].ToString().IndexOf(".")) + ".settings";

        public virtual string GetAppSettingsPath()
        {
            // Used to determine where to store the settings
            var fi = new FileInfo(Assembly.GetEntryAssembly().CodeBase);
            var productName = ApplicationName;

            string settingsDir = Path.Combine(fi.DirectoryName, SettingsFolder);
            if (!Directory.Exists(settingsDir))
            {
                _ = Directory.CreateDirectory(settingsDir);
                string oldFile = Path.Combine(fi.DirectoryName, productName + ".settings");
                if (File.Exists(oldFile))
                {
                    File.Move(oldFile, settingsDir + "\\" + productName + ".settings");
                }
            }

            return settingsDir;
        }

        public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
        {
            this.context = context;

            // do nothing
            return new SettingsPropertyValue(property);
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection props)
        {
            this.context = context;

            // Create new collection of values
            var values = new SettingsPropertyValueCollection();

            // Iterate through the settings to be retrieved
            foreach (SettingsProperty setting in props)
            {
                var value = new SettingsPropertyValue(setting)
                {
                    IsDirty = false,
                    SerializedValue = GetValue(setting)
                };
                values.Add(value);
            }

            return values;
        }

        public override void Initialize(string name, NameValueCollection col)
            => base.Initialize(ApplicationName, col);

        public void Reset(SettingsContext context)
        {
            this.context = context;
            ((XmlElement)SettingsXML.SelectSingleNode(SETTINGSROOT)).RemoveAll();

            SettingsXML.Save(Path.Combine(GetAppSettingsPath(), GetAppSettingsFilename()));
        }

        public void SetLogger(ILogger logger)
            => this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection propvals)
        {
            this.context = context;

            // Iterate through the settings to be stored
            // Only dirty settings are included in propvals, and only ones relevant to this provider
            foreach (SettingsPropertyValue propval in propvals)
            {
                SetValue(propval);
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

        public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
            => this.context = context;

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
            var validXmlChars = text.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray();
            return new string(validXmlChars);
        }

        private string GetValue(SettingsProperty setting)
        {
            string ret;
            try
            {
                string node = string.Empty;
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

        private bool IsRoaming(SettingsProperty prop)
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

        private void SetValue(SettingsPropertyValue propVal)
        {
            XmlElement settingNode;

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
                    // creating a new machine name node if one doesnt exist.
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
}
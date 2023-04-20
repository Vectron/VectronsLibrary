using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace VectronsLibrary.WindowsForms;

/// <summary>
/// Helpers for switching localization.
/// </summary>
public static class RuntimeLocalizer
{
    /// <summary>
    /// Change the current culture of a <see cref="Control"/>.
    /// </summary>
    /// <param name="frm">The control to change the culture off.</param>
    /// <param name="cultureCode">The culture code to change to.</param>
    public static void ChangeCulture(Control frm, string cultureCode)
    {
        var culture = CultureInfo.GetCultureInfo(cultureCode);

        Thread.CurrentThread.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;

        var resources = new ComponentResourceManager(frm.GetType());

        ApplyResourceToControl(resources, frm, culture);
        resources.ApplyResources(frm, "$this", culture);
    }

    private static void ApplyResourceToControl(ComponentResourceManager res, Control control, CultureInfo cultureInfo)
    {
        // See if this is a menuStrip
        if (control.GetType() == typeof(MenuStrip))
        {
            var strip = (MenuStrip)control;

            ApplyResourceToToolStripItemCollection(strip.Items, res, cultureInfo);
        }

        // Apply to all sub-controls
        foreach (Control? c in control.Controls)
        {
            if (c == null)
            {
                continue;
            }

            ApplyResourceToControl(res, c, cultureInfo);

            var text = res.GetString(c.Name + ".Text", cultureInfo);
            if (text != null)
            {
                c.Text = text;
            }
        }

        // Apply to self
        var text2 = res.GetString(control.Name + ".Text", cultureInfo);
        if (text2 != null)
        {
            control.Text = text2;
        }
    }

    private static void ApplyResourceToToolStripItemCollection(ToolStripItemCollection col, ComponentResourceManager res, CultureInfo cultureInfo)
    {
        // Apply to all sub items
        for (var i = 0; i < col.Count; i++)
        {
            ToolStripItem item = (ToolStripMenuItem)col[i];

            if (item.GetType() == typeof(ToolStripMenuItem))
            {
                var menuItem = (ToolStripMenuItem)item;
                ApplyResourceToToolStripItemCollection(menuItem.DropDownItems, res, cultureInfo);
            }

            res.ApplyResources(item, item.Name, cultureInfo);
        }
    }
}
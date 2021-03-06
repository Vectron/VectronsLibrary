﻿using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace VectronsLibrary.Winform
{
    public static class RuntimeLocalizer
    {
        public static void ChangeCulture(Control frm, string cultureCode)
        {
            var culture = CultureInfo.GetCultureInfo(cultureCode);

            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            var resources = new ComponentResourceManager(frm.GetType());

            ApplyResourceToControl(resources, frm, culture);
            resources.ApplyResources(frm, "$this", culture);
        }

        private static void ApplyResourceToControl(ComponentResourceManager res, Control control, CultureInfo lang)
        {
            // See if this is a menuStrip
            if (control.GetType() == typeof(MenuStrip))
            {
                var strip = (MenuStrip)control;

                ApplyResourceToToolStripItemCollection(strip.Items, res, lang);
            }

            // Apply to all sub-controls
            foreach (Control c in control.Controls)
            {
                ApplyResourceToControl(res, c, lang);

                // res.ApplyResources(c, c.Name, lang);
                string text = res.GetString(c.Name + ".Text", lang);
                if (text != null)
                {
                    c.Text = text;
                }
            }

            // Apply to self
            // res.ApplyResources(control, control.Name, lang);
            string text2 = res.GetString(control.Name + ".Text", lang);
            if (text2 != null)
            {
                control.Text = text2;
            }
        }

        private static void ApplyResourceToToolStripItemCollection(ToolStripItemCollection col, ComponentResourceManager res, CultureInfo lang)
        {
            // Apply to all sub items
            for (int i = 0; i < col.Count; i++)
            {
                ToolStripItem item = (ToolStripMenuItem)col[i];

                if (item.GetType() == typeof(ToolStripMenuItem))
                {
                    var menuitem = (ToolStripMenuItem)item;
                    ApplyResourceToToolStripItemCollection(menuitem.DropDownItems, res, lang);
                }

                res.ApplyResources(item, item.Name, lang);
            }
        }
    }
}
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace VectronsLibrary.WindowsForms;

/// <summary>
/// Helper class to store form settings.
/// </summary>
public static class FormGeometry
{
    /// <summary>
    /// Sets the <see cref="Form"/> settings from a stored string.
    /// </summary>
    /// <param name="thisWindowGeometry">Containing the stored geometry.</param>
    /// <param name="formIn">The form to apply the geometry to.</param>
    public static void GeometryFromString(string thisWindowGeometry, Form formIn)
    {
        if (string.IsNullOrEmpty(thisWindowGeometry))
        {
            return;
        }

        var numbers = thisWindowGeometry.Split('|');
        var windowString = numbers[4];
        if (string.Equals(windowString, "Normal", System.StringComparison.Ordinal))
        {
            var windowPoint = new Point(int.Parse(numbers[0], CultureInfo.InvariantCulture), int.Parse(numbers[1], CultureInfo.InvariantCulture));
            var windowSize = new Size(int.Parse(numbers[2], CultureInfo.InvariantCulture), int.Parse(numbers[3], CultureInfo.InvariantCulture));

            var locOkay = GeometryIsBizarreLocation(windowPoint, windowSize);
            var sizeOkay = GeometryIsBizarreSize(windowSize);

            if (locOkay && sizeOkay)
            {
                formIn.Location = windowPoint;
                formIn.Size = windowSize;
                formIn.StartPosition = FormStartPosition.Manual;
                formIn.WindowState = FormWindowState.Normal;
            }
            else if (sizeOkay)
            {
                formIn.Size = windowSize;
            }
        }
        else if (string.Equals(windowString, "Maximized", System.StringComparison.Ordinal))
        {
            formIn.Location = new Point(100, 100);
            formIn.StartPosition = FormStartPosition.Manual;
            formIn.WindowState = FormWindowState.Maximized;
        }
    }

    /// <summary>
    /// Create a string from the form geometry.
    /// </summary>
    /// <param name="mainForm">The <see cref="Form"/> to store the geometry from.</param>
    /// <returns>A string containing all the settings for the form.</returns>
    public static string GeometryToString(Form mainForm)
        => mainForm.Location.X.ToString(CultureInfo.InvariantCulture) + "|" +
            mainForm.Location.Y.ToString(CultureInfo.InvariantCulture) + "|" +
            mainForm.Size.Width.ToString(CultureInfo.InvariantCulture) + "|" +
            mainForm.Size.Height.ToString(CultureInfo.InvariantCulture) + "|" +
            mainForm.WindowState.ToString();

    private static bool GeometryIsBizarreLocation(Point loc, Size size)
    {
        var desktop_X = 0;
        var desktop_Y = 0;
        var desktop_width = Screen.PrimaryScreen?.WorkingArea.Width ?? 0;
        var desktop_height = Screen.PrimaryScreen?.WorkingArea.Height ?? 0;

        foreach (var screen in Screen.AllScreens)
        {
            if (screen.WorkingArea.X < desktop_X)
            {
                desktop_X = screen.WorkingArea.X;
            }

            if (screen.WorkingArea.Y < desktop_Y)
            {
                desktop_Y = screen.WorkingArea.Y;
            }

            if (!screen.Primary)
            {
                if (screen.WorkingArea.X >= desktop_width)
                {
                    desktop_width += screen.WorkingArea.Width;
                }

                if (screen.WorkingArea.Y >= desktop_height)
                {
                    desktop_height += screen.WorkingArea.Height;
                }
            }
        }

        return loc.X >= desktop_X && loc.Y >= desktop_Y && loc.X + size.Width <= desktop_width && loc.Y + size.Height <= desktop_height;
    }

    private static bool GeometryIsBizarreSize(Size size)
        => size.Height <= Screen.PrimaryScreen?.WorkingArea.Height && size.Width <= Screen.PrimaryScreen?.WorkingArea.Width;
}

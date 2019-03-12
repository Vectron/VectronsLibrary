using System.Drawing;
using System.Windows.Forms;

namespace VectronsLibrary.Winform
{
    public static class FormGeometry
    {
        public static void GeometryFromString(string thisWindowGeometry, Form formIn)
        {
            if (string.IsNullOrEmpty(thisWindowGeometry) == true)
            {
                return;
            }

            string[] numbers = thisWindowGeometry.Split('|');
            string windowString = numbers[4];
            if (windowString == "Normal")
            {
                var windowPoint = new Point(int.Parse(numbers[0]), int.Parse(numbers[1]));
                var windowSize = new Size(int.Parse(numbers[2]), int.Parse(numbers[3]));

                bool locOkay = GeometryIsBizarreLocation(windowPoint, windowSize);
                bool sizeOkay = GeometryIsBizarreSize(windowSize);

                if (locOkay == true && sizeOkay == true)
                {
                    formIn.Location = windowPoint;
                    formIn.Size = windowSize;
                    formIn.StartPosition = FormStartPosition.Manual;
                    formIn.WindowState = FormWindowState.Normal;
                }
                else if (sizeOkay == true)
                {
                    formIn.Size = windowSize;
                }
            }
            else if (windowString == "Maximized")
            {
                formIn.Location = new Point(100, 100);
                formIn.StartPosition = FormStartPosition.Manual;
                formIn.WindowState = FormWindowState.Maximized;
            }
        }

        public static string GeometryToString(Form mainForm)
        {
            return mainForm.Location.X.ToString() + "|" +
                mainForm.Location.Y.ToString() + "|" +
                mainForm.Size.Width.ToString() + "|" +
                mainForm.Size.Height.ToString() + "|" +
                mainForm.WindowState.ToString();
        }

        private static bool GeometryIsBizarreLocation(Point loc, Size size)
        {
            bool locOkay;
            int desktop_X = 0;
            int desktop_Y = 0;
            int desktop_width = 0;
            int desktop_height = 0;

            desktop_width = Screen.PrimaryScreen.WorkingArea.Width;
            desktop_height = Screen.PrimaryScreen.WorkingArea.Height;

            foreach (Screen scherm in Screen.AllScreens)
            {
                if (scherm.WorkingArea.X < desktop_X)
                {
                    desktop_X = scherm.WorkingArea.X;
                }

                if (scherm.WorkingArea.Y < desktop_Y)
                {
                    desktop_Y = scherm.WorkingArea.Y;
                }

                if (scherm.Primary == false)
                {
                    if (scherm.WorkingArea.X >= desktop_width)
                    {
                        desktop_width += scherm.WorkingArea.Width;
                    }

                    if (scherm.WorkingArea.Y >= desktop_height)
                    {
                        desktop_height += scherm.WorkingArea.Height;
                    }
                }
            }

            if (loc.X < desktop_X || loc.Y < desktop_Y)
            {
                locOkay = false;
            }
            else if (loc.X + size.Width > desktop_width)
            {
                locOkay = false;
            }
            else if (loc.Y + size.Height > desktop_height)
            {
                locOkay = false;
            }
            else
            {
                locOkay = true;
            }

            return locOkay;
        }

        private static bool GeometryIsBizarreSize(Size size)
        {
            return size.Height <= Screen.PrimaryScreen.WorkingArea.Height && size.Width <= Screen.PrimaryScreen.WorkingArea.Width;
        }
    }
}
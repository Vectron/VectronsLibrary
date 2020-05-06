using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VectronsLibrary.Winform
{
    public enum Line3DStyle
    {
        Flat,
        Inset,
        Outset
    }

    public enum LineOrientation
    {
        Horizontal,
        Vertical,
        DiagonalUp,
        DiagonalDown
    }

    /// <seealso cref="http://beta.unclassified.de/code/dotnet/line/"/>
    //   [Designer(typeof(LineDesigner))]
    public class Line : Control
    {
        private Color borderColor = SystemColors.ControlText;

        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private DashStyle dashStyle = DashStyle.Solid;

        private bool internalResizing = false;

        private Line3DStyle line3DStyle = Line3DStyle.Flat;

        private LineOrientation orientation = LineOrientation.Horizontal;

        private Pen pen1, pen2;

        private int prevWidth, prevHeight;

        public Line()
        {
            InitializeComponent();

            TabStop = false;

            pen1 = new Pen(borderColor, 2);
            pen2 = new Pen(borderColor, 2);
            prevWidth = Width;
            prevHeight = Height;
            //  Orientation = LineOrientation.Horizontal;

            //    this.SizeChanged += new EventHandler(Line_SizeChanged);
        }

        [Description("Border color for solid border style"), Category("Appearance")]
        public Color BorderColor
        {
            get => borderColor;

            set
            {
                borderColor = value;
                pen1.Color = borderColor;
                pen1.DashStyle = dashStyle;
                Invalidate();
            }
        }

        [Description("Border color for solid border style"), Category("Appearance")]
        public DashStyle DashStyle
        {
            get => pen1.DashStyle;

            set
            {
                dashStyle = value;
                pen1.DashStyle = value;
                pen2.DashStyle = value;
                Invalidate();
            }
        }

        [Browsable(false)]
        public override Font Font
        {
            get => base.Font;
            set => base.Font = value;
        }

        [Browsable(false)]
        public override Color ForeColor
        {
            get => base.ForeColor;
            set => base.ForeColor = value;
        }

        [DefaultValue(Line3DStyle.Flat)]
        [Description("Border style"), Category("Appearance")]
        public Line3DStyle Line3DStyle
        {
            get => line3DStyle;

            set
            {
                line3DStyle = value;
                if (line3DStyle == Line3DStyle.Flat)
                {
                    pen1 = new Pen(borderColor, 1)
                    {
                        DashStyle = dashStyle
                    };
                }
                else if (line3DStyle == Line3DStyle.Inset)
                {
                    pen1 = new Pen(SystemColors.ControlLightLight, 1)
                    {
                        DashStyle = dashStyle
                    };
                    pen2 = new Pen(SystemColors.ControlDark, 1)
                    {
                        DashStyle = dashStyle
                    };
                }
                else if (line3DStyle == Line3DStyle.Outset)
                {
                    pen1 = new Pen(SystemColors.ControlDark, 1)
                    {
                        DashStyle = dashStyle
                    };
                    pen2 = new Pen(SystemColors.ControlLightLight, 1)
                    {
                        DashStyle = dashStyle
                    };
                }
                UpdateSize();
            }
        }

        public float LineWith
        {
            get => pen1.Width;

            set
            {
                pen1.Width = value;
                pen2.Width = value;
                UpdateSize();
            }
        }

        [DefaultValue(LineOrientation.Horizontal)]
        [Description("Line orientation"), Category("Appearance")]
        public LineOrientation Orientation
        {
            get => orientation;

            set
            {
                orientation = value;
                prevHeight = Height;
                prevWidth = Width;
                UpdateSize();
            }
        }

        [Browsable(false)]
        public new int TabIndex
        {
            get => base.TabIndex;
            set => base.TabIndex = value;
        }

        [Browsable(false)]
        public new bool TabStop
        {
            get => base.TabStop;
            set => base.TabStop = value;
        }

        [Browsable(false)]
        public override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20 /* WS_EX_TRANSPARENT */;
                return cp;
            }
        }

        protected override Size DefaultSize => new Size(100, 2);

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelÃ¶scht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (pen1 != null)
                {
                    this.pen1.Dispose();
                }
                if (pen2 != null)
                {
                    this.pen2.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (line3DStyle == Line3DStyle.Flat)
            {
                if (orientation == LineOrientation.Horizontal)
                {
                    pe.Graphics.DrawLine(pen1, 0, 0, Width - 1, 0);
                }
                else if (orientation == LineOrientation.Vertical)
                {
                    pe.Graphics.DrawLine(pen1, 0, 0, 0, Height - 1);
                }
                else if (orientation == LineOrientation.DiagonalUp)
                {
                    pe.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                    pe.Graphics.DrawLine(pen1, 0, Height - 1, Width - 1, 0);
                }
                else if (orientation == LineOrientation.DiagonalDown)
                {
                    pe.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                    pe.Graphics.DrawLine(pen1, 0, 0, Width - 1, Height - 1);
                }
            }
            else   // 3D - colours are set in the Line3DStyle_Set property
            {
                if (orientation == LineOrientation.Horizontal)
                {
                    pe.Graphics.DrawLine(pen2, 0, 0, Width - 1, 0);
                    pe.Graphics.DrawLine(pen1, 0, 1, Width - 1, 1);
                }
                else if (orientation == LineOrientation.Vertical)
                {
                    pe.Graphics.DrawLine(pen2, 0, 0, 0, Height - 1);
                    pe.Graphics.DrawLine(pen1, 1, 0, 1, Height - 1);
                }
                else if (orientation == LineOrientation.DiagonalUp)
                {
                    pe.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                    pe.Graphics.DrawLine(pen2, 0, Height - 2, Width - 2, 0);
                    pe.Graphics.DrawLine(pen1, 1, Height - 1, Width - 1, 1);
                }
                else if (orientation == LineOrientation.DiagonalDown)
                {
                    pe.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                    pe.Graphics.DrawLine(pen2, 1, 0, Width - 1, Height - 2);
                    pe.Graphics.DrawLine(pen1, 0, 1, Width - 2, Height - 1);
                }
            }

            // OnPaint-Basisklasse wird aufgerufen
            base.OnPaint(pe);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, height, specified);
        }

        /// <summary>
        /// Erforderliche Methode fÃ¼r die DesignerunterstÃ¼tzung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geÃ¤ndert werden.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        private void Line_SizeChanged(object sender, EventArgs e)
        {
            if (internalResizing)
                return;

            // Only remember the currently alterable dimensions
            if (orientation == LineOrientation.Horizontal)
            {
                prevWidth = Width;
            }
            else if (orientation == LineOrientation.Vertical)
            {
                prevHeight = Height;
            }
            else
            {
                prevWidth = Width;
                prevHeight = Height;
            }
        }

        private void UpdateSize()
        {
            internalResizing = true;
            int lineWidth = (line3DStyle != Line3DStyle.Flat) ? 2 : (int)pen1.Width;
            if (orientation == LineOrientation.Horizontal)
            {
                Width = prevWidth;
                Height = lineWidth;
                MaximumSize = new Size(0, lineWidth);
                MinimumSize = MaximumSize;
            }
            else if (orientation == LineOrientation.Vertical)
            {
                Width = lineWidth;
                Height = prevHeight;
                MaximumSize = new Size(lineWidth, 0);
                MinimumSize = MaximumSize;
            }
            else
            {
                Width = prevWidth;
                Height = prevHeight;
                MaximumSize = new Size();
                MinimumSize = MaximumSize;
            }
            internalResizing = false;
            Invalidate();
        }
    }
}
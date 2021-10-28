using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VectronsLibrary.Winform;

/// <summary>
/// A line control.
/// http://beta.unclassified.de/code/dotnet/line/.
/// </summary>
public class Line : Control
{
    private readonly IContainer components;
    private Color borderColor = SystemColors.ControlText;
    private DashStyle dashStyle = DashStyle.Solid;
    private bool internalResizing;
    private Line3DStyle line3DStyle = Line3DStyle.Flat;
    private LineOrientation orientation = LineOrientation.Horizontal;
    private Pen pen1;
    private Pen pen2;
    private int prevHeight;
    private int prevWidth;

    /// <summary>
    /// Initializes a new instance of the <see cref="Line"/> class.
    /// </summary>
    public Line()
    {
        components = new Container();
        TabStop = false;
        pen1 = new Pen(borderColor, 2);
        pen2 = new Pen(borderColor, 2);
        prevWidth = Width;
        prevHeight = Height;
    }

    /// <summary>
    /// Gets or sets the color used when drawing the border.
    /// </summary>
    [Description("Border color for solid border style")]
    [Category("Appearance")]
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

    /// <summary>
    /// Gets or sets the style used for dashed lines.
    /// </summary>
    [Description("The line dash style")]
    [Category("Appearance")]
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

    /// <inheritdoc/>
    [Browsable(false)]
    public override Font Font
    {
        get => base.Font;
        set => base.Font = value;
    }

    /// <inheritdoc/>
    [Browsable(false)]
    public override Color ForeColor
    {
        get => base.ForeColor;
        set => base.ForeColor = value;
    }

    /// <summary>
    /// Gets or sets the <see cref="Line3DStyle"/> for this <see cref="Line"/>.
    /// </summary>
    [DefaultValue(Line3DStyle.Flat)]
    [Description("Border style")]
    [Category("Appearance")]
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
                    DashStyle = dashStyle,
                };
            }
            else if (line3DStyle == Line3DStyle.Inset)
            {
                pen1 = new Pen(SystemColors.ControlLightLight, 1)
                {
                    DashStyle = dashStyle,
                };
                pen2 = new Pen(SystemColors.ControlDark, 1)
                {
                    DashStyle = dashStyle,
                };
            }
            else if (line3DStyle == Line3DStyle.Outset)
            {
                pen1 = new Pen(SystemColors.ControlDark, 1)
                {
                    DashStyle = dashStyle,
                };
                pen2 = new Pen(SystemColors.ControlLightLight, 1)
                {
                    DashStyle = dashStyle,
                };
            }

            UpdateSize();
        }
    }

    /// <summary>
    /// Gets or sets the thickness of the line.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the <see cref="LineOrientation"/>.
    /// </summary>
    [DefaultValue(LineOrientation.Horizontal)]
    [Description("Line orientation")]
    [Category("Appearance")]
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

    /// <summary>
    ///  Gets or sets the tab index of this control.
    /// </summary>
    [Browsable(false)]
    public new int TabIndex
    {
        get => base.TabIndex;
        set => base.TabIndex = value;
    }

    /// <summary>
    ///  Gets or sets a value indicating whether the user can give the focus to this control using the TAB
    ///  key. This property is read-only.
    /// </summary>
    [Browsable(false)]
    public new bool TabStop
    {
        get => base.TabStop;
        set => base.TabStop = value;
    }

    /// <inheritdoc />
    [Browsable(false)]
    public override string Text
    {
        get => base.Text;
        set => base.Text = value;
    }

    /// <inheritdoc />
    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.ExStyle |= 0x20 /* WS_EX_TRANSPARENT */;
            return cp;
        }
    }

    /// <inheritdoc />
    protected override Size DefaultSize => new(100, 2);

    /// <inheritdoc />
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
                pen1.Dispose();
            }

            if (pen2 != null)
            {
                pen2.Dispose();
            }
        }

        base.Dispose(disposing);
    }

    /// <inheritdoc />
    protected override void OnPaint(PaintEventArgs e)
    {
        if (line3DStyle == Line3DStyle.Flat)
        {
            if (orientation == LineOrientation.Horizontal)
            {
                e.Graphics.DrawLine(pen1, 0, 0, Width - 1, 0);
            }
            else if (orientation == LineOrientation.Vertical)
            {
                e.Graphics.DrawLine(pen1, 0, 0, 0, Height - 1);
            }
            else if (orientation == LineOrientation.DiagonalUp)
            {
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.DrawLine(pen1, 0, Height - 1, Width - 1, 0);
            }
            else if (orientation == LineOrientation.DiagonalDown)
            {
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.DrawLine(pen1, 0, 0, Width - 1, Height - 1);
            }
        }
        else
        {
            // 3D - colours are set in the Line3DStyle_Set property
            if (orientation == LineOrientation.Horizontal)
            {
                e.Graphics.DrawLine(pen2, 0, 0, Width - 1, 0);
                e.Graphics.DrawLine(pen1, 0, 1, Width - 1, 1);
            }
            else if (orientation == LineOrientation.Vertical)
            {
                e.Graphics.DrawLine(pen2, 0, 0, 0, Height - 1);
                e.Graphics.DrawLine(pen1, 1, 0, 1, Height - 1);
            }
            else if (orientation == LineOrientation.DiagonalUp)
            {
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.DrawLine(pen2, 0, Height - 2, Width - 2, 0);
                e.Graphics.DrawLine(pen1, 1, Height - 1, Width - 1, 1);
            }
            else if (orientation == LineOrientation.DiagonalDown)
            {
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.DrawLine(pen2, 1, 0, Width - 1, Height - 2);
                e.Graphics.DrawLine(pen1, 0, 1, Width - 2, Height - 1);
            }
        }

        base.OnPaint(e);
    }

    /// <inheritdoc />
    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        => base.SetBoundsCore(x, y, width, height, specified);

    private void Line_SizeChanged(object sender, EventArgs e)
    {
        if (internalResizing)
        {
            return;
        }

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
        var lineWidth = (line3DStyle != Line3DStyle.Flat) ? 2 : (int)pen1.Width;
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
            MaximumSize = default;
            MinimumSize = MaximumSize;
        }

        internalResizing = false;
        Invalidate();
    }
}
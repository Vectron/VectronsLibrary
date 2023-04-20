namespace VectronsLibrary.WindowsForms;

/// <summary>
/// The orientation of the line.
/// </summary>
public enum LineOrientation
{
    /// <summary>
    /// Line will be drawn horizontal.
    /// </summary>
    Horizontal,

    /// <summary>
    /// Line will be drawn vertical.
    /// </summary>
    Vertical,

    /// <summary>
    /// Line will be drawn from bottom left to upper right corner.
    /// </summary>
    DiagonalUp,

    /// <summary>
    /// Line will be drawn from right corner to upper bottom left.
    /// </summary>
    DiagonalDown,
}
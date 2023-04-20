using System.Windows;
using System.Windows.Media;

namespace Windows.Win32.Graphics.Gdi;

/// <summary>Defines the attributes of a font.</summary>
/// <remarks>
/// <para>The following situations do not support ClearType antialiasing: </para>
/// <para>This doc was truncated.</para>
/// <para><see href="https://docs.microsoft.com/windows/win32/api//dimm/ns-dimm-logfontw#">Read more on docs.microsoft.com</see>.</para>
/// </remarks>
internal partial struct LOGFONTW
{
    public static LOGFONTW FromFont(FontFamily fontFamily, FontStretch fontStretch, FontStyle fontStyle, FontWeight fontWeight, Color color, double size)
    {
        var font = new LOGFONTW
        {
            lfCharSet = FONT_CHARSET.ANSI_CHARSET,
            lfClipPrecision = 0,
            lfEscapement = 0,
            lfFaceName = "",
            lfHeight = 0,
            lfItalic = 0,
            lfOrientation = 0,
            lfOutPrecision = 0,
            lfPitchAndFamily = 0,
            lfQuality = 0,
            lfStrikeOut = 0,
            lfUnderline = 0,
            lfWeight = 0,
            lfWidth = 0,
        };
    }
}
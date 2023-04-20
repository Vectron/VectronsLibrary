using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using VectronsLibrary.Wpf.SandBox.Native;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.Controls.Dialogs;
using Windows.Win32.UI.WindowsAndMessaging;

namespace VectronsLibrary.Wpf.SandBox;

/// <summary>
///  Represents a common dialog box that allows the user to pick a font.
///  This class cannot be inherited.
/// </summary>
public sealed class FontDialog : CommonDialog
{
    private const int DefaultMaxSize = 0;
    private const int DefaultMinSize = 0;
    private CHOOSEFONT_FLAGS dialogOptions;
    private int maxSize = DefaultMaxSize;
    private int minSize = DefaultMinSize;

    /// <summary>
    ///  Occurs when the user clicks the Apply button in the font dialog box.
    /// </summary>
    public event EventHandler? Apply;

    /// <summary>
    ///  Gets or sets a value indicating whether the user can change the character set specified
    ///  in the Script combo box to display a character set other than the one
    ///  currently displayed.
    /// </summary>
    public bool AllowScriptChange
    {
        get => !GetOption(CHOOSEFONT_FLAGS.CF_SELECTSCRIPT);
        set => SetOption(CHOOSEFONT_FLAGS.CF_SELECTSCRIPT, !value);
    }

    /// <summary>
    ///  Gets or sets a value indicating whether the dialog box allows vector font selections.
    /// </summary>
    public bool AllowVectorFonts
    {
        get => !GetOption(CHOOSEFONT_FLAGS.CF_NOVECTORFONTS);
        set => SetOption(CHOOSEFONT_FLAGS.CF_NOVECTORFONTS, !value);
    }

    /// <summary>
    ///  Gets or sets a value indicating whether
    ///  the dialog box displays both vertical and horizontal fonts or only
    ///  horizontal fonts.
    /// </summary>
    public bool AllowVerticalFonts
    {
        get => !GetOption(CHOOSEFONT_FLAGS.CF_NOVERTFONTS);
        set => SetOption(CHOOSEFONT_FLAGS.CF_NOVERTFONTS, !value);
    }

    /// <summary>
    /// Gets or sets the <see cref="Color"/> for this font.
    /// </summary>
    public Color Color
    {
        get;
        set;
    }

    /// <summary>
    ///  Gets or sets a value indicating whether the dialog box allows only the selection of fixed-pitch fonts.
    /// </summary>
    public bool FixedPitchOnly
    {
        get => GetOption(CHOOSEFONT_FLAGS.CF_FIXEDPITCHONLY);
        set => SetOption(CHOOSEFONT_FLAGS.CF_FIXEDPITCHONLY, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="FontFamily"/> for this font.
    /// </summary>
    public FontFamily FontFamily
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the dialog box specifies an error condition if the
    /// user attempts to select a font or style that does not exist.
    /// </summary>
    public bool FontMustExist
    {
        get => GetOption(CHOOSEFONT_FLAGS.CF_FORCEFONTEXIST);
        set => SetOption(CHOOSEFONT_FLAGS.CF_FORCEFONTEXIST, value);
    }

    /// <summary>
    /// Gets or sets the size for this font.
    /// </summary>
    public double FontSize
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the <see cref="FontStretch"/> for this font.
    /// </summary>
    public FontStretch FontStretch
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the <see cref="FontStyle"/> for this font.
    /// </summary>
    public FontStyle FontStyle
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the <see cref="FontWeight"/> for this font.
    /// </summary>
    public FontWeight FontWeight
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the maximum point size a user can select.
    /// </summary>
    public int MaxSize
    {
        get => maxSize;
        set
        {
            if (value < 0)
            {
                value = 0;
            }

            maxSize = value;

            if (maxSize > 0 && maxSize < minSize)
            {
                minSize = maxSize;
            }
        }
    }

    /// <summary>
    ///  Gets or sets a value indicating the minimum point size a user can select.
    /// </summary>
    public int MinSize
    {
        get => minSize;
        set
        {
            if (value < 0)
            {
                value = 0;
            }

            minSize = value;

            if (maxSize > 0 && maxSize < minSize)
            {
                maxSize = minSize;
            }
        }
    }

    /// <summary>
    ///  Gets or sets a
    ///  value indicating whether the dialog box allows selection of fonts for all non-OEM and Symbol character
    ///  sets, as well as the ----n National Standards Institute (ANSI) character set.
    /// </summary>
    public bool ScriptsOnly
    {
        get => GetOption(CHOOSEFONT_FLAGS.CF_SCRIPTSONLY);
        set => SetOption(CHOOSEFONT_FLAGS.CF_SCRIPTSONLY, value);
    }

    /// <summary>
    ///  Gets or sets a value indicating whether the dialog box contains an Apply button.
    /// </summary>
    public bool ShowApply
    {
        get => GetOption(CHOOSEFONT_FLAGS.CF_APPLY);
        set => SetOption(CHOOSEFONT_FLAGS.CF_APPLY, value);
    }

    /// <summary>
    ///  Gets or sets a value indicating whether the dialog box displays the color choice.
    /// </summary>
    public bool ShowColor
    {
        get;
        set;
    }

    /// <summary>
    ///  Gets or sets a value indicating whether the dialog box contains controls that allow the
    ///  user to specify strikethrough, underline, and text color options.
    /// </summary>
    public bool ShowEffects
    {
        get => GetOption(CHOOSEFONT_FLAGS.CF_EFFECTS);
        set => SetOption(CHOOSEFONT_FLAGS.CF_EFFECTS, value);
    }

    /// <summary>
    ///  Gets or sets a value indicating whether the dialog box displays a Help button.
    /// </summary>
    public bool ShowHelp
    {
        get => GetOption(CHOOSEFONT_FLAGS.CF_SHOWHELP);
        set => SetOption(CHOOSEFONT_FLAGS.CF_SHOWHELP, value);
    }

    /// <inheritdoc/>
    public override void Reset()
        => Initialize();

    /// <summary>
    ///  Returns the state of the given options flag.
    /// </summary>
    /// <param name="option">The option value to set.</param>
    /// <returns>true if success otherwise false.</returns>
    internal bool GetOption(CHOOSEFONT_FLAGS option)
        => (dialogOptions & option) != 0;

    /// <summary>
    /// Gets the given option to the given Boolean value.
    /// </summary>
    /// <param name="option">The option value to check.</param>
    /// <param name="value">The requested value.</param>
    internal void SetOption(CHOOSEFONT_FLAGS option, bool value)
    {
        if (value)
        {
            // if value is true, bitwise OR the option with _dialogOptions
            dialogOptions |= option;
        }
        else
        {
            // if value is false, AND the bitwise complement of the
            // option with _dialogOptions
            dialogOptions &= ~option;
        }
    }

    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0010:Add missing cases", Justification = "To many cases to add.")]
    protected override nint HookProc(nint hwnd, int msg, nint wParam, nint lParam)
    {
        // Assume we are successful unless we encounter a problem.
        var returnValue = IntPtr.Zero;

        switch ((WindowMessage)msg)
        {
            case WindowMessage.WM_COMMAND:
                if (wParam == 0x402)
                {
                    LOGFONTW logFont = default;
                    PInvoke.SendMessage((HWND)hwnd, (uint)WindowMessage.WM_CHOOSEFONT_GETLOGFONT, (WPARAM)0, ref logFont);
                    UpdateFont(ref logFont);
                    var index = PInvoke.SendDlgItemMessage((HWND)hwnd, (int)PInvoke.cmb4, PInvoke.CB_GETCURSEL, 0, 0);
                    if (index != PInvoke.CB_ERR)
                    {
                        UpdateColor((int)PInvoke.SendDlgItemMessage((HWND)hwnd, (int)PInvoke.cmb4, PInvoke.CB_GETITEMDATA, (WPARAM)(nuint)index.Value, 0));
                    }

                    OnApply(EventArgs.Empty);
                }

                break;

            case WindowMessage.WM_INITDIALOG:
                if (!ShowColor)
                {
                    var hWndCtl = PInvoke.GetDlgItem((HWND)hwnd, (int)PInvoke.cmb4);
                    PInvoke.ShowWindow(hWndCtl, SHOW_WINDOW_CMD.SW_HIDE);
                    hWndCtl = PInvoke.GetDlgItem((HWND)hwnd, (int)PInvoke.stc4);
                    PInvoke.ShowWindow(hWndCtl, SHOW_WINDOW_CMD.SW_HIDE);
                }

                break;

            default:
                return base.HookProc(hwnd, msg, wParam, lParam);
        }

        // Return IntPtr.Zero to indicate success, unless we have
        // adjusted the return value elsewhere in the function.
        return returnValue;
    }

    /// <inheritdoc/>
    protected override bool RunDialog(nint hwndOwner)
        => RunLegacyDialog(hwndOwner);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0010:Add missing cases", Justification = "default will handle them.")]
    private static bool RunFontDialog(CHOOSEFONTW lpcf)
    {
        var result = false;

        // Make the actual call to ChooseFont .  This function
        // blocks on ChooseFont  until the entire dialog display
        // is completed - any interaction we have with the dialog
        // while it's open takes place through our HookProc.  The
        // return value is a bool;  true = success.
        result = PInvoke.ChooseFont(ref lpcf);

        // result was 0 (false), so an error occurred.
        if (!result)
        {
            // Something may have gone wrong - check for error conditions
            // by calling CommDlgExtendedError to get the specific error.
            var errorCode = PInvoke.CommDlgExtendedError();

            // Throw an appropriate exception if we know what happened:
            switch (errorCode)
            {
                // The dialog box could not be created.
                // The common dialog box function's call to the DialogBox function failed.
                // For example, this error occurs if the common dialog box call specifies an invalid window handle.
                case COMMON_DLG_ERRORS.CDERR_DIALOGFAILURE:
                    throw new InvalidOperationException();

                // The common dialog box function failed to find a specified resource.
                case COMMON_DLG_ERRORS.CDERR_FINDRESFAILURE:
                    throw new InvalidOperationException();

                // The common dialog box function failed during initialization.
                // This error often occurs when sufficient memory is not available.
                case COMMON_DLG_ERRORS.CDERR_INITIALIZATION:
                    throw new InvalidOperationException();

                // The common dialog box function failed to load a specified resource.
                case COMMON_DLG_ERRORS.CDERR_LOADRESFAILURE:
                    throw new InvalidOperationException();

                // The common dialog box function failed to load a specified string.
                case COMMON_DLG_ERRORS.CDERR_LOADSTRFAILURE:
                    throw new InvalidOperationException();

                // The common dialog box function failed to lock a specified resource.
                case COMMON_DLG_ERRORS.CDERR_LOCKRESFAILURE:
                    throw new InvalidOperationException();

                // The common dialog box function was unable to allocate memory for internal structures.
                case COMMON_DLG_ERRORS.CDERR_MEMALLOCFAILURE:
                    throw new InvalidOperationException();

                // The common dialog box function was unable to lock the memory associated with a handle.
                case COMMON_DLG_ERRORS.CDERR_MEMLOCKFAILURE:
                    throw new InvalidOperationException();

                // The ENABLETEMPLATE flag was set in the Flags member of the initialization structure
                // for the corresponding common dialog box,
                // but you failed to provide a corresponding instance handle.
                case COMMON_DLG_ERRORS.CDERR_NOHINSTANCE:
                    throw new InvalidOperationException();

                // The ENABLEHOOK flag was set in the Flags member of the initialization structure
                // for the corresponding common dialog box,
                // but you failed to provide a pointer to a corresponding hook procedure.
                case COMMON_DLG_ERRORS.CDERR_NOHOOK:
                    throw new InvalidOperationException();

                // The ENABLETEMPLATE flag was set in the Flags member of the initialization structure
                // for the corresponding common dialog box,
                // but you failed to provide a corresponding template.
                case COMMON_DLG_ERRORS.CDERR_NOTEMPLATE:
                    throw new InvalidOperationException();

                // The lStructSize member of the initialization structure
                // for the corresponding common dialog box is invalid.
                case COMMON_DLG_ERRORS.CDERR_STRUCTSIZE:
                    throw new InvalidOperationException();

                // The size specified in the nSizeMax member of the CHOOSEFONT structure is
                // less than the size specified in the nSizeMin member.
                case COMMON_DLG_ERRORS.CFERR_MAXLESSTHANMIN:
                    throw new InvalidOperationException();

                // No fonts exist.
                case COMMON_DLG_ERRORS.CFERR_NOFONTS:
                    throw new InvalidOperationException();

                default:
                    break;
            }
        }

        return result;
    }

    private void Initialize()
    {
        dialogOptions = 0;
        SetOption(CHOOSEFONT_FLAGS.CF_SCREENFONTS, true);
        SetOption(CHOOSEFONT_FLAGS.CF_EFFECTS, true);
        SetOption(CHOOSEFONT_FLAGS.CF_TTONLY, true);

        font = null;
        color = Colors.Black;
        usingDefaultIndirectColor = true;
        ShowColor = false;
        minSize = DefaultMinSize;
        maxSize = DefaultMaxSize;
    }

    /// <summary>
    ///  Raises the <see cref="Apply"/> event.
    /// </summary>
    private void OnApply(EventArgs e)
        => Apply?.Invoke(this, e);

    /// <summary>
    /// Performs initialization work in preparation for calling RunFileDialog
    /// to show a file open or save dialog box.
    /// </summary>
    private bool RunLegacyDialog(IntPtr hwndOwner)
    {
        // Once we run the dialog, all of our communication with it is handled
        // by processing WM_NOTIFY messages in our hook procedure, this.HookProc.
        // NativeMethods.WndProc is a delegate with the appropriate signature
        // needed for a Win32 window hook procedure.
        var hookProcPtr = new LPCFHOOKPROC(HookProc);
        var logFont = LOGFONTW.FromFont(font);

        // Create a new CHOOSEFONT structure.  CHOOSEFONT is a structure defined
        // in Win32's commdlg.h that contains most of the information needed to
        // successfully display a font dialog box.
        var cf = default(CHOOSEFONTW);

        // do everything in a try block, so we always free memory in the finalizer
        try
        {
            // lStructSize
            // Specifies the length, in bytes, of the structure.
            cf.lStructSize = (uint)Marshal.SizeOf(typeof(CHOOSEFONTW));

            // hwndOwner
            // Handle to the window that owns the dialog box. This member can be any
            // valid window handle, or it can be NULL if the dialog box has no owner.
            cf.hwndOwner = (HWND)hwndOwner;

            // hDC
            // This property is ignored unless CF_PRINTERFONTS or CF_BOTH are set.
            // Since we do not set either,
            // hDC is ignored, so we can set it to zero.
            cf.hDC = HDC.Null;

            cf.lpLogFont = &logFont;

            // Flags
            // A set of bit flags you can use to initialize the dialog box.
            // Most of these will be set through public properties that then call
            // GetOption or SetOption.  We retrieve the flags using the Options property
            // and then add three additional flags here:
            //
            //     OFN_EXPLORER
            //         display an Explorer-style box (newer style)
            //     OFN_ENABLEHOOK
            //         enable the hook procedure (important for much of our functionality)
            //     OFN_ENABLESIZING
            //         allow the user to resize the dialog box
            cf.Flags = dialogOptions
                | CHOOSEFONT_FLAGS.CF_INITTOLOGFONTSTRUCT
                | CHOOSEFONT_FLAGS.CF_ENABLEHOOK;

            // lpfnHook
            // Pointer to the hook procedure.
            // Ignored unless CF_ENABLEHOOK is set in Flags.
            cf.lpfnHook = hookProcPtr;

            // hInstance
            // This property is ignored unless CF_ENABLETEMPLATE is set.
            // Since we do not set either,
            // hInstance is ignored, so we can set it to zero.
            cf.hInstance = HINSTANCE.Null;

            cf.nSizeMin = minSize;

            cf.nSizeMax = maxSize == 0 ? int.MaxValue : maxSize;

            cf.rgbColors = ShowColor || showEffects
                ? ColorTranslator.ToWin32(color)
                : ColorTranslator.ToWin32(SystemColors.ControlText);

            if (minSize > 0 || maxSize > 0)
            {
                cf.Flags |= CHOOSEFONT_FLAGS.CF_LIMITSIZE;
            }

            // if ShowColor=true then try to draw the sample text in color,
            // if ShowEffects=false then we will draw the sample text in standard control text color regardless.
            // (limitation of windows control)
            Debug.Assert(cf.nSizeMin <= cf.nSizeMax, "min and max font sizes are the wrong way around");

            // This call blocks until the dialog is closed;
            // while dialog is open, all interaction is through HookProc.
            return RunFontDialog(cf);
        }
        finally
        {
        }
    }

    private void UpdateColor(int rgb)
    {
        if (ColorTranslator.ToWin32(color) != rgb)
        {
            color = ColorTranslator.FromOle(rgb);
            usingDefaultIndirectColor = false;
        }
    }

    private void UpdateFont(ref LOGFONTW lf)
    {
        using var dc = User32.GetDcScope.ScreenDC;
        using Font fontInWorldUnits = Font.FromLogFont(lf, dc);

        // The dialog claims its working in points (a device-independent unit),
        // but actually gives us something in world units (device-dependent).
        font = ControlPaint.FontInPoints(fontInWorldUnits);
    }
}
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;
using VectronsLibrary.Wpf.Controls;

namespace VectronsLibrary.Wpf.Behaviors;

/// <summary>
/// Attached properties for <see cref="TextBox"/>.
/// </summary>
public class TextBoxHighlightTextOnFocus : Behavior<TextBox>
{
    /// <summary>
    /// An attached property for selecting all text on focus.
    /// </summary>
    /// <seealso cref="GetHighlightTextOnFocus" />
    /// <seealso cref="SetHighlightTextOnFocus" />
    public static readonly DependencyProperty HighlightTextOnFocusProperty =
        DependencyProperty.RegisterAttached(
            "HighlightTextOnFocus",
            typeof(bool),
            typeof(TextBoxHighlightTextOnFocus),
            new FrameworkPropertyMetadata(
                false,
                HighlightTextOnFocusPropertyChanged));

    /// <summary>
    /// Gets the value of the <see cref="HighlightTextOnFocusProperty"/>.
    /// </summary>
    /// <param name="obj">The <see cref="DependencyObject"/> to get the value for.</param>
    /// <returns>The value of this property.</returns>
    [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
    [AttachedPropertyBrowsableForType(typeof(TextBox))]
    [AttachedPropertyBrowsableForType(typeof(NumericTextBox))]
    public static bool GetHighlightTextOnFocus(DependencyObject obj)
        => (bool)obj.GetValue(HighlightTextOnFocusProperty);

    /// <summary>
    /// Sets the value of the <see cref="HighlightTextOnFocusProperty"/>.
    /// </summary>
    /// <param name="obj">The <see cref="DependencyObject"/> to get the value for.</param>
    /// <param name="value">The new value.</param>
    public static void SetHighlightTextOnFocus(DependencyObject obj, bool value)
        => obj.SetValue(HighlightTextOnFocusProperty, value);

    /// <inheritdoc/>
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.GotKeyboardFocus += OnKeyboardFocusSelectText;
        AssociatedObject.PreviewMouseLeftButtonDown += OnMouseLeftButtonDownSetFocus;
    }

    /// <inheritdoc/>
    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.GotKeyboardFocus -= OnKeyboardFocusSelectText;
        AssociatedObject.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDownSetFocus;
    }

    private static T? FindAncestor<T>(DependencyObject current)
        where T : DependencyObject
    {
        current = VisualTreeHelper.GetParent(current);

        while (current != null)
        {
            if (current is T t)
            {
                return t;
            }

            current = VisualTreeHelper.GetParent(current);
        }

        return null;
    }

    private static void HighlightTextOnFocusPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        if (obj is not TextBox textBox)
        {
            return;
        }

        if ((bool)e.NewValue)
        {
            textBox.GotKeyboardFocus += OnKeyboardFocusSelectText;
            textBox.PreviewMouseLeftButtonDown += OnMouseLeftButtonDownSetFocus;
        }
        else
        {
            textBox.GotKeyboardFocus -= OnKeyboardFocusSelectText;
            textBox.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDownSetFocus;
        }
    }

    private static void OnKeyboardFocusSelectText(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (e.OriginalSource is TextBox textBox)
        {
            textBox.SelectAll();
        }
    }

    private static void OnMouseLeftButtonDownSetFocus(object sender, MouseButtonEventArgs e)
    {
        var tb = FindAncestor<TextBox>((DependencyObject)e.OriginalSource);

        if (tb == null)
        {
            return;
        }

        if (!tb.IsKeyboardFocusWithin)
        {
            _ = tb.Focus();
            e.Handled = true;
        }
    }
}
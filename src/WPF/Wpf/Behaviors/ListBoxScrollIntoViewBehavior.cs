using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace VectronsLibrary.Wpf.Behaviors;

/// <summary>
/// A <see cref="Behavior"/> that keeps the selected item in view.
/// </summary>
public class ListBoxScrollIntoViewBehavior : Behavior<ListBox>
{
    /// <summary>
    /// An attached property for selecting all text on focus.
    /// </summary>
    /// <seealso cref="GetScrollIntoView" />
    /// <seealso cref="SetScrollIntoView" />
    public static readonly DependencyProperty ScrollIntoViewProperty =
        DependencyProperty.RegisterAttached(
            "ScrollIntoView",
            typeof(bool),
            typeof(ListBoxScrollIntoViewBehavior),
            new FrameworkPropertyMetadata(
                false,
                new PropertyChangedCallback(OnScrollIntoViewChanged)));

    /// <summary>
    /// Gets the value of the <see cref="ScrollIntoViewProperty"/>.
    /// </summary>
    /// <param name="obj">The <see cref="DependencyObject"/> to get the value for.</param>
    /// <returns>The value of this property.</returns>
    [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
    [AttachedPropertyBrowsableForType(typeof(ListBox))]
    public static bool GetScrollIntoView(DependencyObject obj)
        => (bool)obj.GetValue(ScrollIntoViewProperty);

    /// <summary>
    /// Sets the value of the <see cref="ScrollIntoViewProperty"/>.
    /// </summary>
    /// <param name="obj">The <see cref="DependencyObject"/> to get the value for.</param>
    /// <param name="value">The new value.</param>
    public static void SetScrollIntoView(DependencyObject obj, bool value)
        => obj.SetValue(ScrollIntoViewProperty, value);

    /// <inheritdoc/>
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.SelectionChanged += ListBox_SelectionChanged;
        if (AssociatedObject is ListBox listBox)
        {
            ScrollIntoView(listBox);
        }
    }

    /// <inheritdoc/>
    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.SelectionChanged -= ListBox_SelectionChanged;
    }

    private static void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListBox listBox)
        {
            return;
        }

        ScrollIntoView(listBox);
    }

    private static void OnScrollIntoViewChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
        if (dependencyObject is not ListBox listBox)
        {
            return;
        }

        if (dependencyPropertyChangedEventArgs.NewValue is bool newValue)
        {
            listBox.SelectionChanged += ListBox_SelectionChanged;
            ScrollIntoView(listBox);
        }
        else
        {
            listBox.SelectionChanged -= ListBox_SelectionChanged;
        }
    }

    private static void ScrollIntoView(ListBox listBox)
    {
        if (listBox.SelectedItem == null)
        {
            return;
        }

        _ = listBox.Dispatcher.BeginInvoke(new Action(() =>
            {
                listBox.UpdateLayout();
                if (listBox.SelectedItem != null)
                {
                    listBox.ScrollIntoView(listBox.SelectedItem);
                }
            }));
    }
}
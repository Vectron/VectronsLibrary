using System;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace VectronsLibrary.Wpf.Behaviors;

/// <summary>
/// A <see cref="Behavior"/> that keeps the selected item in view.
/// </summary>
public class ListBoxScrollIntoViewBehavior : Behavior<ListBox>
{
    /// <inheritdoc/>
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
    }

    /// <inheritdoc/>
    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
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

    private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListBox listBox)
        {
            return;
        }

        ScrollIntoView(listBox);
    }
}
using Microsoft.Xaml.Behaviors;
using System;
using System.Windows.Controls;

namespace VectronsLibrary.Wpf.Behaviors
{
    public class ListBoxScrollIntoViewBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
        }

        private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ListBox listBox))
            {
                return;
            }

            ScrollIntoView(listBox);
        }

        private void ScrollIntoView(ListBox listBox)
        {
            if (listBox.SelectedItem == null)
            {
                return;
            }

            listBox.Dispatcher.BeginInvoke(new Action(() =>
            {
                listBox.UpdateLayout();
                if (listBox.SelectedItem != null)
                {
                    listBox.ScrollIntoView(listBox.SelectedItem);
                }
            }));
        }
    }
}
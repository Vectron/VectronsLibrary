using System;
using System.Windows.Forms;

namespace VectronsLibrary.Winform
{
    /// <summary>
    /// Extensions for <see cref="Form"/>.
    /// </summary>
    public static class WinFormsInvokeExtension
    {
        /// <summary>
        /// Executes the specified delegate on the thread that owns the control's underlying window handle.
        /// </summary>
        /// <param name="form">The control whose window handle the delegate should be invoked on.</param>
        /// <param name="method">A delegate that contains a method to be called in the control's thread context.</param>
        public static void Invoke(this Form form, Action method)
        {
            if (form.InvokeRequired)
            {
                _ = form.Invoke(method);
            }
            else
            {
                method();
            }
        }

        /// <summary>
        /// Executes the specified delegate on the thread that owns the control's underlying window handle, returning a
        /// value.
        /// </summary>
        /// <typeparam name="TResult">The Return Type of the Delegate.</typeparam>
        /// <param name="form">The control whose window handle the delegate should be invoked on.</param>
        /// <param name="method">A delegate that contains a method to be called in the control's thread context and
        /// that returns a value.</param>
        /// <returns>The return value from the delegate being invoked.</returns>
        public static TResult Invoke<TResult>(this Form form, Func<TResult> method)
        {
            return form.InvokeRequired ? (TResult)form.Invoke(method) : method();
        }
    }
}
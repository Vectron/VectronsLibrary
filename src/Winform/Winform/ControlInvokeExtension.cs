using System;
using System.Windows.Forms;

namespace VectronsLibrary.WindowsForms;

/// <summary>
/// A set of methods that are much easier to use than the usual <see cref="Control.Invoke(Delegate)"/> versions, since they
/// accept the standard delegate types as arguments.
/// </summary>
/// <remarks>
/// Instead of using the version of <see cref="Control.Invoke(Delegate)"/> that allows you to pass parameters to your
/// delegate, just capture them in a closure when using one of these.
/// </remarks>
public static class ControlInvokeExtension
{
    /// <summary>
    /// Executes the specified delegate on the thread that owns the control's underlying window handle.
    /// </summary>
    /// <param name="control">The control whose window handle the delegate should be invoked on.</param>
    /// <param name="method">A delegate that contains a method to be called in the control's thread context.</param>
    public static void Invoke(this Control control, Action method)
    {
        if (control.InvokeRequired)
        {
            control.Invoke(method);
        }
        else
        {
            method();
        }
    }

    /// <summary>
    /// Executes the specified delegate on the thread that owns the control's underlying window handle, returning a value.
    /// </summary>
    /// <typeparam name="TResult">The Return Type of the Delegate.</typeparam>
    /// <param name="control">The control whose window handle the delegate should be invoked on.</param>
    /// <param name="method">A delegate that contains a method to be called in the control's thread context and that returns a value.</param>
    /// <returns>The return value from the delegate being invoked.</returns>
    public static TResult Invoke<TResult>(this Control control, Func<TResult> method)
        => control.InvokeRequired ? control.Invoke(method) : method();
}

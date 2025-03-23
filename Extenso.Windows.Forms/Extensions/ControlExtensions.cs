using System.Reflection;

namespace Extenso.Windows.Forms;

public static class ControlExtensions
{
    private delegate object GetControlPropertyThreadSafeDelegate(Control control, string propertyName, object[] args);

    private delegate void SetControlPropertyThreadSafeDelegate(Control control, string propertyName, object propertyValue);

    public static object GetPropertyThreadSafe(this Control control, string propertyName, object[] args) => control.InvokeRequired
            ? control.Invoke(new GetControlPropertyThreadSafeDelegate(
                GetPropertyThreadSafe),
                new object[] { control, propertyName, args })
            : control.GetType().InvokeMember(
                propertyName,
                BindingFlags.GetProperty,
                null,
                control,
                args);

    //FROM: http://dnpextensions.codeplex.com
    /// <summary>
    ///   Returns <c>true</c> if target control is in design mode or one of the target's parent is in design mode.
    ///   Othervise returns <c>false</c>.
    /// </summary>
    /// <param name = "target">Target control. Can not be null.</param>
    /// <example>
    ///   bool designMode = this.button1.IsInWinDesignMode();
    /// </example>
    /// <remarks>
    ///   The design mode is set only to direct controls in desgined control/form.
    ///   However the child controls in controls/usercontrols does not flag for "my parent is in design mode".
    ///   The solution is traversion upon parents of target control.
    ///
    ///   Contributed by tencokacistromy, http://www.codeplex.com/site/users/view/tencokacistromy
    /// </remarks>
    public static bool IsInWinDesignMode(this Control target)
    {
        bool ret = false;

        var ctl = target;
        while (ctl is not null)
        {
            var site = ctl.Site;
            if (site is not null)
            {
                if (site.DesignMode)
                {
                    ret = true;
                    break;
                }
            }
            ctl = ctl.Parent;
        }

        return ret;
    }

    public static void SetPropertyThreadSafe(this Control control, string propertyName, object propertyValue)
    {
        if (control.InvokeRequired)
        {
            control.Invoke(new SetControlPropertyThreadSafeDelegate(
                SetPropertyThreadSafe),
                new object[] { control, propertyName, propertyValue });
        }
        else
        {
            control.GetType().InvokeMember(
                propertyName,
                BindingFlags.SetProperty,
                null,
                control,
                new object[] { propertyValue });
        }
    }
}
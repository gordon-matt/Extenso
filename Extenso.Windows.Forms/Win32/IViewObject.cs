using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Extenso.Windows.Forms.Win32;

/// <summary>
/// Defines the contract for the view object.
/// </summary>
[ComVisible(true), ComImport()]
[GuidAttribute("0000010d-0000-0000-C000-000000000046")]
[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
public interface IViewObject
{
    /// <summary>
    /// Draws the value.
    /// </summary>
    /// <param name="dwDrawAspect">The dw draw aspect.</param>
    /// <param name="lindex">The lindex.</param>
    /// <param name="pvAspect">The pv aspect.</param>
    /// <param name="ptd">The ptd.</param>
    /// <param name="hdcTargetDev">The hdc target dev.</param>
    /// <param name="hdcDraw">The hdc draw.</param>
    /// <param name="lprcBounds">The lprc bounds.</param>
    /// <param name="lprcWBounds">The lprc w bounds.</param>
    /// <param name="pfnContinue">The pfn continue.</param>
    /// <param name="dwContinue">The dw continue.</param>
    /// <returns>The int.</returns>
    [return: MarshalAs(UnmanagedType.I4)]
    [PreserveSig]
    int Draw(
        [MarshalAs(UnmanagedType.U4)] uint dwDrawAspect,
        int lindex,
        IntPtr pvAspect,
        [In] IntPtr ptd,
        IntPtr hdcTargetDev,
        IntPtr hdcDraw,
        [MarshalAs(UnmanagedType.Struct)] ref Rectangle lprcBounds,
        [MarshalAs(UnmanagedType.Struct)] ref Rectangle lprcWBounds,
        IntPtr pfnContinue,
        [MarshalAs(UnmanagedType.U4)] uint dwContinue);

    /// <summary>
    /// Gets the color set.
    /// </summary>
    /// <param name="dwDrawAspect">The dw draw aspect.</param>
    /// <param name="lindex">The lindex.</param>
    /// <param name="pvAspect">The pv aspect.</param>
    /// <param name="ptd">The ptd.</param>
    /// <param name="hicTargetDev">The hic target dev.</param>
    /// <param name="ppColorSet">The pp color set.</param>
    /// <returns>The color set.</returns>
    [PreserveSig]
    int GetColorSet([In, MarshalAs(UnmanagedType.U4)] int dwDrawAspect,
       int lindex, IntPtr pvAspect, [In] IntPtr ptd,
        IntPtr hicTargetDev, [Out] IntPtr ppColorSet);

    /// <summary>
    /// Gets the freeze.
    /// </summary>
    /// <param name="dwDrawAspect">The dw draw aspect.</param>
    /// <param name="lindex">The lindex.</param>
    /// <param name="pvAspect">The pv aspect.</param>
    /// <param name="pdwFreeze">The pdw freeze.</param>
    /// <returns>The freeze.</returns>
    [PreserveSig]
    int Freeze([In, MarshalAs(UnmanagedType.U4)] int dwDrawAspect,
                    int lindex, IntPtr pvAspect, [Out] IntPtr pdwFreeze);

    /// <summary>
    /// Gets the unfreeze.
    /// </summary>
    /// <param name="dwFreeze">The dw freeze.</param>
    /// <returns>The unfreeze.</returns>
    [PreserveSig]
    int Unfreeze([In, MarshalAs(UnmanagedType.U4)] int dwFreeze);

    /// <summary>
    /// Sets the advise.
    /// </summary>
    /// <param name="aspects">The aspects.</param>
    /// <param name="advf">The advf.</param>
    /// <param name="pAdvSink">The p adv sink.</param>
    void SetAdvise([In, MarshalAs(UnmanagedType.U4)] int aspects,
      [In, MarshalAs(UnmanagedType.U4)] int advf,
      [In, MarshalAs(UnmanagedType.Interface)] IAdviseSink pAdvSink);

    /// <summary>
    /// Gets the advise.
    /// </summary>
    /// <param name="paspects">The paspects.</param>
    /// <param name="advf">The advf.</param>
    /// <param name="pAdvSink">The p adv sink.</param>
    void GetAdvise([In, Out, MarshalAs(UnmanagedType.LPArray)] int[] paspects,
      [In, Out, MarshalAs(UnmanagedType.LPArray)] int[] advf,
      [In, Out, MarshalAs(UnmanagedType.LPArray)] IAdviseSink[] pAdvSink);
}
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Extenso.Windows.Forms.Win32;

[ComVisible(true), ComImport()]
[GuidAttribute("0000010d-0000-0000-C000-000000000046")]
[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
public interface IViewObject
{
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

    [PreserveSig]
    int GetColorSet([In, MarshalAs(UnmanagedType.U4)] int dwDrawAspect,
       int lindex, IntPtr pvAspect, [In] IntPtr ptd,
        IntPtr hicTargetDev, [Out] IntPtr ppColorSet);

    [PreserveSig]
    int Freeze([In, MarshalAs(UnmanagedType.U4)] int dwDrawAspect,
                    int lindex, IntPtr pvAspect, [Out] IntPtr pdwFreeze);

    [PreserveSig]
    int Unfreeze([In, MarshalAs(UnmanagedType.U4)] int dwFreeze);

    void SetAdvise([In, MarshalAs(UnmanagedType.U4)] int aspects,
      [In, MarshalAs(UnmanagedType.U4)] int advf,
      [In, MarshalAs(UnmanagedType.Interface)] IAdviseSink pAdvSink);

    void GetAdvise([In, Out, MarshalAs(UnmanagedType.LPArray)] int[] paspects,
      [In, Out, MarshalAs(UnmanagedType.LPArray)] int[] advf,
      [In, Out, MarshalAs(UnmanagedType.LPArray)] IAdviseSink[] pAdvSink);
}
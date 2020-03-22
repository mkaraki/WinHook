using System;
using System.Windows;
using System.Windows.Forms;

namespace WinHook
{
    public delegate void LowLevelMouseProcEventHandler(WPARAM wParam, LowLevelMouseProcEventArgs e);
    public delegate void WM_MOUSE_EventHandler(LowLevelMouseProcEventArgs e);

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    internal struct MSLLHOOKSTRUCT
    {
        public POINT pt;
        public uint mouseData;
        public uint flags;
        public uint time;
        public UIntPtr dwExtraInfo;
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    internal struct POINT
    {
        public int x;
        public int y;
    }

    public class LowLevelMouseProcEventArgs : EventArgs
    {
        internal LowLevelMouseProcEventArgs(MSLLHOOKSTRUCT data)
        {
            Point = new Point(data.pt.x, data.pt.y);
            MouseData = data.mouseData;
            Flags = data.flags;
            Time = data.time;
            ExtraInfo = data.dwExtraInfo;
        }

        public Point Point { get; set; }

        public uint MouseData { get; set; }

        public uint Flags { get; set; }

        public uint Time { get; set; }

        public UIntPtr ExtraInfo { get; set; }
    }
}
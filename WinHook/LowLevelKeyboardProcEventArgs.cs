using System;
using System.Windows.Forms;

namespace WinHook
{
    public delegate void LowLevelKeyboardProcEventHandler(WPARAM wParam, LowLevelKeyboardProcEventArgs e);
    public delegate void WM_KEY_EventHandler(LowLevelKeyboardProcEventArgs e);

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    internal struct KBDLLHOOKSTRUCT
    {
        public uint vkCode;
        public uint scanCode;
        public uint flags;
        public uint time;
        public UIntPtr dwExtraInfo;
    }

    public class LowLevelKeyboardProcEventArgs : EventArgs
    {
        internal LowLevelKeyboardProcEventArgs(KBDLLHOOKSTRUCT data)
        {
            Key = (Keys)data.vkCode;
            ScanCode = data.scanCode;
            Flags = data.flags;
            Time = data.time;
            ExtraInfo = data.dwExtraInfo;
        }

        public Keys Key { get; set; }

        public uint ScanCode { get; set; }

        public uint Flags { get; set; }

        public uint Time { get; set; }

        public UIntPtr ExtraInfo { get; set; }

    }
}
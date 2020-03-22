using System;
using static System.Runtime.InteropServices.Marshal;

namespace WinHook
{
    public class WH_KEYBOARD_LL : WinHook
    {
        public WH_KEYBOARD_LL() : base(WHid.WH_KEYBOARD_LL, GetHInstance(), 0)
        {
        }

        public event LowLevelKeyboardProcEventHandler LowLevelKeyboardProc;

        public event WM_KEY_EventHandler KeyDown;

        public event WM_KEY_EventHandler KeyUp;

        public event WM_KEY_EventHandler SysKeyDown;

        public event WM_KEY_EventHandler SysKeyUp;

        internal override IntPtr Callback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var data = (KBDLLHOOKSTRUCT)PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

            var ea = new LowLevelKeyboardProcEventArgs(data);
            LowLevelKeyboardProc?.Invoke((WPARAM)wParam, ea);

            switch ((WPARAM)wParam)
            {
                case WPARAM.WM_KEYDOWN:
                    KeyDown?.Invoke(ea);
                    break;

                case WPARAM.WM_KEYUP:
                    KeyUp?.Invoke(ea);
                    break;

                case WPARAM.WM_SYSKEYDOWN:
                    SysKeyDown?.Invoke(ea);
                    break;

                case WPARAM.WM_SYSKEYUP:
                    SysKeyUp?.Invoke(ea);
                    break;
            }

            return base.Callback(nCode, wParam, lParam);
        }
    }
}
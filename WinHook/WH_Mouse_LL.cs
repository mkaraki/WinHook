using System;
using static System.Runtime.InteropServices.Marshal;

namespace WinHook
{
    public class WH_MOUSE_LL : WinHook
    {
        public WH_MOUSE_LL() : base(WHid.WH_MOUSE_LL, GetHInstance(), 0)
        {
        }

        public event LowLevelMouseProcEventHandler LowLevelMouseProc;

        public event WM_MOUSE_EventHandler MouseMove;
        public event WM_MOUSE_EventHandler LButtonDown;
        public event WM_MOUSE_EventHandler LButtonUp;
        public event WM_MOUSE_EventHandler RButtonDown;
        public event WM_MOUSE_EventHandler RButtonUp;
        public event WM_MOUSE_EventHandler MouseWheel;
        public event WM_MOUSE_EventHandler MouseHWheel;


        internal override IntPtr Callback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var data = (MSLLHOOKSTRUCT)PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));

            var ea = new LowLevelMouseProcEventArgs(data);
            LowLevelMouseProc?.Invoke((WPARAM)wParam, ea);

            switch ((WPARAM)wParam)
            {
                case WPARAM.WM_MOUSEMOVE:
                    MouseMove?.Invoke(ea);
                    break;

                case WPARAM.WM_LBUTTONDOWN:
                    LButtonDown?.Invoke(ea);
                    break;

                case WPARAM.WM_LBUTTONUP:
                    LButtonUp?.Invoke(ea);
                    break;

                case WPARAM.WM_RBUTTONDOWN:
                    RButtonDown?.Invoke(ea);
                    break;

                case WPARAM.WM_RBUTTONUP:
                    RButtonUp?.Invoke(ea);
                    break;

                case WPARAM.WM_MOUSEWHEEL:
                    MouseWheel?.Invoke(ea);
                    break;

                case WPARAM.WM_MOUSEHWHEEL:
                    MouseHWheel?.Invoke(ea);
                    break;
            }

            return base.Callback(nCode, wParam, lParam);
        }
    }
}
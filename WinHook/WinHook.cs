using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WinHook
{
    delegate IntPtr HOOKPROC(int nCode, IntPtr wParam, IntPtr lParam);

    public delegate void HookCallbackEventHandler(int nCode, IntPtr wParam, IntPtr lParam);
    
    public class WinHook : IDisposable
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HOOKPROC lpfn, IntPtr hmod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr GetModuleHandle([MarshalAs(UnmanagedType.LPWStr), In] string lpModuleName);

        internal static IntPtr GetHInstance()
        { 
            using (var currentProc = Process.GetCurrentProcess())
            using (var currentModule = currentProc.MainModule)
                return GetModuleHandle(currentModule.ModuleName);
        }

        private IntPtr hhk;

        public bool CallNextHook { get; set; } = true;

        internal HOOKPROC CallbackMethod;
        public WinHook(WHid idHook, IntPtr? hmod = null, uint dwThreadId = 0) : this((int)idHook, hmod, dwThreadId) {}

        public WinHook(int idHook, IntPtr? hmod = null, uint dwThreadId = 0) 
        {
            CallbackMethod = Callback;
            hhk = SetWindowsHookEx(idHook, CallbackMethod, hmod ?? IntPtr.Zero, dwThreadId);
        }

        public void Dispose()
        {
            UnhookWindowsHookEx(hhk);
        }

        public event HookCallbackEventHandler HookCallback;

        internal virtual IntPtr Callback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            HookCallback?.Invoke(nCode, wParam, lParam);

            if (CallNextHook)
                return CallNextHookEx(hhk, nCode, wParam, lParam);
            else
                return (IntPtr) 1;
        }
    }
}

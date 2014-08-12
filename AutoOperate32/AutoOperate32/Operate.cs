using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;

namespace AutoOperate32
{
    public class Operate
    {
        const int m_WaitTime = 10;
        static cell m_PWInfo;
        static cell m_CWInfo;

        public Operate()
        {
            m_PWInfo = new cell();
            m_CWInfo = new cell();
        }

        public IntPtr getPWHandle()
        {
            return m_PWInfo.hWnd;
        }

        public IntPtr getCWHandle()
        {
            return m_CWInfo.hWnd;
        }

        public void SendKeyboardEvent(IntPtr hWnd, string cmd)
        {
            foreach (char pi in cmd)
            {
                win32api.SetForegroundWindow(hWnd);
                win32api.keybd_event((byte)pi, 0, 0, (UIntPtr)0);
                win32api.keybd_event((byte)pi, 0, 2, (UIntPtr)0);
                Thread.Sleep(m_WaitTime);
            }
        }

        private static bool EnumWaitCWCallBack(IntPtr hWnd, IntPtr lparam)
        {
            int textLen = win32api.GetWindowTextLength(hWnd);
            if (0 < textLen)
            {
                StringBuilder tsb = new StringBuilder(textLen + 1);
                win32api.GetWindowText(hWnd, tsb, tsb.Capacity);
                if (m_CWInfo.title == tsb.ToString())
                {
                    m_CWInfo.hWnd = hWnd;
                }
            }

            return true;
        }

        public static bool WaitCWShow(string title, int waitTime, int waitCnt)
        {
            m_CWInfo.title = title;
            m_CWInfo.hWnd = IntPtr.Zero;

            int cnt = waitCnt;
            do
            {
                win32api.EnumChildWindows(m_PWInfo.hWnd, new win32api.EnumWindowsDelegate(EnumWaitCWCallBack), IntPtr.Zero);
                if (m_PWInfo.hWnd != IntPtr.Zero)
                {
                    return true;
                }
                Thread.Sleep(waitTime);
                waitCnt--;
            } while (waitCnt > 0);

            return false;
        }

        private static bool EnumWaitPWCallBack(IntPtr hWnd, IntPtr lparam)
        {
            int textLen = win32api.GetWindowTextLength(hWnd);
            if (0 < textLen)
            {
                StringBuilder tsb = new StringBuilder(textLen + 1);
                win32api.GetWindowText(hWnd, tsb, tsb.Capacity);
                if (m_PWInfo.title == tsb.ToString())
                {
                    m_PWInfo.hWnd = hWnd;
                }
            }

            return true;
        }

        public static bool WaitPWShow(string title, int waitTime, int waitCnt)
        {
            m_PWInfo.title = title;
            m_PWInfo.hWnd = IntPtr.Zero;

            int cnt = waitCnt;
            do
            {
                win32api.EnumWindows(new win32api.EnumWindowsDelegate(EnumWaitPWCallBack), IntPtr.Zero);
                if (m_PWInfo.hWnd != IntPtr.Zero)
                {
                    return true;
                }
                Thread.Sleep(waitTime);
                waitCnt--;
            } while (waitCnt > 0);

            return false;
        }

        public class win32api
        {
            [DllImport("user32.dll")]
            public static extern uint keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

            public delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public extern static bool EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lparam);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public extern static bool EnumChildWindows(IntPtr hWndParent, EnumWindowsDelegate lpEnumFunc, IntPtr lparam);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int GetWindowTextLength(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr SetActiveWindow(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr SetFocus(IntPtr hWnd);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetForegroundWindow(IntPtr hWnd);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool IsWindowEnabled(IntPtr hWnd);

            public const int BM_CLICK = 0x00F5;
            [DllImport("user32.dll")]
            public static extern Int32 PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, StringBuilder pvParam, uint fWinIni);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);
        }

        public class cell
        {
            public IntPtr hWnd { set; get; }
            public string title { set; get; }
        }
    }
}

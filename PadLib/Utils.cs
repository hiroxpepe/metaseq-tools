/*
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace PadLib {
    /// <summary>
    /// utility class
    /// </summary>
    /// <author>h.adachi (STUDIO MeowToon)</author>
    public class Utils {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constants

        const string TARGET_TITLE = "Metasequoia 4";

        #region Win32API Constants

        const int INPUT_KEYBOARD = 1;
        const int KEYEVENTF_KEYDOWN = 0x0;
        const int KEYEVENTF_KEYUP = 0x2;
        const int KEYEVENTF_EXTENDEDKEY = 0x1;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb]

        public static void SendKey(int key, bool is_extend = false) {
            if (!setActive()) return;
            INPUT input = getKeyDownInput(key, is_extend);
            SendInput(1, ref input, Marshal.SizeOf(input));
            Thread.Sleep(100); // wait
            input = getKeyUpInput(input, is_extend);
            SendInput(1, ref input, Marshal.SizeOf(input));
        }

        public static void SendKeyWithShift(int key, bool is_extend = false) {
            if (!setActive()) return;
            INPUT input0 = getKeyDownInput(0x10, is_extend); // VK_SHIFT
            INPUT input1 = getKeyDownInput(key, is_extend);
            SendInput(1, ref input0, Marshal.SizeOf(input0));
            SendInput(1, ref input1, Marshal.SizeOf(input1));
            Thread.Sleep(100); // wait
            input1 = getKeyUpInput(input1, is_extend);
            SendInput(1, ref input1, Marshal.SizeOf(input1));
            input0 = getKeyUpInput(input0, is_extend);
            SendInput(1, ref input0, Marshal.SizeOf(input0));
        }

        public static void SendKeyWithCtrl(int key, bool is_extend = false) {
            if (!setActive()) return;
            INPUT input0 = getKeyDownInput(0x11, is_extend); // VK_CONTROL
            INPUT input1 = getKeyDownInput(key, is_extend);
            SendInput(1, ref input0, Marshal.SizeOf(input0));
            SendInput(1, ref input1, Marshal.SizeOf(input1));
            Thread.Sleep(100); // wait
            input1 = getKeyUpInput(input1, is_extend);
            SendInput(1, ref input1, Marshal.SizeOf(input1));
            input0 = getKeyUpInput(input0, is_extend);
            SendInput(1, ref input0, Marshal.SizeOf(input0));
        }

        public static void SendKeyWithAlt(int key, bool is_extend = false) {
            if (!setActive()) return;
            INPUT input0 = getKeyDownInput(0x12, is_extend); // VK_MENU : Alt key
            INPUT input1 = getKeyDownInput(key, is_extend);
            SendInput(1, ref input0, Marshal.SizeOf(input0));
            SendInput(1, ref input1, Marshal.SizeOf(input1));
            Thread.Sleep(100); // wait
            input1 = getKeyUpInput(input1, is_extend);
            SendInput(1, ref input1, Marshal.SizeOf(input1));
            input0 = getKeyUpInput(input0, is_extend);
            SendInput(1, ref input0, Marshal.SizeOf(input0));
        }

        public static void SendKeyWithCtrlAndShift(int key, bool is_extend = false) {
            if (!setActive()) return;
            INPUT input0 = getKeyDownInput(0x11, is_extend); // VK_CONTROL
            INPUT input1 = getKeyDownInput(0x10, is_extend); // VK_SHIFT
            INPUT input2 = getKeyDownInput(key, is_extend);
            SendInput(1, ref input0, Marshal.SizeOf(input0));
            SendInput(1, ref input1, Marshal.SizeOf(input1));
            SendInput(1, ref input2, Marshal.SizeOf(input2));
            Thread.Sleep(100); // wait
            input2 = getKeyUpInput(input2, is_extend);
            SendInput(1, ref input2, Marshal.SizeOf(input2));
            input1 = getKeyUpInput(input1, is_extend);
            SendInput(1, ref input1, Marshal.SizeOf(input1));
            input0 = getKeyUpInput(input0, is_extend);
            SendInput(1, ref input0, Marshal.SizeOf(input0));
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        static bool setActive() {
            foreach (Process p in Process.GetProcesses()) {
                if (p.MainWindowTitle.IndexOf(TARGET_TITLE) >= 0) {
                    SetForegroundWindow(p.MainWindowHandle);
                    return true;
                }
            }
            return false;
        }

        static INPUT getKeyDownInput(int key, bool is_extend) {
            return new INPUT {
                type = INPUT_KEYBOARD,
                ki = new KEYBDINPUT() {
                    wVk = (short) key,
                    wScan = 0,
                    dwFlags = ((is_extend) ? KEYEVENTF_EXTENDEDKEY : 0x0) | KEYEVENTF_KEYDOWN,
                    time = 0,
                    dwExtraInfo = 0
                },
            };
        }

        static INPUT getKeyUpInput(INPUT input, bool is_extend) {
            input.ki.dwFlags = ((is_extend) ? KEYEVENTF_EXTENDEDKEY : 0x0) | KEYEVENTF_KEYUP;
            return input;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Win32API

        #region Win32API Methods

        [DllImport("user32.dll")]
        static extern void SendInput(int nInputs, ref INPUT pInputs, int cbsize);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        #endregion

        #region Win32API Structures

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public int dwExtraInfo;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public int dwExtraInfo;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        };

        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT {
            [FieldOffset(0)]
            public int type;
            [FieldOffset(4)]
            public MOUSEINPUT no;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
        };

        #endregion

    }
}

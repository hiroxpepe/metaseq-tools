/*
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
 * (at your option) any later version.
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace PadLib {
    /// <summary>
    /// @author h.adachi
    /// </summary>
    public class Utils {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constants

        private const string TARGET_TITLE = "Metasequoia 4";

        #region Win32API Constants

        private const int INPUT_KEYBOARD = 1;
        private const int KEYEVENTF_KEYDOWN = 0x0;
        private const int KEYEVENTF_KEYUP = 0x2;
        private const int KEYEVENTF_EXTENDEDKEY = 0x1;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb]

        public static void SendKey(int key, bool isExtend = false) {
            if (!SetActive()) return;
            INPUT _input = getKeyDownInput(key, isExtend);
            SendInput(1, ref _input, Marshal.SizeOf(_input));
            Thread.Sleep(100); // wait
            _input = getKeyUpInput(_input, isExtend);
            SendInput(1, ref _input, Marshal.SizeOf(_input));
        }

        public static void SendKeyWithShift(int key, bool isExtend = false) {
            if (!SetActive()) return;
            INPUT _input0 = getKeyDownInput(0x10, isExtend); // VK_SHIFT
            INPUT _input1 = getKeyDownInput(key, isExtend);
            SendInput(1, ref _input0, Marshal.SizeOf(_input0));
            SendInput(1, ref _input1, Marshal.SizeOf(_input1));
            Thread.Sleep(100); // wait
            _input1 = getKeyUpInput(_input1, isExtend);
            SendInput(1, ref _input1, Marshal.SizeOf(_input1));
            _input0 = getKeyUpInput(_input0, isExtend);
            SendInput(1, ref _input0, Marshal.SizeOf(_input0));
        }

        public static void SendKeyWithCtrl(int key, bool isExtend = false) {
            if (!SetActive()) return;
            INPUT _input0 = getKeyDownInput(0x11, isExtend); // VK_CONTROL
            INPUT _input1 = getKeyDownInput(key, isExtend);
            SendInput(1, ref _input0, Marshal.SizeOf(_input0));
            SendInput(1, ref _input1, Marshal.SizeOf(_input1));
            Thread.Sleep(100); // wait
            _input1 = getKeyUpInput(_input1, isExtend);
            SendInput(1, ref _input1, Marshal.SizeOf(_input1));
            _input0 = getKeyUpInput(_input0, isExtend);
            SendInput(1, ref _input0, Marshal.SizeOf(_input0));
        }

        public static void SendKeyWithAlt(int key, bool isExtend = false) {
            if (!SetActive()) return;
            INPUT _input0 = getKeyDownInput(0x12, isExtend); // VK_MENU : Alt key
            INPUT _input1 = getKeyDownInput(key, isExtend);
            SendInput(1, ref _input0, Marshal.SizeOf(_input0));
            SendInput(1, ref _input1, Marshal.SizeOf(_input1));
            Thread.Sleep(100); // wait
            _input1 = getKeyUpInput(_input1, isExtend);
            SendInput(1, ref _input1, Marshal.SizeOf(_input1));
            _input0 = getKeyUpInput(_input0, isExtend);
            SendInput(1, ref _input0, Marshal.SizeOf(_input0));
        }

        public static void SendKeyWithCtrlAndShift(int key, bool isExtend = false) {
            if (!SetActive()) return;
            INPUT _input0 = getKeyDownInput(0x11, isExtend); // VK_CONTROL
            INPUT _input1 = getKeyDownInput(0x10, isExtend); // VK_SHIFT
            INPUT _input2 = getKeyDownInput(key, isExtend);
            SendInput(1, ref _input0, Marshal.SizeOf(_input0));
            SendInput(1, ref _input1, Marshal.SizeOf(_input1));
            SendInput(1, ref _input2, Marshal.SizeOf(_input2));
            Thread.Sleep(100); // wait
            _input2 = getKeyUpInput(_input2, isExtend);
            SendInput(1, ref _input2, Marshal.SizeOf(_input2));
            _input1 = getKeyUpInput(_input1, isExtend);
            SendInput(1, ref _input1, Marshal.SizeOf(_input1));
            _input0 = getKeyUpInput(_input0, isExtend);
            SendInput(1, ref _input0, Marshal.SizeOf(_input0));
        }

        public static bool SetActive() {
            foreach (Process _p in Process.GetProcesses()) {
                if (0 <= _p.MainWindowTitle.IndexOf(TARGET_TITLE)) {
                    SetForegroundWindow(_p.MainWindowHandle);
                    return true;
                }
            }
            return false;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        static INPUT getKeyDownInput(int key, bool isExtend) {
            return new INPUT {
                type = INPUT_KEYBOARD,
                ki = new KEYBDINPUT() {
                    wVk = (short) key,
                    wScan = 0,
                    dwFlags = ((isExtend) ? KEYEVENTF_EXTENDEDKEY : 0x0) | KEYEVENTF_KEYDOWN,
                    time = 0,
                    dwExtraInfo = 0
                },
            };
        }

        static INPUT getKeyUpInput(INPUT input, bool isExtend) {
            input.ki.dwFlags = ((isExtend) ? KEYEVENTF_EXTENDEDKEY : 0x0) | KEYEVENTF_KEYUP;
            return input;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Win32API

        #region Win32API Methods

        [DllImport("user32.dll")]
        private static extern void SendInput(int nInputs, ref INPUT pInputs, int cbsize);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

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
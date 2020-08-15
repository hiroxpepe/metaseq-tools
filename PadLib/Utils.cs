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

using System.Threading;
using System.Runtime.InteropServices;

namespace PadLib {
    /// <summary>
    /// @author h.adachi
    /// </summary>
    public class Utils {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constants

        #region Win32API Constants

        private const int INPUT_KEYBOARD = 1;
        private const int KEYEVENTF_KEYDOWN = 0x0;
        private const int KEYEVENTF_KEYUP = 0x2;
        private const int KEYEVENTF_EXTENDEDKEY = 0x1;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public static Methods [verb]

        public static void SendKey(int key, bool isExtend = false) {
            INPUT input = new INPUT {
                type = INPUT_KEYBOARD,
                ki = new KEYBDINPUT() {
                    wVk = (short) key,
                    wScan = 0,
                    dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYDOWN,
                    time = 0,
                    dwExtraInfo = 0
                },
            };
            SendInput(1, ref input, Marshal.SizeOf(input));
            Thread.Sleep(100); // wait
            input.ki.dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYUP;
            SendInput(1, ref input, Marshal.SizeOf(input));
        }

        public static void SendKeyWithShift(int key, bool isExtend = false) {
            INPUT input0 = new INPUT {
                type = INPUT_KEYBOARD,
                ki = new KEYBDINPUT() {
                    wVk = (short) 0x10, // VK_SHIFT
                    wScan = 0,
                    dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYDOWN,
                    time = 0,
                    dwExtraInfo = 0
                },
            };
            INPUT input1 = new INPUT {
                type = INPUT_KEYBOARD,
                ki = new KEYBDINPUT() {
                    wVk = (short) key,
                    wScan = 0,
                    dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYDOWN,
                    time = 0,
                    dwExtraInfo = 0
                },
            };
            SendInput(1, ref input0, Marshal.SizeOf(input0));
            SendInput(1, ref input1, Marshal.SizeOf(input1));
            Thread.Sleep(100); // wait
            input1.ki.dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYUP;
            SendInput(1, ref input1, Marshal.SizeOf(input1));
            input0.ki.dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYUP;
            SendInput(1, ref input0, Marshal.SizeOf(input0));
        }

        public static void SendKeyWithCtrl(int key, bool isExtend = false) {
            INPUT input0 = new INPUT {
                type = INPUT_KEYBOARD,
                ki = new KEYBDINPUT() {
                    wVk = (short) 0x11, // VK_CONTROL
                    wScan = 0,
                    dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYDOWN,
                    time = 0,
                    dwExtraInfo = 0
                },
            };
            INPUT input1 = new INPUT {
                type = INPUT_KEYBOARD,
                ki = new KEYBDINPUT() {
                    wVk = (short) key,
                    wScan = 0,
                    dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYDOWN,
                    time = 0,
                    dwExtraInfo = 0
                },
            };
            SendInput(1, ref input0, Marshal.SizeOf(input0));
            SendInput(1, ref input1, Marshal.SizeOf(input1));
            Thread.Sleep(100); // wait
            input1.ki.dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYUP;
            SendInput(1, ref input1, Marshal.SizeOf(input1));
            input0.ki.dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYUP;
            SendInput(1, ref input0, Marshal.SizeOf(input0));
        }

        public static void SendKeyWithAlt(int key, bool isExtend = false) {
            INPUT input0 = new INPUT {
                type = INPUT_KEYBOARD,
                ki = new KEYBDINPUT() {
                    wVk = (short) 0x12, // VK_MENU : Alt key
                    wScan = 0,
                    dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYDOWN,
                    time = 0,
                    dwExtraInfo = 0
                },
            };
            INPUT input1 = new INPUT {
                type = INPUT_KEYBOARD,
                ki = new KEYBDINPUT() {
                    wVk = (short) key,
                    wScan = 0,
                    dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYDOWN,
                    time = 0,
                    dwExtraInfo = 0
                },
            };
            SendInput(1, ref input0, Marshal.SizeOf(input0));
            SendInput(1, ref input1, Marshal.SizeOf(input1));
            Thread.Sleep(100); // wait
            input1.ki.dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYUP;
            SendInput(1, ref input1, Marshal.SizeOf(input1));
            input0.ki.dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYUP;
            SendInput(1, ref input0, Marshal.SizeOf(input0));
        }

        public static void SendKeyWithCtrlAndShift(int key, bool isExtend = false) {
            INPUT input0 = new INPUT {
                type = INPUT_KEYBOARD,
                ki = new KEYBDINPUT() {
                    wVk = (short) 0x11, // VK_CONTROL
                    wScan = 0,
                    dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYDOWN,
                    time = 0,
                    dwExtraInfo = 0
                },
            };
            INPUT input1 = new INPUT {
                type = INPUT_KEYBOARD,
                ki = new KEYBDINPUT() {
                    wVk = (short) 0x10, // VK_SHIFT
                    wScan = 0,
                    dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYDOWN,
                    time = 0,
                    dwExtraInfo = 0
                },
            };
            INPUT input2 = new INPUT {
                type = INPUT_KEYBOARD,
                ki = new KEYBDINPUT() {
                    wVk = (short) key,
                    wScan = 0,
                    dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYDOWN,
                    time = 0,
                    dwExtraInfo = 0
                },
            };
            SendInput(1, ref input0, Marshal.SizeOf(input0));
            SendInput(1, ref input1, Marshal.SizeOf(input1));
            SendInput(1, ref input2, Marshal.SizeOf(input2));
            Thread.Sleep(100); // wait
            input2.ki.dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYUP;
            SendInput(1, ref input2, Marshal.SizeOf(input2));
            input1.ki.dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYUP;
            SendInput(1, ref input1, Marshal.SizeOf(input1));
            input0.ki.dwFlags = ((isExtend) ? (KEYEVENTF_EXTENDEDKEY) : 0x0) | KEYEVENTF_KEYUP;
            SendInput(1, ref input0, Marshal.SizeOf(input0));
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Win32API

        #region Win32API Methods

        [DllImport("user32.dll")]
        private static extern void SendInput(int nInputs, ref INPUT pInputs, int cbsize);

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
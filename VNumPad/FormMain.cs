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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VNumPad {
    /// <summary>
    /// @author h.adachi
    /// </summary>
    public partial class FormMain : Form {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constants

        #region Win32API Constants

        private const int WS_EX_NOACTIVATE = 0x8000000;
        private const int INPUT_KEYBOARD = 1;
        private const int KEYEVENTF_KEYDOWN = 0x0;
        private const int KEYEVENTF_EXTENDEDKEY = 0x1;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public FormMain() {
            InitializeComponent();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // protected override Methods [verb]

        protected override CreateParams CreateParams {
            get {
                CreateParams p = base.CreateParams;
                if (!base.DesignMode) {
                    p.ExStyle = p.ExStyle | (WS_EX_NOACTIVATE);
                }
                return p;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Event handler

        // https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes?redirectedfrom=MSDN

        private void button0_Click(object sender, EventArgs e) {
            sendKey(0x30);
        }

        private void button1_Click(object sender, EventArgs e) {
            sendKey(0x31);
        }

        private void button2_Click(object sender, EventArgs e) {
            sendKey(0x32);
        }

        private void button3_Click(object sender, EventArgs e) {
            sendKey(0x33);
        }

        private void button4_Click(object sender, EventArgs e) {
            sendKey(0x34);
        }

        private void button5_Click(object sender, EventArgs e) {
            sendKey(0x35);
        }

        private void button6_Click(object sender, EventArgs e) {
            sendKey(0x36);
        }

        private void button7_Click(object sender, EventArgs e) {
            sendKey(0x37);
        }

        private void button8_Click(object sender, EventArgs e) {
            sendKey(0x38);
        }

        private void button9_Click(object sender, EventArgs e) {
            sendKey(0x39);
        }

        private void buttonDot_Click(object sender, EventArgs e) {
            sendKey(0xBE);
        }

        private void buttonEnter_Click(object sender, EventArgs e) {
            sendKey(0x0D);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        void sendKey(int key, bool isExtend = false) {
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

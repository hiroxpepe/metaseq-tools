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
using System.Threading.Tasks;
using System.Windows.Forms;

using PadLib;

namespace MetaseqPad {
    /// <summary>
    /// @author h.adachi
    /// </summary>
    public partial class FormMain : Form {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constants

        //#region Win32API Constants

        const int WS_EX_NOACTIVATE = 0x8000000;

        //#endregion

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
                if (!DesignMode) {
                    p.ExStyle = p.ExStyle | (WS_EX_NOACTIVATE);
                }
                return p;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Event handler

        async void tabControl_Selected(object sender, TabControlEventArgs e) {
            string tabName = ((TabControl) sender).SelectedTab.Name;
            if (tabName.Equals("tabPageModeling")) {
                await Task.Run(() => Utils.SendKeyWithCtrlAndShift(0x4D)); // Ctrl + Shift + M key
            } else if (tabName.Equals("tabPageUV")) {
                await Task.Run(() => Utils.SendKeyWithCtrlAndShift(0x50)); // Ctrl + Shift + P key
            }
        }

        async void button1_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x47)); // G key
        }

        async void button2_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x56)); // V key
        }

        async void button3_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x48)); // H key
        }

        async void button4_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x57)); // W key
        }

        async void button5_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x4B)); // K key
        }

        async void button6_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKeyWithAlt(0x57)); // Alt + W key
        }

        async void button7_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKeyWithAlt(0x4A)); // Alt + J key
        }

        async void button8_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKeyWithAlt(0x44)); // Alt + D key
        }

        async void button9_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKeyWithAlt(0x55)); // Alt + U key
        }

        async void button10_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKeyWithAlt(0x4D)); // Alt + M key
        }

        async void button11_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKeyWithAlt(0x41)); // Alt + A key
        }

        async void button12_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKeyWithAlt(0x46)); // Alt + F key
        }

        async void button13_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x30)); //  0 key
        }

        async void button14_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x31)); //  1 key
        }

        async void button15_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x32)); //  2 key
        }

        async void button16_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x33)); //  3 key
        }

        async void button17_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x38)); //  8 key
        }

        async void button18_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x34)); //  4 key
        }

        async void button19_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x39)); //  9 key
        }

        async void button20_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x35)); //  5 key
        }

        async void button21_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x36)); //  6 key
        }

        async void button22_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x37)); //  7 key
        }

        async void button23_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKeyWithAlt(0x30)); // Alt + 0 key
        }

        async void button24_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKeyWithAlt(0x31)); // Alt + 1 key
        }

        async void button25_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKeyWithCtrl(0x53)); // Ctrl + S key
        }

        async void button26_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKeyWithCtrl(0x44)); // Ctrl + D key
        }

        async void button27_Click(object sender, EventArgs e) {
            await Task.Run(() => {
                Utils.SendKeyWithCtrlAndShift(0x4D); // Ctrl + Shift + M key
                Utils.SendKeyWithCtrlAndShift(0x42); // Ctrl + Shift + B key
            });
            tabControl.SelectedIndex = 0;
        }

        async void button28_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKeyWithAlt(0x70)); // Alt + F1 key
        }

        async void button29_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKeyWithAlt(0x71)); // Alt + F2 key
        }

        async void button30_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKeyWithAlt(0x72)); // Alt + F3 key
        }

        async void button31_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKeyWithAlt(0x73)); // Alt + F4 key
        }

        async void button32_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKeyWithAlt(0x74)); // Alt + F5 key
        }

        async void button33_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKeyWithAlt(0x75)); // Alt + F6 key
        }

        async void button34_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKeyWithAlt(0x76)); // Alt + F7 key
        }
    }
}

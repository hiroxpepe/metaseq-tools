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

namespace NumPad {
    /// <summary>
    /// @author h.adachi
    /// </summary>
    public partial class FormMain : Form {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constants

        //#region Win32API Constants

        private const int WS_EX_NOACTIVATE = 0x8000000;

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
                CreateParams _p = base.CreateParams;
                if (!base.DesignMode) {
                    _p.ExStyle = _p.ExStyle | (WS_EX_NOACTIVATE);
                }
                return _p;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Event handler

        // https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes?redirectedfrom=MSDN

        private async void button0_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x30));
        }

        private async void button1_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x31));
        }

        private async void button2_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x32));
        }

        private async void button3_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x33));
        }

        private async void button4_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x34));
        }

        private async void button5_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x35));
        }

        private async void button6_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x36));
        }

        private async void button7_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x37));
        }

        private async void button8_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x38));
        }

        private async void button9_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x39));
        }

        private async void buttonDot_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0xBE));
        }

        private async void buttonEnter_Click(object sender, EventArgs e) {
            await Task.Run(() => Utils.SendKey(0x0D));
        }
    }
}

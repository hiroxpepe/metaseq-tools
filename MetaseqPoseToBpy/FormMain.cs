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

using System.Threading.Tasks;
using System.Windows.Forms;

using MetaseqPoseToBpyLib;

namespace MetaseqPoseToBpy {
    /// <summary>
    /// @author h.adachi
    /// </summary>
    public partial class FormMain : Form {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields

        Context context;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public FormMain() {
            context = new Context();

            InitializeComponent();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Event handler

        private async void buttonFileDrop_DragDrop(object sender, DragEventArgs e) {
            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy
                && e.Data.GetDataPresent(DataFormats.FileDrop, true)) {
                var _data = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                if (_data != null) {
                    foreach (string _filePath in _data) {
                        await Task.Run(() => context.Read(_filePath));
                    }
                    await Task.Run(() => context.Write());
                }
            }
        }

        private void buttonFileDrop_DragEnter(object sender, DragEventArgs e) {
            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy
                 && e.Data.GetDataPresent(DataFormats.FileDrop)) { // FIXME: バリデーター
                e.Effect = DragDropEffects.Copy;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }

        private void buttonFileDrop_DragOver(object sender, DragEventArgs e) {
            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy
                && e.Data.GetDataPresent(DataFormats.FileDrop)) { // FIXME: バリデーター
                e.Effect = DragDropEffects.Copy;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }
    }
}

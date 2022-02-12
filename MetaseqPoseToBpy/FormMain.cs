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
    /// The main form class.
    /// </summary>
    /// <author>Hiroyuki Adachi</author>
    public partial class FormMain : Form {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields

        /// <summary>
        /// The context object to processing the xml files.
        /// </summary>
        Context _context;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public FormMain() {
            _context = new();

            InitializeComponent();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Event handler

        /// <summary>
        /// An event handler where files are dragged and dropped.
        /// </summary>
        /// <param name="sender">The event publisher is provided.</param>
        /// <param name="e">The event parameters are provided.</param>
        private async void buttonFileDrop_DragDrop(object sender, DragEventArgs e) {
            if ((e.AllowedEffect & DragDropEffects.Copy) is DragDropEffects.Copy
                && e.Data.GetDataPresent(DataFormats.FileDrop, true)) {
                var data = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                if (data is not null) {
                    foreach (string _filePath in data) {
                        await Task.Run(() => _context.Read(_filePath));
                    }
                    await Task.Run(() => _context.Write());
                }
            }
        }

        /// <summary>
        /// An event handler where files are dragged and entered.
        /// </summary>
        /// <param name="sender">The event publisher is provided.</param>
        /// <param name="e">The event parameters are provided.</param>
        private void buttonFileDrop_DragEnter(object sender, DragEventArgs e) {
            if ((e.AllowedEffect & DragDropEffects.Copy) is DragDropEffects.Copy
                 && e.Data.GetDataPresent(DataFormats.FileDrop)) { // FIXME: to add a validator.
                e.Effect = DragDropEffects.Copy;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// An event handler where files are dragged and overd.
        /// </summary>
        /// <param name="sender">The event publisher is provided.</param>
        /// <param name="e">The event parameters are provided.</param>
        private void buttonFileDrop_DragOver(object sender, DragEventArgs e) {
            if ((e.AllowedEffect & DragDropEffects.Copy) is DragDropEffects.Copy
                && e.Data.GetDataPresent(DataFormats.FileDrop)) { // FIXME: to add a validator.
                e.Effect = DragDropEffects.Copy;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }
    }
}

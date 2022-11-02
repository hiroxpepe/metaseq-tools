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

using System.Threading.Tasks;
using System.Windows.Forms;

using MqoPoseToBpy.Lib;

namespace MqoPoseToBpy {
    /// <summary>
    /// The main form class.
    /// </summary>
    /// <author>h.adachi (STUDIO MeowToon)</author>
    public partial class FormMain : Form {
#nullable enable

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
        async void buttonFileDrop_DragDrop(object sender, DragEventArgs e) {
            if ((e.AllowedEffect & DragDropEffects.Copy) is DragDropEffects.Copy && e.Data.GetDataPresent(format: DataFormats.FileDrop, autoConvert: true)) {
                string[] data = e.Data.GetData(format: DataFormats.FileDrop, autoConvert: true) as string[];
                if (data is not null) {
                    // old version
                    if (data.Length > 1) {
                        foreach (string file_path in data) {
                            await Task.Run(action: () => _context.Read(file_path: file_path));
                        }
                        await Task.Run(action: () => _context.Write());
                    // new version
                    } else if (data.Length == 1 &&
                        !string.IsNullOrEmpty(value: _textBoxTarget.Text) &&
                        !string.IsNullOrEmpty(value: _textBoxPrefix.Text) &&
                        !string.IsNullOrEmpty(value: _textBoxCutNo.Text)) {
                        string file_path = data[0];
                        await Task.Run(action: () => _context.ReadOne(file_path: file_path));
                        await Task.Run(action: () => _context.WriteOne(
                            target: _textBoxTarget.Text,
                            prefix: _textBoxPrefix.Text, 
                            cut_num: int.Parse(s: _textBoxCutNo.Text), 
                            file_path: file_path
                        ));
                    } else {
                        MessageBox.Show(text: "Requires parameters.", caption: "Error", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Stop);
                    }
                }
            }
        }

        /// <summary>
        /// An event handler where files are dragged and entered.
        /// </summary>
        /// <param name="sender">The event publisher is provided.</param>
        /// <param name="e">The event parameters are provided.</param>
        void buttonFileDrop_DragEnter(object sender, DragEventArgs e) {
            if ((e.AllowedEffect & DragDropEffects.Copy) is DragDropEffects.Copy && e.Data.GetDataPresent(format: DataFormats.FileDrop)) { // FIXME: to add a validator.
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
        void buttonFileDrop_DragOver(object sender, DragEventArgs e) {
            if ((e.AllowedEffect & DragDropEffects.Copy) is DragDropEffects.Copy && e.Data.GetDataPresent(format: DataFormats.FileDrop)) { // FIXME: to add a validator.
                e.Effect = DragDropEffects.Copy;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }
    }
}

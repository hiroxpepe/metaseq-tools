﻿/*
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

using InvertPose.Lib;

namespace InvertPose {
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
                    await Task.Run(action: () => _context.Read(file_path: data[0]));
                    await Task.Run(action: () => _context.Write(file_path: data[0]));
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

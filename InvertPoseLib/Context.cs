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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace InvertPoseLib {
    /// <summary>
    /// The context object for converting the pose XML files.
    /// </summary>
    /// <author>Hiroyuki Adachi</author>
    public class Context {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Context() {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb]

        /// <summary>
        /// Read: TBA
        /// </summary>
        /// <param name="filePath">A pose XML file of Metasequoia 4 is provided.</param>
        public void Read(string filePath) {
            var serializer = new XmlSerializer(typeof(PoseSet));
            var settings = new XmlReaderSettings() { CheckCharacters = false, };
            using var streamReader = new StreamReader(filePath, Encoding.UTF8);
            using var xmlReader = XmlReader.Create(streamReader, settings);
            var poseSet = (PoseSet) serializer.Deserialize(xmlReader);
        }

        /// <summary>
        /// Write: TBA
        /// </summary>
        public void Write() {
            string buff = "skeleton\n";
            File.WriteAllText($"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/skeleton.xml", buff);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes
    }
}

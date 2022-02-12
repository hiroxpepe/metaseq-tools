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

#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields

        PoseSet _poseSet;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Context() {
            _poseSet = new();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb]

        /// <summary>
        /// Read the pose XML file of Metasequoia 4.
        /// </summary>
        /// <param name="filePath">A pose XML file of Metasequoia 4 is provided.</param>
        public void Read(string filePath) {
            var serializer = new XmlSerializer(typeof(PoseSet));
            var settings = new XmlReaderSettings() { CheckCharacters = false, };
            using var streamReader = new StreamReader(filePath, Encoding.UTF8);
            using var xmlReader = XmlReader.Create(streamReader, settings);
            var poseSet = serializer.Deserialize(xmlReader) as PoseSet;
            if (poseSet is not null) {
                _poseSet = poseSet;
            }
        }

        /// <summary>
        /// Write the inverted pose XML file of Metasequoia 4.
        /// </summary>
        /// <param name="filePath">A pose XML file of Metasequoia 4 is provided.</param>
        public void Write(string filePath) {
            // create an inverted XML file path.
            var directoryName = Path.GetDirectoryName(filePath);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            var extension = Path.GetExtension(filePath);
            var path = $"{directoryName}\\{fileNameWithoutExtension}_invert{extension}";
            // convert the object to an XML string.
            var serializer = new XmlSerializer(typeof(PoseSet));
            using var stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, _poseSet);
            var xml = stringWriter.ToString();
            // To imitate Metasequoia 4 output.
            xml = xml.Replace("utf-16", "UTF-8");
            xml = xml.Replace("  ", "    ");
            xml = xml.Replace(" /", "/");
            xml = $"{xml}\r\n";
            File.WriteAllText(path, xml);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes
    }
}

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

namespace StudioMeowToon.PoseSetCore {
    /// <summary>
    /// @author h.adachi
    /// </summary>
    public class Context {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields

        const string xmlFile = @".\data\pose-set.xml";

        List<KeyFrame> keyFrameList;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb]

        public void Read() {
            var _serializer = new XmlSerializer(typeof(PoseSet));
            PoseSet _result;
            var _settings = new XmlReaderSettings() {
                CheckCharacters = false,
            };
            using (var _streamReader = new StreamReader(xmlFile, Encoding.UTF8)) {
                using (var _xmlReader = XmlReader.Create(_streamReader, _settings)) {
                    _result = (PoseSet) _serializer.Deserialize(_xmlReader);
                }
            }
        }

        public void Exec() {
            Read();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes

        class KeyFrame {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, adjectives]

            public RateAndPosition RateAndPosition { get; set; }
            public PoseSet PoseSet { get; set; }
        }

        class RateAndPosition {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, adjectives]

            public int Rate { get; set; }
            public int Position { get; set; }
        }

    }
}

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

        List<KeyFrame> keyFrameList;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public Context() {
            keyFrameList = new List<KeyFrame>();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb]

        public void Read(string filePath) {
            var _serializer = new XmlSerializer(typeof(PoseSet));
            PoseSet _poseSet;
            RateAndPosition _rateAndPosition = new RateAndPosition(filePath);
            var _settings = new XmlReaderSettings() { CheckCharacters = false, };
            using (var _streamReader = new StreamReader(filePath, Encoding.UTF8)) {
                using (var _xmlReader = XmlReader.Create(_streamReader, _settings)) {
                    _poseSet = (PoseSet) _serializer.Deserialize(_xmlReader);
                }
            }
            KeyFrame _keyFrame = new KeyFrame(_rateAndPosition, _poseSet);
            keyFrameList.Add(_keyFrame);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes

        class KeyFrame {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Constructor

            public KeyFrame(RateAndPosition rateAndPosition, PoseSet poseSet) {
                RateAndPosition = rateAndPosition;
                PoseSet = poseSet;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, adjectives]

            public RateAndPosition RateAndPosition { get; }
            public PoseSet PoseSet { get; }
        }

        class RateAndPosition {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Constructor

            public RateAndPosition(string filePath) {
                string _fileName = filePath.Split('\\').Last();
                Rate = int.Parse(_fileName.Split('_')[1]);
                Position = int.Parse(_fileName.Split('_')[2].Replace(".xml", ""));
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, adjectives]

            public int Rate { get; }
            public int Position { get; }
        }

    }
}

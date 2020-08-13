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
            var _settings = new XmlReaderSettings() { CheckCharacters = false, };
            using (var _streamReader = new StreamReader(filePath, Encoding.UTF8)) {
                using (var _xmlReader = XmlReader.Create(_streamReader, _settings)) {
                    keyFrameList.Add(
                        new KeyFrame(
                            new RateAndPosition(filePath),
                            (PoseSet) _serializer.Deserialize(_xmlReader)
                        )
                    );
                }
            }
        }

        public void Write() {
            string _buff = "import bpy\n\n";
            keyFrameList.ForEach(_keyFrame => {
                _keyFrame.PoseSet.Pose.ToList().ForEach(_pose => {
                    _buff += $"ob = bpy.context.active_object.pose.bones['{_pose.name}']\n";
                    _buff += $"ob.rotation_mode = 'XYZ'\n";
                    _buff += $"ob.rotation_euler.x = {_pose.rotB}\n";
                    _buff += $"ob.rotation_euler.y = {_pose.rotH}\n";
                    _buff += $"ob.rotation_euler.z = {_pose.rotP}\n";
                    _buff += $"ob.keyframe_insert('rotation_euler', frame = {_keyFrame.RateAndPosition.Position})\n\n";
                });
            });
            File.WriteAllText($"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/metaseq_animation.py", _buff);
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

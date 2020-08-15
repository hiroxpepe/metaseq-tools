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

namespace MetaseqPoseToBpyLib {
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
            int _fps = 0, _frame_end = 0;
            keyFrameList.ForEach(_keyFrame => {
                _keyFrame.PoseSet.Pose.ToList().ForEach(_pose => {
                    var _euler = getRotationEuler(_pose);
                    if (_euler != null) {
                        _buff += $"ob = bpy.context.active_object.pose.bones['{_pose.name}']\n";
                        _buff += $"ob.rotation_mode = '{_euler.Mode}'\n";
                        _buff += $"ob.rotation_euler.x = {_euler.X}\n";
                        _buff += $"ob.rotation_euler.y = {_euler.Y}\n";
                        _buff += $"ob.rotation_euler.z = {_euler.Z}\n";
                        _buff += $"ob.keyframe_insert('rotation_euler', frame = {_keyFrame.RateAndPosition.Position}, group = '{_pose.name}')\n\n";
                    }
                });
                _fps = _keyFrame.RateAndPosition.Rate;
                _frame_end = _keyFrame.RateAndPosition.Position;
            });
            _buff += $"bpy.context.scene.render.fps = {_fps}\n";
            _buff += $"bpy.data.scenes['Scene'].frame_end = {_frame_end}\n";
            File.WriteAllText($"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/metaseq_animation.py", _buff);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        RotationEuler getRotationEuler(PoseSetPose pose) {
            RotationEuler _euler = new RotationEuler();
            string _name = pose.name;
            if (_name.Equals("Hips") || _name.Equals("Spine") ||
                _name.Equals("UpperChest") || _name.Equals("Neck") ||
                _name.Equals("Head") || _name.Equals("Head_end")) {
                _euler.X = toRadian(Decimal.ToDouble(pose.rotP));
                _euler.Y = toRadian(Decimal.ToDouble(pose.rotH));
                _euler.Z = toRadian(Decimal.ToDouble(pose.rotB));
                _euler.Mode = "ZXY";
                return _euler;
            } else if (_name.Equals("LeftUpperLeg") || _name.Equals("RightUpperLeg")) {
                _euler.X = -toRadian(Decimal.ToDouble(pose.rotP));
                _euler.Y = -toRadian(Decimal.ToDouble(pose.rotH));
                _euler.Z = toRadian(Decimal.ToDouble(pose.rotB));
                _euler.Mode = "ZXY";
                return _euler;
            } else if (_name.Equals("LeftLowerLeg") || _name.Equals("LeftFoot") ||
                _name.Equals("LeftToeBase") || _name.Equals("LeftToeEnd") ||
                _name.Equals("RightLowerLeg") || _name.Equals("RightFoot") ||
                _name.Equals("RightToeBase") || _name.Equals("RightToeEnd")) {
                _euler.X = toRadian(Decimal.ToDouble(pose.rotP));
                _euler.Y = -toRadian(Decimal.ToDouble(pose.rotH));
                _euler.Z = -toRadian(Decimal.ToDouble(pose.rotB));
                _euler.Mode = "ZXY";
                return _euler;
            } else if (_name.Equals("LeftShoulder") || _name.Equals("LeftUpperArm") ||
                _name.Equals("LeftLowerArm") || _name.Equals("LeftHand") ||
                _name.Equals("LeftThumbProximal") || _name.Equals("LeftIndexProximal") ||
                _name.Equals("LeftMiddleProximal") || _name.Equals("LeftRingProximal") || 
                _name.Equals("LeftLittleProximal")) {
                _euler.X = -toRadian(Decimal.ToDouble(pose.rotH));
                _euler.Y = toRadian(Decimal.ToDouble(pose.rotP));
                _euler.Z = toRadian(Decimal.ToDouble(pose.rotB));
                _euler.Mode = "ZYX";
                return _euler;
            } else if (_name.Equals("RightShoulder") || _name.Equals("RightUpperArm") ||
                _name.Equals("RightLowerArm") || _name.Equals("RightHand") ||
                _name.Equals("RightThumbProximal") || _name.Equals("RightIndexProximal") ||
                _name.Equals("RightMiddleProximal") || _name.Equals("RightRingProximal") || 
                _name.Equals("RightLittleProximal")) {
                _euler.X = toRadian(Decimal.ToDouble(pose.rotH));
                _euler.Y = -toRadian(Decimal.ToDouble(pose.rotP));
                _euler.Z = toRadian(Decimal.ToDouble(pose.rotB));
                _euler.Mode = "ZYX";
                return _euler;
            }
            return null;
        }

        double toRadian(double angle) {
            return (double) (angle * Math.PI / 180); // Blender のスクリプトはラジアンで設定
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes

        class RotationEuler {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, adjectives]

            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
            public string Mode { get; set; }
        }

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

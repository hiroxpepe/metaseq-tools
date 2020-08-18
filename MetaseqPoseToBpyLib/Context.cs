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
                            new RateAndLocation(filePath),
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
                        _buff += $"ob.keyframe_insert('rotation_euler', frame = {_keyFrame.RateAndLocation.Location}, group = '{_pose.name}')\n\n";
                    }
                });
                _fps = _keyFrame.RateAndLocation.Rate;
                _frame_end = _keyFrame.RateAndLocation.Location;
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
            if (getPattern(_name) == 1) {
                _euler.X = toRadian(Decimal.ToDouble(pose.rotP));
                _euler.Y = toRadian(Decimal.ToDouble(pose.rotH));
                _euler.Z = toRadian(Decimal.ToDouble(pose.rotB));
                _euler.Mode = "ZXY";
                return _euler;
            } else if (getPattern(_name) == 2) {
                _euler.X = -toRadian(Decimal.ToDouble(pose.rotP));
                _euler.Y = -toRadian(Decimal.ToDouble(pose.rotH));
                _euler.Z = toRadian(Decimal.ToDouble(pose.rotB));
                _euler.Mode = "ZXY";
                return _euler;
            } else if (getPattern(_name) == 3) {
                _euler.X = toRadian(Decimal.ToDouble(pose.rotP));
                _euler.Y = -toRadian(Decimal.ToDouble(pose.rotH));
                _euler.Z = -toRadian(Decimal.ToDouble(pose.rotB));
                _euler.Mode = "ZXY";
                return _euler;
            } else if (getPattern(_name) == 4) {
                _euler.X = -toRadian(Decimal.ToDouble(pose.rotH));
                _euler.Y = toRadian(Decimal.ToDouble(pose.rotP));
                _euler.Z = toRadian(Decimal.ToDouble(pose.rotB));
                _euler.Mode = "ZYX";
                return _euler;
            } else if (getPattern(_name) == 5) {
                _euler.X = toRadian(Decimal.ToDouble(pose.rotH));
                _euler.Y = -toRadian(Decimal.ToDouble(pose.rotP));
                _euler.Z = toRadian(Decimal.ToDouble(pose.rotB));
                _euler.Mode = "ZYX";
                return _euler;
            }
            return null;
        }

        int getPattern(string name) {
            if (name.Equals("Hips") || name.Equals("Spine") ||
                name.Equals("UpperChest") || name.Equals("Neck") ||
                name.Equals("Head") || name.Equals("Head_end")) {
                return 1;
            }
            else if (name.Equals("LeftUpperLeg") || name.Equals("RightUpperLeg")) {
                return 2;
            }
            else if (name.Equals("LeftLowerLeg") || name.Equals("LeftFoot") ||
                name.Equals("LeftToeBase") || name.Equals("LeftToeEnd") ||
                name.Equals("RightLowerLeg") || name.Equals("RightFoot") ||
                name.Equals("RightToeBase") || name.Equals("RightToeEnd")) {
                return 3;
            }
            else if (name.Equals("LeftShoulder") || name.Equals("LeftUpperArm") ||
                name.Equals("LeftLowerArm") || name.Equals("LeftHand") ||
                name.Equals("LeftThumbProximal") || name.Equals("LeftIndexProximal") ||
                name.Equals("LeftMiddleProximal") || name.Equals("LeftRingProximal") ||
                name.Equals("LeftLittleProximal")) {
                return 4;
            }
            else if (name.Equals("RightShoulder") || name.Equals("RightUpperArm") ||
                name.Equals("RightLowerArm") || name.Equals("RightHand") ||
                name.Equals("RightThumbProximal") || name.Equals("RightIndexProximal") ||
                name.Equals("RightMiddleProximal") || name.Equals("RightRingProximal") ||
                name.Equals("RightLittleProximal")) {
                return 5;
            }
            return 0;
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

            public KeyFrame(RateAndLocation rateAndLocation, PoseSet poseSet) {
                RateAndLocation = rateAndLocation;
                PoseSet = poseSet;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, adjectives]

            public RateAndLocation RateAndLocation { get; }
            public PoseSet PoseSet { get; }
        }

        class RateAndLocation {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Constructor

            public RateAndLocation(string filePath) {
                string _fileName = filePath.Split('\\').Last();
                Rate = int.Parse(_fileName.Split('_')[1]);
                Location = int.Parse(_fileName.Split('_')[2].Replace(".xml", ""));
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, adjectives]

            public int Rate { get; }
            public int Location { get; }
        }

    }
}

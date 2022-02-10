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

        List<KeyFrame> _keyFrameList;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        public Context() {
            _keyFrameList = new List<KeyFrame>();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb]

        public void Read(string filePath) {
            var serializer = new XmlSerializer(typeof(PoseSet));
            var settings = new XmlReaderSettings() { CheckCharacters = false, };
            using (var streamReader = new StreamReader(filePath, Encoding.UTF8)) {
                using (var xmlReader = XmlReader.Create(streamReader, settings)) {
                    _keyFrameList.Add(
                        new KeyFrame(
                            new RateAndLocation(filePath),
                            (PoseSet) serializer.Deserialize(xmlReader)
                        )
                    );
                }
            }
        }

        public void Write() {
            string buff = "import bpy\n\n";
            int fps = 0, frame_end = 0;
            _keyFrameList.ForEach(keyFrame => {
                keyFrame.PoseSet.Pose.ToList().ForEach(pose => {
                    var euler = getRotationEuler(pose);
                    if (euler != null) {
                        buff += $"ob = bpy.context.active_object.pose.bones['{pose.name}']\n";
                        buff += $"ob.rotation_mode = '{euler.Mode}'\n";
                        buff += $"ob.rotation_euler.x = {euler.X}\n";
                        buff += $"ob.rotation_euler.y = {euler.Y}\n";
                        buff += $"ob.rotation_euler.z = {euler.Z}\n";
                        buff += $"ob.keyframe_insert('rotation_euler', frame = {keyFrame.RateAndLocation.Location}, group = '{pose.name}')\n\n";
                    }
                    //var _location = getLocation(_pose);
                    //if (_location != null) {
                    //    _buff += $"ob = bpy.context.active_object.pose.bones['{_pose.name}']\n";
                    //    _buff += $"ob.location.x = {_location.X}\n";
                    //    _buff += $"ob.location.y = {_location.Y}\n";
                    //    _buff += $"ob.location.z = {_location.Z}\n";
                    //    _buff += $"ob.keyframe_insert('location', frame = {_keyFrame.RateAndLocation.Location}, group = '{_pose.name}')\n\n";
                    //}
                    // ob.location.y -= 1
                });
                fps = keyFrame.RateAndLocation.Rate;
                frame_end = keyFrame.RateAndLocation.Location;
            });
            buff += $"bpy.context.scene.render.fps = {fps}\n";
            buff += $"bpy.data.scenes['Scene'].frame_end = {frame_end}\n";
            File.WriteAllText($"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/metaseq_animation.py", buff);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        RotationEuler getRotationEuler(PoseSetPose pose) {
            RotationEuler euler = new RotationEuler();
            if (getPattern(pose.name) == 1) {
                euler.X = toRadian(decimal.ToDouble(pose.rotP));
                euler.Y = toRadian(decimal.ToDouble(pose.rotH));
                euler.Z = toRadian(decimal.ToDouble(pose.rotB));
                euler.Mode = "ZXY";
                return euler;
            } else if (getPattern(pose.name) == 2) {
                euler.X = -toRadian(decimal.ToDouble(pose.rotP));
                euler.Y = -toRadian(decimal.ToDouble(pose.rotH));
                euler.Z = toRadian(decimal.ToDouble(pose.rotB));
                euler.Mode = "ZXY";
                return euler;
            } else if (getPattern(pose.name) == 3) {
                euler.X = toRadian(decimal.ToDouble(pose.rotP));
                euler.Y = -toRadian(decimal.ToDouble(pose.rotH));
                euler.Z = -toRadian(decimal.ToDouble(pose.rotB));
                euler.Mode = "ZXY";
                return euler;
            } else if (getPattern(pose.name) == 4) {
                euler.X = -toRadian(decimal.ToDouble(pose.rotH));
                euler.Y = toRadian(decimal.ToDouble(pose.rotP));
                euler.Z = toRadian(decimal.ToDouble(pose.rotB));
                euler.Mode = "ZYX";
                return euler;
            } else if (getPattern(pose.name) == 5) {
                euler.X = toRadian(decimal.ToDouble(pose.rotH));
                euler.Y = -toRadian(decimal.ToDouble(pose.rotP));
                euler.Z = toRadian(decimal.ToDouble(pose.rotB));
                euler.Mode = "ZYX";
                return euler;
            } else if (getPattern(pose.name) == 6) {
                euler.X = toRadian(decimal.ToDouble(pose.rotP));
                euler.Y = -toRadian(decimal.ToDouble(pose.rotH));
                euler.Z = toRadian(decimal.ToDouble(pose.rotB));
                euler.Mode = "ZXY";
                return euler;
            }
            return null;
        }

        int getPattern(string name) {
            if (name.Equals("Hips") || name.Equals("Spine") ||
                name.Equals("UpperChest") || name.Equals("Neck") ||
                name.Equals("Head") || name.Equals("Head_end") || name.Equals("Hair")) {
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
            } else if (name.Equals("LeftBustBase") || name.Equals("RightBustBase")) {
                return 6;
            }
            return 0;
        }

        // FIXME:
        Location getLocation(PoseSetPose pose) {
            Location location = new Location();
            location.X = decimal.ToDouble(pose.mvX);
            location.Y = decimal.ToDouble(pose.mvY);
            location.Z = decimal.ToDouble(pose.mvZ);
            return location;
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

        // FIXME:
        class Location {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, adjectives]

            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
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
                string fileName = filePath.Split('\\').Last();
                Rate = int.Parse(fileName.Split('_')[1]);
                Location = int.Parse(fileName.Split('_')[2].Replace(".xml", ""));
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, adjectives]

            public int Rate { get; }
            public int Location { get; }
        }

    }
}

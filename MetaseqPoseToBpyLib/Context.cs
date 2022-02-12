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
    /// The context object for exchanging the pose XML files of Metasequoia 4 to Blender's Python script.
    /// </summary>
    /// <author>Hiroyuki Adachi</author>
    public class Context {

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields

        /// <summary>
        /// The list object of KeyFrame objects.
        /// </summary>
        List<KeyFrame> _keyFrameList;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Context() {
            _keyFrameList = new();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb]

        /// <summary>
        /// Read the pose XML file of Metasequoia 4 to the field list.
        /// </summary>
        /// <param name="filePath">A pose XML file of Metasequoia 4 is provided.</param>
        public void Read(string filePath) {
            var serializer = new XmlSerializer(typeof(PoseSet));
            var settings = new XmlReaderSettings() { CheckCharacters = false, };
            using var streamReader = new StreamReader(filePath, Encoding.UTF8);
            using var xmlReader = XmlReader.Create(streamReader, settings);
            _keyFrameList.Add(new KeyFrame(
                new RateAndLocation(filePath),
                (PoseSet) serializer.Deserialize(xmlReader)
            ));
        }

        /// <summary>
        /// Write the Python script as a file for Blender.
        /// </summary>
        public void Write() {
            string buff = "import bpy\n\n";
            int fps = 0, frame_end = 0;
            _keyFrameList.ForEach(keyFrame => {
                keyFrame.PoseSet.Pose.ToList().ForEach(pose => {
                    var euler = getRotationEuler(pose);
                    if (euler is not null) {
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

        /// <summary>
        /// Get the RotationEuler object.
        /// </summary>
        /// <param name="pose">A PoseSetPose object is provided.</param>
        /// <returns>Return a RotationEuler object.</returns>
        RotationEuler getRotationEuler(PoseSetPose pose) {
            var euler = getPattern(pose.name) switch {
                1 => new RotationEuler(
                    toRadian(decimal.ToDouble(pose.rotP)),
                    toRadian(decimal.ToDouble(pose.rotH)),
                    toRadian(decimal.ToDouble(pose.rotB)),
                    "ZXY"
                ),
                2 => new RotationEuler(
                    -toRadian(decimal.ToDouble(pose.rotP)),
                    -toRadian(decimal.ToDouble(pose.rotH)),
                    toRadian(decimal.ToDouble(pose.rotB)),
                    "ZXY"
                ),
                3 => new RotationEuler(
                    toRadian(decimal.ToDouble(pose.rotP)),
                    -toRadian(decimal.ToDouble(pose.rotH)),
                    -toRadian(decimal.ToDouble(pose.rotB)),
                    "ZXY"
                ),
                4 => new RotationEuler(
                    -toRadian(decimal.ToDouble(pose.rotH)),
                    toRadian(decimal.ToDouble(pose.rotP)),
                    toRadian(decimal.ToDouble(pose.rotB)),
                    "ZYX"
                ),
                5 => new RotationEuler(
                    toRadian(decimal.ToDouble(pose.rotH)),
                    -toRadian(decimal.ToDouble(pose.rotP)),
                    toRadian(decimal.ToDouble(pose.rotB)),
                    "ZYX"
                ),
                6 => new RotationEuler(
                    toRadian(decimal.ToDouble(pose.rotP)),
                    -toRadian(decimal.ToDouble(pose.rotH)),
                    toRadian(decimal.ToDouble(pose.rotB)),
                    "ZXY"
                ),
                0 or _ => null
            };
            return euler;
        }

        /// <summary>
        /// Get the pattern number.
        /// </summary>
        /// <param name="name">A name of the bone of Metasequoia 4 is provided.</param>
        /// <returns>Return a pattern number.</returns>
        int getPattern(string name) {
            var pattern = name switch {
                "Hips" or "Spine" or "UpperChest" or "Neck" or "Head" or "Head_end" or "Hair"
                    => 1,
                "LeftUpperLeg" or "RightUpperLeg"
                    => 2,
                "LeftLowerLeg" or "LeftFoot" or "LeftToeBase" or "LeftToeEnd" or
                "RightLowerLeg" or "RightFoot" or "RightToeBase" or "RightToeEnd"
                    => 3,
                "LeftShoulder" or "LeftUpperArm" or "LeftLowerArm" or "LeftHand" or "LeftThumbProximal" or
                "LeftIndexProximal" or "LeftMiddleProximal" or "LeftRingProximal" or "LeftLittleProximal"
                    => 4,
                "RightShoulder" or "RightUpperArm" or "RightLowerArm" or "RightHand" or "RightThumbProximal" or
                "RightIndexProximal" or "RightMiddleProximal" or "RightRingProximal" or "RightLittleProximal"
                    => 5,
                "LeftBustBase" or "RightBustBase"
                    => 6,
                _
                    => 0
            };
            return pattern;
        }

        // FIXME:
        //Location getLocation(PoseSetPose pose) {
        //    var location = new Location();
        //    location.X = decimal.ToDouble(pose.mvX);
        //    location.Y = decimal.ToDouble(pose.mvY);
        //    location.Z = decimal.ToDouble(pose.mvZ);
        //    return location;
        //}

        /// <summary>
        /// Get the radian value of the angle.
        /// </summary>
        /// <param name="angle">An angle is provided.</param>
        /// <returns>Return a radian value of the angle.</returns>
        double toRadian(double angle) {
            return (double) (angle * Math.PI / 180); // Blender script angles are set in radian.
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes

        /// <summary>
        /// A value object that holds rotation axis and mode string.
        /// </summary>
        /// <param name="X">An x-axis parameter of the pose is provided.</param>
        /// <param name="Y">A y-axis parameter of the pose is provided.</param>
        /// <param name="Z">A z-axis parameter of the pose is provided.</param>
        /// <param name="Mode">A Blender `rotation_mode` string is provided.</param>
        record RotationEuler(double X, double Y, double Z, string Mode);

        // FIXME:
        //record Location {

        //    ///////////////////////////////////////////////////////////////////////////////////////////
        //    // Properties [noun, adjectives]

        //    public double X { get; set; }
        //    public double Y { get; set; }
        //    public double Z { get; set; }
        //}

        /// <summary>
        /// A value object that holds the data to make Blender script.
        /// </summary>
        /// <param name="RateAndLocation">A RateAndLocation object is provided.</param>
        /// <param name="PoseSet">A PoseSet object is provided.</param>
        record KeyFrame(RateAndLocation RateAndLocation, PoseSet PoseSet);

        /// <summary>
        /// A value object that holds the data from the file name.
        /// </summary>
        record RateAndLocation {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Constructor

            /// <summary>
            /// Default constructor.
            /// Convert the file name to parameters.
            /// </summary>
            /// <param name="filePath">A pose XML file of Metasequoia 4 is provided.</param>
            public RateAndLocation(string filePath) {
                string fileName = filePath.Split('\\').Last();
                Rate = int.Parse(fileName.Split('_')[1]);
                Location = int.Parse(fileName.Split('_')[2].Replace(".xml", ""));
            }

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, adjectives]

            /// <summary>
            /// The value of "bpy.context.scene.render.fps" for Blender script.
            /// </summary>
            public int Rate { get; }

            /// <summary>
            /// The value of "bpy.data.scenes['Scene'].frame_end" for Blender script.
            /// </summary>
            public int Location { get; }
        }
    }
}

/*
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MqoPoseToBpy.Lib {
    /// <summary>
    /// The context object for exchanging the pose XML files of Metasequoia 4 to Blender's Python script.
    /// </summary>
    /// <author>h.adachi (STUDIO MeowToon)</author>
    public class Context {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields

        /// <summary>
        /// The list object of KeyFrame objects.
        /// </summary>
        List<KeyFrame> _keyframe_list;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Context() {
            _keyframe_list = new();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb]

        /// <summary>
        /// Read the pose XML file of Metasequoia 4 to the field list.
        /// </summary>
        /// <param name="file_path">A pose XML file of Metasequoia 4 is provided.</param>
        public void Read(string file_path) {
            XmlSerializer serializer = new(type: typeof(PoseSet));
            XmlReaderSettings settings = new() { CheckCharacters = false, };
            using StreamReader stream_reader = new(path: file_path, encoding: Encoding.UTF8);
            using var xml_reader = XmlReader.Create(input: stream_reader, settings: settings);
            _keyframe_list.Add(new KeyFrame(
                RateAndLocation: new RateAndLocation(file_path),
                PoseSet: (PoseSet) serializer.Deserialize(xml_reader)
            ));
        }

        /// <summary>
        /// Read the pose XML file of Metasequoia 4 to the field list.
        /// </summary>
        /// <param name="file_path">A pose XML file of Metasequoia 4 is provided.</param>
        public void ReadOne(string file_path) {
            XmlSerializer serializer = new(type: typeof(PoseSet));
            XmlReaderSettings settings = new() { CheckCharacters = false, };
            using StreamReader stream_reader = new(path: file_path, encoding: Encoding.UTF8);
            using var xmlReader = XmlReader.Create(input: stream_reader, settings: settings);
            _keyframe_list.Add(new KeyFrame(
                RateAndLocation: null, 
                PoseSet: (PoseSet) serializer.Deserialize(xmlReader))
            );
        }

        /// <summary>
        /// Write the Python script as a file for Blender.
        /// </summary>
        public void Write() {
            string buff = "import bpy\n\n";
            int fps = 0, frame_end = 0;
            _keyframe_list.ForEach(keyFrame => {
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
                    var position = getPosition(pose);
                    if (position is not null) {
                        buff += $"ob = bpy.context.active_object.pose.bones['{pose.name}']\n";
                        buff += $"ob.location.x = {position.X}\n";
                        buff += $"ob.location.y = {position.Y}\n";
                        buff += $"ob.location.z = {position.Z}\n";
                        buff += $"ob.keyframe_insert('location', frame = {keyFrame.RateAndLocation.Location}, group = '{pose.name}')\n\n";
                    }
                });
                fps = keyFrame.RateAndLocation.Rate;
                frame_end = keyFrame.RateAndLocation.Location;
            });
            buff += $"bpy.context.scene.render.fps = {fps}\n";
            buff += $"bpy.data.scenes['Scene'].frame_end = {frame_end}\n";
            File.WriteAllText($"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/metaseq_animation.py", buff);
            _keyframe_list.Clear();
        }

        /// <summary>
        /// Write the Python script as a file for Blender.
        /// </summary>
        /// <param name="target">A object name of Blender is provided.</param>
        /// <param name="prefix">A prefix string to add for Blender bone is provided.</param>
        /// <param name="cut_num">A cut number to set for my Blender addon is provided.</param>
        /// <param name="file_path">A file path string to write a file is provided.</param>
        public void WriteOne(string target, string prefix, int cut_num, string file_path) {
            // create a file path to output.
            string directory_name = Path.GetDirectoryName(file_path);
            string file_name_without_extension = Path.GetFileNameWithoutExtension(file_path);
            string path = $"{directory_name}\\{prefix}_{cut_num}_pose.py";
            // create a bpy script for Blender.
            string space = "    ";
            string buff = "import bpy\n\n";
            buff += $"def keyframe_insert(frame: int) -> None:\n";
            buff += $"{space}ob = bpy.data.objects[\"{target}\"]\n";
            buff += $"{space}bpy.context.view_layer.objects.active = ob\n\n";
            KeyFrame keyframe = _keyframe_list.First();
            keyframe.PoseSet.Pose.ToList().ForEach(pose => {
                var euler = getRotationEuler(pose);
                if (euler is not null) {
                    buff += $"{space}ob = bpy.context.active_object.pose.bones[\"{prefix}_{pose.name}\"]\n";
                    buff += $"{space}ob.rotation_mode = \"{euler.Mode}\"\n";
                    buff += $"{space}ob.rotation_euler.x = {euler.X}\n";
                    buff += $"{space}ob.rotation_euler.y = {euler.Y}\n";
                    buff += $"{space}ob.rotation_euler.z = {euler.Z}\n";
                    buff += $"{space}ob.keyframe_insert(\"rotation_euler\", frame=frame, group=\"{prefix}_{pose.name}\")\n\n";
                }
                var position = getPosition(pose);
                if (position is not null) {
                    buff += $"{space}ob = bpy.context.active_object.pose.bones[\"{prefix}_{pose.name}\"]\n";
                    buff += $"{space}ob.location.x = {position.X}\n";
                    buff += $"{space}ob.location.y = {position.Y}\n";
                    buff += $"{space}ob.location.z = {position.Z}\n";
                    buff += $"{space}ob.keyframe_insert(\"location\", frame=frame, group=\"{prefix}_{pose.name}\")\n\n";
                }
            });
            // write a bpy script.
            File.WriteAllText(path, buff);
            _keyframe_list.Clear();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        /// <summary>
        /// Get the RotationEuler object.
        /// </summary>
        /// <param name="pose">A PoseSetPose object is provided.</param>
        /// <returns>Return a RotationEuler object.</returns>
        RotationEuler getRotationEuler(PoseSetPose pose) {
            RotationEuler euler = getPattern(pose.name) switch {
                1 => new(
                    X: toRadian(decimal.ToDouble(pose.rotP)),
                    Y: toRadian(decimal.ToDouble(pose.rotH)),
                    Z: toRadian(decimal.ToDouble(pose.rotB)),
                    Mode: "ZXY"
                ),
                2 => new(
                    X: -toRadian(decimal.ToDouble(pose.rotP)),
                    Y: -toRadian(decimal.ToDouble(pose.rotH)),
                    Z: toRadian(decimal.ToDouble(pose.rotB)),
                    Mode: "ZXY"
                ),
                3 => new(
                    X: toRadian(decimal.ToDouble(pose.rotP)),
                    Y: -toRadian(decimal.ToDouble(pose.rotH)),
                    Z: -toRadian(decimal.ToDouble(pose.rotB)),
                    Mode: "ZXY"
                ),
                4 => new(
                    X: -toRadian(decimal.ToDouble(pose.rotH)),
                    Y: toRadian(decimal.ToDouble(pose.rotP)),
                    Z: toRadian(decimal.ToDouble(pose.rotB)),
                    Mode: "ZYX"
                ),
                5 => new(
                    X: toRadian(decimal.ToDouble(pose.rotH)),
                    Y: -toRadian(decimal.ToDouble(pose.rotP)),
                    Z: toRadian(decimal.ToDouble(pose.rotB)),
                    Mode: "ZYX"
                ),
                6 => new(
                    X: toRadian(decimal.ToDouble(pose.rotP)),
                    Y: -toRadian(decimal.ToDouble(pose.rotH)),
                    Z: toRadian(decimal.ToDouble(pose.rotB)),
                    Mode: "ZXY"
                ),
                7 => new(
                    X: toRadian(decimal.ToDouble(pose.rotP)),
                    Y: -toRadian(decimal.ToDouble(pose.rotB)),
                    Z: toRadian(decimal.ToDouble(pose.rotH)),
                    Mode: "ZXY"
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
            int pattern = name switch {
                "Hips" or "Spine" or "Chest" or "UpperChest" or "Neck" or "Head" or "Head_end" or 
                "Hair" or "HatBase" or "HatMid" or "HatTop"
                    => 1,
                "LeftUpperLeg" or "RightUpperLeg"
                    => 2,
                "LeftLowerLeg" or "LeftFoot" or "LeftToeBase" or "LeftToeEnd" or
                "RightLowerLeg" or "RightFoot" or "RightToeBase" or "RightToeEnd"
                    => 3,
                "LeftShoulder" or "LeftUpperArm" or "LeftLowerArm" or "LeftHand" or 
                "LeftThumbProximal" or "LeftThumbIntermediate" or "LeftThumbDistal" or
                "LeftIndexProximal" or "LeftIndexIntermediate" or "LeftIndexDistal" or
                "LeftMiddleProximal" or "LeftMiddleIntermediate" or "LeftMiddleDistal" or
                "LeftRingProximal" or "LeftRingIntermediate" or "LeftRingDistal" or
                "LeftLittleProximal" or "LeftLittleIntermediate" or "LeftLittleDistal"
                    => 4,
                "RightShoulder" or "RightUpperArm" or "RightLowerArm" or "RightHand" or 
                "RightThumbProximal" or "RightThumbIntermediate" or "RightThumbDistal" or
                "RightIndexProximal" or "RightIndexIntermediate" or "RightIndexDistal" or
                "RightMiddleProximal" or "RightMiddleIntermediate" or "RightMiddleDistal" or
                "RightRingProximal" or "RightRingIntermediate" or "RightRingDistal" or
                "RightLittleProximal" or "RightLittleIntermediate" or "RightLittleDistal"
                    => 5,
                "LeftBustBase" or "RightBustBase"
                    => 6,
                "TailBase" or "TailCenter" or "TailMid" or "TailTop"
                    => 7, 
                _
                    => 0
            };
            return pattern;
        }

        /// <summary>
        /// Get the Position object.
        /// </summary>
        /// <param name="pose">A PoseSetPose object is provided.</param>
        /// <returns>Return a Position object.</returns>
        Position getPosition(PoseSetPose pose) {
            if (pose.name is "Hips") {
                Position position = new();
                position.X = decimal.ToDouble(pose.mvX);
                position.Y = decimal.ToDouble(pose.mvY);
                position.Z = decimal.ToDouble(pose.mvZ);
                return position;
            }
            return null;
        }

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

        /// <summary>
        /// A value object that holds the position.
        /// </summary>
        record Position {

            ///////////////////////////////////////////////////////////////////////////////////////////
            // Properties [noun, adjectives]

            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
        }

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
            /// <param name="file_path">A pose XML file of Metasequoia 4 is provided.</param>
            public RateAndLocation(string file_path) {
                string file_name = file_path.Split('\\').Last();
                Rate = int.Parse(file_name.Split('_')[1]);
                Location = int.Parse(file_name.Split('_')[2].Replace(".xml", ""));
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

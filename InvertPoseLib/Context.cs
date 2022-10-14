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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace InvertPose.Lib {
    /// <summary>
    /// The context object for converting the pose XML files.
    /// </summary>
    /// <author>h.adachi (STUDIO MeowToon)</author>
    public class Context {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields

        /// <summary>
        /// The list object that contains the original pose XML.
        /// </summary>
        PoseSet _original_poseset;

        /// <summary>
        /// The list object that is outputted as the inverted pose XML.
        /// </summary>
        PoseSet _inversed_poseset;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Context() {
            _original_poseset = new();
            _inversed_poseset = new();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb]

        /// <summary>
        /// Read the pose XML file of Metasequoia 4.
        /// </summary>
        /// <param name="file_path">A pose XML file of Metasequoia 4 is provided.</param>
        public void Read(string file_path) {
            XmlSerializer serializer = new(typeof(PoseSet));
            XmlReaderSettings settings = new() { CheckCharacters = false, };
            using StreamReader stream_reader1 = new(file_path, Encoding.UTF8);
            using StreamReader stream_reader2 = new(file_path, Encoding.UTF8);
            using XmlReader xml_reader1 = XmlReader.Create(stream_reader1, settings);
            using XmlReader xml_reader2 = XmlReader.Create(stream_reader2, settings);
            PoseSet poseset1 = serializer.Deserialize(xml_reader1) as PoseSet;
            PoseSet poseset2 = serializer.Deserialize(xml_reader2) as PoseSet;
            if (poseset1 is not null) {
                if (poseset2 is not null) {
                    _original_poseset = poseset1;
                    _inversed_poseset = poseset2;
                    invert();
                } else {
                    // TODO: ERROR LOG
                }
            } else {
                // TODO: ERROR LOG
            }
        }

        /// <summary>
        /// Write the inverted pose XML file of Metasequoia 4.
        /// </summary>
        /// <param name="file_path">A pose XML file of Metasequoia 4 is provided.</param>
        public void Write(string file_path) {
            // create an inverted XML file path.
            string directory_name = Path.GetDirectoryName(file_path);
            string file_name_without_extension = Path.GetFileNameWithoutExtension(file_path);
            string extension = Path.GetExtension(file_path);
            string path = $"{directory_name}\\{file_name_without_extension}_invert{extension}";
            // convert the object to an XML string.
            XmlSerializer serializer = new(typeof(PoseSet));
            using StringWriter string_writer = new();
            serializer.Serialize(string_writer, _inversed_poseset);
            string xml = string_writer.ToString();
            // To imitate Metasequoia 4 output.
            xml = xml.Replace("utf-16", "UTF-8");
            xml = xml.Replace("  ", "    ");
            xml = xml.Replace(" /", "/");
            xml = $"{xml}\r\n";
            File.WriteAllText(path, xml);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        /// <summary>
        /// Invert all poses.
        /// </summary>
        void invert() {
            _original_poseset.Pose.ToList().ForEach(pose => {
                if (pose.name is "Hips" or "Spine" or "Chest" or "UpperChest" or "Neck" or "Head" or "Head_end" or 
                    "Hair" or "HatBase" or "HatMid" or "HatTop" or "SkirtBase" or "SkirtEnd" or
                    "TailBase" or "TailCenter" or "TailMid" or "TailTop") {
                    PoseSetPose new_pose = getNewPose();
                    new_pose.name = pose.name;
                    new_pose.rotP = pose.rotP;
                    new_pose.rotH = -(pose.rotH);
                    new_pose.rotB = -(pose.rotB);
                    if (pose.name is "Hips") {
                        new_pose.mvX = -pose.mvX;
                    }
                    applyInvertPose(new_pose);
                }
                if (pose.name is "LeftBustBase" or "RightBustBase" or
                    "RightUpperLeg" or "RightLowerLeg" or "LeftUpperLeg" or "LeftLowerLeg" or 
                    "LeftFoot" or "RightFoot" or "LeftToeBase" or "LeftToeEnd" or "RightToeBase" or "RightToeEnd" or
                    "LeftShoulder" or "LeftUpperArm" or "LeftLowerArm" or "LeftHand" or
                    "RightShoulder" or "RightUpperArm" or "RightLowerArm" or "RightHand" or
                    "LeftThumbProximal" or "LeftThumbIntermediate" or "LeftThumbDistal" or
                    "LeftIndexProximal" or "LeftIndexIntermediate" or "LeftIndexDistal" or
                    "LeftMiddleProximal" or "LeftMiddleIntermediate" or "LeftMiddleDistal" or
                    "LeftRingProximal" or "LeftRingIntermediate" or "LeftRingDistal" or
                    "LeftLittleProximal" or "LeftLittleIntermediate" or "LeftLittleDistal" or
                    "RightThumbProximal" or "RightThumbIntermediate" or "RightThumbDistal" or
                    "RightIndexProximal" or "RightIndexIntermediate" or "RightIndexDistal" or
                    "RightMiddleProximal" or "RightMiddleIntermediate" or "RightMiddleDistal" or
                    "RightRingProximal" or "RightRingIntermediate" or "RightRingDistal" or
                    "RightLittleProximal" or "RightLittleIntermediate" or "RightLittleDistal") {
                    PoseSetPose symmetric_pose = getSymmetricPose(pose.name);
                    PoseSetPose new_pose = getNewPose();
                    new_pose.name = pose.name;
                    new_pose.rotP = symmetric_pose.rotP;
                    new_pose.rotH = -(symmetric_pose.rotH);
                    new_pose.rotB = -(symmetric_pose.rotB);
                    applyInvertPose(new_pose);
                }
            });
        }

        /// <summary>
        /// Apply an inverted PoseSetPose object to the output list. 
        /// </summary>
        /// <param name="inverted_pose">An inverted PoseSetPose object is provided.</param>
        void applyInvertPose(PoseSetPose inverted_pose) {
            List<PoseSetPose> new_pose_list = _inversed_poseset.Pose.ToList().Where(pose => pose.name != inverted_pose.name).ToList();
            new_pose_list.Add(inverted_pose);
            _inversed_poseset.Pose = new_pose_list.ToArray();
        }

        /// <summary>
        /// Get the symmetric PoseSetPose object.
        /// </summary>
        /// <param name="name">A name of bone name is provided.</param>
        /// <returns>Return a symmetric PoseSetPose object.</returns>
        PoseSetPose getSymmetricPose(string name) {
            string search = flatten(name);
            if (search.Contains("left")) {
                search = search.Replace("left", "right");
            } 
            else if (search.Contains("right")) {
                search = search.Replace("right", "left");
            }
            PoseSetPose result = _original_poseset.Pose.ToList().Where(pose => flatten(pose.name).Equals(search)).First();
            return result;
        }

        /// <summary>
        /// Flatten the bone name.
        /// </summary>
        /// <param name="name">A bone name is provided.</param>
        /// <returns>Return the flatted bone name.</returns>
        string flatten(string name) {
            name = name.ToLower();
            name = name.Replace("_", "");
            return name;
        }

        /// <summary>
        /// Get a new PoseSetPose object.
        /// </summary>
        /// <returns>Return a new PoseSetPose object.</returns>
        PoseSetPose getNewPose() {
            PoseSetPose pose = new();
            pose.name = "";
            pose.mvX = 0.0000000m;
            pose.mvY = 0.0000000m;
            pose.mvZ = 0.0000000m;
            pose.rotB = 0.0000000m;
            pose.rotH = 0.0000000m;
            pose.rotP = 0.0000000m;
            pose.scX = 1.0000000m;
            pose.scY = 1.0000000m;
            pose.scZ = 1.0000000m;
            return pose;
        }
    }
}

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
using static System.IO.File;

namespace InvertPose.Lib {
    /// <summary>
    /// context object for converting the pose XML files.
    /// </summary>
    /// <author>h.adachi (STUDIO MeowToon)</author>
    public class Context {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields

        /// <summary>
        /// list object that contains the original pose XML.
        /// </summary>
        PoseSet _original_poseset;

        /// <summary>
        /// list object that is outputted as the inverted pose XML.
        /// </summary>
        PoseSet _inversed_poseset;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        /// <summary>
        /// default constructor.
        /// </summary>
        public Context() {
            _original_poseset = new();
            _inversed_poseset = new();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb]

        /// <summary>
        /// reads the pose XML file of Metasequoia 4.
        /// </summary>
        /// <param name="file_path">A pose XML file of Metasequoia 4 is provided.</param>
        public void Read(string file_path) {
            XmlSerializer serializer = new(type: typeof(PoseSet));
            XmlReaderSettings settings = new() { CheckCharacters = false, };
            using StreamReader stream_reader1 = new(file_path, Encoding.UTF8);
            using StreamReader stream_reader2 = new(file_path, Encoding.UTF8);
            using XmlReader xml_reader1 = XmlReader.Create(input: stream_reader1, settings: settings);
            using XmlReader xml_reader2 = XmlReader.Create(input: stream_reader2, settings: settings);
            PoseSet poseset1 = serializer.Deserialize(xmlReader: xml_reader1) as PoseSet;
            PoseSet poseset2 = serializer.Deserialize(xmlReader: xml_reader2) as PoseSet;
            if (poseset1 is not null && poseset2 is not null) {
                _original_poseset = poseset1;
                _inversed_poseset = poseset2;
                invert();
            } else {
                // TODO: ERROR LOG
            }
        }

        /// <summary>
        /// writes the inverted pose XML file of Metasequoia 4.
        /// </summary>
        /// <param name="file_path">A pose XML file of Metasequoia 4 is provided.</param>
        public void Write(string file_path) {
            // create an inverted XML file path.
            string directory_name = Path.GetDirectoryName(path: file_path);
            string file_name_without_extension = Path.GetFileNameWithoutExtension(path: file_path);
            string extension = Path.GetExtension(path: file_path);
            string path = $"{directory_name}\\{file_name_without_extension}_invert{extension}";
            // convert the object to an XML string.
            XmlSerializer serializer = new(type: typeof(PoseSet));
            using StringWriter string_writer = new();
            serializer.Serialize(textWriter: string_writer, o: _inversed_poseset);
            string xml = string_writer.ToString();
            // To imitate Metasequoia 4 output.
            xml = xml.Replace(oldValue: "utf-16", newValue: "UTF-8");
            xml = xml.Replace(oldValue: "  ", newValue: "    ");
            xml = xml.Replace(oldValue: " /", newValue: "/");
            xml = $"{xml}\r\n";
            WriteAllText(path: path, contents: xml);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        /// <summary>
        /// inverts all poses.
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
                    applyInvertPose(inverted_pose: new_pose);
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
                    PoseSetPose symmetric_pose = getSymmetricPose(name: pose.name);
                    PoseSetPose new_pose = getNewPose();
                    new_pose.name = pose.name;
                    new_pose.rotP = symmetric_pose.rotP;
                    new_pose.rotH = -(symmetric_pose.rotH);
                    new_pose.rotB = -(symmetric_pose.rotB);
                    applyInvertPose(inverted_pose: new_pose);
                }
            });
        }

        /// <summary>
        /// applies inverted PoseSetPose objects to the output list.
        /// </summary>
        /// <param name="inverted_pose">An inverted PoseSetPose object is provided.</param>
        void applyInvertPose(PoseSetPose inverted_pose) {
            List<PoseSetPose> new_pose_list = _inversed_poseset.Pose.ToList().Where(predicate: x => x.name != inverted_pose.name).ToList();
            new_pose_list.Add(item: inverted_pose);
            _inversed_poseset.Pose = new_pose_list.ToArray();
        }

        /// <summary>
        /// gets the symmetric PoseSetPose object.
        /// </summary>
        /// <param name="name">A name of bone name is provided.</param>
        /// <returns>Return a symmetric PoseSetPose object.</returns>
        PoseSetPose getSymmetricPose(string name) {
            string search_name = flatten(name);
            if (search_name.Contains(value: "left")) {
                search_name = search_name.Replace(oldValue: "left", newValue: "right");
            } 
            else if (search_name.Contains(value: "right")) {
                search_name = search_name.Replace(oldValue: "right", newValue: "left");
            }
            PoseSetPose result = _original_poseset.Pose.ToList().Where(predicate: x => flatten(x.name).Equals(search_name)).First();
            return result;
        }

        /// <summary>
        /// flattens the bone name.
        /// </summary>
        /// <param name="name">A bone name is provided.</param>
        /// <returns>Return the flatted bone name.</returns>
        string flatten(string name) {
            name = name.ToLower();
            name = name.Replace("_", "");
            return name;
        }

        /// <summary>
        /// gets a new PoseSetPose object.
        /// </summary>
        /// <returns>Return a new PoseSetPose object.</returns>
        PoseSetPose getNewPose() {
            return new() {
                name = string.Empty,
                mvX = 0.0000000m,
                mvY = 0.0000000m,
                mvZ = 0.0000000m,
                rotB = 0.0000000m,
                rotH = 0.0000000m,
                rotP = 0.0000000m,
                scX = 1.0000000m,
                scY = 1.0000000m,
                scZ = 1.0000000m
            };
        }
    }
}

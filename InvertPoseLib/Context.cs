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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace InvertPoseLib {
    /// <summary>
    /// The context object for converting the pose XML files.
    /// </summary>
    /// <author>Hiroyuki Adachi</author>
    public class Context {
#nullable enable

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Fields

        /// <summary>
        /// The list object that contains the original pose XML.
        /// </summary>
        PoseSet _originalPoseSet;

        /// <summary>
        /// The list object that is outputted as the inverted pose XML.
        /// </summary>
        PoseSet _inversedPoseSet;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Context() {
            _originalPoseSet = new();
            _inversedPoseSet = new();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // public Methods [verb]

        /// <summary>
        /// Read the pose XML file of Metasequoia 4.
        /// </summary>
        /// <param name="filePath">A pose XML file of Metasequoia 4 is provided.</param>
        public void Read(string filePath) {
            XmlSerializer serializer = new(typeof(PoseSet));
            XmlReaderSettings settings = new() { CheckCharacters = false, };
            using StreamReader streamReader1 = new(filePath, Encoding.UTF8);
            using StreamReader streamReader2 = new(filePath, Encoding.UTF8);
            using XmlReader xmlReader1 = XmlReader.Create(streamReader1, settings);
            using XmlReader xmlReader2 = XmlReader.Create(streamReader2, settings);
            PoseSet poseSet1 = serializer.Deserialize(xmlReader1) as PoseSet;
            PoseSet poseSet2 = serializer.Deserialize(xmlReader2) as PoseSet;
            if (poseSet1 is not null) {
                if (poseSet2 is not null) {
                    _originalPoseSet = poseSet1;
                    _inversedPoseSet = poseSet2;
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
        /// <param name="filePath">A pose XML file of Metasequoia 4 is provided.</param>
        public void Write(string filePath) {
            // create an inverted XML file path.
            string directoryName = Path.GetDirectoryName(filePath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            string path = $"{directoryName}\\{fileNameWithoutExtension}_invert{extension}";
            // convert the object to an XML string.
            XmlSerializer serializer = new(typeof(PoseSet));
            using StringWriter stringWriter = new();
            serializer.Serialize(stringWriter, _inversedPoseSet);
            string xml = stringWriter.ToString();
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
            _originalPoseSet.Pose.ToList().ForEach(pose => {
                if (pose.name is "Hips" or "Spine" or "Chest" or "UpperChest" or "Neck" or "Head" or "Head_end" or 
                    "Hair" or "HatBase" or "HatMid" or "HatTop" or "SkirtBase" or "SkirtEnd" or
                    "TailBase" or "TailCenter" or "TailMid" or "TailTop") {
                    PoseSetPose newPose = getNewPose();
                    newPose.name = pose.name;
                    newPose.rotP = pose.rotP;
                    newPose.rotH = -(pose.rotH);
                    newPose.rotB = -(pose.rotB);
                    applyInvertPose(newPose);
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
                    PoseSetPose symmetricPose = getSymmetricPose(pose.name);
                    PoseSetPose newPose = getNewPose();
                    newPose.name = pose.name;
                    newPose.rotP = symmetricPose.rotP;
                    newPose.rotH = -(symmetricPose.rotH);
                    newPose.rotB = -(symmetricPose.rotB);
                    applyInvertPose(newPose);
                }
            });
        }

        /// <summary>
        /// Apply an inverted PoseSetPose object to the output list. 
        /// </summary>
        /// <param name="invertedPose">An inverted PoseSetPose object is provided.</param>
        void applyInvertPose(PoseSetPose invertedPose) {
            List<PoseSetPose> newPoseList = _inversedPoseSet.Pose.ToList().Where(pose => pose.name != invertedPose.name).ToList();
            newPoseList.Add(invertedPose);
            _inversedPoseSet.Pose = newPoseList.ToArray();
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
            PoseSetPose result = _originalPoseSet.Pose.ToList().Where(pose => flatten(pose.name).Equals(search)).First();
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

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // inner Classes
    }
}

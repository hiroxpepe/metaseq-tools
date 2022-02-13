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
            var serializer = new XmlSerializer(typeof(PoseSet));
            var settings = new XmlReaderSettings() { CheckCharacters = false, };
            using var streamReader = new StreamReader(filePath, Encoding.UTF8);
            using var xmlReader = XmlReader.Create(streamReader, settings);
            var poseSet = serializer.Deserialize(xmlReader) as PoseSet;
            if (poseSet is not null) {
                _originalPoseSet = poseSet;
                _inversedPoseSet = poseSet;
                invert();
            }
        }

        /// <summary>
        /// Write the inverted pose XML file of Metasequoia 4.
        /// </summary>
        /// <param name="filePath">A pose XML file of Metasequoia 4 is provided.</param>
        public void Write(string filePath) {
            // create an inverted XML file path.
            var directoryName = Path.GetDirectoryName(filePath);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            var extension = Path.GetExtension(filePath);
            var path = $"{directoryName}\\{fileNameWithoutExtension}_invert{extension}";
            // convert the object to an XML string.
            var serializer = new XmlSerializer(typeof(PoseSet));
            using var stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, _inversedPoseSet);
            var xml = stringWriter.ToString();
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
                    "Hair" or "HatBase" or "HatMid" or "HatTop") {
                    var newPose = getNewPose();
                    newPose.name = pose.name;
                    newPose.rotP = pose.rotP; // pitch: x-axis
                    newPose.rotH = -(pose.rotH); // head : y-axis
                    newPose.rotB = -(pose.rotB); // bank : z-axis
                    applyInvertPose(newPose);
                }
                if (pose.name is "LeftBustBase" or "RightBustBase") {
                    var symmetricPose = getSymmetricPose(pose.name);
                    var newPose = getNewPose();
                    newPose.name = pose.name;
                    newPose.rotP = symmetricPose.rotP; // pitch: x-axis
                    newPose.rotH = -(symmetricPose.rotH); // head : y-axis
                    newPose.rotB = -(symmetricPose.rotB); // bank : z-axis
                    applyInvertPose(newPose);
                }
            });
        }

        /// <summary>
        /// Apply an inverted PoseSetPose object to the output list. 
        /// </summary>
        /// <param name="invertedPose">An inverted PoseSetPose object is provided.</param>
        void applyInvertPose(PoseSetPose invertedPose) {
            var newPoseList = _inversedPoseSet.Pose.ToList().Where(pose => pose.name != invertedPose.name).ToList();
            newPoseList.Add(invertedPose);
            _inversedPoseSet.Pose = newPoseList.ToArray();
        }

        /// <summary>
        /// Get the symmetric PoseSetPose object.
        /// </summary>
        /// <param name="name">A name of bone name is provided.</param>
        /// <returns>Return a symmetric PoseSetPose object.</returns>
        PoseSetPose getSymmetricPose(string name) {
            var search = flatten(name);
            if (search.Contains("left")) {
                search = search.Replace("left", "right");
            } 
            else if (search.Contains("right")) {
                search = search.Replace("right", "left");
            }
            var result = _originalPoseSet.Pose.ToList().Where(pose => flatten(pose.name).Equals(search)).First();
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
            var pose = new PoseSetPose();
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

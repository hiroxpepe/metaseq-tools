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

        const string ROTATION_MODE = "ZXY"; // 軸の回転の順番:おそらくメタセコの仕様

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

                _keyFrame.PoseSet.Pose.ToList().Where(_pose =>
                    _pose.name.Equals("Hips") || _pose.name.Equals("Spine") ||
                    _pose.name.Equals("UpperChest") || _pose.name.Equals("Neck") ||
                    _pose.name.Equals("Head") || _pose.name.Equals("Head_end"))
                    .ToList().ForEach(_pose => {
                        _buff += $"ob = bpy.context.active_object.pose.bones['{_pose.name}']\n";
                        _buff += $"ob.rotation_mode = '{ROTATION_MODE}'\n";
                        _buff += $"ob.rotation_euler.x = {toRadian(Decimal.ToDouble(_pose.rotP))}\n";
                        _buff += $"ob.rotation_euler.y = {toRadian(Decimal.ToDouble(_pose.rotH))}\n";
                        _buff += $"ob.rotation_euler.z = {toRadian(Decimal.ToDouble(_pose.rotB))}\n";
                        _buff += $"ob.keyframe_insert('rotation_euler', frame = {_keyFrame.RateAndPosition.Position})\n\n";
                    });

                _keyFrame.PoseSet.Pose.ToList().Where(_pose =>
                    _pose.name.Equals("LeftUpperLeg") || _pose.name.Equals("RightUpperLeg"))
                    .ToList().ForEach(_pose => {
                        _buff += $"ob = bpy.context.active_object.pose.bones['{_pose.name}']\n";
                        _buff += $"ob.rotation_mode = '{ROTATION_MODE}'\n";
                        _buff += $"ob.rotation_euler.x = {-toRadian(Decimal.ToDouble(_pose.rotP))}\n";
                        _buff += $"ob.rotation_euler.y = {-toRadian(Decimal.ToDouble(_pose.rotH))}\n";
                        _buff += $"ob.rotation_euler.z = {toRadian(Decimal.ToDouble(_pose.rotB))}\n";
                        _buff += $"ob.keyframe_insert('rotation_euler', frame = {_keyFrame.RateAndPosition.Position})\n\n";
                    });

                _keyFrame.PoseSet.Pose.ToList().Where(_pose =>
                    _pose.name.Equals("LeftLowerLeg") || _pose.name.Equals("LeftFoot") ||
                    _pose.name.Equals("LeftToeBase") || _pose.name.Equals("LeftToeEnd") ||
                    _pose.name.Equals("RightLowerLeg") || _pose.name.Equals("RightFoot") ||
                    _pose.name.Equals("RightToeBase") || _pose.name.Equals("RightToeEnd"))
                    .ToList().ForEach(_pose => {
                        _buff += $"ob = bpy.context.active_object.pose.bones['{_pose.name}']\n";
                        _buff += $"ob.rotation_mode = '{ROTATION_MODE}'\n";
                        _buff += $"ob.rotation_euler.x = {toRadian(Decimal.ToDouble(_pose.rotP))}\n";
                        _buff += $"ob.rotation_euler.y = {-toRadian(Decimal.ToDouble(_pose.rotH))}\n";
                        _buff += $"ob.rotation_euler.z = {-toRadian(Decimal.ToDouble(_pose.rotB))}\n";
                        _buff += $"ob.keyframe_insert('rotation_euler', frame = {_keyFrame.RateAndPosition.Position})\n\n";
                    });

                _keyFrame.PoseSet.Pose.ToList().Where(_pose =>
                    _pose.name.Equals("LeftShoulder") || _pose.name.Equals("LeftUpperArm") ||
                    _pose.name.Equals("LeftLowerArm") || _pose.name.Equals("LeftHand") ||
                    _pose.name.Equals("LeftThumbProximal") || _pose.name.Equals("LeftIndexProximal") ||
                    _pose.name.Equals("LeftMiddleProximal") || _pose.name.Equals("LeftRingProximal") || _pose.name.Equals("LeftLittleProximal"))
                    .ToList().ForEach(_pose => {
                        _buff += $"ob = bpy.context.active_object.pose.bones['{_pose.name}']\n";
                        _buff += $"ob.rotation_mode = '{ROTATION_MODE}'\n";
                        _buff += $"ob.rotation_euler.x = {-toRadian(Decimal.ToDouble(_pose.rotH))}\n";
                        _buff += $"ob.rotation_euler.y = {toRadian(Decimal.ToDouble(_pose.rotP))}\n";
                        _buff += $"ob.rotation_euler.z = {toRadian(Decimal.ToDouble(_pose.rotB))}\n";
                        _buff += $"ob.keyframe_insert('rotation_euler', frame = {_keyFrame.RateAndPosition.Position})\n\n";
                    });

                _keyFrame.PoseSet.Pose.ToList().Where(_pose =>
                    _pose.name.Equals("RightShoulder") || _pose.name.Equals("RightUpperArm") ||
                    _pose.name.Equals("RightLowerArm") || _pose.name.Equals("RightHand") ||
                    _pose.name.Equals("RightThumbProximal") || _pose.name.Equals("RightIndexProximal") ||
                    _pose.name.Equals("RightMiddleProximal") || _pose.name.Equals("RightRingProximal") || _pose.name.Equals("RightLittleProximal"))
                    .ToList().ForEach(_pose => {
                        _buff += $"ob = bpy.context.active_object.pose.bones['{_pose.name}']\n";
                        _buff += $"ob.rotation_mode = '{ROTATION_MODE}'\n";
                        _buff += $"ob.rotation_euler.x = {toRadian(Decimal.ToDouble(_pose.rotH))}\n";
                        _buff += $"ob.rotation_euler.y = {-toRadian(Decimal.ToDouble(_pose.rotP))}\n";
                        _buff += $"ob.rotation_euler.z = {toRadian(Decimal.ToDouble(_pose.rotB))}\n";
                        _buff += $"ob.keyframe_insert('rotation_euler', frame = {_keyFrame.RateAndPosition.Position})\n\n";
                    });

            });
            File.WriteAllText($"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/metaseq_animation.py", _buff);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        // private Methods [verb]

        double toRadian(double angle) {
            return (double) (angle * Math.PI / 180); // Blender のスクリプトはラジアンで設定
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

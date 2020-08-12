using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace PoseSetCore {
    public class Context {
        // シリアライズ先のファイル
        const string xmlFile = @".\data\pose-set.xml";

        public void Exec() {
            // デシリアライズする
            var _serializer = new XmlSerializer(typeof(PoseSet));
            PoseSet _result;
            var _settings = new XmlReaderSettings() {
                CheckCharacters = false,
            };
            using (var _streamReader = new StreamReader(xmlFile, Encoding.UTF8)) {
                using (var _xmlReader
                        = XmlReader.Create(_streamReader, _settings)) {
                    _result = (PoseSet) _serializer.Deserialize(_xmlReader);
                }
            }
            //Console.WriteLine($"{_result.Pose.First().name}, {_result.Pose.First().mvX}");
        }
    }
}

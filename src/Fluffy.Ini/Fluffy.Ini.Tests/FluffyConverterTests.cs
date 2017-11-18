using Fluffy.Ini.Tests.TestObjects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fluffy.Ini.Tests
{
    [TestFixture]
    public class FluffyConverterTests
    {
        [Test]
        public void SerializeOneLevelObject()
        {
            OneLevelObject oneLevel = new OneLevelObject
            {
                OneSettings = 69,
                OtherSettings = 123
            };
            string ini = FluffyConverter.SerializeObject(oneLevel).Trim().Replace("\r\n", "\n");
            string original = File.ReadAllText(BuildIniFilePath("SerializeOneLevelObject.ini")).Trim().Replace("\r\n", "\n");

            Assert.AreEqual(original, ini);
        }

        [Test]
        public void DeserializeOneLevelObject()
        {
            OneLevelObject oneLevel = new OneLevelObject
            {
                OneSettings = 69,
                OtherSettings = 123
            };

            string ini = FluffyConverter.SerializeObject(oneLevel).Trim().Replace("\r\n", "\n");
            OneLevelObject deserialized = FluffyConverter.DeserializeObject<OneLevelObject>(ini);

            Assert.AreEqual(oneLevel, deserialized);
        }

        [Test]
        public void SerializeTwoLevelObject()
        {
            TwoLevelObject testObj = new TwoLevelObject();
            testObj.OneSection.OneSettings = 10;
            testObj.OneSection.OtherSettings = 20;
            testObj.OtherSection.OneSettings = 30;
            testObj.OtherSection.OtherSettings = 40;

            string ini = FluffyConverter.SerializeObject(testObj).Trim().Replace("\r\n", "\n");
            string original = File.ReadAllText(BuildIniFilePath("SerializeTwoLevelObject.ini")).Trim().Replace("\r\n", "\n");
            Assert.AreEqual(original, ini);
        }

        [Test]
        public void DeserializeTwoLevelObject()
        {
            TwoLevelObject testObj = new TwoLevelObject();
            testObj.OneSection.OneSettings = 10;
            testObj.OneSection.OtherSettings = 20;
            testObj.OtherSection.OneSettings = 30;
            testObj.OtherSection.OtherSettings = 40;

            string ini = FluffyConverter.SerializeObject(testObj).Trim();
            TwoLevelObject deserialized = FluffyConverter.DeserializeObject<TwoLevelObject>(ini);
            Assert.AreEqual(testObj, deserialized);
        }

        [Test]
        public void SerializeComment()
        {
            string ini = FluffyConverter.SerializeObject(new { Settings = new CommentObject { Value = 99 } }).Trim().Replace("\r\n", "\n");
            string original = File.ReadAllText(BuildIniFilePath("SerializeComment.ini")).Trim().Replace("\r\n", "\n");

            Assert.AreEqual(original, ini);
        }

        [Test]
        public void SerializeFluffyIgnore()
        {
            string ini = FluffyConverter.SerializeObject(new { Settings = new FluffyIgnoreObject { Meta = "Some metadata", Value = 99 } }).Trim().Replace("\r\n", "\n");
            string original = File.ReadAllText(BuildIniFilePath("SerializeFluffyIgnore.ini")).Trim().Replace("\r\n", "\n");

            Assert.AreEqual(original, ini);
        }


        [Test]
        public void DeserializeFluffyIgnore()
        {
            string original = File.ReadAllText(BuildIniFilePath("DeserializeFluffyIgnore.ini")).Trim().Replace("\r\n", "\n");
            RootObject root = FluffyConverter.DeserializeObject<RootObject>(original);

            Assert.AreEqual(root.Settings.Value, 99);
            Assert.IsNull(root.Settings.Meta);
        }

        private string BuildIniFilePath(string file)
        {
            return $"{AppDomain.CurrentDomain.BaseDirectory}\\ResultFiles\\{file}";
        }
    }
}

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
            Assert.True(true);
        }

        [Test]
        public void SerializeTwoLevelObject()
        {
            TwoLevelObject testObj = new TwoLevelObject();
            testObj.OneSection.OneSettings = 10;
            testObj.OneSection.OtherSettings = 20;
            testObj.OtherSection.OneSettings = 30;
            testObj.OtherSection.OtherSettings = 40;

            string ini = FluffyConverter.SerializeObject(testObj).Trim();
            string original = File.ReadAllText(BuildIniFilePath("SerializeTwoLevelObject.ini")).Trim();
            Assert.AreEqual(original, ini);
        }

        private string BuildIniFilePath(string file)
        {
            return $"{AppDomain.CurrentDomain.BaseDirectory}\\ResultFiles\\{file}";
        }
    }
}

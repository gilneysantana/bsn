using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using bsn.core.utils;

namespace bsn.testes
{
    [TestClass]
    public class UtilTest
    {
        [TestMethod]
        public void ToCSV_123()
        {
            string x = Utils.ToCSV(1,2,3);
            Assert.AreEqual(@"""1"",""2"",""3""", x);
        }

        [TestMethod]
        public void ToCSV_1x3()
        {
            string x = Utils.ToCSV(1,"x",3);
            Assert.AreEqual(@"""1"",""x"",""3""", x);
        }

        [TestMethod]
        public void ToCSV_ExemploWikipedia()
        {
            string orig = Utils.ToCSV(1997,"Ford","E350","Super, \"luxurious\" truck");
            string dest = "\"1997\",\"Ford\",\"E350\",\"Super\\, \"\"luxurious\"\" truck\"";
            Assert.AreEqual(dest, orig);
        }

        [TestMethod]
        public void FromCSV_ToCSV_Roundtrip()
        {
            var arrOrigem = new string[] { "aaa", "bbb", "\"ccc\",\"ddd\"" };
            string csv = Utils.ToCSV(arrOrigem);
            var arrDestino = Utils.FromCSV(csv);

            Assert.IsTrue(arrOrigem.SequenceEqual(arrDestino));
        }
    }
}

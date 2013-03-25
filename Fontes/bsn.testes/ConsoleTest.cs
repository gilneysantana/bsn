using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using bsn.console;

namespace bsn.testes
{
    [TestClass]
    public class ConsoleTest
    {
        [TestMethod]
        public void TratarArgumentos()
        {
            var x = Program.TratarArgumentos(new string[] {"cmd", "-v","-regex","%bla%","-debug"});

            Assert.IsTrue(x.Contains(new KeyValuePair<string,string>("-regex","%bla%")));
            Assert.IsTrue(x.Contains(new KeyValuePair<string, string>("-v", null)));
            Assert.IsTrue(x.Contains(new KeyValuePair<string, string>("-debug", null)));
        }
    }
}

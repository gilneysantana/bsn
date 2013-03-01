using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using bsn.dal.sqlite;
using bsn.core;

namespace bsn.testes
{
    [TestClass]
    public class SqlSiteTest
    {
        [TestMethod]
        public void GetSitePorNomeInfonet()
        {
            var site = Site.GetSitePorNome("Infonet");

            Assert.AreEqual("Infonet", site.Nome);
        }
    }
}

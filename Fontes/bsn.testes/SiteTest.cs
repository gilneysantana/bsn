using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using bsn.core;
using bsn.core.busca;
using bsn.dal.sqlite;


namespace bsn.testes
{
    [TestClass]
    public class SiteTest
    {

        //private static string infonet;

        //private static int INFONET_ACTIVE_ANNOUCEMENT = 163648;

        [TestInitialize]
        public void setUp()
        {
            //infonet = "Infonet";
        }

        public static void tearDown()
        {
        }

        [TestMethod]
        public void GetSiteInfonetMongoInfonet()
        {
            var site = Site.GetSitePorNome("Infonet");
            Assert.AreEqual("Infonet", site.Nome);
        }

    }
}

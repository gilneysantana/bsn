using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using bsn.core;
using MongoDB.Bson;

namespace bsn.testes
{
    [TestClass]
    public class SiteTest
    {

        private static Site infonet;

        private static int INFONET_ACTIVE_ANNOUCEMENT = 163648;

        [TestInitialize]
        public void setUp()
        {
            infonet = Site.GetSitePorNome("Infonet");
        }

        public static void tearDown()
        {
        }

        [TestMethod]
        public void ExpiredAnnouncementIsNotValidTest()
        {
            Pagina page = Pagina.CarregarDoArquivo("infonet.com.br-expirado-id=150000.htm");

            Assert.IsFalse(infonet.isValidPage(page));
        }

        [TestMethod]
        public void ActiveAnnoucementIsValidTest()
        {
            string paginaValidaInfonet = infonet.getAnnouncementURL(INFONET_ACTIVE_ANNOUCEMENT);
            Pagina page = new RecuperadorPagina().retrieve(paginaValidaInfonet);
            Assert.IsTrue(infonet.isValidPage(page));
        }

        [TestMethod]
        public void ActiveAnnoucementIsAnnoucementTest()
        {
            string paginaValidaInfonet = infonet.getAnnouncementURL(INFONET_ACTIVE_ANNOUCEMENT);
            Pagina page = new RecuperadorPagina().retrieve(paginaValidaInfonet);
            Assert.IsTrue(infonet.isAnnouncement(page));
        }

        [TestMethod]
        public void GetSiteInfonetMongoInfonet()
        {
            var site = Site.GetSitePorNome("Infonet");
            Assert.AreEqual("Infonet", site.Nome);
        }

    }
}

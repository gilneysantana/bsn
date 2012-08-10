using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using bsn.core;

namespace bsn.testes
{
    /*
     * This test must be improved. It test with the infonet, but should test with controlled data.
     */
    [TestClass]
    public class BuscadorTest
    {

        /*
         * Do not change this because the test is using Infonet. It is necessary to
         * change the test to use controlled data.
         */

        private static Buscador realEstateRetriever;

        [TestInitialize]
        public void setUp()
        {
            realEstateRetriever = new Buscador(Site.GetSitePorNome("Infonet"));
        }

        [TestCleanup]
        public void tearDown()
        {
            realEstateRetriever = null;
        }

        [TestMethod]
        public void nextRealEstateTest()
        {
            Anuncio realEstate = realEstateRetriever.ProximoAnuncio();
            Assert.IsNotNull(realEstate);
            Assert.IsNotNull(realEstate.getArea());
            Assert.IsNotNull(realEstate.Bairro);
            Assert.IsNotNull(realEstate.getNumberOfRooms());
            Assert.IsNotNull(realEstate.Preco);
        }

        //[TestMethod]
        //public void numberOfPagesNotFoundEqualsZero()
        //{
        //    Buscador realEstateRetriever = new Buscador(SiteClassificado.GetSitePorNome("Infonet"));

        //    Assert.IsNull(realEstateRetriever.ProximoAnuncio());
        //}


        [TestMethod]
        public void LoopProximoAnuncio()
        {
            Buscador realEstateRetriever = new Buscador(Site.GetSitePorNome("Infonet"));

            for (int i = 0; i < 2; i++)
            {
                System.Console.WriteLine(realEstateRetriever.ProximoAnuncio());
            }

            Assert.IsTrue(true);
        }



    }
}

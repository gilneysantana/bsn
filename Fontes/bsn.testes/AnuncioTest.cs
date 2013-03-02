using Microsoft.VisualStudio.TestTools.UnitTesting;
using bsn.core.analise;
using System;

namespace bsn.testes
{

    [TestClass]
    public class AnuncioTest
    {

        //private static Anuncio realEstate;

        //[TestInitialize]
        //public void setUp()
        //{
        //    realEstate = new Anuncio();
        //}

        //[TestCleanup]
        //public void tearDown()
        //{
        //    realEstate = null;
        //}

        [TestMethod]
        public void SqliteFind()
        {
            var anuncio = Anuncio.SqliteFind("Infonet", 248534);

            Assert.AreEqual(0, anuncio.NumeroQuartos);
            Assert.AreEqual(232, anuncio.Area);
            Assert.AreEqual(350000, anuncio.Preco);
            Assert.AreEqual("Centro", anuncio.Bairro);
            Assert.AreEqual(TipoImovel.Casa, anuncio.TipoImovel);
            Assert.AreEqual(TipoTransacao.Venda, anuncio.TipoTransacao);
        }


    }
}

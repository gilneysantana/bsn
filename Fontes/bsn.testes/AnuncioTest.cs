using Microsoft.VisualStudio.TestTools.UnitTesting;
using bsn.core.analise;
using System;

using bsn.core;

namespace bsn.testes
{

    [TestClass]
    public class AnuncioTest
    {

        [TestMethod]
        public void SqliteFind()
        {
            var anuncio = Anuncio.SqliteFind("Infonet", 248534);

            Assert.AreEqual(0, anuncio.NumeroQuartos);
            Assert.AreEqual(232, anuncio.Area);
            Assert.AreEqual(350000, anuncio.Preco);
            Assert.AreEqual("Centro", anuncio.Bairro);
            Assert.AreEqual(TipoImovel.CS, anuncio.TipoImovel);
            Assert.AreEqual(TipoTransacao.VD, anuncio.TipoTransacao);
        }

        [TestMethod]
        public void EqualsTrue()
        {
            var site1 = new Site("X");
            var site2 = new Site("X");

            var anuncio1 = new Anuncio(new Alvo(site1, 1));
            var anuncio2 = new Anuncio(new Alvo(site2, 1));

            Assert.IsTrue(anuncio1.Equals(anuncio2));
        }

        [TestMethod]
        public void EqualsFalse()
        {
            var site1 = new Site("X");
            var site2 = new Site("Y");

            var anuncio1 = new Anuncio(new Alvo(site1, 1));
            var anuncio2 = new Anuncio(new Alvo(site2, 1));

            Assert.IsFalse(anuncio1.Equals(anuncio2));
        }

        [TestMethod]
        public void ToCSV()
        {
            var alvo = new Alvo(new Site("adsf"), 1);
            var anuncio = new Anuncio(alvo);
            anuncio.Area = 123;
            anuncio.Preco = 456;
            anuncio.Bairro = "bairro";
            anuncio.NumeroQuartos = 4;

            string strCSV = anuncio.ToCSV();

            Assert.AreEqual("\"456\",\"123\",\"bairro\",\"adsf\",\"1\",\"NI\",\"NI\",\"4\"",
                strCSV);
        }

        [TestMethod]
        public void FromCSV_ToCSV_Roundtrip()
        {
            var alvoOrigem = new Alvo(new Site("asdf"), 1);

            var anuncioOrigem = new Anuncio(alvoOrigem);
            anuncioOrigem.Area = 111;
            anuncioOrigem.Preco = 222;
            anuncioOrigem.Bairro = "zzz";
            anuncioOrigem.TipoImovel = TipoImovel.AP;
            anuncioOrigem.TipoTransacao = TipoTransacao.AL;
            anuncioOrigem.NumeroQuartos = 4;

            var anuncioDestino = Anuncio.FromCSV(anuncioOrigem.ToCSV());

            Assert.AreEqual(anuncioOrigem.Area, anuncioDestino.Area);
            Assert.AreEqual(anuncioOrigem.Preco, anuncioDestino.Preco);
            Assert.AreEqual(anuncioOrigem.Bairro, anuncioDestino.Bairro);
            Assert.AreEqual(anuncioOrigem.TipoImovel, anuncioDestino.TipoImovel);
            Assert.AreEqual(anuncioOrigem.TipoTransacao, anuncioDestino.TipoTransacao);
            Assert.AreEqual(anuncioOrigem.NumeroQuartos, anuncioDestino.NumeroQuartos);

            Assert.IsTrue(anuncioOrigem.Equals(anuncioDestino));
        }

    }
}

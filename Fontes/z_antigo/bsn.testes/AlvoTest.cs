using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using bsn.core;
using bsn.core.busca;
using bsn.core.analise;
using bsn.dal.sqlite;

namespace bsn.testes
{
    [TestClass]
    public class AlvoTest
    {
        //private SQLiteDatabase sqlite = null;
        private Site siteInfonet = null;

        [TestInitialize]
        public void TestInicitalize()
        {
            this.siteInfonet = Site.GetSitePorNome("Infonet"); 
        }

        [TestMethod]
        public void ParseToCSV_Roundtrip()
        {
            var alvoOrigem = new Alvo(siteInfonet, 1);
            alvoOrigem.HistoricoStatus = "teste";
            alvoOrigem.UltimaVisita = new DateTime(2013, 2, 1, 1, 1, 1);
            alvoOrigem.DuracaoVisita = new TimeSpan(1, 2, 3, 123);
            alvoOrigem.RetornoRequisicao = "codigo html da página";
            alvoOrigem.LinkVisitado = "http://teste.com.br";
            alvoOrigem.UltimaExcecao = "OutOfBeerException";

            alvoOrigem.Anuncio = new Anuncio(alvoOrigem);
            alvoOrigem.Anuncio.Area = 111;
            alvoOrigem.Anuncio.Bairro = "asdfqwer";
            alvoOrigem.Anuncio.Preco = 222;
            alvoOrigem.Anuncio.TipoImovel = TipoImovel.AP;
            alvoOrigem.Anuncio.TipoTransacao = TipoTransacao.AL;

            string alvoCSV = alvoOrigem.ToCSV();
            var alvoDestino = Alvo.FromCSV(alvoCSV);

            Assert.AreEqual(alvoOrigem.UltimaVisita, alvoDestino.UltimaVisita);
            Assert.AreEqual(alvoOrigem.HistoricoStatus, alvoDestino.HistoricoStatus);
            Assert.AreEqual(alvoOrigem.UltimaVisita, alvoDestino.UltimaVisita);
            Assert.AreEqual(alvoOrigem.DuracaoVisita, alvoDestino.DuracaoVisita);
            Assert.AreEqual(alvoOrigem.RetornoRequisicao, alvoDestino.RetornoRequisicao);
            Assert.AreEqual(alvoOrigem.LinkVisitado, alvoDestino.LinkVisitado);
            Assert.AreEqual(alvoOrigem.UltimaExcecao, alvoDestino.UltimaExcecao);

            Assert.IsNotNull(alvoDestino.Anuncio);
            Assert.AreEqual(alvoOrigem.Anuncio.Area, alvoDestino.Anuncio.Area);
            Assert.AreEqual(alvoOrigem.Anuncio.Bairro, alvoDestino.Anuncio.Bairro);
            Assert.AreEqual(alvoOrigem.Anuncio.Preco, alvoDestino.Anuncio.Preco);
            Assert.AreEqual(alvoOrigem.Anuncio.TipoImovel, alvoDestino.Anuncio.TipoImovel);
            Assert.AreEqual(alvoOrigem.Anuncio.TipoTransacao, alvoDestino.Anuncio.TipoTransacao);
            Assert.IsTrue(alvoOrigem.Anuncio.Equals(alvoDestino.Anuncio));
        }

        [TestMethod]
        public void ParseToCSV_Roundtrip_AnuncioNulo()
        {
            var alvoOrigem = new Alvo(siteInfonet, 1);
            alvoOrigem.HistoricoStatus = "teste";
            alvoOrigem.UltimaVisita = new DateTime(2013, 2, 1, 1, 1, 1);
            alvoOrigem.DuracaoVisita = new TimeSpan(1, 0, 0);
            alvoOrigem.RetornoRequisicao = "codigo html da página";
            alvoOrigem.LinkVisitado = "http://teste.com.br";

            string alvoCSV = alvoOrigem.ToCSV();
            var alvoDestino = Alvo.FromCSV(alvoCSV);

            Assert.AreEqual(alvoOrigem.UltimaVisita, alvoDestino.UltimaVisita);
            Assert.AreEqual(alvoOrigem.HistoricoStatus, alvoDestino.HistoricoStatus);
            Assert.AreEqual(alvoOrigem.UltimaVisita, alvoDestino.UltimaVisita);
            Assert.AreEqual(alvoOrigem.DuracaoVisita, alvoDestino.DuracaoVisita);
            Assert.AreEqual(alvoOrigem.RetornoRequisicao, alvoDestino.RetornoRequisicao);
            Assert.AreEqual(alvoOrigem.LinkVisitado, alvoDestino.LinkVisitado);
        }

        [TestMethod]
        public void AlvoEquals_True()
        {
            var alvo1 = new Alvo(new Site("xxx"), 4);
            var alvo2 = new Alvo(new Site("xxx"), 4);

            Assert.AreEqual(alvo1, alvo2);
            Assert.IsTrue(alvo1.Equals(alvo2));
            Assert.IsTrue(alvo2.Equals(alvo1));
        }

        [TestMethod]
        public void AlvoEquals_False()
        {
            var alvo1 = new Alvo(new Site("xxx"), 4);
            var alvo2 = new Alvo(new Site("yyy"), 4);

            Assert.AreNotEqual(alvo1, alvo2);
        }
    }
}

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using bsn.dal.sqlite;
using bsn.core;
using bsn.core.analise;

namespace bsn.testes
{
    [TestClass]
    public class SqlSiteTest
    {

        [TestInitialize]
        public void TestInitialize()
        {

        }

        [TestMethod]
        public void Site_GetSitePorNome_Infonet()
        {
            var site = Site.GetSitePorNome("Infonet");

            Assert.AreEqual("Infonet", site.Nome);
        }

        [TestMethod]
        public void Anuncio_SqliteSalvar_Update_Infonet2()
        {
            var alvo = new Alvo("Infonet", 2);
            var anuncio = new Anuncio(alvo);
            anuncio.NumeroQuartos = 0;
            anuncio.TipoImovel = TipoImovel.Casa;
            anuncio.TipoTransacao = TipoTransacao.Venda;
            anuncio.Bairro = "Cirurgia";

            anuncio.SqliteSalvar();
        }

        [TestMethod]
        public void Alvo_SqliteSalvar_Update_Roundtrip()
        {
            var site = Site.GetSitePorNome("Infonet");
            var alvoOrigem = new Alvo(site, 1);
            alvoOrigem.LinkVisitado = "http://teste";
            alvoOrigem.Status = "n";
            alvoOrigem.UltimaVisita = DateTime.Today.AddMinutes(100);
            alvoOrigem.RetornoRequisicao = "<html/>";
            alvoOrigem.UltimaExcecao = "TooManyNerdsException";

            alvoOrigem.SqliteSalvar();

            var alvoDestino = Alvo.SqliteFind(alvoOrigem.SiteOrigem.Nome, 
                alvoOrigem.Id);

            Assert.AreEqual(alvoOrigem, alvoDestino);
            Assert.AreEqual(alvoOrigem.LinkVisitado, alvoDestino.LinkVisitado);
            Assert.AreEqual(alvoOrigem.Status, alvoDestino.Status);
            Assert.AreEqual(alvoOrigem.UltimaVisita, alvoDestino.UltimaVisita);
            Assert.AreEqual(alvoOrigem.RetornoRequisicao, alvoDestino.RetornoRequisicao);
            Assert.AreEqual(alvoOrigem.UltimaExcecao, alvoDestino.UltimaExcecao);
        }

        [TestMethod]
        public void Alvo_SqliteFind_Infonet248534()
        {
            var alvo = Alvo.SqliteFind("Infonet", 248534);

            Assert.AreEqual(248534, alvo.Id);
        }

        [TestMethod]
        public void Alvo_SqliteFind_Infonet_ComDataPreenchida()
        {
            var alvo = Alvo.SqliteFind("Infonet", 1);
            Assert.AreEqual(1, alvo.Id);
        }

        [TestMethod]
        public void Alvo_SqliteFind_Infonet_Colecao()
        {
            var x = Alvo.SqliteFind("Infonet");

            Assert.IsTrue(x.Count > 0); 
        }

        [TestMethod]
        public void Alvo_SqliteSalvar_ComAnuncio()
        {
            var site = Site.GetSitePorNome("Infonet");
            var alvo = new Alvo(site, 3);
            alvo.LinkVisitado = "http://teste";
            alvo.Status = "n";
            alvo.UltimaVisita = DateTime.Now;
            alvo.RetornoRequisicao = "<html/>";

            alvo.Anuncio = new Anuncio(alvo);
            alvo.Anuncio.TipoImovel = TipoImovel.Casa;
            alvo.Anuncio.TipoTransacao = TipoTransacao.Venda;
            alvo.Anuncio.Bairro = "...";

            alvo.SqliteSalvar();
        }

        [TestMethod]
        public void Alvo_SqliteSalvar_HistoricoIgual()
        {
            var alvo = new Alvo(new Site("Infonet"), 4);
            alvo.RetornoRequisicao = DateTime.Now.ToString();
            alvo.Status = "nn";
            alvo.SqliteSalvar();

            var alvoDestino = Alvo.SqliteFind("Infonet", 4);

            Assert.AreEqual(alvo.RetornoRequisicao, alvoDestino.RetornoRequisicao);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Alvo_SqliteSalvar_HistoricoMenor()
        {
            var alvo = new Alvo(new Site("Infonet"), 5);
            alvo.RetornoRequisicao = DateTime.Now.ToString();
            alvo.Status = "n";
            alvo.SqliteSalvar();

            var alvoDestino = Alvo.SqliteFind("Infonet", 5);

            Assert.AreEqual(alvo.RetornoRequisicao, alvoDestino.RetornoRequisicao);
        }
    }
}

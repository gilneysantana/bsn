using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using bsn.dal.sqlite;
using bsn.core;
using bsn.core.analise;
using bsn.core.utils;
using bsn.console;

namespace bsn.testes
{
    [TestClass]
    public class SqlSiteTest
    {
        private SQLiteDatabase cashew_tdd = Utils.DB();

        [TestInitialize]
        public void TestInitialize()
        {
            var d = new Dictionary<string,string>();
            d.Add("siteOrigem", "Infonet");
            d.Add("id", "1");
            d.Add("area", "100");
            d.Add("numeroQuartos", "4");
            d.Add("preco", "9.99");
            cashew_tdd.Insert("anuncio", d);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            cashew_tdd.Delete("anuncio", "siteOrigem = 'Infonet' and id = 1");
            cashew_tdd.Delete("anuncio", "siteOrigem = 'Infonet' and id = 3");
        }

        [TestMethod]
        public void Site_GetSitePorNome_Infonet()
        {
            var site = Site.GetSitePorNome("Infonet");

            Assert.AreEqual("Infonet", site.Nome);
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
            alvo.Anuncio.TipoImovel = TipoImovel.CS;
            alvo.Anuncio.TipoTransacao = TipoTransacao.VD;
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
        }


        [TestMethod]
        public void Anuncio_SqliteFind()
        {
            var anuncio = Anuncio.SqliteFind("Infonet", 1);

            Assert.AreEqual(9.99m, anuncio.Preco);
        }

        [TestMethod]
        public void Anuncio_SqliteSalvar_Insert_Roundtrip()
        {
            var alvo = new Alvo("Infonet", 3);
            var anuncioOrigem = new Anuncio(alvo);
            anuncioOrigem.NumeroQuartos = 4;
            anuncioOrigem.Area = 555.55m;
            anuncioOrigem.TipoImovel = TipoImovel.CS;
            anuncioOrigem.TipoTransacao = TipoTransacao.VD;
            anuncioOrigem.Preco = 9.99m;
            anuncioOrigem.Bairro = "Cirurgia";

            anuncioOrigem.SqliteSalvar();
            var anuncioDestino = Anuncio.SqliteFind("Infonet", 3);

            Assert.AreEqual(anuncioOrigem.NumeroQuartos, anuncioDestino.NumeroQuartos);
            Assert.AreEqual(anuncioOrigem.Area, anuncioDestino.Area);
            Assert.AreEqual(anuncioOrigem.TipoImovel, anuncioDestino.TipoImovel);
            Assert.AreEqual(anuncioOrigem.TipoTransacao, anuncioDestino.TipoTransacao);
            Assert.AreEqual(anuncioOrigem.Preco, anuncioDestino.Preco);
            Assert.AreEqual(anuncioOrigem.Bairro, anuncioDestino.Bairro);
        }

        //[TestMethod]
        //public void Bsn_ConsultarSqlite()
        //{
        //    Bsn bsn = new Bsn();

        //    var param = new Dictionary<string, string>();

        //    bsn.ConsultarSqlite(param);
        //}
    }
}

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
        public void Alvo_SqliteSalvar_Update_Infonet1()
        {
            var site = Site.GetSitePorNome("Infonet");
            var alvo = new Alvo(site, 1);
            alvo.LinkVisitado = "http://teste";
            alvo.Status = "n";
            alvo.UltimaVisita = DateTime.Now;
            alvo.RetornoRequisicao = "<html/>";

            alvo.SqliteSalvar();
        }

        [TestMethod]
        public void Alvo_SqliteFind_Infonet248534()
        {
            var alvo = Alvo.SqliteFind("Infonet", 248534);

            Assert.AreEqual(248534, alvo.Id);
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

    }
}

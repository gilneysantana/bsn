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
    public class AlvoTest
    {
        private SQLiteDatabase sqlite = null;
        private Site siteInfonet = null;

        [TestInitialize]
        public void TestInicitalize()
        {
            this.sqlite = SQLiteDatabase.GetDB_Testes();
            this.siteInfonet = Site.GetSitePorNome("Infonet"); 
        }

        [TestMethod]
        public void ParseCSVTest()
        {
            var alvoOrigem = new Alvo(siteInfonet, 1);
            alvoOrigem.HistoricoStatus = "teste";
            alvoOrigem.UltimaVisita = new DateTime(2013, 2, 1, 1, 1, 1);
            alvoOrigem.DuracaoVisita = new TimeSpan(1, 0, 0);
            alvoOrigem.RetornoRequisicao = "codigo html da página";
            alvoOrigem.LinkVisitado = "http://teste.com.br";

            var alvoDestino = Alvo.Parse(alvoOrigem.ToCSV());

            Assert.AreEqual(alvoOrigem.UltimaVisita, alvoDestino.UltimaVisita);
            Assert.AreEqual(alvoOrigem.HistoricoStatus, alvoDestino.HistoricoStatus);
            Assert.AreEqual(alvoOrigem.UltimaVisita, alvoDestino.UltimaVisita);
            Assert.AreEqual(alvoOrigem.DuracaoVisita, alvoDestino.DuracaoVisita);
            Assert.AreEqual(alvoOrigem.RetornoRequisicao, alvoDestino.RetornoRequisicao);
            Assert.AreEqual(alvoOrigem.LinkVisitado, alvoDestino.LinkVisitado);
        }

        [TestMethod]
        public void SqliteFindTest()
        {
            var alvo = Alvo.SqliteFind("Infonet", 248534);

            Assert.AreEqual(248534, alvo.Id);
        }

        [TestMethod]
        public void SqliteSalvar_Update()
        {
            var alvo = new Alvo(this.siteInfonet, 1);
            alvo.LinkVisitado = "http://teste";
            alvo.Status = "n";
            alvo.UltimaVisita = DateTime.Now;
            alvo.RetornoRequisicao = "<html/>";

            alvo.SqliteSalvar();
        }
    }
}

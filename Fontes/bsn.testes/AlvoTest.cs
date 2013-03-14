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
            alvoOrigem.DuracaoVisita = new TimeSpan(1, 0, 0);
            alvoOrigem.RetornoRequisicao = "codigo html da página";
            alvoOrigem.LinkVisitado = "http://teste.com.br";

            alvoOrigem.Anuncio = new Anuncio(alvoOrigem);
            alvoOrigem.Anuncio.Area = 111;
            alvoOrigem.Anuncio.Bairro = "asdfqwer";
            alvoOrigem.Anuncio.Preco = 222;

            string alvoCSV = alvoOrigem.ToCSV();
            var alvoDestino = Alvo.Parse(alvoCSV);

            Assert.AreEqual(alvoOrigem.UltimaVisita, alvoDestino.UltimaVisita);
            Assert.AreEqual(alvoOrigem.HistoricoStatus, alvoDestino.HistoricoStatus);
            Assert.AreEqual(alvoOrigem.UltimaVisita, alvoDestino.UltimaVisita);
            Assert.AreEqual(alvoOrigem.DuracaoVisita, alvoDestino.DuracaoVisita);
            Assert.AreEqual(alvoOrigem.RetornoRequisicao, alvoDestino.RetornoRequisicao);
            Assert.AreEqual(alvoOrigem.LinkVisitado, alvoDestino.LinkVisitado);

            Assert.IsNotNull(alvoDestino.Anuncio);
            Assert.AreEqual(alvoOrigem.Anuncio.Area, alvoDestino.Anuncio.Area);
            Assert.AreEqual(alvoOrigem.Anuncio.Bairro, alvoDestino.Anuncio.Bairro);
            Assert.AreEqual(alvoOrigem.Anuncio.Preco, alvoDestino.Anuncio.Preco);
            Assert.IsTrue(alvoOrigem.Anuncio.Equals(alvoDestino.Anuncio));
        }


    }
}

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using bsn.core;
using bsn.core.busca;

namespace bsn.testes
{
    [TestClass]
    public class AlvoTest
    {
        [TestMethod]
        public void ParseTest()
        {
            var alvoOrigem = new Alvo("Infonet", 1);
            alvoOrigem.HistoricoStatus = "teste";
            alvoOrigem.UltimaVisita = new DateTime(2013, 2, 1, 1, 1, 1);
            alvoOrigem.DuracaoVisita = new TimeSpan(1, 0, 0);
            alvoOrigem.RetornoRequisicao = "codigo html da página";

            var alvoDestino = Alvo.Parse(alvoOrigem.ToCSV());

            Assert.AreEqual(alvoOrigem.UltimaVisita, alvoDestino.UltimaVisita);
            Assert.AreEqual(alvoOrigem.HistoricoStatus, alvoDestino.HistoricoStatus);
            Assert.AreEqual(alvoOrigem.UltimaVisita, alvoDestino.UltimaVisita);
            Assert.AreEqual(alvoOrigem.DuracaoVisita, alvoDestino.DuracaoVisita);
            Assert.AreEqual(alvoOrigem.RetornoRequisicao, alvoDestino.RetornoRequisicao);
        }

        [TestMethod]
        public void GetAlvoAtualizadoTest()
        {
            var buscador = new Buscador(); 
            var alvo = new Alvo("Infonet", 242506);

            var alvoAtual = buscador.GetAlvoAtualizado(alvo);

            Assert.IsTrue(alvoAtual.RetornoRequisicao.Contains(
                "sendo 3 suites, lavabo, wc social, sala 3 amb"));
        }
    }
}

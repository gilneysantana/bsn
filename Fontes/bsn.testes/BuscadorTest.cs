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
    public class BuscadorTest
    {

        /// <summary>
        /// Este teste depende de conexão com a internet
        /// </summary>
        [TestMethod]
        public void GetAlvoAtualizadoTest_StatusIgualP()
        {

            Alvo alvo = new Alvo("Infonet", 1);

            var buscador = new Buscador();
            var alvoAtualizado = buscador.GetAlvoAtualizado(alvo);

            Assert.AreEqual("r", alvoAtualizado.Status);
        }

        [TestMethod]
        public void GetAlvoAtualizadoTest_RetornoRequisicaoConteudoCorreto()
        {
            var buscador = new Buscador(); 
            var alvo = new Alvo("Infonet", 242506);

            var alvoAtual = buscador.GetAlvoAtualizado(alvo);

            Assert.IsTrue(alvoAtual.RetornoRequisicao.Contains(
                "sendo 3 suites"));
        }

        [TestMethod]
        public void GetAlvoAtualizadoTest_RetornoRequisicaoFelizola908()
        {
            var buscador = new Buscador(); 
            var alvo = new Alvo("Felizola", 908);

            var alvoAtual = buscador.GetAlvoAtualizado(alvo);

            //Assert.IsTrue(alvoAtual.RetornoRequisicao.Contains(
            //    "3 SALAS, 2 VARANDAS,"));
            Assert.IsTrue(alvoAtual.RetornoRequisicao.Contains(
                "Imóvel"));
        }
    }
}

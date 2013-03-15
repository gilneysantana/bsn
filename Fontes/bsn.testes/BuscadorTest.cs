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
        Buscador buscador;

        [TestInitialize]
        public void CriarBuscador()
        {
            buscador = new Buscador();
            //Comentar a linha abaixo qd não houver Proxy
            buscador.UrlProxy = "http://inet-se.petrobras.com.br";
        }

        /// <summary>
        /// Este teste depende de conexão com a internet
        /// </summary>
        [TestMethod]
        public void GetAlvoAtualizadoTest_StatusIgualP()
        {
            Alvo alvo = new Alvo("Infonet", 1);

            var alvoAtualizado = buscador.Buscar(alvo);

            Assert.AreEqual("r", alvoAtualizado.Status);
        }

        [TestMethod]
        public void GetAlvoAtualizadoTest_RetornoRequisicaoConteudoCorreto()
        {
            var alvo = new Alvo("Infonet", 242506);

            var alvoAtual = buscador.Buscar(alvo);

            Assert.IsTrue(alvoAtual.RetornoRequisicao.Contains(
                "sendo 3 suites"));
        }

        [TestMethod]
        public void GetAlvoAtualizadoTest_RetornoRequisicaoFelizola908()
        {
            var alvo = new Alvo("Felizola", 908);

            var alvoAtual = buscador.Buscar(alvo);

            //Assert.IsTrue(alvoAtual.RetornoRequisicao.Contains(
            //    "3 SALAS, 2 VARANDAS,"));
            Assert.IsTrue(alvoAtual.RetornoRequisicao.Contains(
                "Imovel"));
        }
    }
}

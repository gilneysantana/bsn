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
    public class BuscadorTest
    {

        /// <summary>
        /// Este teste depende de conexão com a internet
        /// </summary>
        [TestMethod]
        public void AtualizarUrlTest()
        {
            var buscador = new Buscador();
            Alvo url = new Alvo(Site.GetSitePorNome("Infonet"), 1);
            var urlResultado = buscador.GetAlvoAtualizado(url);

            Assert.AreEqual('p', urlResultado.Status);
            Assert.IsTrue(urlResultado.RetornoRequisicao.Contains("www.infonet.com.br"));
        }

    }
}

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using bsn.core;

namespace bsn.testes
{

    [TestClass]
    public class RecuperadorPaginaTest
    {

        [TestMethod]
        [ExpectedException(typeof(WebException))]
        public void testRetrievePageInvalidUrl()
        {
            new RecuperadorPagina().retrieve("http://naoexiste.pne/");
        }

        [TestMethod]
        public void testRetrievePage()
        {
            Pagina page = new RecuperadorPagina().retrieve("http://www.pudim.com.br/");
            Assert.IsNotNull(page);
            Assert.IsTrue(page.GetContent.Contains("mailto:pudim@pudim.com.br"));
        }

    }
}
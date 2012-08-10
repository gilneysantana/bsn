using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using bsn.core;

namespace bsn.testes.dados
{
    [TestClass]
    public class SitesTest
    {
        [TestMethod]
        public void TestarFelizola()
        {
            Pagina pagina = Pagina.CarregarDoArquivo("felizolaimobiliaria.com.br-id=860.htm");

            Site site = new Site();
            site.TemplateUrl = Site.PLACE_HOLDER;
            site.RegexPreco = @"<span.*>Valor:</span> R\$(.*?)<br />";
            site.RegexBairro = "<span.*>Bairro</span>: (.*)<br>";

            Anuncio anuncio = site.ExtrairAnuncio(pagina);
            Assert.AreEqual(180000, anuncio.Preco);
            Assert.AreEqual("Mosqueiro", anuncio.Bairro);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System;

using bsn.core;


namespace bsn.testes
{

    /**
     * @author Gilney
     * 
     */
    [TestClass]
    public class PaginaTest
    {

        [TestMethod]
        public void RecuperarArquivo()
        {
            Pagina pagina = Pagina.CarregarDoArquivo("arquivoParaTest.txt");

            Assert.IsNotNull(pagina);
            Assert.IsTrue(pagina.GetContent.Contains("164784561298765"));
            Assert.IsTrue(!pagina.GetContent.Contains("conteudo que não existe no arquivo"));
        }


    }
}

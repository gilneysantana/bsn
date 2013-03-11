using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using bsn.console;
using bsn.core;

namespace bsn.testes
{
    [TestClass]
    public class BsnTest
    {
        [TestMethod]
        public void GetAlvoAtualizadoComProxy()
        {
            var bsn = new Bsn();
            bsn.UrlProxy = "http://inet-se.petrobras.com.br";
            var alvo = new Alvo("Infonet", 248534);

            bsn.GetAlvoAtualizado(alvo);
        }
    }
}

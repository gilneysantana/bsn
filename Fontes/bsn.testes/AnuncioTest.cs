using Microsoft.VisualStudio.TestTools.UnitTesting;
using bsn.core;
using System;

namespace bsn.testes
{

    [TestClass]
    public class AnuncioTest
    {

        private static Anuncio realEstate;

        [TestInitialize]
        public void setUp()
        {
            realEstate = new Anuncio();
        }

        [TestCleanup]
        public void tearDown()
        {
            realEstate = null;
        }

    }
}

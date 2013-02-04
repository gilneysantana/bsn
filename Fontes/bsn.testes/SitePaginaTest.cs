using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using bsn.core;
using bsn.core.analise;

namespace bsn.testes
{
    [TestClass]
    public class SitePaginaTest
    {
        private IList tuplas = new ArrayList();

        [TestInitialize]
        public void setUp()
        {
            var site = Site.GetSitePorNome("Infonet");
            Alvo alvo = null;
            Anuncio anuncioInfonetEsperado = null;
            var tupla = Tuple.Create(alvo, anuncioInfonetEsperado);

            //// Apartamento, Venda
            //var url = new Alvo(site, "infonet.com.br-expirado-id=150000.htm");
            //var anuncioInfonetEsperado = new Anuncio(null, "Orlando Dantas.", 2, 0, 110000m, null, null);
            //anuncioInfonetEsperado.TipoImovel = TipoImovel.Apartamento;
            //anuncioInfonetEsperado.TipoTransacao = TipoTransacao.Venda;
            //var tupla = Tuple.Create(url, anuncioInfonetEsperado);
            //tuplas.Add(tupla);

            //// Casa, Venda
            //url = new Alvo(site, "infonet.com.br-id=229265.htm");
            //anuncioInfonetEsperado = new Anuncio(null, "PEREIRA LOBO", 3, 216, 220000m, null, null);
            //anuncioInfonetEsperado.TipoImovel = TipoImovel.Casa;
            //anuncioInfonetEsperado.TipoTransacao = TipoTransacao.Venda;
            //tupla = Tuple.Create(url, anuncioInfonetEsperado);
            //tuplas.Add(tupla);

            //// Casa, Aluguel
            //url = new Alvo(site, "infonet.com.br-id=229132.htm");
            //anuncioInfonetEsperado = new Anuncio(null, "Orlando Dantas", 2, 0, 700, null, null);
            //anuncioInfonetEsperado.TipoImovel = TipoImovel.Casa;
            //anuncioInfonetEsperado.TipoTransacao = TipoTransacao.Aluguel;
            //tupla = Tuple.Create(url, anuncioInfonetEsperado);
            //tuplas.Add(tupla);

            // Casa, Aluguel
            alvo = new Alvo(site, 235250);
            alvo.CarregarDoArquivo("infonet.com.br-id=235250.htm");

            anuncioInfonetEsperado = new Anuncio(null, "JABOTIANA", 4, 0, 450000, null, null);
            anuncioInfonetEsperado.TipoImovel = TipoImovel.Casa;
            anuncioInfonetEsperado.TipoTransacao = TipoTransacao.Venda;
            tupla = Tuple.Create(alvo, anuncioInfonetEsperado);
            tuplas.Add(tupla);

            #region Felizola
            site = Site.GetSitePorNome("Felizola");

            // Casa, Venda 
            alvo = new Alvo(site, 860);
            alvo.CarregarDoArquivo("felizolaimobiliaria.com.br-id=860.htm");
            anuncioInfonetEsperado = new Anuncio(null, "Mosqueiro", 3, 0, 180000, null, null);
            anuncioInfonetEsperado.TipoImovel = TipoImovel.Casa;
            anuncioInfonetEsperado.TipoTransacao = TipoTransacao.Venda;
            tupla = Tuple.Create(alvo, anuncioInfonetEsperado);
            tuplas.Add(tupla);

            // Apartamento, Venda
            alvo = new Alvo(site, 1759);
            alvo.CarregarDoArquivo("felizolaimobiliaria.com.br-id=1759.htm");
            anuncioInfonetEsperado = new Anuncio(null, "Luzia", 3, 0, 285000, null, null);
            anuncioInfonetEsperado.TipoImovel = TipoImovel.Apartamento;
            anuncioInfonetEsperado.TipoTransacao = TipoTransacao.Venda;
            tupla = Tuple.Create(alvo, anuncioInfonetEsperado);
            tuplas.Add(tupla);

            // Apartamento, Aluguel 
            alvo = new Alvo(site, 1769);
            alvo.CarregarDoArquivo("felizolaimobiliaria.com.br-id=1769.htm");
            anuncioInfonetEsperado = new Anuncio(null, "AEROPORTO", 3, 0, 700, null, null);
            anuncioInfonetEsperado.TipoImovel = TipoImovel.Apartamento;
            anuncioInfonetEsperado.TipoTransacao = TipoTransacao.Aluguel;
            tupla = Tuple.Create(alvo, anuncioInfonetEsperado);
            tuplas.Add(tupla);

            #endregion 

            #region Zelar
            site = Site.GetSitePorNome("Zelar");

            // Apartamento, Aluguel 
            alvo = new Alvo(site, 28804);
            alvo.CarregarDoArquivo("zelarimoveis.com.br-id=28804.htm");
            anuncioInfonetEsperado = new Anuncio(null, "Farolândia", 3, 0, 800, null, null);
            anuncioInfonetEsperado.TipoImovel = TipoImovel.Apartamento;
            anuncioInfonetEsperado.TipoTransacao = TipoTransacao.Aluguel;
            tupla = Tuple.Create(alvo, anuncioInfonetEsperado);
            tuplas.Add(tupla);

            #endregion 
        }

        [TestMethod]
        public void TestarTuplas()
        {
            foreach (Tuple<Alvo, Anuncio> t in this.tuplas)
            {
                var anuncioExtraido = t.Item1.SiteOrigem.ExtrairAnuncio(t.Item1);

                try
                {
                    Assert.AreEqual(t.Item2.Bairro, anuncioExtraido.Bairro);
                    Assert.AreEqual(t.Item2.Preco, anuncioExtraido.Preco);
                    Assert.AreEqual(t.Item2.NumeroQuartos, anuncioExtraido.NumeroQuartos);
                    Assert.AreEqual(t.Item2.Area, anuncioExtraido.Area);
                    Assert.AreEqual(t.Item2.TipoImovel, anuncioExtraido.TipoImovel);
                    Assert.AreEqual(t.Item2.TipoTransacao, anuncioExtraido.TipoTransacao);
                }
                catch (Exception ex)
                {
                    throw new Exception(t.ToString(), ex);
                }
            }
        }
    }
}

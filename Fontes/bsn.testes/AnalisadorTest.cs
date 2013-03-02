using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using bsn.core;
using bsn.core.analise;
using bsn.dal.sqlite;

namespace bsn.testes
{
    [TestClass]
    public class AnalisadorTest
    {
        private IList tuplas = new ArrayList();

        [TestInitialize]
        public void setUp()
        {
            var site = "Infonet";
            Alvo alvo = null;
            Anuncio anuncioInfonetEsperado = null;
            var tupla = Tuple.Create(alvo, anuncioInfonetEsperado);

            var ids = new int[]{248534, 250210, 250055, 249890};

            foreach (int id in ids)
            {
                tupla = Tuple.Create(Alvo.SqliteFind(site, id), Anuncio.SqliteFind(site, id));
                tuplas.Add(tupla);
            }

            #region Felizola
            site = "Felizola";

            //alvo = Alvo.SqliteFind(site, 860);
            //Assert.IsNotNull(alvo);
            //anuncioInfonetEsperado = new Anuncio(null, "Mosqueiro", 3, 0, 180000);
            //anuncioInfonetEsperado.TipoImovel = TipoImovel.Casa;
            //anuncioInfonetEsperado.TipoTransacao = TipoTransacao.Venda;
            //tupla = Tuple.Create(alvo, anuncioInfonetEsperado);
            //tuplas.Add(tupla);

            //// Apartamento, Venda
            //alvo = new Alvo(site, 1759, SQLiteDatabase.GetDB_Testes());
            //alvo.CarregarDoArquivo("felizolaimobiliaria.com.br-id=1759.htm");
            //anuncioInfonetEsperado = new Anuncio(null, "Luzia", 3, 0, 285000);
            //anuncioInfonetEsperado.TipoImovel = TipoImovel.Apartamento;
            //anuncioInfonetEsperado.TipoTransacao = TipoTransacao.Venda;
            //tupla = Tuple.Create(alvo, anuncioInfonetEsperado);
            //tuplas.Add(tupla);

            //// Apartamento, Aluguel 
            //alvo = new Alvo(site, 1769, SQLiteDatabase.GetDB_Testes());
            //alvo.CarregarDoArquivo("felizolaimobiliaria.com.br-id=1769.htm");
            //anuncioInfonetEsperado = new Anuncio(null, "AEROPORTO", 3, 0, 700);
            //anuncioInfonetEsperado.TipoImovel = TipoImovel.Apartamento;
            //anuncioInfonetEsperado.TipoTransacao = TipoTransacao.Aluguel;
            //tupla = Tuple.Create(alvo, anuncioInfonetEsperado);
            //tuplas.Add(tupla);

            //#endregion 

            //#region Zelar
            //site = "Zelar";

            //// Apartamento, Aluguel 
            //alvo = new Alvo(site, 28804, SQLiteDatabase.GetDB_Testes());
            //alvo.CarregarDoArquivo("zelarimoveis.com.br-id=28804.htm");
            //anuncioInfonetEsperado = new Anuncio(null, "Farolândia", 3, 0, 800);
            //anuncioInfonetEsperado.TipoImovel = TipoImovel.Apartamento;
            //anuncioInfonetEsperado.TipoTransacao = TipoTransacao.Aluguel;
            //tupla = Tuple.Create(alvo, anuncioInfonetEsperado);
            //tuplas.Add(tupla);

            #endregion
        }

        [TestMethod]
        public void TestarTuplas()
        {
            var analisador = new Analisador();

            foreach (Tuple<Alvo, Anuncio> t in this.tuplas)
            {
                var alvoAnalisado = analisador.Analisar(t.Item1);
                var anuncioExtraido = alvoAnalisado.Anuncio;

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

using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using bsn.core;
using bsn.core.analise;
using bsn.core.busca;
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
            Tuple<Alvo, Anuncio> tupla;

            var site = "Infonet";
            foreach (int id in new int[]{248534, 250210, 250055, 249890})
            {
                tupla = Tuple.Create(Alvo.SqliteFind(site, id), Anuncio.SqliteFind(site, id));
                tuplas.Add(tupla);
            }

            site = "Felizola";
            foreach (int id in new int[]{908,1542})
            {
                tupla = Tuple.Create(Alvo.SqliteFind(site, id), Anuncio.SqliteFind(site, id));
                tuplas.Add(tupla);
            }

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

        }

        [TestMethod]
        public void Laco_Banco_Analisador()
        {
            var analisador = new Analisador();

            foreach (Tuple<Alvo, Anuncio> t in this.tuplas)
            {
                var alvoAnalisado = analisador.Analisar(t.Item1);
                var anuncioExtraido = alvoAnalisado.Anuncio;
                Assert.IsNotNull(anuncioExtraido, string.Format("Alvo: {0}. Última exceção: {1}", 
                    alvoAnalisado.ToString(), alvoAnalisado.UltimaExcecao));

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

        //[TestMethod]
        //public void TestarAlvoFelizola908_Buscador_Analisador()
        //{
        //    var buscador = new Buscador();
        //    var analisador = new Analisador();
        //    var alvo = new Alvo("Felizola", 908);

        //    var alvoAtual = buscador.GetAlvoAtualizado(alvo);
        //    var alvoAnalisado = analisador.Analisar(alvoAtual);

        //    Assert.IsNotNull(alvoAnalisado.Anuncio);
        //}

        //[TestMethod]
        //public void TestarAlvoFelizola908_Buscador_Banco_Analisador()
        //{
        //    var buscador = new Buscador();
        //    var analisador = new Analisador();
        //    var alvo = new Alvo("Felizola", 908);

        //    var alvoAtualizado = buscador.GetAlvoAtualizado(alvo);
        //    alvoAtualizado.SqliteSalvar();
        //    var alvoVindoSqlite = Alvo.SqliteFind("Felizola", 908);
        //    var alvoAnalisado = analisador.Analisar(alvoVindoSqlite);

        //    Assert.IsNotNull(alvoAnalisado.Anuncio);
        //}
    }
}

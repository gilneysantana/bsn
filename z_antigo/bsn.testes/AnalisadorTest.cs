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
            foreach (int id in new int[]{908,1542,1112,1668})
            {
                var alvo = Alvo.SqliteFind(site, id);
                var anuncio = Anuncio.SqliteFind(site, id);
                
                if (alvo == null)
                    throw new ApplicationException(
                        string.Format("Não existe Alvo ('{0}',{1}) na base de dados", site, id));

                if (anuncio == null)
                    throw new ApplicationException(
                        string.Format("Não existe Anuncio '{0}' na base dados", alvo.GetLink()));

                tupla = Tuple.Create(alvo, anuncio);
                tuplas.Add(tupla);
            }

            site = "Zelar";
            foreach (int id in new int[] {27957} )
            {
                var alvo = Alvo.SqliteFind(site, id);
                var anuncio = Anuncio.SqliteFind(site, id);
                
                if (alvo == null)
                    throw new ApplicationException(
                        string.Format("Não existe Alvo ('{0}',{1}) na base de dados", site, id));

                if (anuncio == null)
                    throw new ApplicationException(
                        string.Format("Não existe Anuncio '{0}' na base dados", alvo.GetLink()));

                tupla = Tuple.Create(alvo, anuncio);
                tuplas.Add(tupla);
            }
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

    }
}

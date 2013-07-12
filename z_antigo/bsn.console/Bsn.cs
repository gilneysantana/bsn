using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using bsn.core;
using bsn.core.busca;
using bsn.core.analise;

namespace bsn.console
{
    public class Bsn
    {
        private Analisador analisador = new Analisador();
        public string UrlProxy { get; set; }
        public bool ModoVerboso { get; set; }

        /// <summary>
        /// Pede que o buscador se conecte ao Site de Origem e retorna uma
        /// versão atualizada do Conteúdo do Alvo
        /// </summary>
        /// <param name="alvo"></param>
        /// <returns></returns>
        public Alvo Buscar(Alvo alvo)
        {
            Buscador buscador = new Buscador();

            if (!string.IsNullOrEmpty(UrlProxy))
            {
                buscador.UrlProxy = UrlProxy;
            }

            return buscador.Buscar(alvo);
        }


        /// <summary>
        /// Retorna o Alvo passado com o Anuncio extraido
        /// </summary>
        /// <param name="alvo"></param>
        /// <returns></returns>
        public Alvo Analisar(Alvo alvo)
        {
            var alvoAnalisado = analisador.Analisar(alvo);

            if (ModoVerboso)
            {
                alvoAnalisado.RetornoRequisicao = "...RetornoRequisicao...(Modo verboso)";
                alvoAnalisado.LinkVisitado = "...LinkVisitado...(Modo verboso)";
                Console.WriteLine(string.Format("Anuncio: '{0}'", 
                    alvoAnalisado.Anuncio));
            }

            return alvoAnalisado;
        }

        /// <summary>
        /// Gerar uma lista de Alvos 
        /// </summary>
        /// <param name="nomeSite"></param>
        /// <param name="idInicio"></param>
        /// <param name="idFim"></param>
        /// <returns></returns>
        public IList<Alvo> GetAlvos(Site site, int idInicio, int idFim)
        {
            //var site = Site.GetSitePorNome(nomeSite);
            var retorno = new List<Alvo>();

            for (int i = idInicio; i <= idFim; i++)
            {
                retorno.Add(new Alvo(site, i));
            }

            return retorno;
        }

        public void Persistir(Alvo alvo)
        {
            alvo.SqliteSalvar();
        }

        public void ConsultarSqlite(IDictionary<string, string> param)
        {
                string tabela = param["-tabela"];
                string nomeSite = param["-site"];

                if (tabela == "alvo")
                {
                    IList<Alvo> alvos = new List<Alvo>();

                    if (param.ContainsKey("-id"))
                    {
                        var alvo = Alvo.SqliteFind(nomeSite, Convert.ToInt32(param["-id"]));
                        if (alvo != null) alvos.Add(alvo);
                    }
                    else if (param.ContainsKey("-hist"))
                    {
                        alvos = Alvo.SqliteFindPorHistorico(param["-hist"]);
                    }
                    else if (param.ContainsKey("-inutil"))
                    {
                        alvos = Alvo.SqliteFindCandidatosDesativacao();
                    }
                    else if (param.ContainsKey("-ativo"))
                    {
                        alvos = Alvo.SqliteFind(nomeSite);
                    }
                    else
                        Console.WriteLine("Informe uma opção: -id, -hist, -desativar, -todos");

                    if (alvos.Count == 0)
                        Console.WriteLine("Nenhum registro encontrado");
                    else
                    {
                        Console.WriteLine("#TYPE bsn.core.Alvo");
                        Console.WriteLine(Alvo.CabecalhoCSV());

                        foreach (Alvo a in alvos)
                            Console.WriteLine(a.ToCSV());
                    }
                }
                else if (tabela == "site")
                {
                    var site = Site.GetSitePorNome(nomeSite);

                    if (site != null)
                    {
                        Console.WriteLine("#TYPE bsn.core.Site");
                        Console.WriteLine(Site.CabecalhoCSV());
                        Console.WriteLine(site.ToCSV());
                    }
                    else
                        Console.WriteLine("Site '{0}' não foi encontrado na tabela '{1}'",
                            nomeSite, tabela);
                }
                else if (tabela == "anuncio")
                {
                    var anuncio = Anuncio.SqliteFind(nomeSite, Convert.ToInt32(param["-id"]));

                    if (anuncio != null)
                    {
                        Console.WriteLine("#TYPE bsn.core.Anuncio");
                        Console.WriteLine(Anuncio.CabecalhoCSV());
                        Console.WriteLine(anuncio.ToCSV());
                    }
                    else
                        Console.WriteLine("Site '{0}' não foi encontrado na tabela '{1}'",
                            nomeSite, tabela);
                }
                else
                {
                    Console.WriteLine("Tabela {0} não é reconhecida", tabela);
                }
            
        }
    }
}

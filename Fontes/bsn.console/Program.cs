using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;

using bsn.core;
using bsn.core.utils;
using bsn.dal.sqlite;
using System.Threading;   

namespace bsn.console
{
    class Program
    {
        private static bool modoVerboso = false;

        static void Main(string[] args)
        {
            if (args.Contains("-v"))
                modoVerboso = true;

            if (args.Contains("--debug"))
                Debugger.Break(); 

            try
            {
                switch (args[0])
                {
                    case "alvo":
                        alvo(args);
                        break;
                    case "buscar":
                        buscar(args);
                        break;
                    case "analisar":
                        analisar(args);
                        break;
                    case "regex":
                        string conteudo = Console.In.ReadToEnd();
                        Utils.Matches(args[1], conteudo);
                        break;
                    case "sqlite":
                        sqlite(args);
                        break;
                    case "help":
                        printHelp();
                        break;
                    case "delay":
                        Delay(Convert.ToInt32(args[1]));
                        break;
                    default:
                        Console.WriteLine("Comando desconhecido: '{0}'", args[0]);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(string.Format("Erro no uso da opção '{0}': {1}", args[0], ex));
            }
        }

        private static void Delay(int segundos)
        {
            string linha;
            while ((linha = Console.ReadLine()) != null)
            {
                Thread.Sleep(segundos * 1000);
                WriteLineVerbose("Dormindo");
                Console.WriteLine(linha);
            }
        }

        private static void printHelp()
        {
            Console.WriteLine(@"
AJUDA
-----
    Exemplos:
        bsn alvo Infonet 1 10 - Gera Alvos de 1 a 10
        bsn alvo Infonet 10   - É o mesmo que 'alvo Infonet 10 10', gera apenas 1 alvo

    bsn buscar
        pipe_de_alvos |.\bsn buscar
        pipe_de_alvos |.\bsn buscar -p      - Utiliza proxy default

    bsn analisar

    bsn regex
        algumaString | .\bsn regex ""ExpressaoRegular""

    bsn sqlite
        pipe_de_alvos | .\bsn sqlite - Persiste alvos 
        .\bsn Infonet 254932 - Busca o alvo informado e joga na saida do pipe

    Atribuindo à variáveis:
        $alvo = .\bsn alvo Infonet 235250 | .\bsn.exe buscar | ConvertFrom-Csv

        Salvando em arquivo: Out-File arquivo.txt   
        Lendo de arquivo: Import-Csv arquivo.txt
");
        }

        static void alvo(string[] args)
        {
            var bsn = new Bsn();
            string nomeSite = args[1];
            int idInicio = Convert.ToInt32(args[2]);
            int idFim = idInicio;
 
            if (args.Length == 4)
                idFim = Convert.ToInt32(args[3]);

            Console.WriteLine("#TYPE bsn.core.Alvo");
            Console.WriteLine(Alvo.CabecalhoCSV());

            var site = Site.GetSitePorNome(nomeSite);

            foreach (Alvo alvo in bsn.GetAlvos(site, idInicio, idFim))
            {
                Console.WriteLine(alvo.ToCSV());
            }
        }

        static void buscar(string[] args)
        {
            var bsn = new Bsn();

            if (args.Contains("-p"))
                bsn.UrlProxy = "http://inet-se.petrobras.com.br";

            // Ignoro as duas primeiras linhas (cabeçalho)
            Console.ReadLine();
            Console.ReadLine();

            Console.WriteLine("#TYPE bsn.core.Alvo");
            Console.WriteLine(Alvo.CabecalhoCSV());

            string csvAlvo;
            while ((csvAlvo = Console.ReadLine()) != null)
            {
                if (args.Contains("--delay"))
                    Thread.Sleep(3000);

                var alvo = Alvo.FromCSV(csvAlvo);
                Console.WriteLine(bsn.Buscar(alvo).ToCSV());
            }
        }

        static void analisar(string[] args)
        {
            var bsn = new Bsn();
            bsn.ModoVerboso = modoVerboso;

            WriteLineVerbose("Iniciando análise em modo verbodo.");

            // Ignoro as duas primeiras linhas (cabeçalho)
            Console.ReadLine();
            Console.ReadLine();

            Console.WriteLine("#TYPE bsn.core.Alvo");
            Console.WriteLine(Alvo.CabecalhoCSV());

            string csvAlvo;
            while ((csvAlvo = Console.ReadLine()) != null)
            {
                var alvo = Alvo.FromCSV(csvAlvo);
                WriteLineVerbose("Alvo após FromCSV: " + alvo);

                var alvoAnalisado = bsn.Analisar(alvo);
                WriteLineVerbose("Alvo Analisado: ");
                Console.WriteLine(alvoAnalisado.ToCSV());
            }

            WriteLineVerbose("Fim análise em modo verboso.");
            WriteLineVerbose("-----------------------------------");
        }

        static void sqlite(string[] args)
        {
            var bsn = new Bsn();

            if (args.Length == 1)
            {
                string tipo = Console.ReadLine();
                string colunas = Console.ReadLine();

                Console.WriteLine(string.Format("Conectado ao '{0}'",
                    Utils.DB().DbConnection));

                if (tipo != "#TYPE bsn.core.Alvo")
                    throw new Exception("Não é um Alvo");

                Console.WriteLine("Persistindo Alvos...");

                string alvoCsv;
                while ((alvoCsv = Console.ReadLine()) != null)
                {
                    var alvo = Alvo.FromCSV(alvoCsv);
                    try
                    {
                        Console.Write(string.Format("Alvo ('{0}', {1})",
                            alvo.SiteOrigem.Nome, alvo.Id));
                        bsn.Persistir(alvo);
                        Console.WriteLine(" - OK");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(" - FALHOU: {0}", ex.Message);
                    }
                }
            }
            else
            {
                string tabela = args[1];
                string nomeSite = args[2];

                if (tabela == "alvo")
                {
                    Console.WriteLine("#TYPE bsn.core.Alvo");
                    Console.WriteLine(Alvo.CabecalhoCSV());

                    if (args.Length == 4)
                    {
                        string id = args[3];
                        var alvo = Alvo.SqliteFind(nomeSite, Convert.ToInt32(id));

                        if (alvo != null)
                        {
                            Console.WriteLine(alvo.ToCSV());
                        }
                        else
                            Console.WriteLine("Nenhum registro encontrado");
                    }
                    else
                    {
                        var alvos = Alvo.SqliteFind(nomeSite);

                        foreach (Alvo a in alvos)
                        {
                            Console.WriteLine(a.ToCSV());
                        }

                        if (alvos.Count == 0)
                            Console.WriteLine("Nenhum registro encontrado");
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
                else
                {
                    Console.WriteLine("Tabela {0} não é reconhecida", tabela);
                }
            }

        }

        private static void WriteLineVerbose(string msg, params object[] args)
        {
            if (modoVerboso)
                Console.WriteLine(string.Format(msg, args));
        }
    }
}

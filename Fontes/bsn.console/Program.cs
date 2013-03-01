using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using bsn.core;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using bsn.core.utils;
using bsn.dal.sqlite;   

namespace bsn.console
{
    class Program
    {
        static void Main(string[] args)
        {
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
                    default:
                        printHelp();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(string.Format("Erro no uso da opção '{0}': {1}", args[0], ex));
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
            // Ignoro as duas primeiras linhas (cabeçalho)
            Console.ReadLine();
            Console.ReadLine();

            Console.WriteLine("#TYPE bsn.core.Alvo");
            Console.WriteLine(Alvo.CabecalhoCSV());

            var bsn = new Bsn();
            string csvAlvo;
            while ((csvAlvo = Console.ReadLine()) != null)
            {
                var alvo = Alvo.Parse(csvAlvo);
                Console.WriteLine(bsn.GetAlvoAtualizado(alvo).ToCSV());
            }
        }

        static void analisar(string[] args)
        {
            var bsn = new Bsn();
            // Ignoro as duas primeiras linhas (cabeçalho)
            Console.ReadLine();
            Console.ReadLine();

            Console.WriteLine("#TYPE bsn.core.Alvo");
            Console.WriteLine(Alvo.CabecalhoCSV());

            string alvoCsv;
            while ((alvoCsv = Console.ReadLine()) != null)
            {
                var alvo = Alvo.Parse(alvoCsv);
                Console.WriteLine(bsn.GetAnuncioAlvo(alvo).ToCSV());
            }
        }

        static void sqlite(string[] args)
        {

            if (args.Length == 1)
            {
                string tipo = Console.ReadLine();
                string colunas = Console.ReadLine();

                if (tipo != "#TYPE bsn.core.Alvo")
                    throw new Exception("Não é um Alvo");

                Console.WriteLine(string.Format("Persistindo em '{0}'",
                    Utils.DB().DbConnection));

                string alvoCsv;
                while ((alvoCsv = Console.ReadLine()) != null)
                {
                    var alvo = Alvo.Parse(alvoCsv);
                    alvo.SqliteSalvar();
                }
            }
            else
            {
                string nomeSite = args[1];
                string id = args[2];

                var alvo = Alvo.SqliteFind(nomeSite, Convert.ToInt32(id));

                if (alvo != null)
                {
                    Console.WriteLine("#TYPE bsn.core.Alvo");
                    Console.WriteLine(Alvo.CabecalhoCSV());
                    Console.WriteLine(alvo.ToCSV());
                }
                else
                    Console.WriteLine("Nenhum registro encontrado");
            }

        }
    }
}

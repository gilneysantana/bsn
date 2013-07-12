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
//using bsn.dal.sqlite;
using System.Threading;   

namespace bsn.console
{
    public class Program
    {
        private static bool modoVerboso = false;
        private static bool force = false;
        private static IDictionary<string,string> param;

        static void Main(string[] args)
        {
            param = TratarArgumentos(args);

            if (param.ContainsKey("-v"))
                modoVerboso = true;

            if (param.ContainsKey("-debug"))
                Debugger.Break();

            if (param.ContainsKey("-force"))
                force = true;

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
                        sqlite(param);
                        break;
                    case "help":
                        printHelp();
                        break;
                    //case "delay":
                    //    Delay(Convert.ToInt32(args[1]));
                    //    break;
                    case "acao":
                        acao(param);
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

        //private static void Delay(int segundos)
        //{
        //    string linha;
        //    while ((linha = Console.ReadLine()) != null)
        //    {
        //        Thread.Sleep(segundos * 1000);
        //        WriteLineVerbose("Dormindo");
        //        Console.WriteLine(linha);
        //    }
        //}

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
        .\bsn sqlite -tabela alvo -site Felizola -ativo
        .\bsn sqlite -tabela alvo -site Felizola -inutil
        .\bsn sqlite -tabela alvo -site Felizola -id #x
        .\bsn sqlite -tabela alvo -site Felizola -hist #padrao

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

            if (args.Contains("-proxy"))
                bsn.UrlProxy = "http://inet-se.petrobras.com.br";

            Console.WriteLine(Console.ReadLine());
            Console.WriteLine(Console.ReadLine());

            string csvAlvo;
            while ((csvAlvo = Console.ReadLine()) != null)
            {
                if (param.ContainsKey("-delay"))
                    Thread.Sleep(Convert.ToInt32(param["-delay"]) * 1000);

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

                if (force)
                    alvo.Status = "[force]r";

                var alvoAnalisado = bsn.Analisar(alvo);
                WriteLineVerbose("Alvo Analisado: ");
                Console.WriteLine(alvoAnalisado.ToCSV());
            }

            WriteLineVerbose("Fim análise em modo verboso.");
            WriteLineVerbose("-----------------------------------");
        }

        static void sqlite(IDictionary<string, string> param)
        {
            var bsn = new Bsn();

            if (param.ContainsKey("-alterar"))
            {
                string tipo = Console.ReadLine();
                string colunas = Console.ReadLine();

                if (tipo != "#TYPE bsn.core.Alvo")
                    throw new Exception("Não é um Alvo");

                Console.WriteLine("Persistindo Alvos...");

                string alvoCsv;
                while ((alvoCsv = Console.ReadLine()) != null)
                {
                    var alvo = Alvo.FromCSV(alvoCsv);
                    try
                    {
                        Console.Write(alvo);
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
                bsn.ConsultarSqlite(param);
            }

        }

        static void acao(IDictionary<string, string> param)
        {
            Console.WriteLine(Console.ReadLine());
            Console.WriteLine(Console.ReadLine());

            string csvAlvo;
            while ((csvAlvo = Console.ReadLine()) != null)
            {
                var alvo = Alvo.FromCSV(csvAlvo);

                if (param.ContainsKey("-cancelar"))
                {
                    Trace.Assert(alvo.HistoricoStatus.Contains("[0]ar[0]ar[0]a")); 
                    alvo.Status = "x";
                    alvo.RetornoRequisicao = "";
                }

                Console.WriteLine(alvo.ToCSV());
            }
        }

        private static void WriteLineVerbose(string msg, params object[] args)
        {
            if (modoVerboso)
                Console.WriteLine(string.Format(msg, args));
        }

        public static IDictionary<string, string> TratarArgumentos(string[] args)
        {
            var retorno = new Dictionary<string, string>();

            for (int i = 1; i < args.Length; i++)
            {
                string param = args[i];

                string valor = null;
                if ((i+1) < args.Length && !args[i+1].StartsWith("-"))
                {
                    valor = args[i + 1];
                    i++;
                }

                retorno.Add(param, valor);
            }

            return retorno;
        }
    }
}

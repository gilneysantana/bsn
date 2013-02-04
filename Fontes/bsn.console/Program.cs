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

namespace bsn.console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args[0] == "-a")
                {
                    opcaoA(args);
                }

                if (args[0] == "-b")
                {
                    opcaoB(args);
                }

                if (args[0] == "-c")
                {
                    opcaoC(args);
                }
                if (args[0] == "-re")
                {
                    string conteudo = Console.In.ReadToEnd();
                    Utils.Matches(args[1], conteudo);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }

        static void opcaoA(string[] args)
        {
            var bsn = new Bsn();
            string site = args[1];
            int idInicio = Convert.ToInt32(args[2]);
            int idFim = Convert.ToInt32(args[3]);

            Console.WriteLine("#TYPE bsn.core.Alvo");
            Console.WriteLine(Alvo.CabecalhoCSV());

            foreach (Alvo alvo in bsn.GetAlvos(site, idInicio, idFim))
            {
                Console.WriteLine(alvo.ToCSV());
            }
        }

        static void opcaoB(string[] args)
        {
            var bsn = new Bsn();
            // Ignoro as duas primeiras linhas (cabeçalho)
            Console.ReadLine();
            Console.ReadLine();

            Console.WriteLine("#TYPE bsn.core.Alvo");
            Console.WriteLine(Alvo.CabecalhoCSV());

            string s;
            while ((s = Console.ReadLine()) != null)
            {
                var alvo = Alvo.Parse(s);
                Console.WriteLine(bsn.GetAlvoAtualizado(alvo).ToCSV());
            }
        }

        static void opcaoC(string[] args)
        {
            var bsn = new Bsn();
            // Ignoro as duas primeiras linhas (cabeçalho)
            Console.ReadLine();
            Console.ReadLine();

            Console.WriteLine(Alvo.CabecalhoCSV());

            string s;
            while ((s = Console.ReadLine()) != null)
            {
                var alvo = Alvo.Parse(s);
                Console.WriteLine(bsn.GetAnuncioAlvo(alvo));
            }
        }

    }
}

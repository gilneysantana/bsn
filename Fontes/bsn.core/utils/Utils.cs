using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;

using bsn.dal.sqlite;


namespace bsn.core.utils
{
    public class Utils
    {
        public static string ExtrairCampoString(string strRegex, string conteudo)
        {
            Regex regex = new Regex(strRegex, RegexOptions.IgnoreCase);
            Match retorno = regex.Match(conteudo);

            if (retorno.Groups.Count == 2)
            {
                return retorno.Groups[1].Value.Trim();
            }
            else if (retorno.Groups.Count == 3)
            {
                return (retorno.Groups[1].Value + retorno.Groups[2].Value).Trim();
            }
            else
            {
                return string.Empty;
            }
        }

        public static void Matches(string strRegex, string conteudo)
        {
            Regex regex = new Regex(strRegex, RegexOptions.IgnoreCase);
            Match retorno = regex.Match(conteudo);

            foreach (Group g in retorno.Groups)
            {
                Console.WriteLine(string.Format("Group.Value: '{0}'", g.Value));
                Console.WriteLine();
            }
        }

        public static SQLiteDatabase DB()
        {
            string strCon = ConfigurationManager.ConnectionStrings["strCon"].ConnectionString;
            return new SQLiteDatabase(strCon);
        }
        
        public static SQLiteDatabase DBSite()
        {
            string strCon = ConfigurationManager.ConnectionStrings["strConSite"].ConnectionString;
            return new SQLiteDatabase(strCon);
        }

        public static string ObterExcecoes(Exception ex)
        {
            var msg = new StringBuilder();

            while (ex != null)
            {
                msg.Append(ex.Message + " --> ");
                ex = ex.InnerException;
            }

            return msg.ToString();
        }

        public static string ToUtf8(string strIso)
        {
            var iso = Encoding.GetEncoding("iso-8859-1");
            var utf = Encoding.UTF8;

            byte[] isoBytes = iso.GetBytes(strIso);
            byte[] utf8Bytes = Encoding.Convert(iso, utf, isoBytes);

            return utf.GetString(utf8Bytes);  
        }

        public static string ToUtf8FromWindows(string strIso)
        {
            var iso = Encoding.GetEncoding("Windows-1252");
            var utf = Encoding.UTF8;

            byte[] isoBytes = iso.GetBytes(strIso);
            byte[] utf8Bytes = Encoding.Convert(iso, utf, isoBytes);

            return utf.GetString(utf8Bytes);  
        }

        public static string ToCSV(params object[] campos)
        {
            var s = new string[campos.Length];

            for(int i = 0; i < campos.Length; i++)
              s[i] = string.Format("\"{0}\"", campos[i]
                  .ToString()
                  .Replace("\"", escAspa)
                  .Replace(",", escVirg));

            return string.Join(",", s);
        }

        private static string escAspa = "\"\"";
        private static string escVirg = "\\,";
        private static string escVirgTemp = "${virgula}";

        public static string[] FromCSV(string strCSV)
        {
            strCSV = strCSV.Replace(escVirg, escVirgTemp);
            string[] arr = strCSV.Split(',');

            var arrStrings = new List<string>();

            foreach (string campo in arr)
            {
                arrStrings.Add(campo
                    .Remove(campo.Length - 1)
                    .Substring(1)
                    .Replace(escAspa, "\"")
                    .Replace(escVirgTemp, ","));
            }

            return arrStrings.ToArray();
        }
    }
}

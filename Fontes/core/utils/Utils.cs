using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace bsn.core.utils
{
    public class Utils
    {
        public static string ExtrairCampo(string strRegex, string conteudo)
        {
            Regex regex = new Regex(strRegex, RegexOptions.IgnoreCase);
            Match retorno = regex.Match(conteudo);

            if (retorno.Groups.Count == 2)
                return retorno.Groups[1].Value;
            else
                return null;
        }

        public static void Matches(string strRegex, string conteudo)
        {
            Regex regex = new Regex(strRegex, RegexOptions.IgnoreCase);
            Match retorno = regex.Match(conteudo);

            foreach (Group g in retorno.Groups)
            {
                Console.WriteLine(string.Format("Group.Value: '{0}'", g.Value));
            }
        }
    }
}

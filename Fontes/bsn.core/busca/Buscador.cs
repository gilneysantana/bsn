using System;
using System.Net;
using System.Collections.Generic;

namespace bsn.core.busca
{
    /**
     * Class that extracts real estates of a web site.
     * 
     * @author Gilney
     * 
     */
    public class Buscador
    {
        private WebClient MyWebClient = new WebClient();

        /// <summary>
        /// Atualiza uma Url contra seu Site de origem
        /// </summary>
        public Alvo GetAlvoAtualizado(Alvo alvo)
        {
            if (alvo.Status == "x")
                throw new Exception("Não é permitido atualizar uma Url em status 'x'");

            alvo.UltimaVisita = DateTime.Now;
            try
            {
                alvo.RetornoRequisicao = MyWebClient.DownloadString(alvo.GetLink());

                if (alvo.SiteOrigem.Nome == "Felizola")
                    alvo.RetornoRequisicao = ToUtf8(alvo.RetornoRequisicao);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Não foi possível recuperar o conteúdo da URL '{0}'.",
                    alvo.GetLink()), ex);
            }
            alvo.DuracaoVisita = DateTime.Now - alvo.UltimaVisita;
            alvo.LinkVisitado = alvo.GetLink();
            alvo.Status = "r";
            alvo.UltimaVisita = DateTime.Now;

            alvo.RetornoRequisicao = alvo.RetornoRequisicao
                .Replace("\r\n", "").Replace("\n", "").Replace("\r", "")
                .Replace("\"","").Replace("'","").Replace("&acirc;","â");

            return alvo;
        }

        private string ToUtf8(string s_unicode)
        {
            System.Text.Encoding iso_8859_1 = System.Text.Encoding.GetEncoding("iso-8859-1");
            System.Text.Encoding utf_8 = System.Text.Encoding.UTF8;

            // Convert to ISO-8859-1 bytes.
            byte[] isoBytes = iso_8859_1.GetBytes(s_unicode);

            // Convert to UTF-8.
            byte[] utf8Bytes = System.Text.Encoding.Convert(iso_8859_1, utf_8, isoBytes);

            return Convert.ToBase64String(utf8Bytes);
        }
    }
}

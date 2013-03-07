using System;
using System.Net;
using System.Collections.Generic;
using System.Text;

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

            alvo.RetornoRequisicao = alvo.RetornoRequisicao
                .Replace("\r\n", "").Replace("\n", "").Replace("\r", "")
                .Replace("\"","").Replace("'","").Replace("&acirc;","â");

            return alvo;
        }

        private string ToUtf8(string strIso)
        {
            var iso = Encoding.GetEncoding("iso-8859-1");
            var utf = Encoding.UTF8;

            byte[] isoBytes = iso.GetBytes(strIso);
            byte[] utf8Bytes = Encoding.Convert(iso, utf, isoBytes);

            return utf.GetString(utf8Bytes);  
        }
    }
}

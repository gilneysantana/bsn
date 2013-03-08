using System;
using System.Net;
using System.Collections.Generic;
using System.Text;

using bsn.core.utils;

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
                    alvo.RetornoRequisicao = Utils.ToUtf8(alvo.RetornoRequisicao);
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
                .Replace("\"","").Replace("'","").Replace("&acirc;","â")
                .Replace("ó", "o").Replace("á","a").Replace("é","e")
                .Replace("í","i").Replace("ú","u").Replace("ã","a")
                .Replace("â","a");

            return alvo;
        }

    }
}

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
            if (alvo.Status == 'x')
                throw new Exception("Não é permitido atualizar uma Url em status 'x'");

            alvo.UltimaVisita = DateTime.Now;
            alvo.RetornoRequisicao = MyWebClient.DownloadString(alvo.GetLink());
            alvo.DuracaoVisita = DateTime.Now - alvo.UltimaVisita;
            alvo.LinkVisitado = alvo.GetLink();
            alvo.HistoricoStatus += "r";
            alvo.UltimaVisita = DateTime.Now;

            alvo.RetornoRequisicao = alvo.RetornoRequisicao.Replace("\r\n", "")
                .Replace("\n", "").Replace("\r", "").Replace("\"","");

            return alvo;
        }

        public IList<Alvo> AtualizarAlvo(IList<Alvo> alvos)
        {
            var alvosAtualizados = new List<Alvo>();

            foreach (Alvo alvo in alvos)
            {
                alvosAtualizados.Add(GetAlvoAtualizado(alvo));
            }

            return alvosAtualizados;
        }

    }
}

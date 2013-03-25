using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using bsn.core.utils;

namespace bsn.core.analise
{
    public class Analisador
    {
        public Alvo Analisar(Alvo alvo)
        {
            if (alvo == null)
                throw new ApplicationException("O alvo passado como parâmetro não pode ser nulo");

            try
            {
                if (alvo.Status != "r")
                    throw new Exception("Não é permitido analisar um alvo que não esteja no status de 'Refreshed'");

                alvo.Anuncio = alvo.SiteOrigem.ExtrairAnuncio(alvo);

                var perc = Math.Round(alvo.Anuncio.PercentualSucesso);
                alvo.Status = string.Format("[{0}]a", perc);
            }
            catch (Exception ex)
            {
                alvo.Status = "[e]a";
                alvo.UltimaExcecao = Utils.ObterExcecoes(ex);
            }

            return alvo;
        }
    }
}

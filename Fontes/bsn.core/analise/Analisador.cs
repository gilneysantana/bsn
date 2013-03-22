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

                if (alvo.Anuncio.PercentualSucesso != 100m)
                    throw new Exception("Percentual de Sucesso inaceitável: " + alvo.Anuncio.PercentualSucesso);

                alvo.Status = "a";
            }
            catch (Exception ex)
            {
                alvo.Status = "ae";
                alvo.UltimaExcecao = Utils.ObterExcecoes(ex);
            }

            return alvo;
        }
    }
}

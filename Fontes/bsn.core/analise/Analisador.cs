using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using bsn.core.utils;

namespace bsn.core.analise
{
    public class Analisador
    {
        public Alvo Analisar(Alvo alvo)
        {
            Trace.Assert(alvo != null, "O alvo passado como parâmetro não pode ser nulo");
            string status = "/";

            try
            {
                if (alvo.Status != "r")
                    throw new Exception("Não é permitido analisar um alvo que não esteja no status de 'Refreshed'");

                alvo.Anuncio = alvo.SiteOrigem.ExtrairAnuncio(alvo);
                
                status += string.Format("[{0}]", Math.Round(alvo.Anuncio.PercentualSucesso));
            }
            catch (Exception ex)
            {
                status += "[e]";
                alvo.UltimaExcecao = Utils.ObterExcecoes(ex);
            }
            finally
            {
                alvo.Status = status + "a";
            }

            return alvo;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bsn.core.analise
{
    class Analisador
    {
        public Alvo Analisar(Alvo alvo)
        {
            if (alvo.SiteOrigem.isAnnouncement(alvo))
            {
                try
                {
                    alvo.Anuncio = alvo.SiteOrigem.ExtrairAnuncio(alvo);
                    alvo.HistoricoStatus += "a";
                }
                catch (Exception)
                {
                    alvo.HistoricoStatus += "e";
                }
            }
            else
            {
                alvo.HistoricoStatus += 'e';
            }

            return alvo;
        }
    }
}

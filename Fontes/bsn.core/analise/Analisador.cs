﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bsn.core.analise
{
    class Analisador
    {
        public Alvo Analisar(Alvo alvo)
        {
            if (alvo.Status != "r")
                throw new Exception("Não é permitido analisar um alvo que não esteja no status de 'Refreshed'");

            try
            {
                alvo.Anuncio = alvo.SiteOrigem.ExtrairAnuncio(alvo);
                alvo.Status = "a";
            }
            catch (Exception ex)
            {
                alvo.Status += 'e';

            }

            return alvo;
        }
    }
}

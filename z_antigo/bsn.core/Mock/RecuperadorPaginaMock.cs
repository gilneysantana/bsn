using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bsn.core;

namespace bsn.core.Mock
{
    public class RecuperadorPaginaMock : RecuperadorPagina
    {
        public override Pagina retrieve(string url)
        {
            return new Pagina("Não implementado");
        }
    }
}

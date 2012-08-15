using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace bsn.dal
{
    class RepositorioMock
    {
        public IQueryable<T> Repositorio<T>(string colecao)
        {
            return new ArrayList().AsQueryable();
        }

    }
}

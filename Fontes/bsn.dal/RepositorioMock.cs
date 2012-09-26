using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace bsn.dal
{
    public class RepositorioMock<T> : RepositorioMongoDB
    {
        private List<T> colecao;

        public RepositorioMock(List<T> colecao)
        {
            this.colecao = colecao;
        }

        public IQueryable<T> Repositorio()
        {
            return colecao.AsQueryable<T>();
        }
    }
}

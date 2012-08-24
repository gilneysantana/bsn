using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace bsn.dal
{
    public class RepositorioMock
    {
        //private List<T>

        public IQueryable<T> Repositorio<T>(List<T> colecao)
        {
            if (colecao == "sites")
            {
                var repositorioSite = new System.Collections.Generic.List<T>();

                //repositorioSite.Add();
                return repositorioSite.AsQueryable<T>();
            }

            return null;
        }

    }
}

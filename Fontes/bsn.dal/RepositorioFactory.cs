using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bsn.dal
{
    public class RepositorioFactory
    {
        public static IQueryable<T> Repositorio<T>(Colecao colecao)
        {
            string descricaoColecao = string.Empty;

            switch (colecao)
            {
                case Colecao.anuncios:
                    descricaoColecao = "anuncios";
                    break;
                case Colecao.sites:
                    descricaoColecao = "sites";
                    break;
            }

            //return new RepositorioMongoDB().Repositorio<T>(descricaoColecao);
            return new RepositorioMock().Repositorio<T>(descricaoColecao);

        }
    }

    public enum Colecao
    {
        anuncios,
        sites
    }
}

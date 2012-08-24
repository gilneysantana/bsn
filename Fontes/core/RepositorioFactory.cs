using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bsn.dal;

namespace bsn.core
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

            #region RetornaMock
            if (descricaoColecao == "sites")
            {
                var repositorioSite = new System.Collections.Generic.List<T>();

                switch (typeof(T).ToString())
                {
                    case "Site":
                        repositorioSite.Add(((T)new Site()));
                        break;
                    case "Anuncio":
                        break;
                }

                //repositorioSite.Add();
                return repositorioSite.AsQueryable<T>();
            }

            return null;

            #endregion

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

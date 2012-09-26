using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bsn.dal;

namespace bsn.core
{
    public class RepositorioFactory
    {


        public static IQueryable<Site> RepositorioSite()
        {
                var repositorioSite = new System.Collections.Generic.List<Site>();
                return new RepositorioMock<Site>(repositorioSite).Repositorio();
        }

        public static IQueryable<Anuncio> RepositorioAnuncio()
        {
                var repositorioSite = new System.Collections.Generic.List<Anuncio>();
                return new RepositorioMock<Anuncio>(repositorioSite).Repositorio();
        }
    }

    public enum EnumColecao
    {
        anuncios,
        sites
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bsn.dal;
using MongoDB.Driver;

using bsn.core.analise;

namespace bsn.core
{
    public static class Rep
    {

        public static MongoCollection<Site> Sites()
        {
                return new RepositorioMongoDB().Repositorio<Site>("sites");
        }

        public static MongoCollection<Anuncio> Anuncios()
        {
                return new RepositorioMongoDB().Repositorio<Anuncio>("anuncios");
        }
    }
}

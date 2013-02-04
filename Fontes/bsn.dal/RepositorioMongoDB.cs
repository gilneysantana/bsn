using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


namespace bsn.dal
{
    public class RepositorioMongoDB
    {
        private string connectionString = "mongodb://localhost/?safe=true";

        private MongoDatabase GetDatabase()
        {
            var server = MongoServer.Create(this.connectionString);
            return server.GetDatabase("bsn");
        }

        public MongoCollection<T> obterTodosRegistros<T>(string colecao)
        {
            return this.GetDatabase().GetCollection<T>(colecao);
        }     

        public MongoCollection<T> Repositorio<T>(string colecao)
        {
            return this.GetDatabase().GetCollection<T>(colecao);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using bsn.core.analise;

namespace bsn.core.busca
{
    public class Engine
    {
        public void Iniciar()
        {
            int tamanhoRajada = 10;

            var sitesCollection = new bsn.dal.RepositorioMongoDB().obterTodosRegistros<Site>("sites");
            var anunciosCollection = new bsn.dal.RepositorioMongoDB().obterTodosRegistros<Anuncio>("anuncios");

            foreach (var site in sitesCollection.FindAll())
            {
                Buscador buscador = new Buscador(site);

                for (int i = 0; i < tamanhoRajada; i++)
                {
                    var pagina = buscador.ProximaPagina();

                    if (pagina != null)
                    {
                        Url.urls.Add(pagina);
                    }
                }
            }

            //para cada anuncio com mais de 5 dias
            //visite novamente e verifique se está expirado

        }

        public void Parar()
        {
        }
    }
}

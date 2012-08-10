using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bsn.core
{
    public class Engine
    {
        public void Iniciar()
        {
            int tamanhoRajada = 10;

            var sitesCollection = new bsn.dal.MongoDB().obterTodosRegistros<Site>("sites");
            var anunciosCollection = new bsn.dal.MongoDB().obterTodosRegistros<Anuncio>("anuncios");

            foreach (var site in sitesCollection.FindAll())
            {
                Buscador buscador = new Buscador(site);

                for (int i = 0; i < tamanhoRajada; i++)
                {
                    var anuncio = buscador.ProximoAnuncio();

                    if (anuncio != null)
                    {
                        anunciosCollection.Insert(anuncio);
                    }
                }

                sitesCollection.Save(site);
            }

            //para cada anuncio com mais de 5 dias
            //visite novamente e verifique se está expirado

        }

        public void Parar()
        {
        }
    }
}

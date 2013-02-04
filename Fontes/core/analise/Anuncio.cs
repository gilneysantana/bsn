using System;
using System.Linq;
using bsn.dal;
using System.Collections.Generic;

using MongoDB.Driver.Linq;
using MongoDB.Bson;

namespace bsn.core.analise
{

    /**
     * Class that encapsulates information about real estate.
     * 
     * @author The BSN Team
     * 
     */
    [Serializable]
    public class Anuncio
    {
        private ObjectId id;
        private Alvo paginaOrigem;
        private string bairro;
        private int numeroQuartos;
        private decimal area;
        private TipoImovel tipoImovel;
        private TipoTransacao tipoTransacao;

        private decimal preco;

        public ObjectId Id
        {
            get { return id; }
            set { id = value; }
        }

        public Alvo PaginaOrigem
        {
            get { return paginaOrigem; }
            //set { paginaOrigem = value; }
        }

        /*
         * TODO: avaliar necessidade de armazenar os dados abaixo. acredito que
         * devemos armazenar apenas o link (ver mÃ©todo abaixo) para o anuncio
         * original. A ideia Ã© que o nosso visitante seja obrigado a clicar no link
         * da imobiliÃ¡ria (esses cliques sÃ£o uma possÃ­vel fonte de renda) para
         * entrar em contato com o vendedor. Ass.: Gilney (reescrito em UTF-8 por
         * Fernando).
         */
        private string contact;
        private string phoneNumber;

        public Anuncio()
        {
        }

        public Anuncio(Alvo paginaOrigem)
        {
            this.paginaOrigem = paginaOrigem;
        }

        public Anuncio(Alvo paginaOrigem, string bairro, int numberOfRooms, decimal area,
                decimal price, string contact, string phoneNumber)
        {
            this.bairro = bairro;
            this.numeroQuartos = numberOfRooms;
            this.area = area;
            this.preco = price;
            this.contact = contact;
            this.phoneNumber = phoneNumber;
        }

        #region Propriedades

        public TipoTransacao TipoTransacao
        {
            get { return tipoTransacao; }
            set { tipoTransacao = value; }
        }

        public decimal Area
        {
            get { return area; }
            set { area = value; }
        }

        public string Bairro 
        {
            get
            {
                return bairro;
            }
            set
            {
                this.bairro = value;
            }
        }

        public int NumeroQuartos
        {
            get
            {
                return numeroQuartos;
            }
            set
            {
                numeroQuartos = value; 
            }
        }

        public decimal Preco 
        {
            get
            {
            return preco;
            }
            set
            {
                this.preco = value;
            }
        }

        public string getLinkAd()
        {
            // TODO: devemos guardar o link ou apenas o codigo e montar o link sob
            // demanda?
            return "www.infonet.com.br?codigo=99999";
        }

        public TipoImovel TipoImovel
        {
            get { return tipoImovel; }
            set { tipoImovel = value; }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("<Anuncio  Bairro = '{0}' Preço = '{1}'/>", 
                this.Bairro, this.Preco);
        }

        public static int ObterMaxCodigoAnuncio(string site)
        {
            return (from a in Rep.Anuncios().AsQueryable()
                    select a).Max<Anuncio>(a => a.NumeroQuartos);
        }

        public static Anuncio Parse(string anuncioCSV)
        {
            Anuncio anuncio = new Anuncio();

            return anuncio;
        }
    }

    public enum TipoImovel
    {
        Casa,
        Apartamento
    }

    public enum TipoTransacao
    {
        Venda,
        Aluguel
    }
}

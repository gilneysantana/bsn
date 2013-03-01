using System;
using System.Linq;
using System.Collections.Generic;

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
        private Alvo alvo;
        private string bairro;
        private int numeroQuartos;
        private decimal area;
        private TipoImovel tipoImovel;
        private TipoTransacao tipoTransacao;

        private decimal preco;

        public Alvo PaginaOrigem
        {
            get { return alvo; }
        }

        public Anuncio()
        {
        }

        public Anuncio(Alvo paginaOrigem)
        {
            this.alvo = paginaOrigem;
        }

        public Anuncio(Alvo paginaOrigem, string bairro, int numberOfRooms, decimal area,
                decimal price)
        {
            this.bairro = bairro;
            this.numeroQuartos = numberOfRooms;
            this.area = area;
            this.preco = price;
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

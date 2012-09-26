using System;
using System.Linq;
using bsn.dal;

namespace bsn.core
{

    /**
     * Class that encapsulates information about real estate.
     * 
     * @author The BSN Team
     * 
     */
    public class Anuncio
    {
        private string bairro;
        private int numeroQuartos;
        private decimal area;
        private decimal preco;
        private string link;

        public string Link
        {
            get { return link; }
            set { link = value; }
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

        public Anuncio(string bairro, int numberOfRooms, decimal area,
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

        public int getNumberOfRooms()
        {
            return numeroQuartos;
        }

        public void setNumberOfRooms(int numberOfRooms)
        {
            this.numeroQuartos = numberOfRooms;
        }

        public decimal getArea()
        {
            return area;
        }

        public void setArea(decimal area)
        {
            this.area = area;
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

        public string getContact()
        {
            return contact;
        }

        public void setContact(string contact)
        {
            this.contact = contact;
        }

        public string getPhoneNumber()
        {
            return phoneNumber;
        }

        public void setPhoneNumber(string phoneNumber)
        {
            this.phoneNumber = phoneNumber;
        }

        public string getLinkAd()
        {
            // TODO: devemos guardar o link ou apenas o codigo e montar o link sob
            // demanda?
            return "www.infonet.com.br?codigo=99999";
        }

        #endregion

        public override string ToString()
        {
            return string.Format("<Anuncio  Bairro = '{0}' Preço = '{1}'/>", this.Bairro, this.Preco);
        }

        public static int ObterMaxCodigoAnuncio(string site)
        {
            return (from a in RepositorioFactory.Repositorio<Anuncio>(EnumColecao.anuncios)
                    select a).Max<Anuncio>(a => a.getNumberOfRooms());
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;

using bsn.core.utils;

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
        private Alvo Alvo {get; set;}
        private string bairro;
        private int numeroQuartos;
        private decimal area;
        private TipoImovel tipoImovel = TipoImovel.NaoInformado;
        private TipoTransacao tipoTransacao = TipoTransacao.NaoInformado;

        private decimal preco;

        private Anuncio()
        {
        }

        public Anuncio(Alvo paginaOrigem)
        {
            this.Alvo = paginaOrigem;
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

        public string TipoTransacaoBD
        {
            get 
            {
                switch (this.TipoTransacao)
                {
                    case analise.TipoTransacao.Aluguel:
                        return "a";
                    case analise.TipoTransacao.Venda:
                        return "v";
                    default:
                        throw new Exception("TipoTransacao não tem representação em DB");
                }
            }
        }

        public string TipoImovelBD
        {
            get 
            {
                switch (this.TipoImovel)
                {
                    case analise.TipoImovel.Apartamento:
                        return "a";
                    case analise.TipoImovel.Casa:
                        return "c";
                    default:
                        throw new Exception("TipoImovel não tem representação em DB");
                }
            }
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

        #region Persistencia/Stream

        public static string CabecalhoCSV()
        {
            return @"""SiteOrigem"", ""Id"", ""HistoricoStatus"", ""Anuncio"", ""DuracaoVisita"", ""UltimaVisita"", ""RetornoRequisicao"", ""LinkVisitado""";
        }

        public static Anuncio Parse(string anuncioCSV)
        {
            anuncioCSV = anuncioCSV.Replace("\",\"", "\"^\"");
            var campos = anuncioCSV.Split('^');

            string preco = campos[0].Trim('"', ' ');
            string area = campos[1].Trim('"', ' ');
            string bairro = campos[2].Trim('"', ' ');
            string alvoSite = campos[3].Trim('"', ' ');
            int alvoId = Convert.ToInt32(campos[4].Trim('"', ' '));

            var retorno = new Anuncio();
            retorno.Preco = Convert.ToDecimal(preco);
            retorno.Area = Convert.ToDecimal(area);
            retorno.Bairro = bairro;
            retorno.Alvo = new Alvo(new Site(alvoSite), alvoId);
            return retorno;
        }

        public string ToCSV()
        {
            return Utils.ToCSV(this.Preco, this.Area, this.Bairro, this.Alvo.SiteOrigem.Nome, this.Alvo.Id);
        }

        public static Anuncio Parse(System.Data.DataRow anuncioRow)
        {
            var siteOrigem = Site.GetSitePorNome(anuncioRow["siteOrigem"].ToString());
            var id = Convert.ToInt32(anuncioRow["id"]);

            var ret = new Anuncio(new Alvo(siteOrigem, id));
            ret.Bairro = anuncioRow["bairro"].ToString();
            ret.Area = Convert.ToDecimal(anuncioRow["area"].ToString());
            ret.NumeroQuartos = Convert.ToInt32(anuncioRow["numeroQuartos"].ToString());
            ret.Preco = Convert.ToDecimal(anuncioRow["preco"].ToString());

            string ti = anuncioRow["tipoImovel"].ToString();
            ret.TipoImovel = (ti == "a") ? TipoImovel.Apartamento : TipoImovel.Casa;
            string tt = anuncioRow["tipoTransacao"].ToString();
            ret.TipoTransacao = (tt == "a") ? TipoTransacao.Aluguel : TipoTransacao.Venda;

            return ret;
        }

        public void SqliteSalvar()
        {
            if (this.Alvo == null)
                throw new ApplicationException("Não foi possível persitir o Anuncio. A propriedade 'Alvo' é null");

            var anuncio = Anuncio.SqliteFind(
                this.Alvo.SiteOrigem.Nome, this.Alvo.Id);

            var db = utils.Utils.DB();
            var campos = new Dictionary<string, string>();
            campos.Add("siteOrigem", this.Alvo.SiteOrigem.Nome);
            campos.Add("id", this.Alvo.Id.ToString());
            campos.Add("bairro", this.Bairro);
            campos.Add("preco", this.Preco.ToString());
            campos.Add("area", this.Area.ToString());
            campos.Add("numeroQuartos", this.NumeroQuartos.ToString());
            campos.Add("tipoImovel", this.TipoImovelBD);
            campos.Add("tipoTransacao", this.TipoTransacaoBD);

            if (anuncio == null)
            {
                db.Insert("anuncio", campos);
            }
            else
            {
                string where = string.Format("siteOrigem = '{0}' and id = '{1}'",
                    this.Alvo.SiteOrigem.Nome, this.Alvo.Id);
                db.Update("anuncio", campos, where);
            }
        }

        public static Anuncio SqliteFind(string site, int id)
        {
            var db = utils.Utils.DB();
            string sql = string.Format(
                @"select * 
                  from anuncio 
                  where siteOrigem = '{0}' and id = {1}", site, id);

            var dt = db.GetDataTable(sql);

            if (dt.Rows.Count == 0)
                return null;

            if (dt.Rows.Count > 1)
                throw new Exception("Base inconsistente");

            return Anuncio.Parse(dt.Rows[0]);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("<Anuncio  Bairro = '{0}' Preço = '{1}'/>", 
                this.Bairro, this.Preco);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Anuncio))
                return false;

            Anuncio outro = (Anuncio)obj;
            bool igual = true;

            if (this.Alvo.SiteOrigem.Nome != outro.Alvo.SiteOrigem.Nome)
                igual = false;

            if (this.Alvo.Id != outro.Alvo.Id)
                igual = false;

            if (this.area != outro.Area)
                igual = false;

            return igual;
        }

    }

    public enum TipoImovel
    {
        NaoInformado,
        Casa,
        Apartamento
    }

    public enum TipoTransacao
    {
        NaoInformado,
        Venda,
        Aluguel
    }
}

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
        private TipoImovel tipoImovel = TipoImovel.NI;
        private TipoTransacao tipoTransacao = TipoTransacao.NI;

        private decimal preco;

        private Anuncio()
        {
        }

        public Anuncio(Alvo alvoOrigem)
        {
            this.Alvo = alvoOrigem;
        }

        public Anuncio(Alvo alvoOrigem, string bairro, int numberOfRooms, 
            decimal area, decimal price)
        {
            this.bairro = bairro;
            this.numeroQuartos = numberOfRooms;
            this.area = area;
            this.preco = price;
        }

        #region Propriedades

        public TipoImovel TipoImovel
        {
            get { return tipoImovel; }
            set { tipoImovel = value; }
        }

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

        public decimal PercentualSucesso
        {
            get
            {
                int sucesso = 0;

                if (Bairro != "") sucesso++;
                if (TipoImovel != TipoImovel.IN) sucesso++;
                if (TipoTransacao != TipoTransacao.IN) sucesso++;
                if (Preco != -1) sucesso++;
                if (Area != -1) sucesso++;

                return Math.Round((sucesso / 5m) * 100, 2);
            }
        }

        #endregion

        #region Persistencia/Stream

        public static string CabecalhoCSV()
        {
            return @"""SiteOrigem"", ""Id"", ""HistoricoStatus"", ""Anuncio"", ""DuracaoVisita"", ""UltimaVisita"", ""RetornoRequisicao"", ""LinkVisitado""";
        }

        public static Anuncio FromCSV(string anuncioCSV)
        {
            if (string.IsNullOrEmpty(anuncioCSV))
                return null;

            var campos = Utils.FromCSV(anuncioCSV);

            string preco = campos[0];
            string area = campos[1];
            string bairro = campos[2];
            string alvoSite = campos[3];
            int alvoId = Convert.ToInt32(campos[4]);
            string imovel = campos[5];
            string transacao = campos[6];

            var retorno = new Anuncio();
            retorno.Preco = Convert.ToDecimal(preco);
            retorno.Area = Convert.ToDecimal(area);
            retorno.Bairro = bairro;
            retorno.Alvo = new Alvo(new Site(alvoSite), alvoId);
            retorno.TipoImovel 
                = (TipoImovel) Enum.Parse(typeof(TipoImovel), imovel);
            retorno.TipoTransacao 
                = (TipoTransacao) Enum.Parse(typeof(TipoTransacao), transacao);
            return retorno;
        }

        public string ToCSV()
        {
            return Utils.ToCSV(this.Preco, this.Area, this.Bairro, 
                this.Alvo.SiteOrigem.Nome, this.Alvo.Id, this.TipoImovel,
                this.TipoTransacao);
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
            ret.TipoImovel = (ti == "a") ? TipoImovel.AP : TipoImovel.CS;
            string tt = anuncioRow["tipoTransacao"].ToString();
            ret.TipoTransacao = (tt == "a") ? TipoTransacao.AL : TipoTransacao.VD;

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
            campos.Add("tipoImovel", this.TipoImovel.ToString());
            campos.Add("tipoTransacao", this.TipoTransacao.ToString());

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
        NI,
        AP,
        CS,
        IN
    }

    public enum TipoTransacao
    {
        NI,
        AL,
        VD,
        IN
    }
}

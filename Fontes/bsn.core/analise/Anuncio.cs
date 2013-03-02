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

        #region Persistencia/Stream

        public static string CabecalhoCSV()
        {
            return @"""SiteOrigem"", ""Id"", ""HistoricoStatus"", ""Anuncio"", ""DuracaoVisita"", ""UltimaVisita"", ""RetornoRequisicao"", ""LinkVisitado""";
        }

        public static Anuncio Parse(string anuncioCSV)
        {
            anuncioCSV = anuncioCSV.Replace("\",\"", "\"^\"");
            var campos = anuncioCSV.Split('^');

            //string nomeSite = campos[0].Trim('"', ' ');
            //int id = Convert.ToInt32(campos[1].Trim('"', ' '));
            //string historico = campos[2].Trim('"', ' ');
            //string duracao = campos[4].Trim('"', ' ');
            //string ultimaVisita = campos[5].Trim('"', ' ');
            //string retornoReq = campos[6].Trim('"', ' ');
            //string linkVisitado = campos[7].Trim('"', ' ');

            var anuncio = new Anuncio(); 
            //alvo.HistoricoStatus = historico;
            //alvo.Anuncio = Anuncio.Parse(campos[3]);
            //alvo.DuracaoVisita = TimeSpan.FromSeconds(Convert.ToDouble(duracao));
            //alvo.UltimaVisita = DateTime.Parse(ultimaVisita);
            //alvo.RetornoRequisicao = retornoReq;
            //alvo.LinkVisitado = linkVisitado;

            return anuncio;
        }

        public string ToCSV()
        {
            return string.Format(@"""{0}"",""{1}"",""{2}""",
                this.Preco, this.Area, this.Bairro);
        }

        public static Anuncio Parse(System.Data.DataRow anuncioRow)
        {
            var retorno = new Anuncio();
            //retorno.Id = Convert.ToInt32(anuncioRow["id"]);
            //retorno.RetornoRequisicao = anuncioRow["retornoRequisicao"].ToString();
            //retorno.LinkVisitado = anuncioRow["linkVisitado"].ToString();
            //retorno.HistoricoStatus = anuncioRow["historicoStatus"].ToString();
            //retorno.SiteOrigem = Site.GetSitePorNome(anuncioRow["siteOrigem"].ToString());
            return retorno;
        }

        public void SqliteSalvar()
        {
            var alvoExistente = new Anuncio(); // buscar do banco

            var db = utils.Utils.DB();
            var campos = new Dictionary<string, string>();
            campos.Add("siteOrigem", this.alvo.SiteOrigem.Nome);
            campos.Add("id", this.alvo.Id.ToString());
            campos.Add("bairro", this.Bairro);
            campos.Add("preco", this.Preco.ToString());

            if (alvoExistente == null)
            {
                db.Insert("alvo", campos);
            }
            else
            {
                string where = string.Format("siteOrigem = '{0}' and id = '{1}'",
                    this.alvo.SiteOrigem.Nome, this.alvo.Id);
                db.Update("alvo", campos, where);
            }
        }

        public static Alvo SqliteFind(string site, int id)
        {
            var db = utils.Utils.DB();
            string sql = string.Format(
                @"select * 
                  from alvo 
                  where siteOrigem = '{0}' and id = {1}", site, id);

            var dt = db.GetDataTable(sql);

            if (dt.Rows.Count == 0)
                return null;

            if (dt.Rows.Count > 1)
                throw new Exception("Base inconsistente");

            return Alvo.Parse(dt.Rows[0]);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("<Anuncio  Bairro = '{0}' Preço = '{1}'/>", 
                this.Bairro, this.Preco);
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

using System;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Collections;

using bsn.core.busca;
using bsn.core.analise;
using bsn.dal.sqlite;   

namespace bsn.core
{
    /**
     * Abstraction of a web page
     * 
     * @author Gilney 
     */
    [Serializable]
    public class Alvo
    {
        private SQLiteDatabase sqlLiteDB = null;

        public SQLiteDatabase SqlLiteDB
        {
            get { return sqlLiteDB; }
            set { sqlLiteDB = value; }
        }
        public static IList<Alvo> urls = new List<Alvo>();

        private Site siteOrigem;
        private Anuncio anuncio;
        private int id;
        private string linkVisitado = string.Empty;
        private string historicoStatus = "n"; // e = erro http; p = pagina do site; a = anuncio; x = anuncio expirado/removido // q = qualquer página que não seja anuncio; c = cancelado
        private string retornoRequisicao = string.Empty;
        private DateTime ultimaVisita = DateTime.MinValue;
        private string ultimaExcecao = string.Empty;

        public TimeSpan DuracaoVisita { get; set; }
        public DateTime UltimaVisita
        {
            get { return ultimaVisita; }
            set { ultimaVisita = value; }
        } 

        #region Construtores
        private Alvo()
        {
        }

        public Alvo(Site site, int id)
        {
            this.SiteOrigem = site;
            this.id = id;
        }

        public Alvo(string nomeSite, int id)
        {
            this.SiteOrigem = Site.GetSitePorNome(nomeSite);
            this.id = id;
        }
        
        #endregion

        #region Propriedades
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string UltimaExcecao
        {
            get { return ultimaExcecao; }
            set { ultimaExcecao = value; }
        }

        public string HistoricoStatus
        {
            get { return historicoStatus; }
            set { historicoStatus = value; }
        }

        public string Status
        {
            get
            {
                return HistoricoStatus[HistoricoStatus.Length - 1].ToString();
            }

            set
            {
                this.historicoStatus += value;
            }
        }

        public string LinkVisitado
        {
            get { return linkVisitado; }
            set { linkVisitado = value; }
        }

        public Site SiteOrigem
        {
            get { return siteOrigem; }
            set { siteOrigem = value; }
        }

        /**
         * Returns the <code>Page</code>'s content.
         * 
         * @return {@link string} <code>Page</code>'s content
         */
        public string RetornoRequisicao
        {
            get
            {
                return retornoRequisicao;
            }
            set
            {
                this.retornoRequisicao = value;
            }
        }

        public Anuncio Anuncio
        {
            get { return anuncio; }
            set { anuncio = value; }
        }
        #endregion

        public bool isValidPage()
        {
            return this.SiteOrigem.isValidPage(this);
        }

        public string GetLink()
        {
            return this.SiteOrigem.GetUrlMontada(this.Id);
        }

        #region Persistencia/Stream

        public static string CabecalhoCSV()
        {
            return @"""SiteOrigem"", ""Id"", ""HistoricoStatus"", ""Anuncio"", ""DuracaoVisita"", ""UltimaVisita"", ""RetornoRequisicao"", ""LinkVisitado""";
        }

        public static Alvo FromCSV(string alvoCSV)
        {
            var campos = utils.Utils.FromCSV(alvoCSV);

            var alvo = new Alvo(campos[0], Convert.ToInt32(campos[1]));
            alvo.HistoricoStatus = campos[2];
            alvo.Anuncio = Anuncio.FromCSV(campos[3]);
            alvo.DuracaoVisita = TimeSpan.FromSeconds(Convert.ToDouble(campos[4]));
            alvo.UltimaVisita = DateTime.Parse(campos[5]);
            alvo.RetornoRequisicao = campos[6];
            alvo.LinkVisitado = campos[7];

            return alvo;
        }

        public string ToCSV()
        {
            string anuncio = this.Anuncio != null ? this.Anuncio.ToCSV() : "";
            string duracaoVisita = this.DuracaoVisita.TotalSeconds.ToString("F3");

            return utils.Utils.ToCSV(this.SiteOrigem, this.Id.ToString(), this.HistoricoStatus, anuncio, 
                    duracaoVisita, this.UltimaVisita, this.RetornoRequisicao,
                    this.LinkVisitado);
        }

        public static Alvo Parse(System.Data.DataRow alvoRow)
        {
            var siteOrigem = Site.GetSitePorNome(alvoRow["siteOrigem"].ToString());
            var id = Convert.ToInt32(alvoRow["id"]);
            Alvo retorno = new Alvo(siteOrigem, id);
            retorno.RetornoRequisicao = alvoRow["retornoRequisicao"].ToString();
            retorno.LinkVisitado = alvoRow["linkVisitado"].ToString();
            retorno.HistoricoStatus = alvoRow["historicoStatus"].ToString();
            return retorno;
        }

        public void SqliteSalvar()
        {
            var alvoExistente = Alvo.SqliteFind(this.SiteOrigem.Nome, this.Id);

            var db = utils.Utils.DB();
            var campos = new Dictionary<string, string>();
            campos.Add("siteOrigem", this.SiteOrigem.Nome);
            campos.Add("id", this.Id.ToString());
            campos.Add("retornoRequisicao", this.RetornoRequisicao);
            campos.Add("linkVisitado", this.LinkVisitado);
            campos.Add("historicoStatus", this.HistoricoStatus);

            if (alvoExistente == null)
            {
                db.Insert("alvo", campos);
            }
            else
            {
                string where = string.Format("siteOrigem = '{0}' and id = '{1}'", 
                    this.SiteOrigem.Nome, this.Id);
                db.Update("alvo", campos, where);
            }

            if (this.Anuncio != null)
                this.Anuncio.SqliteSalvar();
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
            return string.Format("({0}, {1})", 
                this.SiteOrigem.Nome, this.Id); 
        }
    }

}
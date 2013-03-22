using System;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Data;

using bsn.core.busca;
using bsn.core.analise;
using bsn.core.utils;
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

        public static string CabecalhoCSV()
        {
            return @"""SiteOrigem"", ""Id"", ""HistoricoStatus"", ""DuracaoVisita"", ""UltimaVisita"", ""RetornoRequisicao"", ""LinkVisitado"", ""Anuncio"", ""UltimaExcecao""";
        }

        public static Alvo FromCSV(string alvoCSV)
        {
            var campos = Utils.FromCSV(alvoCSV);

            if (campos.Length != 9)
                throw new Exception(string.Format(
                    "A string '{0}' deveria retornar um array de 9 itens. Retornou {1}.",
                    alvoCSV, campos.Length));

            Alvo alvo;

            try
            {
                alvo = new Alvo(campos[0], Convert.ToInt32(campos[1]));
                alvo.HistoricoStatus = campos[2];
                alvo.DuracaoVisita = TimeSpan.FromSeconds(Convert.ToDouble(campos[3]));
                alvo.UltimaVisita = DateTime.Parse(campos[4]);
                alvo.RetornoRequisicao = campos[5];
                alvo.LinkVisitado = campos[6];
                alvo.Anuncio = Anuncio.FromCSV(campos[7]);
                alvo.UltimaExcecao = campos[8];
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro ao parsear a strind '{0}'. Array gerado tem tamanho {1}.",
                    alvoCSV, campos.Length), ex);
            }

            return alvo;
        }

        public string ToCSV()
        {
            string anuncio = this.Anuncio != null ? this.Anuncio.ToCSV() : "";
            string duracaoVisita = this.DuracaoVisita.TotalSeconds.ToString("F3");

            return Utils.ToCSV(this.SiteOrigem, this.Id.ToString(), this.HistoricoStatus, 
                    duracaoVisita, this.UltimaVisita, this.RetornoRequisicao,
                    this.LinkVisitado, anuncio, this.UltimaExcecao);
        }

        #region Persistencia
        public static Alvo Parse(System.Data.DataRow alvoRow)
        {
            Alvo retorno = null;
            var siteOrigem = Site.GetSitePorNome(alvoRow["siteOrigem"].ToString());
            var id = Convert.ToInt32(alvoRow["id"]);
            retorno = new Alvo(siteOrigem, id);
            retorno.RetornoRequisicao = alvoRow["retornoRequisicao"].ToString();
            retorno.LinkVisitado = alvoRow["linkVisitado"].ToString();
            retorno.HistoricoStatus = alvoRow["historicoStatus"].ToString();
            retorno.UltimaExcecao = alvoRow["ultimaExcecao"].ToString();
            var ultimaVisita = alvoRow["ultimaVisita"].ToString();
            if (!string.IsNullOrEmpty(ultimaVisita))
                retorno.UltimaVisita = Convert.ToDateTime(ultimaVisita);

            return retorno;
        }

        public void SqliteSalvar()
        {
            var alvoExistente = Alvo.SqliteFind(this.SiteOrigem.Nome, this.Id);

            var db = Utils.DB();
            var campos = new Dictionary<string, string>();
            campos.Add("siteOrigem", this.SiteOrigem.Nome);
            campos.Add("id", this.Id.ToString());
            campos.Add("retornoRequisicao", this.RetornoRequisicao);
            campos.Add("linkVisitado", this.LinkVisitado);
            campos.Add("historicoStatus", this.HistoricoStatus);
            campos.Add("ultimaVisita", this.UltimaVisita.ToString("yyyy-MM-dd HH:mm:ss"));
            campos.Add("ultimaExcecao", this.UltimaExcecao);

            if (alvoExistente == null)
            {
                db.Insert("alvo", campos);
            }
            else
            {
                if (!this.HistoricoStatus.StartsWith(alvoExistente.HistoricoStatus))
                    throw new ApplicationException(string.Format("Não é permitido atualizar um alvo com histórico menor que o atual. Atual: '{0}'; Nova: '{1}'",
                        alvoExistente.HistoricoStatus, this.HistoricoStatus));

                string where = string.Format("siteOrigem = '{0}' and id = '{1}'", 
                    this.SiteOrigem.Nome, this.Id);
                db.Update("alvo", campos, where);
            }

            if (this.Anuncio != null)
                this.Anuncio.SqliteSalvar();
        }

        public static Alvo SqliteFind(string site, int id)
        {
            var db = Utils.DB(); 
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

        public static IList<Alvo> SqliteFind(string site)
        {
            var db = Utils.DB(); 
            string sql = string.Format(
                @"select * 
                  from alvo 
                  where siteOrigem = '{0}'", site);

            var dt = db.GetDataTable(sql);
            var alvos = new List<Alvo>();

            foreach (DataRow row in dt.Rows)
            {
                alvos.Add(Alvo.Parse(row));
            }

            return alvos;
        }
        #endregion

        public override string ToString()
        {
            return string.Format("({0}, {1})", 
                this.SiteOrigem.Nome, this.Id); 
        }

        public override bool Equals(object obj)
        {
            var outro = (Alvo)obj;
            bool retorno = true;

            if (this.SiteOrigem.Nome != outro.SiteOrigem.Nome) retorno = false;
            if (this.Id != outro.Id) retorno = false;

            return retorno;
        }
    }

}
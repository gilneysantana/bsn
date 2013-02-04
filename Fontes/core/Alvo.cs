using System;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Management.Automation;
using System.Management.Automation.Host;

using bsn.core.busca;
using bsn.core.analise;

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
        public static IList<Alvo> urls = new List<Alvo>();

        private Site siteOrigem;
        private int id;
        private string linkVisitado;
        private string historicoStatus = "n"; // e = erro http; p = pagina do site; a = anuncio; x = anuncio expirado/removido // q = qualquer página que não seja anuncio; c = cancelado
        private string retornoRequisicao = string.Empty;
        private Anuncio anuncio;

        private DateTime ultimaVisita = DateTime.MinValue;
        public TimeSpan DuracaoVisita { get; set; }

        public DateTime UltimaVisita
        {
            get { return ultimaVisita; }
            set { ultimaVisita = value; }
        } 

        #region Construtores
        public Alvo()
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

        public void CarregarDoArquivo(string nomeArquivo)
        {
            this.retornoRequisicao = ExtractContentFromFile(nomeArquivo);
        }

        #region Propriedades
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string HistoricoStatus
        {
            get { return historicoStatus; }
            set { historicoStatus = value; }
        }

        public char Status
        {
            get
            {
                return HistoricoStatus[HistoricoStatus.Length - 1];
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

        private static string ExtractContentFromFile(string fileName)
        {
            string caminhoCompleto = @"C:\projetos\bsn\Fontes\bsn.testes\dados\" + fileName;

            if (!File.Exists(caminhoCompleto))
                throw new ApplicationException(string.Format("O arquivo '{0}' "
                    + "não existe.", caminhoCompleto));

            string retorno = string.Empty;

            if (fileName.ToUpper().Contains("FELIZOLA") || 
                fileName.ToUpper().Contains("ZELAR"))
            {
                retorno = File.ReadAllText(caminhoCompleto, 
                    Encoding.GetEncoding("iso-8859-1"));
            }
            else
            {
                retorno = File.ReadAllText(caminhoCompleto);
            }

            return retorno;
        }

        public bool isValidPage()
        {
            return this.SiteOrigem.isValidPage(this);
        }

        public string GetLink()
        {
            return this.SiteOrigem.GetUrlMontada(this.Id);
        }

        public string ToCSV()
        {
            string anuncio = this.Anuncio != null ? this.Anuncio.ToString() : "";
            string duracaoVisita = this.DuracaoVisita.TotalSeconds.ToString("F3");

            return string.Format(@"""{0}"",""{1}"",""{2}"",""{3}"",""{4}"",""{5}"",""{6}""", 
                this.SiteOrigem, this.Id.ToString(), this.HistoricoStatus, anuncio, 
                    duracaoVisita, this.UltimaVisita, this.RetornoRequisicao); 
           
        }

        public static string CabecalhoCSV()
        {
            return @"""SiteOrigem"", ""Id"", ""HistoricoStatus"", ""Anuncio"", ""DuracaoVisita"", ""UltimaVisita"", ""RetornoRequisicao""";
        }

        public static Alvo Parse(string alvoCSV)
        {
            alvoCSV = alvoCSV.Replace("\",\"", "\"^\"");
            var campos = alvoCSV.Split('^');

            string nomeSite = campos[0].Trim('"', ' ');
            int id = Convert.ToInt32(campos[1].Trim('"', ' '));
            string historico = campos[2].Trim('"', ' ');
            string duracao = campos[4].Trim('"', ' ');
            string ultimaVisita = campos[5].Trim('"', ' ');
            string retornoReq = campos[6].Trim('"', ' ');

            var alvo = new Alvo(nomeSite, id);
            alvo.HistoricoStatus = historico;
            alvo.Anuncio = Anuncio.Parse(campos[3]);
            alvo.DuracaoVisita = TimeSpan.FromSeconds(Convert.ToDouble(duracao));
            alvo.UltimaVisita = DateTime.Parse(ultimaVisita);
            alvo.RetornoRequisicao = retornoReq;

            return alvo;
        }
    }

}
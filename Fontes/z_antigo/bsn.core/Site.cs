using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;

using bsn.core.analise;
using bsn.core.utils;
using bsn.dal.sqlite;   


namespace bsn.core
{
    /**
     * This class will be the persisted in our site's catalog
     * 
     * @author gilney
     * 
     */
    public class Site
    {
        /**
         * Place holder used in the url template.
         */
        //public static string PLACE_HOLDER = "${PLACE_HOLDER}";
        //todo: excluir linha acima

        #region Variáveis privadas
        private string nome;
        private string templateURL;
        private string announcementSignature;
        private string expiredAnnouncementSignature;
        private string removedAnnouncementSignature;
        private string pageNotFoundSignature;
        private string regexBairro;
        private string regexPreco;
        private string regexArea;
        private string regexTipoImovel;
        private string regexTipoTransacao;
        #endregion

        public Site(string nome)
        {
            this.Nome = nome;
        }

        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        public string RegexPreco
        {
            get { return regexPreco; }
            set { regexPreco = value; }
        }

        public string RegexBairro
        {
            get { return regexBairro; }
            set { regexBairro = value; }
        }

        public string RegexTipoImovel
        {
            get { return regexTipoImovel; }
            set { regexTipoImovel = value; }
        }

        public string RegexTipoTransacao
        {
            get { return regexTipoTransacao; }
            set { regexTipoTransacao = value; }
        }

        public string TemplateUrl
        {
            get
            {
                return templateURL;
            }
            set
            {
                this.templateURL = value;
            }
        }

        public string AnnoucementeSignature
        {
            get
            {
                return announcementSignature;
            }
            set
            {
                announcementSignature = value;
            }
        }

        public string ExpiredAnnouncementSignature
        {
            get
            {
                return expiredAnnouncementSignature;
            }
            set
            {
                this.expiredAnnouncementSignature = value;
            }
        }

        public string RemovedAnnouncementSignature
        {
            get
            {
                return removedAnnouncementSignature;
            }
            set
            {
                this.removedAnnouncementSignature = value;
            }
        }

        public string PageNotFoundSignature
        {
            get
            {
                return pageNotFoundSignature;
            }
            set
            {
                this.pageNotFoundSignature = value;
            }
        }

        public IList<string> getTextsInIgnoredPages()
        {
            IList<string> textsInIgnoredPages = new List<string>();

            textsInIgnoredPages.Add(ExpiredAnnouncementSignature);
            textsInIgnoredPages.Add(RemovedAnnouncementSignature);
            textsInIgnoredPages.Add(PageNotFoundSignature);

            return textsInIgnoredPages;
        }

        /**
         * Return true when the Page doesn't match any Ignored signature
         * 
         * @param page
         * @return
         */
        public bool isValidPage(Alvo page)
        {
            foreach (string text in getTextsInIgnoredPages())
            {
                if (page.RetornoRequisicao.Contains(text))
                {
                    return false;
                }
            }
            return true;
        }

        public bool isPageNotFound(Alvo page)
        {
            return page.RetornoRequisicao.Contains(PageNotFoundSignature);
        }

        /**
         * returns true if page is a Announcement of site.
         * 
         * @param page
         * @return
         */
        public bool isAnnouncement(Alvo page)
        {
            if (!isValidPage(page))
            {
                return false;
            }
            return page.RetornoRequisicao.Contains(this.AnnoucementeSignature);
        }

        private string ExtrairCampo(string strRegex, Alvo pagina)
        {
            return Utils.ExtrairCampoString(strRegex, pagina.RetornoRequisicao);
        }

        private decimal ExtrairCampoDecimal(string strRegex, Alvo pagina)
        {
            var campo = Utils.ExtrairCampoString(strRegex, pagina.RetornoRequisicao);

            if (!string.IsNullOrEmpty(campo))
                return Convert.ToDecimal(campo);
            else
                return -1m;
        }

        private int ExtrairCampoInt(string strRegex, Alvo pagina)
        {
            var campo = Utils.ExtrairCampoString(strRegex, pagina.RetornoRequisicao);

            if (!string.IsNullOrEmpty(campo))
                return Convert.ToInt32(campo);
            else
                return -1;
        }

        private TipoImovel ObterTipoImovel(Alvo alvo)
        {
            string tipoImovel = ExtrairCampo(RegexTipoImovel, alvo);

            if (tipoImovel == null)
                return TipoImovel.IN;

            try
            {
                switch (tipoImovel.ToUpper())
                {
                    case "APARTAMENTOS":
                    case "APARTAMENTO":
                        return TipoImovel.AP;
                    case "CASAS":
                    case "CASA":
                    case "CASA EM CONDOMINIO":
                        return TipoImovel.CS;
                    case "SALA COMERCIAL":
                        return TipoImovel.SC;
                    case "PONTO COMERCIAL":
                        return TipoImovel.PC;
                    default:
                        return TipoImovel.IN;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Excecao ocorreu ao tentar buscar a regex #{0}# no Alvo({1},{2})",
                    RegexTipoImovel, alvo.SiteOrigem, alvo.Id), ex);
            }
        }

        private TipoTransacao ObterTipoTransacao(Alvo pagina)
        {
            string tipoTransacao = ExtrairCampo(RegexTipoTransacao, pagina);

            if (tipoTransacao == null)
                return TipoTransacao.IN;

            switch (tipoTransacao.ToUpper())
            {
                case "ALUGAR":
                case "ALUGUEL":
                    return TipoTransacao.AL;
                case "VENDER":
                case "VENDA":
                    return TipoTransacao.VD;
                default:
                    return TipoTransacao.IN;
            }
        }

        private decimal ObterPreco(Alvo alvo)
        {
            string preco = Utils.ExtrairCampoString(this.RegexPreco, alvo.RetornoRequisicao);

            if (string.IsNullOrEmpty(preco))
                return -1;

            decimal retorno;
            if (Decimal.TryParse(preco, out retorno))
            {
                return retorno;
            }
            else 
            {
                return 0;
            }
        }

        public Anuncio ExtrairAnuncio(Alvo alvo)
        {
            try
            {
                var novoAnuncio = new Anuncio(alvo);
                novoAnuncio.Bairro = this.ExtrairCampo(this.RegexBairro, alvo);
                novoAnuncio.Preco = ObterPreco(alvo);
                novoAnuncio.NumeroQuartos = this.ExtrairCampoInt(this.RegexNumeroQuartos, alvo);
                novoAnuncio.Area = ExtrairCampoDecimal(this.RegexArea, alvo);
                novoAnuncio.TipoImovel = ObterTipoImovel(alvo);
                novoAnuncio.TipoTransacao = ObterTipoTransacao(alvo);

                return novoAnuncio;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(
                    "Não foi possível extrair Anuncio do Alvo({0}).", alvo), ex);
            }
        }

        public int LastValidPageRequest { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", this.Nome);
        }

        public string RegexNumeroQuartos { get; set; }

        public static Site GetSitePorNome(string nomeSite)
        {
            var sqliteDB = Utils.DBSite();

            string sql = string.Format(@"
                select * from site where nome = '{0}'", nomeSite);

            var rows = sqliteDB.GetDataTable(sql).Rows;

            if (rows.Count != 1)
                throw new ApplicationException(string.Format(
                    "A consulta ao Site ('{1}') deveria retornar exatamente 1 registro. Retornou {0}.",
                    rows.Count, nomeSite));

            return Site.Parse(rows[0]);
        }
    
        public string RegexArea
        {
            get { return regexArea; }

            set { regexArea = value; }
        }

        public string GetUrlMontada(int id)
        {
            return string.Format(this.TemplateUrl, id.ToString()); 
        }

        private static Site Parse(DataRow row)
        {
            Site retorno = new Site(row["nome"].ToString());

            retorno.RegexBairro = row["regexBairro"].ToString();
            retorno.RegexNumeroQuartos = row["regexNumeroQuartos"].ToString();
            retorno.RegexPreco = row["regexPreco"].ToString();
            retorno.RegexArea = row["regexArea"].ToString();
            retorno.RegexTipoImovel = row["regexTipoImovel"].ToString();
            retorno.RegexTipoTransacao = row["regexTipoTransacao"].ToString();
            retorno.TemplateUrl = row["templateUrl"].ToString();

            return retorno;
        }

        public static string CabecalhoCSV()
        {
            return @"""Nome"", ""RegexPreco"", ""RegexBairro"", ""RegexTipoImovel"", ""RegexTipoTransacao""";
        }

        public string ToCSV()
        {
            return string.Format(@"""{0}"",""{1}"",""{2}"",""{3}"",""{4}""", 
                this.Nome, this.RegexPreco, this.RegexBairro, 
                this.RegexTipoImovel, this.RegexTipoTransacao); 
        }
    }
}

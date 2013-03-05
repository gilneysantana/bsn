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
        private SQLiteDatabase sqliteDB;

        public SQLiteDatabase SqliteDB
        {
            get { return sqliteDB; }
            set { sqliteDB = value; }
        }

        /**
         * Place holder used in the url template.
         */
        public static string PLACE_HOLDER = "${PLACE_HOLDER}";

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
         * Returns the URL of a specific Announcement, which is the of Announcement
         * applied to site's Template URL
         * 
         * @param announcementCode
         * */
        public string getAnnouncementURL(int announcementCode)
        {
            return this.TemplateUrl.Replace(Site.PLACE_HOLDER,
                    announcementCode.ToString());
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
            return Utils.ExtrairCampo(strRegex, pagina.RetornoRequisicao);
        }

        //private bool ExisteRegex(string strRegex, Alvo pagina)
        //{
        //    Regex regex = new Regex(strRegex, RegexOptions.IgnoreCase);
        //    return regex.IsMatch(pagina.RetornoRequisicao);
        //}

        private TipoImovel ObterTipoImovel(Alvo alvo)
        {
            string tipoImovel = ExtrairCampo(RegexTipoImovel, alvo);

            try
            {
                switch (tipoImovel.ToUpper())
                {
                    case "APARTAMENTOS":
                    case "APARTAMENTO":
                        return TipoImovel.Apartamento;
                    case "CASAS":
                    case "CASA":
                        return TipoImovel.Casa;
                    default:
                        throw new Exception(string.Format("Não foi possivel detectar"
                            + " o TipoImovel para a Pagina {0}. A extração do campo retorno '{1}'.",
                            alvo.SiteOrigem, tipoImovel));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Exceção ocorreu ao tentar buscar a regex '{0}' no Alvo({1},{2})",
                    RegexTipoImovel, alvo.SiteOrigem, alvo.Id), ex);
            }
        }

        private TipoTransacao ObterTipoTransacao(Alvo pagina)
        {
            string tipoTransacao = ExtrairCampo(RegexTipoTransacao, pagina);
            switch (tipoTransacao.ToUpper())
            {
                case "ALUGAR":
                case "ALUGUEL":
                    return TipoTransacao.Aluguel;
                case "VENDER":
                case "VENDA":
                    return TipoTransacao.Venda;
                default:
                    throw new Exception(string.Format("Não foi possível detectar o TipoTransacao para a Pagina" 
                        + " {0}. A extração do campo retornou '{1}'.", 
                        pagina.SiteOrigem, tipoTransacao));
            }
        }

        public Anuncio ExtrairAnuncio(Alvo alvo)
        {
            try
            {
                var novoAnuncio = new Anuncio(alvo);
                novoAnuncio.Bairro = this.ExtrairCampo(this.RegexBairro, alvo).Trim();
                novoAnuncio.Preco = Convert.ToDecimal(this.ExtrairCampo(this.RegexPreco, alvo));
                novoAnuncio.NumeroQuartos = Convert.ToInt32(this.ExtrairCampo(this.RegexNumeroQuartos, alvo));
                novoAnuncio.TipoImovel = ObterTipoImovel(alvo);
                novoAnuncio.TipoTransacao = ObterTipoTransacao(alvo);
                string area = ExtrairCampo(RegexArea, alvo);
                if (area != null)
                    novoAnuncio.Area = Convert.ToDecimal(area);

                return novoAnuncio;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(
                    "Erro ao tentar extrair Anuncio do Alvo({0})", alvo), ex);
            }
        }

        public int LastValidPageRequest { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", this.Nome);
        }

        public string RegexNumeroQuartos { get; set; }

        #region Repositorio
        private static IQueryable<Site> Repositorio()
        {
            var sites = new List<Site>();

            Site novoSite = new Site();

            novoSite = new Site();
            novoSite.Nome = "Felizola";
            novoSite.RegexBairro = "<span .*?>Bairro</span>: (.*?)<br>";
            novoSite.RegexNumeroQuartos = "<span .*?>Nº de Quartos:</span> (.*?)<br />";
            novoSite.RegexPreco = "<span .*?>Valor:</span> (?:R\\$)*(.*?),00<br />";
            novoSite.RegexArea = "<span.*>&Aacute;rea:.*</span><span.*>(.*)</span>";
            novoSite.RegexTipoImovel = "<span .*?>Imóvel:</span> (.*?)<br />";
            novoSite.RegexTipoTransacao = "<span .*?>Tipo de Negócio</span>: (.*?)<br>";
            novoSite.TemplateUrl = string.Format("http://felizolaimobiliaria.com.br/index.php?option=com_hotproperty&task=view&id={0}", 
                Site.PLACE_HOLDER);
            sites.Add(novoSite);

            novoSite = new Site();
            novoSite.Nome = "Zelar";
            novoSite.RegexBairro = "<b>Bairro: </b>(.*)</td>";
            novoSite.RegexNumeroQuartos = "<td>([\\d]*) - Quarto</td>";
            novoSite.RegexPreco = "Valor R\\$ <.*>(.*)</td>";
            novoSite.RegexArea = "";
            novoSite.RegexTipoImovel = "<b>Tipo: </b>(.*)</td>";
            novoSite.RegexTipoTransacao = "<b>Pretensão: </b>(.*)</td>";
            novoSite.TemplateUrl = string.Format("http://www.mostraimoveis.com.br/SE/zelar/MeusImoveis.php?txtParceiro=80&txtImovel={0}&txtEstado=&txtCidade=&txtBairro=&txtTipoImovel=&txtPretensao=&xValor=&yValor=",
                Site.PLACE_HOLDER);
            sites.Add(novoSite);


            return sites.AsQueryable<Site>();
        }

        public static Site GetSitePorNome(string nomeSite)
        {
            var sqliteDB = Utils.DB();

            string sql = string.Format(@"
                select * from site where nome = '{0}'", nomeSite);

            var rows = sqliteDB.GetDataTable(sql).Rows;

            if (rows.Count != 1)
                throw new ApplicationException(string.Format(
                    "A consulta ao Site deveria retornar exatamente 1 registro. Retornou {0}.",
                    rows.Count));

            return Site.Parse(rows[0]);
        }

        #endregion 
    
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
            Site retorno = new Site();

            retorno.Nome = row["nome"].ToString();
            retorno.RegexBairro = row["regexBairro"].ToString();
            retorno.RegexNumeroQuartos = row["regexNumeroQuartos"].ToString();
            retorno.RegexPreco = row["regexPreco"].ToString();
            retorno.RegexArea = row["regexArea"].ToString();
            retorno.RegexTipoImovel = row["regexTipoImovel"].ToString();
            retorno.RegexTipoTransacao = row["regexTipoTransacao"].ToString();
            retorno.TemplateUrl = row["templateUrl"].ToString();

            return retorno;
        }
    }
}

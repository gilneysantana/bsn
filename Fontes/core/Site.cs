using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

using bsn.dal;
using bsn.core.analise;
using bsn.core.utils;


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
        public static string PLACE_HOLDER = "${PLACE_HOLDER}";

        #region Variáveis privadas
        private ObjectId id;
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

        public ObjectId Id
        {
            get { return id; }
            set { id = value; }
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

                if (!this.templateURL.Contains(PLACE_HOLDER))
                    throw new Exception(string.Format("O TemplateUrl deve conter a tag '{0}'.", PLACE_HOLDER));
                  
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

        public Anuncio ExtrairAnuncio(Alvo pagina)
        {
            try
            {
                var novoAnuncio = new Anuncio(pagina);
                novoAnuncio.Bairro = this.ExtrairCampo(this.RegexBairro, pagina);
                novoAnuncio.Preco = Convert.ToDecimal(this.ExtrairCampo(this.RegexPreco, pagina));
                novoAnuncio.NumeroQuartos = Convert.ToInt32(this.ExtrairCampo(this.RegexNumeroQuartos, pagina));
                novoAnuncio.Area = Convert.ToDecimal(ExtrairCampo(RegexArea, pagina));
                novoAnuncio.TipoImovel = ObterTipoImovel(pagina);
                novoAnuncio.TipoTransacao = ObterTipoTransacao(pagina);
                return novoAnuncio;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro ao tentar extrair Anuncio da página {0}",
                    pagina.SiteOrigem.Nome), ex);
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
            novoSite.Nome = "Infonet";
            novoSite.AnnoucementeSignature = "Bairro:";
            novoSite.ExpiredAnnouncementSignature = "ncio expirado!";
            novoSite.PageNotFoundSignature = "o foi encontrada";
            novoSite.RegexBairro = "<span.*>Bairro:.*</span><span.*>(.*)<br>";
            novoSite.RegexNumeroQuartos = "<span.*>N&uacute;mero de quartos:.*</span><span.*>(.*)<br>";
            novoSite.RegexPreco = "<span.*>Pre&ccedil;o:.*</span><span.*>(.*)<br>";
            novoSite.RegexArea = "<span.*>&Aacute;rea:.*</span><span.*>(.*)<br>";
            novoSite.RegexTipoImovel = "<span.*class=\"navegacao\">(.*) para (?:vender|alugar)</span>";
            novoSite.RegexTipoTransacao = "<span.*>(?:Apartamentos|Casas) para (.*?)<br>";
            novoSite.RemovedAnnouncementSignature = "ncio removido!";
            novoSite.TemplateUrl = string.Format("http://classificados.infonet.com.br/ClassificadosApp/publico/retrieveAnuncioPortal.jsp?CdAnuncio={0}", 
                Site.PLACE_HOLDER);
            sites.Add(novoSite);

            novoSite = new Site();
            novoSite.Nome = "Felizola";
            novoSite.AnnoucementeSignature = "Bairro:";
            novoSite.ExpiredAnnouncementSignature = "ncio expirado!";
            novoSite.PageNotFoundSignature = "o foi encontrada";
            novoSite.RegexBairro = "<span .*?>Bairro</span>: (.*?)<br>";
            novoSite.RegexNumeroQuartos = "<span .*?>Nº de Quartos:</span> (.*?)<br />";
            novoSite.RegexPreco = "<span .*?>Valor:</span> (?:R\\$)*(.*?),00<br />";
            novoSite.RegexArea = "<span.*>&Aacute;rea:.*</span><span.*>(.*)</span>";
            novoSite.RegexTipoImovel = "<span .*?>Imóvel:</span> (.*?)<br />";
            novoSite.RegexTipoTransacao = "<span .*?>Tipo de Negócio</span>: (.*?)<br>";
            novoSite.RemovedAnnouncementSignature = "ncio removido!";
            novoSite.TemplateUrl = string.Format("http://nuncioPortal.jsp?CdAnuncio={0}", Site.PLACE_HOLDER);
            sites.Add(novoSite);

            novoSite = new Site();
            novoSite.Nome = "Zelar";
            novoSite.AnnoucementeSignature = "Bairro:";
            novoSite.ExpiredAnnouncementSignature = "";
            novoSite.PageNotFoundSignature = "";
            novoSite.RegexBairro = "<b>Bairro: </b>(.*)</td>";
            novoSite.RegexNumeroQuartos = "<td>([\\d]*) - Quarto</td>";
            novoSite.RegexPreco = "Valor R\\$ <.*>(.*)</td>";
            novoSite.RegexArea = "";
            novoSite.RegexTipoImovel = "<b>Tipo: </b>(.*)</td>";
            novoSite.RegexTipoTransacao = "<b>Pretensão: </b>(.*)</td>";
            novoSite.RemovedAnnouncementSignature = "";
            novoSite.TemplateUrl = string.Format("http://www.mostraimoveis.com.br/SE/zelar/MeusImoveis.php?txtParceiro=80&txtImovel={0}&txtEstado=&txtCidade=&txtBairro=&txtTipoImovel=&txtPretensao=&xValor=&yValor=",
                Site.PLACE_HOLDER);
            sites.Add(novoSite);


            return sites.AsQueryable<Site>();
        }

        public static Site GetSitePorNome(string site)
        {
            try
            {
                return (from a in Site.Repositorio()
                        where a.Nome == site
                        select a).First();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(
                    "Erro ao tentar recuperar Site de nome '{0}'", site), ex);
            }
        }

        #endregion 
    
        public string RegexArea
        {
            get { return regexArea; }

            set { regexArea = value; }
        }

        public string GetUrlMontada(int id)
        {
            return this.TemplateUrl.Replace(Site.PLACE_HOLDER, id.ToString());
        }

    }
}


            //novoSite.AnnoucementeSignature = "Bairro:";
            //novoSite.ExpiredAnnouncementSignature = "ncio expirado!";
            //novoSite.Nome = "Infonet";
            //novoSite.PageNotFoundSignature = "o foi encontrada";
            //novoSite.RegexBairro = "<span.*>Bairro:.*</span><span.*>(.*)</span>";
            //novoSite.RegexNumeroQuartos = "<span.*>N&uacute;mero de quartos:.*</span><span.*>(.*)</span>";
            //novoSite.RegexPreco = "<span.*>Pre&ccedil;o:.*</span><span.*>(.*)</span>";
            //novoSite.RegexArea = "<span.*>&Aacute;rea:.*</span><span.*>(.*)</span>";
            //novoSite.RegexTipoImovel = "<span.*>(.*) para (?:vender|alugar)</span>";
            //novoSite.RegexTipoTransacao = "<span.*>(?:Apartamentos|Casas) para (.*?)</span>";
            //novoSite.RemovedAnnouncementSignature = "ncio removido!";
            //novoSite.TemplateUrl = string.Format("http://classificados.infonet.com.br/ClassificadosApp/publico/retrieveAnuncioPortal.jsp?CdAnuncio={0}", 
            //    Site.PLACE_HOLDER);

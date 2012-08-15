using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

using bsn.dal;


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

        private ObjectId id;
        private string nome;
        private string templateURL;
        private string announcementSignature;
        private string expiredAnnouncementSignature;
        private string removedAnnouncementSignature;
        private string pageNotFoundSignature;
        private string regexBairro;
        private string regexPreco;

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

        public void setTemplateUrl(string templateURL)
        {
            this.templateURL = templateURL;
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
        public bool isValidPage(Pagina page)
        {
            foreach (string text in getTextsInIgnoredPages())
            {
                if (page.GetContent.Contains(text))
                {
                    return false;
                }
            }
            return true;
        }

        public bool isPageNotFound(Pagina page)
        {
            return page.GetContent.Contains(PageNotFoundSignature);
        }

        /**
         * returns true if page is a Announcement of site.
         * 
         * @param page
         * @return
         */
        public bool isAnnouncement(Pagina page)
        {
            if (!isValidPage(page))
            {
                return false;
            }
            return page.GetContent.Contains(this.AnnoucementeSignature);
        }
     
        public static Site GetSitePorNome(string nomeSite)
        {
            var collection = new bsn.dal.RepositorioMongoDB().obterTodosRegistros<Site>("sites");
            var query = Query.EQ("Nome", nomeSite);
            return collection.FindOne(query);
        }

        private string ExtrairCampo(string strRegex, Pagina pagina)
        {
            Regex regex = new Regex(strRegex, RegexOptions.IgnoreCase);
            Match retorno = regex.Match(pagina.GetContent);

            if (retorno.Groups.Count != 2)
                return null;
            else
                return retorno.Groups[1].Value;
        }

        public Anuncio ExtrairAnuncio(Pagina pagina)
        {
            var novoAnuncio = new Anuncio();
            novoAnuncio.Link = pagina.Link;
            novoAnuncio.Bairro = this.ExtrairCampo(this.RegexBairro, pagina);
            novoAnuncio.Preco = Convert.ToDecimal(this.ExtrairCampo(this.RegexPreco, pagina));
            return novoAnuncio;
        }

        public int LastValidPageRequest { get; set; }
    }
}

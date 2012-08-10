using System;

namespace bsn.core
{
    /**
     * Class that extracts real estates of a web site.
     * 
     * @author Gilney
     * 
     */
    public class Buscador
    {
        /**
         * Site to be scanned.
         */
        private Site siteAlvo;
        private int MAX_NOT_FOUND_PAGES = 2;
        private RecuperadorPagina recuperadorPagina = new RecuperadorPagina();

        private Buscador() { }

        /**
         * Initializes the object.
         * 
         * @param targetSite
         *            - the site to be scanned.
         * @param numberOfNotFoundPages
         *            - the number of not found pages found in sequence before stop.
         */
        public Buscador(Site targetSite)
        {
            this.siteAlvo = targetSite;
        }

        //public Buscador(Site targetSite, RecuperadorPagina recuperador) : this(targetSite)
        //{
        //    this.recuperadorPagina = recuperador; 
        //}

        #region Propriedades
        private Site SiteAlvo
        {
            get
            {
                return this.siteAlvo;
            }
        }

        #endregion

        /**
         * Method that returns the next real estate of the web site or null if there
         * is not a real estate to be returned.
         * 
         * @return - The next real estate of the web site or null if there is not a
         *         real estate.
         * 
         * @throws MalformedURLException
         *             - exception thrown if {@link URL}s create during the
         *             execution are malformed.
         * 
         * @throws IOException
         *             - If an I/O error occurs.
         */
        public Anuncio ProximoAnuncio()
        {
            int numberOfNotFoundPagesInSequence = 0;
            int nextRequestParameter = this.SiteAlvo.LastValidPageRequest;

            while (numberOfNotFoundPagesInSequence < MAX_NOT_FOUND_PAGES)
            {
                nextRequestParameter++;

                Pagina pagina = this.recuperadorPagina.retrieve(this.SiteAlvo.getAnnouncementURL(nextRequestParameter));

                if (!SiteAlvo.isValidPage(pagina))
                {
                    if (SiteAlvo.isPageNotFound(pagina))
                    {
                        numberOfNotFoundPagesInSequence++;
                    }
                    continue;
                }

                numberOfNotFoundPagesInSequence = 0;

                if (SiteAlvo.isAnnouncement(pagina))
                {
                    SiteAlvo.LastValidPageRequest = nextRequestParameter;

                    return this.siteAlvo.ExtrairAnuncio(pagina);
                }
            }

            return null;
        }

     
    }
}

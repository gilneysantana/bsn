using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace bsn.core
{
    public class RecuperadorPagina
    {

            /**
     * Retrieves the content of a web page at the given URL.
     * 
     * @param url
     *            {@link URL} the URL for the web page to be retrieved
     * @return {@link Page} object which represents the retrieved web page
     * @throws IOException
     *             If an I/O error occurs
     */
        public virtual Pagina retrieve(string url)
        {
            WebClient MyWebClient = new WebClient();

            Byte[] PageHTMLBytes;
            if (!string.IsNullOrEmpty(url))
            {
                PageHTMLBytes = MyWebClient.DownloadData(url);

                UTF8Encoding oUTF8 = new UTF8Encoding();
                string pagina = oUTF8.GetString(PageHTMLBytes);
                Pagina retorno = new Pagina(pagina);
                retorno.Link = url;
                return retorno;
            }

            return null;
        }

    }
}

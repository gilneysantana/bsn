using System;
using System.Text;
using System.Net;
using System.IO;
namespace bsn.core
{
    /**
     * Abstraction of a web page
     * 
     * @author Gilney 
     */
    public class Pagina
    {

        /**
         * Page's content
         */
        private string content;
        private string link;

        public string Link
        {
            get { return link; }
            set { link = value; }
        }

        /**
         * Constructor that takes the <code>Page</code>'s content.
         * 
         * @param content
         *            {@link string} <code>Page</code>'s content
         */
        public Pagina(string content)
        {
            this.content = content;
        }

        /**
         * Returns the <code>Page</code>'s content.
         * 
         * @return {@link string} <code>Page</code>'s content
         */
        public string GetContent
        {
            get
            {
                return content;
            }
        }

        public static Pagina CarregarDoArquivo(string nomeArquivo)
        {
            string caminhoCompleto = @"C:\Users\PC\Documents\Visual Studio 2010\Projects\bsn\bsn.testes\dados\" + nomeArquivo;

            if (!File.Exists(caminhoCompleto))
                throw new ApplicationException(string.Format("O arquivo '{0}' não existe.", caminhoCompleto));

            Pagina retorno = new Pagina(File.ReadAllText(caminhoCompleto));

            retorno.Link = nomeArquivo;
            return retorno;
        }
    }

}
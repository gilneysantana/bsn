using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace bsn.core.dal
{
    public class SqlSite
    {
        public Site GetSitePorNome(string noteSite)
        {
            string sql = string.Format(@"
                select * 
                from site where nome = '{0}'", noteSite);

            var sqlite = new SQLiteDatabase();
            var rowSite = sqlite.GetDataTable(sql).Rows[0];
            return RowToSite(rowSite);
        }

        private Site RowToSite(DataRow row)
        {
            Site retorno = new Site();

            retorno.Nome = row["nome"              ].ToString();
            retorno.RegexBairro = row["regexBairro"       ].ToString();
            retorno.RegexNumeroQuartos = row["regexNumeroQuartos"].ToString();
            retorno.RegexPreco = row["regexPreco"        ].ToString();
            retorno.RegexArea = row["regexArea"         ].ToString();
            retorno.RegexTipoImovel = row["regexTipoImovel"   ].ToString();
            retorno.RegexTipoTransacao = row["regexTipoTransacao"].ToString();
            retorno.TemplateUrl = row["templateUrl"       ].ToString();

            return retorno;
        }
    }
}

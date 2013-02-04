using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Driver;
using MongoDB.Driver.Linq;


namespace bsn.core
{
    public class _ConsultasAdhoc
    {
        public static object ConsultaGenerica()
        {
            return (from a in Rep.Sites().AsQueryable()
                    select a);
        }
    }
}

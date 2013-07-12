using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;

namespace bsn.core.powershell
{

    [Cmdlet(VerbsCommunications.Send, "Consultando")]
    public class GetSqlite : Cmdlet
    {
        private string name;

        [Parameter(Mandatory = true)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        protected override void ProcessRecord()
        {
            WriteObject("Hello " + name + "!");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLogs
{
    public class SearchLog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string QueryXml { get; set; }
    }
}

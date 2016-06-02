using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Web.Controllers
{
    public class Query
    {
        public Query()
        {
            ColumnNamesList = new List<string>();
            TableContent = new List<List<string>>();
        }
        public List<string> ColumnNamesList { get; set; }
        public List<List<string>> TableContent { get; set; }
    }
}

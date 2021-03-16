using System;
using System.Collections.Generic;
using System.Text;

namespace ColdStart1App.Shared
{
    public class Catalog
    {
        public Catalog()
        {
            icecreams = new List<CatalogItem>();
        }

        public List<CatalogItem> icecreams { get; set; }
    }
}

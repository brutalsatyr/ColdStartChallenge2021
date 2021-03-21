using System;
using System.Collections.Generic;
using System.Text;

namespace ColdStart1App.Shared
{
    public class RecommendationRequest
    {
        public List<CatalogItem> Catalogitems { get; set; }
        public string UserName { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ColdStart1App.Shared
{
    public class CatalogItem
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<Preorder> Orders { get; set; }

    }
}

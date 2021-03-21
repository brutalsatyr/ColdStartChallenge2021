using System;
using System.Collections.Generic;
using System.Text;

namespace ColdStart1App.Shared
{
    public class Driver
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUri { get; set; }
        public ICollection<Preorder> Orders { get; set; }
    }
}

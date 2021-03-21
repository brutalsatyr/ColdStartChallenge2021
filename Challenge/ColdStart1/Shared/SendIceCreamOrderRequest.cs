using System;
using System.Collections.Generic;
using System.Text;

namespace ColdStart1App.Shared
{
    public class SendIceCreamOrderRequest
    {
        public Preorder Preorder { get; set; }

        public bool IsRecommended { get; set; }

        public string EventId { get; set; }
    }
}

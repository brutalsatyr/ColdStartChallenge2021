﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ColdStart1App.Shared
{
    public class Preorder
    {

        public Preorder(string user, int iceCreamId)
        {
            Id = Guid.NewGuid();
            Date = DateTime.Now;
            Status = "New";
            User = user;
            IcecreamId = iceCreamId;
            FullAddress = "1 Microsoft Way, Redmond, WA 98052, USA";
        }

        public Guid Id { get; set; }
        public string User { get; set; }
        public DateTime Date { get; set; }
        public int IcecreamId { get; set; }
        public string Status { get; set; }
        public string DriverId { get; set; }
        public string FullAddress { get; set; }
        public string LastPosition { get; set; }

    }
}
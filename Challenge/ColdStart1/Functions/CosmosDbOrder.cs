using ColdStart1App.Shared;
using System;

namespace Functions
{
    public class CosmosDbOrder
    {
        public CosmosDbOrder()
        {

        }

        public CosmosDbOrder(Preorder preOrder, CatalogItem iceCream)
        {
            id = preOrder.Id;
            User = preOrder.User;
            Date = preOrder.Date;
            Status = preOrder.Status;
            FullAddress = preOrder.FullAddress;
            LastPosition = null;
            DeliveryPosition = null;
            Icecream = new CosmosCatalogItem
            {
                IcecreamId = iceCream.Id,
                Name = iceCream.Name,
                Description = iceCream.Description,
                ImageUrl = iceCream.ImageUrl
            };
            Driver = new CosmosDriver();
        }

        public Guid id { get; set; }
        public string User { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public CosmosCatalogItem Icecream { get; set; }
        public CosmosDriver Driver { get; set; }
        public string FullAddress { get; set; }
        public string LastPosition { get; set; }
        public string DeliveryPosition { get; set; }
    }

    public class CosmosCatalogItem
    {
        public int IcecreamId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CosmosDriver
    {
        public int DriverId { get; set; }
        public string Name { get; set; }
        public string ImageUri { get; set; }
    }
}

using SalesMine.Core.DomainObjects;
using System;

namespace SalesMine.Catalog.API.Models
{
    public class Product : Entity, IAggregateRoot
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public decimal Value { get; set; }

        public DateTime RegisterDate { get; set; }

        public string Image { get; set; }

        public int QtyInStock { get; set; }
    }
}

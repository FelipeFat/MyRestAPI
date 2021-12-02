using System;

namespace MyRest.Business.Models
{
    public class Product: Entity
    {
        public Guid SupplierId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal ProductValue { get; set; }
        public DateTime InsertDate { get; set; }
        public bool Active { get; set; }

        /* EF Relations */
        public Supplier Supplier { get; set; }
    }
}
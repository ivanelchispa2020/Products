using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Products.Api.Models
{
    public class ProductsResponse   /// TENEMOS QUE HECER PARA PODER RELACIONAR LAS TABLAS
    {
       public int ProductId { get; set; }

         public string Description { get; set; }

        public string Image { get; set; }

         public decimal Price { get; set; }

         public bool IsActive { get; set; }

        public DateTime LastPurchase { get; set; }

        public double Stock { get; set; }

        public string Remarks { get; set; }
      }
}
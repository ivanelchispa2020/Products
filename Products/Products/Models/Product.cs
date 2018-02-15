using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public int CategoryId { get; set; }
    
        public Category Category { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastPurchase { get; set; }

        public double Stock { get; set; }

        public string Remarks { get; set; }


        public string ImageFullPath // PARA QUE CONCATENE TODA LA IMAGEN DE LA RUTA
        {
            get
            { return string.Format("http://pruebaproductos.somee.com/{0}", Image.Substring(1));   }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Products.Api.Models
{
    public class CategoryResponse /// PARA PODER RELACIONAR CON LAS TABLAS RELACIONADAS
    {
   
        public int CategoryId { get; set; }

        public string Description { get; set; }

        public List<ProductsResponse> Products { get; set; } // relacion muchos

    }
}
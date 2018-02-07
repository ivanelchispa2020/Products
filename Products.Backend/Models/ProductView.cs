using Products.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Products.Backend.Models
{
    [NotMapped]  // NO LO MAPEA A LA BASE DE DATOS
    public class ProductView : IProduct
    {
        [Display(Name ="Image")] /// ATRIBUTO PARA QUE MUESTRE ESE NOMBRE 
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
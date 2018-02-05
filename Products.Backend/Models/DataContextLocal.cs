using Products.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Products.Backend.Models
{
    public class DataContextLocal : DataContext
    {
        public System.Data.Entity.DbSet<Products.Domain.ICategory> ICategories { get; set; }
    }
}
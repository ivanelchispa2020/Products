using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Domain
{
   public class DataContext :DbContext
    {

        public DataContext() : base("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }




        public System.Data.Entity.DbSet<Products.Domain.ICategory> ICategories { get; set; }

        public System.Data.Entity.DbSet<Products.Domain.IProduct> IProducts { get; set; }

    }
}

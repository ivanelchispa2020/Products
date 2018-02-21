using Products.Domain;

namespace Products.Api.Models
{
    public class ProductRequest : IProduct
    {
        public byte[] ImageArray { get; set; }
    }
}
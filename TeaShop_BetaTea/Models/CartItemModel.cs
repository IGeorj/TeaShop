using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeaShop_BetaTea.Models
{
    public class CartItemModel
    {
        [Key]
        public int CartItemId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("ProductId")]
        public ProductModel Product { get; set; }
        public int ProductId { get; set; }

        [ForeignKey("OrderId")]
        public OrderModel Order { get; set; }
        public int OrderId { get; set; }

    }
}
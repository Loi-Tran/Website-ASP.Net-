using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopOnline
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public string Img { get; set; }
        public decimal Price { get; set; }
        public decimal PromotionPrice { get; set; }
        public string Code { get; set; }
        public int Qty { get; set; }
        public decimal Amount { get; set; }
        public CartItem() { }
        public CartItem(int proid,string name, string detail, string img,decimal price,decimal promotionprice, string code, int qty) {
            this.ProductId = proid;
            this.Name = name;
            this.Detail = detail;
            this.Img = img;
            this.Price = price;
            this.PromotionPrice = promotionprice;
            this.Qty = qty;
            this.Code = code;
            this.Amount = promotionprice * qty;
        }
    }
}
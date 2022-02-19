using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopOnline
{
    public class XCart
    {
        public List<CartItem> AddCart(CartItem cartItem)
        {
            List<CartItem> listcart;
            if (System.Web.HttpContext.Current.Session["MyCart"].Equals(""))
            {
                listcart = new List<CartItem>();
                listcart.Add(cartItem);
                System.Web.HttpContext.Current.Session["MyCart"] = listcart;
            }
            else
            {
                listcart = (List<CartItem>)System.Web.HttpContext.Current.Session["MyCart"];//ép kiểu
                //Kiểm tra id có trong danh sách chưa
                if (listcart.Where(m => m.ProductId == cartItem.ProductId).Count() != 0)
                {
                    int vt = 0;
                    foreach (var item in listcart)
                    {
                        if (item.ProductId == cartItem.ProductId)
                        {
                            listcart[vt].Qty += 1;
                            listcart[vt].Amount = (listcart[vt].Qty * listcart[vt].PromotionPrice);
                        }
                        vt++;
                    }
                    System.Web.HttpContext.Current.Session["MyCart"] = listcart;
                }
                else
                {
                    listcart.Add(cartItem);
                    System.Web.HttpContext.Current.Session["MyCart"] = listcart;
                }

            }
            return listcart;
        }
        public void UpdateCart(string[] arrqty)
        {
            List<CartItem> listcart = this.getCart();
            int vt = 0;
            foreach(CartItem cartItem in listcart)
            {
                listcart[vt].Qty =int.Parse(arrqty[vt]);
                listcart[vt].Amount = (listcart[vt].Qty * listcart[vt].PromotionPrice);
                vt++;
            }
            System.Web.HttpContext.Current.Session["MyCart"] = listcart;
        }
        public void DelCart(int? productid=null)
        {
            if(productid!=null)
            {
                if (!System.Web.HttpContext.Current.Session["MyCart"].Equals(""))
                {
                    List<CartItem> listcart = (List<CartItem>)System.Web.HttpContext.Current.Session["MyCart"];
                    int vt = 0;
                    foreach (var item in listcart)
                    {
                        if (item.ProductId == productid)
                        {
                            listcart.RemoveAt(vt);
                            break;
                        }
                        vt++;
                    }
                    System.Web.HttpContext.Current.Session["MyCart"] = listcart;
                }
            }
            else
            {
                System.Web.HttpContext.Current.Session["MyCart"] = "";
            }

        }
        public List<CartItem> getCart()
        {
            if (System.Web.HttpContext.Current.Session["MyCart"].Equals(""))
            {
                return null;
            }    
                return (List<CartItem>)System.Web.HttpContext.Current.Session["MyCart"];
        }
    }
}
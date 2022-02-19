using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyClass.Models;
using MyClass.DAO;

namespace ShopOnline.Controllers
{
    public class GiohangController : Controller
    {
        ProductDao productDao = new ProductDao();
        UserDao userDao = new UserDao();
        OrderDao orderDao = new OrderDao();
        XCart xCart = new XCart();
        OrderdetailDao orderdetailDao = new OrderdetailDao();
        // GET: Cart
        public ActionResult Index()
        {
            List<CartItem> listcart = xCart.getCart();
            return View("Index", listcart);
        }
        public ActionResult CartAdd(int productid)
        {
            Product product = productDao.getRow(productid);//chi tiết sản phẩm
            CartItem cartItem = new CartItem(product.ID,product.Name,product.Detail,product.Image,product.Price,product.PromotionPrice,product.Code,1);
            //Add vào giỏ hàng
            List<CartItem> listcart = xCart.AddCart(cartItem);
            return RedirectToAction("Index","Giohang");
        }
        public ActionResult CartDel(int productid)
        {
            xCart.DelCart(productid);
            return RedirectToAction("Index", "Giohang");
        }
        public ActionResult CartUpdate(FormCollection form)
        {
            if (!string.IsNullOrEmpty(form["CAPNHAT"]))
            {
                var listqty = form["qty"];
                var listarr = listqty.Split(',');
                xCart.UpdateCart(listarr);
            }    
            return RedirectToAction("Index", "Giohang");
        }
        public ActionResult CarDeAll(FormCollection form)
        {
            xCart.DelCart();
            return RedirectToAction("Index", "Giohang");
        }

        //thanh toán
        public ActionResult ThanhToan()
        {       
            List<CartItem> listcart = xCart.getCart();
            //Kiểm tra đăng nhập trang người dùng==>Khách hàng
            if (Session["UserCustomer"].Equals(""))
            {
                return Redirect("~/dang-nhap");//Chuyển hướng đến Url
            }
            int userid = int.Parse(Session["CustomerId"].ToString());//Mã người đăng nhập
            User user = userDao.getRow(userid);
            ViewBag.user = user;
            return View("ThanhToan", listcart);
        }
        public ActionResult DatMua(FormCollection field)
        {
            //Lưu thông tin vào csdl oder và oderdetail
            int userid = int.Parse(Session["CustomerId"].ToString());//Mã người đăng nhập
            User user = userDao.getRow(userid);
            //Lấy thông tin
            String note = field["Note"];
            // Tạo đối tượng đơn hàng
            Order order = new Order();
            order.UserId = userid;
            order.Note = note;
            order.Status = 1;
            order.CreatedDate = DateTime.Now;
            if (orderDao.Insert(order)==1)
            {
                //Thêm vào chi tiết đơn hàng
                List<CartItem> listcart = xCart.getCart();
                foreach(CartItem cartItem in listcart)
                {
                    Orderdetail orderdetail = new Orderdetail();
                    orderdetail.OrderId = order.ID;
                    orderdetail.ProductId = cartItem.ProductId;
                    orderdetail.Price = cartItem.PromotionPrice;
                    orderdetail.Qty = cartItem.Qty;
                    orderdetail.Amount = cartItem.Amount;
                    orderdetailDao.Insert(orderdetail);//Lưu
                }    
            }
            return Redirect("~/thanh-cong");//Chuyển hướng đến Url
        }
        public ActionResult ThanhCong()
        {
            List<CartItem> listcart = xCart.getCart();
            return View("ThanhCong", listcart);
        }
    }
}
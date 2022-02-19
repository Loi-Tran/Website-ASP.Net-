using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BotDetect.Web.UI.Mvc;
using MyClass.DAO;
using MyClass.Models;
using ShopOnline.Models;

namespace ShopOnline.Controllers
{
    public class KhachhangController : Controller
    {
        ShopOnlineDbContext objModel = new ShopOnlineDbContext();
        UserDao userDao = new UserDao();
        // GET: Khachhang
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection filed)
        {
            String username = filed["username"];
            String password = XString.ToMD5(filed["password"]);

            User row_user = userDao.getRow(username, "Customer");
            String strError = "";
            if (row_user == null)
            {
                strError = "Tên đăng nhập không tồn tại";
            }
            else
            {
                if (password.Equals(row_user.Password))
                {
                    Session["UserCustomer"] = username;
                    Session["CustomerId"] = row_user.ID;
                    return Redirect("~/");
                }
                else
                {
                    strError = password;
                }
            }
            ViewBag.Error = "<h5 class='text-danger'>" + strError + "</h5>";
            return View("DangNhap");
        }
        [HttpGet]
        public ActionResult DangKi()
        {
            return View();
        }
        //Post: Register
        [HttpPost]
        [CaptchaValidation("CaptchaCode", "registerCaptcha", "Mã xác nhận không đúng!")]
        public ActionResult DangKi(RegisterModel model)
        {

            if (ModelState.IsValid)
            {
               
                var dao = new UserDao();
                if (dao.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if (dao.CheckEmail(model.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var user = new User();
                    user.Name = model.Name;
                    user.UserName = model.UserName;
                    user.Password = XString.ToMD5(model.Password);
                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.CreatedDate = DateTime.Now;
                    user.Status = 1;
                    user.Roles = "Customer";
                    var result = dao.Insert(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Đăng ký thành công";
                        model = new RegisterModel();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công.");
                    }
                }
            }           
            return View(model);
        }
       
        public ActionResult DangXuat()
        {
            Session["UserCustomer"] = "";
            Session["CustomerId"] = "";
            return RedirectToAction("DangNhap");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.Models;
using MyClass.DAO;

namespace ShopOnline.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        UserDao userDao = new UserDao();
        // GET: Admin/User
        public ActionResult Index()
        {
            return View(userDao.getList("Index"));
        }

        // GET: Admin/User/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = userDao.getRow(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                user.CreatedDate = DateTime.Now;
                userDao.Insert(user);
                return RedirectToAction("Index");
            }
            TempData["message"] = new XMessage("success", "Thêm thành công");
            return View(user);
        }

        // GET: Admin/User/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = userDao.getRow(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                user.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
                user.ModifieDate = DateTime.Now;
                userDao.Update(user);
                return RedirectToAction("Index");
            }
            TempData["message"] = new XMessage("success", " Cập nhật thành công ");
            return View(user);
        }

        // GET: Admin/User/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = userDao.getRow(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            TempData["message"] = new XMessage("success", "Xóa thành công");
            return View(user);
        }

        // POST: Admin/User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            User user = userDao.getRow(id);
            userDao.Delete(user);
            return RedirectToAction("Index");
        }
        public ActionResult Trash()
        {
            return View(userDao.getList("Trash"));
        }
        public ActionResult Status(long? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Index", "User");
            }
            User user = userDao.getRow(id);
            if (user == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "User");
            }
            user.Status = (user.Status == 1) ? 2 : 1;
            user.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            user.ModifieDate = DateTime.Now;
            userDao.Update(user);
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công");
            return RedirectToAction("Index", "User");
        }
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Index", "User");
            }
            User user = userDao.getRow(id);
            if (user == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "User");
            }
            user.Status = 0;// Trạng thái rac = 0
            user.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            user.ModifieDate = DateTime.Now;
            userDao.Update(user);
            TempData["message"] = new XMessage("success", "Xóa vào thùng rác thành công");
            return RedirectToAction("Index", "User");
        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Trash", "User");
            }
            User user = userDao.getRow(id);
            if (user == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Trash", "User");
            }
            user.Status = 2;// Trạng thái rac = 0
            user.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            user.ModifieDate = DateTime.Now;
            userDao.Update(user);
            TempData["message"] = new XMessage("success", "Khôi phục thành công");
            return RedirectToAction("Trash", "User");
        }
        public String NameCustomer(int userid)
        {
            User user = userDao.getRow(userid);
            if(user==null)
            {
                return "";
            }    
            else
            {
                return user.Name;
            }    
        }
    }
}
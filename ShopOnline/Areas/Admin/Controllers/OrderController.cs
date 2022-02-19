using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Models;

namespace ShopOnline.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        OrderDao orderDao = new OrderDao();
        OrderdetailDao orderdetailDao = new OrderdetailDao();

        // GET: Admin/Order
        public ActionResult Index()
        {
            List<Order> list = orderDao.getList("Index");
            return View(list);
        }

        // GET: Admin/Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = orderDao.getRow(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListChiTiet = orderdetailDao.getList(id);
            return View(order);
        }

        // GET: Admin/Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Order order)
        {
            if (ModelState.IsValid)
            {
                orderDao.Insert(order);
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Admin/Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = orderDao.getRow(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                order.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
                order.ModifieDate = DateTime.Now;
                orderDao.Update(order);
                return RedirectToAction("Index");
            }
            TempData["message"] = new XMessage("success", "Cập nhật thành công");
            return View(order);
        }

        // GET: Admin/Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = orderDao.getRow(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = orderDao.getRow(id);
            orderDao.Delete(order);
            TempData["message"] = new XMessage("success", "Xóa thành công");
            return RedirectToAction("Trash", "Order");
        }

        public ActionResult Trash()
        {
            return View(orderDao.getList("Trash"));
        }
        public ActionResult Status(long? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Index", "Order");
            }
            Order order = orderDao.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Order");
            }
            order.Status = (order.Status == 1) ? 2 : 1;
            order.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            order.ModifieDate = DateTime.Now;
            orderDao.Update(order);
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công");
            return RedirectToAction("Index", "Order");
        }
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Index", "Order");
            }
            Order order = orderDao.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Order");
            }
            order.Status = 0;// Trạng thái rac = 0
            order.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            order.ModifieDate = DateTime.Now;
            orderDao.Update(order);
            TempData["message"] = new XMessage("success", "Xóa vào thùng rác thành công");
            return RedirectToAction("Index", "Order");
        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Trash", "Order");
            }
            Order order = orderDao.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Trash", "Order");
            }
            order.Status = 2;// Trạng thái rac = 0
            order.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            order.ModifieDate = DateTime.Now;
            orderDao.Update(order);
            TempData["message"] = new XMessage("success", "Khôi phục thành công");
            return RedirectToAction("Trash", "Order");
        }
        public ActionResult Destroy(long? id)
        {
            Order order = orderDao.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index");
            }
            if(order.Status==1||order.Status==2)
            {
                order.Status = 0;
                order.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
                order.ModifieDate = DateTime.Now;
            }    
            else
            {
                if(order.Status==3)
                {
                    TempData["message"] = new XMessage("danger", "Đơn hàng đang vận chuyển không thể hủy");
                }
                if (order.Status == 4)
                {
                    TempData["message"] = new XMessage("danger", "Đơn hàng đã giao không thể hủy");
                }
                return RedirectToAction("Index");
            }    
            orderDao.Update(order);
            TempData["message"] = new XMessage("success", "Đã hủy đơn hàng thành công");
            return RedirectToAction("Index");
        }
        public ActionResult DaXacMinh(long? id)
        {
            Order order = orderDao.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index");
            }
            if (order.Status == 1)
            {
                order.Status = 2;
                order.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
                order.ModifieDate = DateTime.Now;
            }  
            else
            {
                if (order.Status == 3)
                {
                    TempData["message"] = new XMessage("danger", "Đơn hàng đang vận chuyển");
                }
                if (order.Status == 4)
                {
                    TempData["message"] = new XMessage("danger", "Đơn hàng đã giao");
                }
                return RedirectToAction("Index");
            }    
            orderDao.Update(order);
            TempData["message"] = new XMessage("success", "Xác minh đơn hàng thành công");
            return RedirectToAction("Index");
        }
        public ActionResult DangVanChuyen(long? id)
        {
            Order order = orderDao.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index");
            }
            if (order.Status == 2)
            {
                order.Status = 3;
                order.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
                order.ModifieDate = DateTime.Now;
            }
            else
            {
                if (order.Status == 1)
                {
                    TempData["message"] = new XMessage("danger", "Đơn hàng đang chờ xác minh");
                }
                if (order.Status == 4)
                {
                    TempData["message"] = new XMessage("danger", "Đơn hàng đã giao");
                }
                return RedirectToAction("Index");
            }    
            orderDao.Update(order);
            TempData["message"] = new XMessage("success", "Đang vận chuyển");
            return RedirectToAction("Index", "Order");
        }
        public ActionResult ThanhCong(long? id)
        {
            Order order = orderDao.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index");
            }
            if (order.Status == 3 )
            {
                order.Status = 4;
                order.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
                order.ModifieDate = DateTime.Now;
            }
            else
            {
                if (order.Status == 2)
                {
                    TempData["message"] = new XMessage("danger", "Đơn hàng đang chờ vận chuyển");
                }
                if (order.Status == 1)
                {
                    TempData["message"] = new XMessage("danger", "Đơn hàng đang chờ xác minh");
                }
                return RedirectToAction("Index");
            }    
            orderDao.Update(order);
            TempData["message"] = new XMessage("success", "Đã giao hàng");
            return RedirectToAction("Index", "Order");
        }
    }
}
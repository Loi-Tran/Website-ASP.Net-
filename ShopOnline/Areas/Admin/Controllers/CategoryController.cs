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
using ShopOnline;

namespace ShopOnline.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        CategoryDao categoryDao = new CategoryDao();
        LinkDao linkDao = new LinkDao();

        // GET: Admin/Category
        public ActionResult Index()
        {
            return View(categoryDao.getList("Index"));
        }

        // GET: Admin/Category/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryDao.getRow(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.ListCat = new SelectList(categoryDao.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(categoryDao.getList("Index"), "DisplayOrder", "Name", 0);
            return View();
        }

        // POST: Admin/Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                //Xử lý thêm thông tin
                category.Slug = XString.Str_slug(category.Name);
                if (category.ParentID == null)
                {
                    category.ParentID = 0;
                }
                if (category.DisplayOrder == null)
                {
                    category.DisplayOrder = 1;
                }
                else
                {
                    category.DisplayOrder += 1;
                }
                category.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                category.CreatedDate = DateTime.Now;
                if (categoryDao.Insert(category) == 1)
                {
                    Link link = new Link();
                    link.Slug = category.Slug;
                    link.TableId = category.ID;
                    link.TypeLink = "category";
                    linkDao.Insert(link);
                }
                TempData["message"] = new XMessage("success", "Thêm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(categoryDao.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(categoryDao.getList("Index"), "DisplayOrder", "Name", 0);
            return View(category);
        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(long? id)
        {
            ViewBag.ListCat = new SelectList(categoryDao.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(categoryDao.getList("Index"), "DisplayOrder", "Name", 0);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryDao.getRow(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                //Xử lý thêm thông tin
                category.Slug = XString.Str_slug(category.Name);
                if (category.ParentID == null)
                {
                    category.ParentID = 0;
                }
                if (category.DisplayOrder == null)
                {
                    category.DisplayOrder = 1;
                }
                else
                {
                    category.DisplayOrder += 1;
                }
                category.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
                category.ModifieDate = DateTime.Now;
                if (categoryDao.Update(category) == 1)
                {
                    Link link = linkDao.getRow(category.ID, "category");
                    link.Slug = category.Slug;
                    linkDao.Update(link);
                    //Cập nhật menu
                }
                TempData["message"] = new XMessage("success", "Cập nhật thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(categoryDao.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(categoryDao.getList("Index"), "DisplayOrder", "Name", 0);
            return View(category);
        }

        // GET: Admin/Category/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryDao.getRow(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Category category = categoryDao.getRow(id);
            Link link = linkDao.getRow(category.ID, "category");
            if (categoryDao.Delete(category) == 1)
            {
                linkDao.Delete(link);
            }
            TempData["message"] = new XMessage("success", "Xóa thành công");
            return RedirectToAction("Trash", "Category");
        }
        public ActionResult Trash()
        {
            return View(categoryDao.getList("Trash"));
        }
        public ActionResult Status(long? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Index", "Category");
            }
            Category category = categoryDao.getRow(id);
            if (category == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Category");
            }
            category.Status = (category.Status == 1) ? 2 : 1;
            category.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            category.ModifieDate = DateTime.Now;
            categoryDao.Update(category);
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công");
            return RedirectToAction("Index", "Category");
        }
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Index", "Category");
            }
            Category category = categoryDao.getRow(id);
            if (category == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Category");
            }
            category.Status = 0;// Trạng thái rac = 0
            category.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            category.ModifieDate = DateTime.Now;
            categoryDao.Update(category);
            TempData["message"] = new XMessage("success", "Xóa vào thùng rác thành công");
            return RedirectToAction("Index", "Category");
        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Trash", "Category");
            }
            Category category = categoryDao.getRow(id);
            if (category == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Trash", "Category");
            }
            category.Status = 2;// Trạng thái rac = 0
            category.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            category.ModifieDate = DateTime.Now;
            categoryDao.Update(category);
            TempData["message"] = new XMessage("success", "Khôi phục thành công");
            return RedirectToAction("Trash", "Category");
        }
    }
}

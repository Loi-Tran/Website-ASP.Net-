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
using ShopOnline;

namespace ShopOnline.Areas.Admin.Controllers
{
    public class ContentController : BaseController
    {
        ContentDao contentDao = new ContentDao();
        LinkDao linkDao = new LinkDao();

        // GET: Admin/Content
        public ActionResult Index()
        {
            return View(contentDao.getList("Index"));
        }

        // GET: Admin/Content/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = contentDao.getRow(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // GET: Admin/Content/Create
        public ActionResult Create()
        {
            ViewBag.ListCat = new SelectList(contentDao.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(contentDao.getList("Index"), "DisplayOrder", "Name", 0);
            return View();
        }

        // POST: Admin/Content/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Content content)
        {
            if (ModelState.IsValid)
            {
                //Xử lý thêm thông tin
                content.Slug = XString.Str_slug(content.Name);
                if (content.ParentID == null)
                {
                    content.ParentID = 0;
                }
                if (content.DisplayOrder == null)
                {
                    content.DisplayOrder = 1;
                }
                else
                {
                    content.DisplayOrder += 1;
                }
                content.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                content.CreatedDate = DateTime.Now;
                if (contentDao.Insert(content) == 1)
                {
                    Link link = new Link();
                    link.Slug = content.Slug;
                    link.TableId = content.ID;
                    link.TypeLink = "content";
                    linkDao.Insert(link);
                }
                TempData["message"] = new XMessage("success", "Thêm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(contentDao.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(contentDao.getList("Index"), "DisplayOrder", "Name", 0);
            return View(content);
        }

        // GET: Admin/Content/Edit/5
        public ActionResult Edit(long? id)
        {
            ViewBag.ListCat = new SelectList(contentDao.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(contentDao.getList("Index"), "DisplayOrder", "Name", 0);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = contentDao.getRow(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // POST: Admin/Content/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Content content)
        {
            if (ModelState.IsValid)
            {
                //Xử lý thêm thông tin
                content.Slug = XString.Str_slug(content.Name);
                if (content.ParentID == null)
                {
                    content.ParentID = 0;
                }
                if (content.DisplayOrder == null)
                {
                    content.DisplayOrder = 1;
                }
                else
                {
                    content.DisplayOrder += 1;
                }
                content.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
                content.ModifieDate = DateTime.Now;
                if (contentDao.Update(content) == 1)
                {
                    Link link = linkDao.getRow(content.ID, "content");
                    link.Slug = content.Slug;
                    linkDao.Update(link);
                }
                TempData["message"] = new XMessage("success", "Cập nhật thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(contentDao.getList("Index"), "Id", "Name", 0);
            ViewBag.ListOrder = new SelectList(contentDao.getList("Index"), "DisplayOrder", "Name", 0);
            return View(content);
        }

        // GET: Admin/Content/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = contentDao.getRow(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // POST: Admin/Content/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Content content = contentDao.getRow(id);
            Link link = linkDao.getRow(content.ID, "content");
            if (contentDao.Delete(content) == 1)
            {
                linkDao.Delete(link);
            }
            TempData["message"] = new XMessage("success", "Xóa thành công");
            return RedirectToAction("Trash", "content");
        }
        public ActionResult Trash()
        {
            return View(contentDao.getList("Trash"));
        }
        public ActionResult Status(long? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Index", "content");
            }
            Content content = contentDao.getRow(id);
            if (content == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "content");
            }
            content.Status = (content.Status == 1) ? 2 : 1;
            content.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            content.ModifieDate = DateTime.Now;
            contentDao.Update(content);
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công");
            return RedirectToAction("Index", "content");
        }
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Index", "content");
            }
            Content content = contentDao.getRow(id);
            if (content == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "content");
            }
            content.Status = 0;// Trạng thái rac = 0
            content.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            content.ModifieDate = DateTime.Now;
            contentDao.Update(content);
            TempData["message"] = new XMessage("success", "Xóa vào thùng rác thành công");
            return RedirectToAction("Index", "content");
        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Trash", "content");
            }
            Content content = contentDao.getRow(id);
            if (content == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Trash", "content");
            }
            content.Status = 2;// Trạng thái rac = 0
            content.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            content.ModifieDate = DateTime.Now;
            contentDao.Update(content);
            TempData["message"] = new XMessage("success", "Khôi phục thành công");
            return RedirectToAction("Trash", "content");
        }
    }
}

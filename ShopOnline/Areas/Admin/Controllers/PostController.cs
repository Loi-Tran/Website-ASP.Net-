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
    public class PostController : BaseController
    {
        PostDao postDao = new PostDao();
        ContentDao contentDao = new ContentDao();

        // GET: Admin/Post
        public ActionResult Index()
        {
            return View(postDao.getList("Index", "Post"));
        }

        // GET: Admin/Post/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = postDao.getRow(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Admin/post/Create
        public ActionResult Create()
        {
            ViewBag.ListContent = new SelectList(contentDao.getList("Index"), "Id", "Name");
            return View();
        }

        // POST: Admin/post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                post.PostType = "Post";
                post.Slug= XString.Str_slug(post.Name);
                post.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                post.CreatedDate = DateTime.Now;
                postDao.Insert(post);
                return RedirectToAction("Index");
            }
            ViewBag.ListContent = new SelectList(contentDao.getList("Index"), "Id", "Name");
            TempData["message"] = new XMessage("success", "Thêm thành công");
            return View(post);
        }

        // GET: Admin/post/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = postDao.getRow(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListContent = new SelectList(contentDao.getList("Index"), "Id", "Name");
            return View(post);
        }

        // POST: Admin/post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                post.PostType = "Post";
                post.Slug = XString.Str_slug(post.Name);
                post.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
                post.ModifieDate = DateTime.Now;
                postDao.Update(post);
                return RedirectToAction("Index");
            }
            ViewBag.ListContent = new SelectList(contentDao.getList("Index"), "Id", "Name");
            TempData["message"] = new XMessage("success", "Sửa thành công");
            return View(post);
        }

        // GET: Admin/post/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = postDao.getRow(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Admin/post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Post post = postDao.getRow(id);
            postDao.Delete(post);
            TempData["message"] = new XMessage("success", "Xóa thành công");
            return RedirectToAction("Trash","Post");
        }
        public ActionResult Trash()
        {
            return View(postDao.getList("Trash"));
        }
        public ActionResult Status(long? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Index", "Post");
            }
            Post post = postDao.getRow(id);
            if (post == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Post");
            }
            post.Status = (post.Status == 1) ? 2 : 1;
            post.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            post.ModifieDate = DateTime.Now;
            postDao.Update(post);
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công");
            return RedirectToAction("Index", "Post");
        }
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Index", "Post");
            }
            Post post = postDao.getRow(id);
            if (post == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Post");
            }
            post.Status = 0;// Trạng thái rac = 0
            post.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            post.ModifieDate = DateTime.Now;
            postDao.Update(post);
            TempData["message"] = new XMessage("success", "Xóa vào thùng rác thành công");
            return RedirectToAction("Index", "Post");
        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Trash", "Post");
            }
            Post post = postDao.getRow(id);
            if (post == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Trash", "Post");
            }
            post.Status = 2;// Trạng thái rac = 0
            post.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            post.ModifieDate = DateTime.Now;
            postDao.Update(post);
            TempData["message"] = new XMessage("success", "Khôi phục thành công");
            return RedirectToAction("Trash", "Post");
        }

    }
}

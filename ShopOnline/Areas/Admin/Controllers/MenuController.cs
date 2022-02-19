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
    public class MenuController : BaseController
    {
        CategoryDao categoryDao = new CategoryDao();
        ContentDao contentDao = new ContentDao();
        PostDao postDao = new PostDao();
        MenuDao menuDao = new MenuDao();
        SupplierDao supplierDao = new SupplierDao();
        // GET: Admin/Menu
        public ActionResult Index()
        {
            ViewBag.ListCategory = categoryDao.getList("Index");
            ViewBag.ListContent = contentDao.getList("Index");
            ViewBag.ListPage = postDao.getList("Index","Page");
            List<Menu> menu = menuDao.getList("Index");
            return View("Index",menu);
        }
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            if(!string.IsNullOrEmpty(form["ThemCategory"]))
            {
                if(!string.IsNullOrEmpty(form["nameCategory"]))
                {
                    var listitem = form["nameCategory"];
                    var listarr = listitem.Split(',');
                    foreach(var row in listarr)
                    {
                        int id = int.Parse(row);
                        Category category = categoryDao.getRow(id);
                        Menu menu = new Menu();
                        menu.Text = category.Name;
                        menu.Link = category.Slug;
                        menu.TableID = category.ID;
                        menu.TypeMenu = "category";
                        menu.Position = form["Position"];
                        menu.ParentID = 0;
                        menu.DisplayOrder = 0;
                        menu.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                        menu.CreatedDate = DateTime.Now;
                        menu.Status = 2;
                        menuDao.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục sản phẩm");
                }
            }

            //Content
            if (!string.IsNullOrEmpty(form["ThemContent"]))
            {
                if (!string.IsNullOrEmpty(form["nametopic"]))
                {
                    var listitem = form["nametopic"];
                    var listarr = listitem.Split(',');
                    foreach (var row in listarr)
                    {
                        int id = int.Parse(row);
                        Content content = contentDao.getRow(id);
                        Menu menu = new Menu();
                        menu.Text = content.Name;
                        menu.Link = content.Slug;
                        menu.TableID = content.ID;
                        menu.TypeMenu = "content";
                        menu.Position = form["Position"];
                        menu.ParentID = 0;
                        menu.DisplayOrder = 0;
                        menu.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                        menu.CreatedDate = DateTime.Now;
                        menu.Status = 2;
                        menuDao.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục sản phẩm");
                }
            }
            //Page
            if (!string.IsNullOrEmpty(form["ThemPage"]))
            {
                if (!string.IsNullOrEmpty(form["namepage"]))
                {
                    var listitem = form["namepage"];
                    var listarr = listitem.Split(',');
                    foreach (var row in listarr)
                    {
                        int id = int.Parse(row);
                        Post post = postDao.getRow(id);
                        Menu menu = new Menu();
                        menu.Text = post.Name;
                        menu.Link = post.Slug;
                        menu.TableID = post.ID;
                        menu.TypeMenu = "page";
                        menu.Position = form["Position"];
                        menu.ParentID = 0;
                        menu.DisplayOrder = 0;
                        menu.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                        menu.CreatedDate = DateTime.Now;
                        menu.Status = 2;
                        menuDao.Insert(menu);
                    }
                    TempData["message"] = new XMessage("success", "Thêm thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa chọn danh mục sản phẩm");
                }
            }
            // ThêmCustom
            if (!string.IsNullOrEmpty(form["ThemCustom"]))
            {
                if (!string.IsNullOrEmpty(form["name"]) && !string.IsNullOrEmpty(form["link"]))
                {
                    Menu menu = new Menu();
                    menu.Text = form["name"];
                    menu.Link = form["link"];
                    menu.TypeMenu = "custom";
                    menu.Position = form["Position"];
                    menu.ParentID = 0;
                    menu.DisplayOrder = 0;
                    menu.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                    menu.CreatedDate = DateTime.Now;
                    menu.Status = 2;
                    menuDao.Insert(menu);
                    TempData["message"] = new XMessage("success", "Thêm thành công");
                }
                else
                {
                    TempData["message"] = new XMessage("danger", "Chưa nhập đủ thông tin");
                }
            }    
                return RedirectToAction("Index", "Menu");
        }
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = menuDao.getRow(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = menuDao.getRow(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Menu menu = menuDao.getRow(id);
            menuDao.Delete(menu);
            TempData["message"] = new XMessage("success", "Xóa thành công");
            return RedirectToAction("Trash", "Menu");
        }
        public ActionResult Edit(int? id)
        {
            ViewBag.ListMenu = new SelectList(menuDao.getList("Index"), "Id", "Text");
            ViewBag.ListOrder = new SelectList(menuDao.getList("Index"), "DisplayOrder", "Text");
            Menu menu = menuDao.getRow(id);
            return View("Edit", menu);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {
                //Xử lý thêm thông tin
                if (menu.ParentID == null)
                {
                    menu.ParentID = 0;
                }
                if (menu.DisplayOrder == null)
                {
                    menu.DisplayOrder = 1;
                }
                else
                {
                    menu.DisplayOrder += 1;
                }
                menu.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
                menu.ModifieDate = DateTime.Now;
                menuDao.Update(menu);
                TempData["message"] = new XMessage("success", "Cập nhật thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListMenu = new SelectList(menuDao.getList("Index"), "Id", "Text");
            ViewBag.ListOrder = new SelectList(menuDao.getList("Index"), "DisplayOrder", "Text");
            return View(menu);
        }
        public ActionResult Status(int? id)
        {
            if(id==null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index");
            }
            Menu menu = menuDao.getRow(id);
            if(menu==null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index");
            }
            menu.Status = (menu.Status == 1) ? 2 : 1;
            menu.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            menu.ModifieDate = DateTime.Now;
            menuDao.Update(menu);
            TempData["message"] = new XMessage("success", "Thành công");
            return RedirectToAction("Index");
        }
        public ActionResult Deltrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index");
            }
            Menu menu = menuDao.getRow(id);
            if (menu == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index");
            }
            menu.Status = 0;
            menu.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            menu.ModifieDate = DateTime.Now;
            menuDao.Update(menu);
            TempData["message"] = new XMessage("success", "Thành công");
            return RedirectToAction("Index");
        }
        public ActionResult Trash()
        {
            List<Menu> menu = menuDao.getList("Trash");
            return View("Trash", menu);
        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Trash");
            }
            Menu menu = menuDao.getRow(id);
            if (menu == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Trash");
            }
            menu.Status = 2;
            menu.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            menu.ModifieDate = DateTime.Now;
            menuDao.Update(menu);
            TempData["message"] = new XMessage("success", "Thành công");
            return RedirectToAction("Trash");
        }
    }
}

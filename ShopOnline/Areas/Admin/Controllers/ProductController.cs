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
using System.IO;

namespace ShopOnline.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        ProductDao productDao = new ProductDao();
        CategoryDao categoryDao = new CategoryDao();
        SupplierDao supplierDao = new SupplierDao();

        // GET: Admin/Product
        public ActionResult Index()
        {
            return View(productDao.GetListJoin("Index"));
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = productDao.getRow(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.ListCat = new SelectList(categoryDao.getList("Index"), "Id", "Name", 0);
            ViewBag.ListSup = new SelectList(supplierDao.getList("Index"), "Id", "Name", 0);
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.Slug = XString.Str_slug(product.Name);
                product.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                product.CreatedDate = DateTime.Now;
                //upload file
                var img = Request.Files["Image"]; // lấy thông tin file
                if (img.ContentLength!=0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jepg", ".png", ".gif" };
                    //kiểm tra tập tin
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))
                    {
                        //upload hình
                        string imgName = product.Slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        product.Image = imgName;
                        string PathDir = "~/Assets/client/images/products/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }
                productDao.Insert(product);
                TempData["message"] = new XMessage("success", "Thêm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(categoryDao.getList("Index"), "Id", "Name", 0);
            ViewBag.ListSup = new SelectList(supplierDao.getList("Index"), "Id", "Name", 0);
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(long? id)
        {
            ViewBag.ListCat = new SelectList(categoryDao.getList("Index"), "Id", "Name", 0);
            ViewBag.ListSup = new SelectList(supplierDao.getList("Index"), "Id", "Name", 0);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = productDao.getRow(id);
            if (product == null)
            {
                return HttpNotFound();
            }
           
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Product product)
        {
            if (ModelState.IsValid)
            {
                product.Slug = XString.Str_slug(product.Name);
                product.CreatedBy = (Session["UserId"].Equals("")) ? 1 : int.Parse(Session["UserId"].ToString());
                product.CreatedDate = DateTime.Now;
                //upload file
                var img = Request.Files["Image"]; // lấy thông tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jepg", ".png", ".gif" };
                    //kiểm tra tập tin
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))
                    {
                        //upload hình
                        string imgName = product.Slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                      
                        string PathDir = "~/Assets/client/images/products/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        // Xóa file
                        if (product.Image!=null)
                        {
                            string DelPath = Path.Combine(Server.MapPath(PathDir), product.Image);
                            System.IO.File.Delete(DelPath);// xóa hình
                        }
                        img.SaveAs(PathFile);
                        product.Image = imgName;
                    }
                }
                //end upload

                productDao.Update(product);
                TempData["message"] = new XMessage("success", "Cập nhật thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(categoryDao.getList("Index"), "Id", "Name", 0);
            ViewBag.ListSup = new SelectList(supplierDao.getList("Index"), "Id", "Name", 0);
            return View(product);
        }

        // GET: Admin/Product/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = productDao.getRow(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Product product = productDao.getRow(id);
            string PathDir = "~/Assets/client/images/products/";
            //Xóa hình cũ
            if (product.Image != null)
            {
                string pathImgDelete = Path.Combine(Server.MapPath(PathDir), product.Image);
                System.IO.File.Delete(pathImgDelete);
            }
            productDao.Delete(product);
            TempData["message"] = new XMessage("success", "Xóa thành công");
            return RedirectToAction("Trash","Product");
        }
        public ActionResult Trash()
        {
            return View(productDao.GetListJoin("Trash"));
        }
        public ActionResult Status(long? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã không tồn tại");
                return RedirectToAction("Index", "Product");
            }
            Product product = productDao.getRow(id);
            if (product == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Product");
            }
            product.Status = (product.Status == 1) ? 2 : 1;
            product.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            product.ModifieDate = DateTime.Now;
            productDao.Update(product);
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công");
            return RedirectToAction("Index", "Product");
        }
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã không tồn tại");
                return RedirectToAction("Index", "Product");
            }
            Product product = productDao.getRow(id);
            if (product == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Product");
            }
            product.Status = 0;// Trạng thái rac = 0
            product.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            product.ModifieDate = DateTime.Now;
            productDao.Update(product);
            TempData["message"] = new XMessage("success", "Xóa vào thùng rác thành công");
            return RedirectToAction("Index", "Product");
        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã không tồn tại");
                return RedirectToAction("Trash", "Product");
            }
            Product product = productDao.getRow(id);
            if (product == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Trash", "Product");
            }
            product.Status = 2;// Trạng thái rac = 0
            product.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            product.ModifieDate = DateTime.Now;
            productDao.Update(product);
            TempData["message"] = new XMessage("success", "Khôi phục thành công");
            return RedirectToAction("Trash", "Product");
        }
        ///
        public String ProductImg(int productid)
        {
            Product product = productDao.getRow(productid);
            if (product == null)
            {
                return "";
            }
            else
            {
                return product.Image;
            }
        }
        public String ProductName(int productid)
        {
            Product product = productDao.getRow(productid);
            if (product == null)
            {
                return "";
            }
            else
            {
                return product.Name;
            }
        }
    }
}

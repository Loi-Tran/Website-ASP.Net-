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
    public class SupplierController : BaseController
    {
        SupplierDao supplierDao = new SupplierDao();
        LinkDao linkDao = new LinkDao();

        // GET: Admin/ProductCategories
        public ActionResult Index()
        {
            return View(supplierDao.getList("Index"));
        }

        // GET: Admin/ProductCategories/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = supplierDao.getRow(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // GET: Admin/ProductCategories/Create
        public ActionResult Create()
        {
            ViewBag.ListOrder = new SelectList(supplierDao.getList("Index"), "Orders", "Name", 0);
            return View();
        }

        // POST: Admin/ProductCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                //Xử lý thêm thông tin
                supplier.Slug = XString.Str_slug(supplier.Name);
               
                if (supplier.Orders == null)
                {
                    supplier.Orders = 1;
                }
                else
                {
                    supplier.Orders += 1;
                }
                //upload file
                var img = Request.Files["img"]; // lấy thông tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jepg", ".png", ".gif" };
                    //kiểm tra tập tin
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))
                    {                    
                        //upload hình
                        string imgName = supplier.Slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        supplier.Img = imgName;
                        string PathDir = "~/Assets/client/images/suppliers/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }

                //end upload
                supplier.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                supplier.CreatedDate = DateTime.Now;
                if (supplierDao.Insert(supplier) == 1)
                {
                    Link link = new Link();
                    link.Slug = supplier.Slug;
                    link.TableId = supplier.ID;
                    link.TypeLink = "supplier";
                    linkDao.Insert(link);
                }
                TempData["message"] = new XMessage("success", "Thêm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListOrder = new SelectList(supplierDao.getList("Index"), "Orders", "Name", 0);
            return View(supplier);
        }

        // GET: Admin/ProductCategories/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = supplierDao.getRow(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListOrder = new SelectList(supplierDao.getList("Index"), "Orders", "Name", 0);
            return View(supplier);
        }

        // POST: Admin/ProductCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                //Xử lý thêm thông tin
                supplier.Slug = XString.Str_slug(supplier.Name);
                if (supplier.Orders == null)
                {
                    supplier.Orders = 1;
                }
                else
                {
                    supplier.Orders += 1;
                }
                //upload file
                var img = Request.Files["img"]; // lấy thông tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jepg", ".png", ".gif" };
                    //kiểm tra tập tin
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))
                    {
                        //upload hình
                        string imgName = supplier.Slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        supplier.Img = imgName;
                        string PathDir = "~/Assets/client/images/suppliers/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        // Xóa file
                        if (supplier.Img.Length>0)
                        {
                            string DelPath = Path.Combine(Server.MapPath(PathDir), supplier.Img);
                            System.IO.File.Delete(DelPath);// xáo hình
                        }
                        img.SaveAs(PathFile);
                    }
                }

                //end upload
                supplier.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
                supplier.ModifieDate = DateTime.Now;
                if (supplierDao.Update(supplier) == 1)
                {
                    Link link = linkDao.getRow(supplier.ID, "Supplier");
                    link.Slug = supplier.Slug;
                    linkDao.Update(link);
                }
                TempData["message"] = new XMessage("success", "Cập nhật thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListOrder = new SelectList(supplierDao.getList("Index"), "Orders", "Name", 0);
            return View(supplier);
        }

        // GET: Admin/ProductCategories/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = supplierDao.getRow(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Admin/ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Supplier supplier = supplierDao.getRow(id);
            Link link = linkDao.getRow(supplier.ID, "supplier");
            string PathDir = "~/Assets/client/images/suppliers/";
            // Xóa file
            if (supplier.Img.Length>0)
            {
                string DelPath = Path.Combine(Server.MapPath(PathDir), supplier.Img);
                System.IO.File.Delete(DelPath);// xáo hình
            }
            if (supplierDao.Delete(supplier) == 1)
            {
                linkDao.Delete(link);
            }
            TempData["message"] = new XMessage("success", "Xóa thành công");
            return RedirectToAction("Trash", "Supplier");
        }
        public ActionResult Trash()
        {
            return View(supplierDao.getList("Trash"));
        }
        public ActionResult Status(long? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Index", "Supplier");
            }
            Supplier supplier = supplierDao.getRow(id);
            if (supplier == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Supplier");
            }
            supplier.Status = (supplier.Status == 1) ? 2 : 1;
            supplier.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            supplier.ModifieDate = DateTime.Now;
            supplierDao.Update(supplier);
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công");
            return RedirectToAction("Index", "Supplier");
        }
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Index", "Supplier");
            }
            Supplier supplier = supplierDao.getRow(id);
            if (supplier == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Supplier");
            }
            supplier.Status = 0;// Trạng thái rac = 0
            supplier.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            supplier.ModifieDate = DateTime.Now;
            supplierDao.Update(supplier);
            TempData["message"] = new XMessage("success", "Xóa vào thùng rác thành công");
            return RedirectToAction("Index", "Supplier");
        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Trash", "Supplier");
            }
            Supplier supplier = supplierDao.getRow(id);
            if (supplier == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Trash", "Supplier");
            }
            supplier.Status = 2;// Trạng thái rac = 0
            supplier.ModifieBy = Convert.ToInt32(Session["UserId"].ToString());
            supplier.ModifieDate = DateTime.Now;
            supplierDao.Update(supplier);
            TempData["message"] = new XMessage("success", "Khôi phục thành công");
            return RedirectToAction("Trash", "Supplier");
        }
    }
}

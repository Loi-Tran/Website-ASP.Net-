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
    public class SliderController : BaseController
    {
        SliderDao sliderDao = new SliderDao();
        LinkDao linkDao = new LinkDao();

        // GET: Admin/ProductCategories
        public ActionResult Index()
        {
            return View(sliderDao.getList("Index"));
        }

        // GET: Admin/ProductCategories/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = sliderDao.getRow(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // GET: Admin/ProductCategories/Create
        public ActionResult Create()
        {
            ViewBag.ListOrder = new SelectList(sliderDao.getList("Index"), "DisplayOrder", "Name", 0);
            return View();
        }

        // POST: Admin/ProductCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Slider slider)
        {
            if (ModelState.IsValid)
            {
                //Xử lý thêm thông tin
                slider.Link = XString.Str_slug(slider.Name);

                if (slider.DisplayOrder == null)
                {
                    slider.DisplayOrder = 1;
                }
                else
                {
                    slider.DisplayOrder += 1;
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
                        string imgName = slider.Link + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        slider.Image = imgName;
                        string PathDir = "~/Assets/client/images/slider/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }

                //end upload
                slider.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                slider.CreatedDate = DateTime.Now;

                sliderDao.Insert(slider);
                TempData["message"] = new XMessage("success", "Thêm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListOrder = new SelectList(sliderDao.getList("Index"), "DisplayOrder", "Name", 0);
            return View(slider);
        }

        // GET: Admin/ProductCategories/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = sliderDao.getRow(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListOrder = new SelectList(sliderDao.getList("Index"), "DisplayOrder", "Name", 0);
            return View(slider);
        }

        // POST: Admin/ProductCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Slider slider)
        {
            if (ModelState.IsValid)
            {
                //Xử lý thêm thông tin
                slider.Link = XString.Str_slug(slider.Name);
                if (slider.DisplayOrder == null)
                {
                    slider.DisplayOrder = 1;
                }
                else
                {
                    slider.DisplayOrder += 1;
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
                        string imgName = slider.Link + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        slider.Image = imgName;
                        string PathDir = "~/Assets/client/images/suppliers/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        // Xóa file
                        if (slider.Image.Length > 0)
                        {
                            string DelPath = Path.Combine(Server.MapPath(PathDir), slider.Image);
                            System.IO.File.Delete(DelPath);// xáo hình
                        }
                        img.SaveAs(PathFile);
                    }
                }

                //end upload
                slider.ModifiedBy = Convert.ToInt32(Session["UserId"].ToString());
                slider.ModifiedDate = DateTime.Now;
                sliderDao.Update(slider);
                TempData["message"] = new XMessage("success", "Cập nhật thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListOrder = new SelectList(sliderDao.getList("Index"), "DisplayOrder", "Name", 0);
            return View(slider);
        }

        // GET: Admin/ProductCategories/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = sliderDao.getRow(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Admin/ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Slider slider = sliderDao.getRow(id);
            Link link = linkDao.getRow(slider.ID, "slider");
            string PathDir = "~/Assets/client/images/sliders/";
            // Xóa file
            if (slider.Image.Length > 0)
            {
                string DelPath = Path.Combine(Server.MapPath(PathDir), slider.Image);
                System.IO.File.Delete(DelPath);// xáo hình
            }
            if (sliderDao.Delete(slider) == 1)
            {
                linkDao.Delete(link);
            }
            TempData["message"] = new XMessage("success", "Xóa thành công");
            return RedirectToAction("Trash", "Slider");
        }
        public ActionResult Trash()
        {
            return View(sliderDao.getList("Trash"));
        }
        public ActionResult Status(long? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Index", "Slider");
            }
            Slider slider = sliderDao.getRow(id);
            if (slider == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Slider");
            }
            slider.Status = (slider.Status == 1) ? 2 : 1;
            slider.ModifiedBy = Convert.ToInt32(Session["UserId"].ToString());
            slider.ModifiedDate = DateTime.Now;
            sliderDao.Update(slider);
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công");
            return RedirectToAction("Index", "Slider");
        }
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Index", "Slider");
            }
            Slider slider = sliderDao.getRow(id);
            if (slider == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Slider");
            }
            slider.Status = 0;// Trạng thái rac = 0
            slider.ModifiedBy = Convert.ToInt32(Session["UserId"].ToString());
            slider.ModifiedDate = DateTime.Now;
            sliderDao.Update(slider);
            TempData["message"] = new XMessage("success", "Xóa vào thùng rác thành công");
            return RedirectToAction("Index", "Slider");
        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Trash", "Slider");
            }
            Slider slider = sliderDao.getRow(id);
            if (slider == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Trash", "Slider");
            }
            slider.Status = 2;// Trạng thái rac = 0
            slider.ModifiedBy = Convert.ToInt32(Session["UserId"].ToString());
            slider.ModifiedDate = DateTime.Now;
            sliderDao.Update(slider);
            TempData["message"] = new XMessage("success", "Khôi phục thành công");
            return RedirectToAction("Trash", "Slider");
        }
    }
}

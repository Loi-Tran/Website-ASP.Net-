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
    public class FeedbackController : BaseController
    {
        FeedbackDao feedbackDao = new FeedbackDao();

        // GET: Admin/feedback
        public ActionResult Index()
        {
            return View(feedbackDao.getList("Index"));
        }

        // GET: Admin/feedback/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = feedbackDao.getRow(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }      
        // GET: Admin/feedback/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = feedbackDao.getRow(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Feedback feedback = feedbackDao.getRow(id);
            feedbackDao.Delete(feedback);
            TempData["message"] = new XMessage("success", "Xóa thành công");
            return RedirectToAction("Trash", "Feedback");
        }
        public ActionResult Trash()
        {
            return View(feedbackDao.getList("Trash"));
        }
        public ActionResult Status(long? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Index", "Feedback");
            }
            Feedback feedback = feedbackDao.getRow(id);
            if (feedback == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Feedback");
            }
            feedback.Status = (feedback.Status == 1) ? 2 : 1;           
            feedbackDao.Update(feedback);
            TempData["message"] = new XMessage("success", "Thay đổi trạng thái thành công");
            return RedirectToAction("Index", "Feedback");
        }
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Index", "Feedback");
            }
            Feedback feedback = feedbackDao.getRow(id);
            if (feedback == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Index", "Feedback");
            }
            feedback.Status = 0;// Trạng thái rac = 0           
            feedbackDao.Update(feedback);
            TempData["message"] = new XMessage("success", "Xóa vào thùng rác thành công");
            return RedirectToAction("Index", "Feedback");
        }
        public ActionResult Retrash(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Mã loại không tồn tại");
                return RedirectToAction("Trash", "Feedback");
            }
            Feedback feedback = feedbackDao.getRow(id);
            if (feedbackDao == null)
            {
                TempData["message"] = new XMessage("danger", "Mẫu tin không tồn tại");
                return RedirectToAction("Trash", "Feedback");
            }
            feedback.Status = 2;// Trạng thái rac = 0
            feedbackDao.Update(feedback);
            TempData["message"] = new XMessage("success", "Khôi phục thành công");
            return RedirectToAction("Trash", "Feedback");
        }
    }
}

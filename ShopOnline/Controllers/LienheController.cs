using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Models;

namespace ShopOnline.Controllers
{
    public class LienheController : Controller
    {
        private ShopOnlineDbContext db = new ShopOnlineDbContext();
        FeedbackDao feedbackDao = new FeedbackDao();
        // GET: Contact
        public ActionResult LienHe()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LienHe(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                
                feedback.Status = 1;
                feedback.CreatedDate = DateTime.Now;
                var result = feedbackDao.Insert(feedback);
                if (result > 0)
                {
                    ViewBag.Success = "Gửi thành công";
                }
                else
                {
                    ModelState.AddModelError("", "Gửi không thành công.");
                }
            }
            return View(feedback);
        }
    }
}
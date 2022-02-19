using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Models;

namespace ShopOnline.Controllers
{
    public class ModuleController : Controller
    {
        private MenuDao menuDao = new MenuDao();
        private SliderDao sliderDao = new SliderDao();
        private CategoryDao categoryDao = new CategoryDao();
        // GET: Module
        public ActionResult MainMenu()
        {
            List<Menu> list = menuDao.getListByParentId("mainmenu",0);
            return View("MainMenu", list);
        }
        public ActionResult MainMenuSub(int id)
        {
            Menu menu = menuDao.getRow(id);
            List<Menu> list = menuDao.getListByParentId("mainmenu", id);
            if(list.Count==0)
            {
                return View("MainMenuSub1", menu);// ko có con
            }    
            else
            {
                ViewBag.menu = menu;
                return View("MainMenuSub2", list);//có con
            }    
        }
        //Slideshow
        public ActionResult Slideshow()
        {
            List<Slider> list = sliderDao.getListByPosition("Slideshow");
            return View("Slideshow", list);
        }
        //ListCategory
        public ActionResult ListCategory()
        {
            List<Category> list = categoryDao.getListByParentId(0);
            return View("ListCategory", list);
        }
        //PostLastNews
        public ActionResult PostLastNews()
        {
            return View("PostLastNews");
        }
        public ActionResult MenuFooter()
        {
            List<Menu> list = menuDao.getListByParentId("footermenu", "page", 0);
            return View("MenuFooter", list);
        }
        public ActionResult MenuCategoryFooter()
        {
            List<Menu> list = menuDao.getListByParentId("footermenu", "category", 0);
            return View("MenuCategoryFooter", list);
        }
    }
}
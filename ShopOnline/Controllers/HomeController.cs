using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyClass.Models;
using MyClass.DAO;
using PagedList;

namespace ShopOnline.Controllers
{
    public class HomeController : Controller
    {
        LinkDao linkDao = new LinkDao();
        ProductDao productDao = new ProductDao();
        PostDao postDao = new PostDao();
        CategoryDao categoryDao = new CategoryDao();
        ContentDao contentDao = new ContentDao();
        // Url mặc định hoặc URL bất kỳ
        public ActionResult Index(string slug=null,int? page=null)
        {
            if (slug == null)
            {
                return this.Home();
            }
            else
            {
                Link link = linkDao.getRow(slug);
                if (link != null)
                {
                    // slug có trong bảng link
                    string typelink = link.TypeLink;
                    switch (typelink)
                    {
                        case "category":
                            {
                                return this.ProductCategory(slug,page);
                            }
                        case "content":
                            {
                                return this.PostContent(slug,page);
                            }
                        case "page":
                            {
                                return this.PostPage(slug);
                            }
                        case "supplier":
                            {
                                return this.ProductSupplier(slug);
                            }
                        default:
                            {
                                return this.Error404(slug);
                            }
                    }
                }
                else
                {
                    //slug có trong bảng product
                    Product product = productDao.getRow(slug);
                    if(product!=null)
                    {
                        return this.ProductDetail(slug);
                    }    
                    else
                    {
                        Post post = postDao.getRow(slug);
                        if(post!=null)
                        {
                            return this.PostDetail(post);
                        }    
                        else
                        {
                            return this.Error404(slug);
                        }    
                    }    
                    //slug có trong bảng Post Post-type=post
                    // slug không có trong bảng link
                }
            }    
        }
        //Trang chủ
        public ActionResult Home()
        {
           
            List<Category> list = categoryDao.getListByParentId(0);
            return View("Home", list);
        }
        public ActionResult HomeProduct(int id)
        {
            Category category = categoryDao.getRow(id);
            ViewBag.Category = category;
            //Danh mục loại theo 3 cấp
            List<int> listcatid = new List<int>();
            listcatid.Add(id);//cấp 1
            List<Category> listcategory2 = categoryDao.getListByParentId(id);
            if(listcategory2.Count()!=0)
            {
                foreach(var category2 in listcategory2)
                {
                    listcatid.Add(category2.ID);//cấp 2
                    List<Category> listcategory3 = categoryDao.getListByParentId(category2.ID);
                    if(listcategory3.Count()!=0)
                    {
                        foreach(var category3 in listcategory3)
                        {
                            listcatid.Add(category3.ID);//cấp 3
                        }                           
                    }    
                }    
            }    
            List<ProductInfo> list = productDao.getListByListCatId(listcatid,4);
            return View("HomeProduct", list);
        }
        //Nhóm Action Product
        public ActionResult Product(int? page)
        {
            int pageNumber = page ?? 1;//Trang hien tai
            int pageSize = 9;//So mau tin hien thi tren 1 trang
            IPagedList<ProductInfo> list = productDao.getList(pageSize, pageNumber);
            return View("Product", list);
        }
        public ActionResult ProductCategory(string slug,int? page)
        {
            int pageNumber = page ?? 1;//Trang hien tai
            int pageSize = 9;//So mau tin hien thi tren 1 trang
            Category category = categoryDao.getRow(slug);
            ViewBag.Category = category;
            //Danh mục loại theo 3 cấp
            List<int> listcatid = new List<int>();
            listcatid.Add(category.ID);//cấp 1
            List<Category> listcategory2 = categoryDao.getListByParentId(category.ID);
            if (listcategory2.Count() != 0)
            {
                foreach (var category2 in listcategory2)
                {
                    listcatid.Add(category2.ID);//cấp 2
                    List<Category> listcategory3 = categoryDao.getListByParentId(category2.ID);
                    if (listcategory3.Count() != 0)
                    {
                        foreach (var category3 in listcategory3)
                        {
                            listcatid.Add(category3.ID);//cấp 3
                        }
                    }
                }
            }
            IPagedList<ProductInfo> list = productDao.getListByListCatId(listcatid, pageSize, pageNumber);
            return View("ProductCategory", list);
        }
        public ActionResult ProductSupplier(string slug)
        {

            return View("ProductSupplier");
        }
        public ActionResult ProductDetail(String slug)
        {
            int limit = 3;
            var row = productDao.getRow(slug);
            int catid = row.CategoryID;//Thuộc loại nào
            List<int> liscatid = categoryDao.getListId(catid);
            var listother = productDao.getList(liscatid, limit, row.ID, true);
            ViewBag.ListOther = listother;
            return View("ProductDetail", row);
        }
       
        //Nhóm Post
        public ActionResult Post()
        {
          
            List<Post> list = postDao.getList("Post");
            return View("Post", list);
        }
        public ActionResult PostContent(string slug,int? page)
        {
            Content content = contentDao.getRow(slug);
            ViewBag.Content = content;
            List<Post> list= postDao.getListByTopicId(content.ID, "Post", null);
            return View("PostContent", list);
        }
        public ActionResult PostPage(string slug)
        {
            Post post = postDao.getRow(slug,"Page");
            return View("PostPage", post);
        }
        public ActionResult PostDetail(Post post)
        {
            ViewBag.ListOther = postDao.getListByTopicId(post.ContentId,"Post",post.ID);
            return View("PostDetail", post);
        }
        //Hàm lỗi 
        public ActionResult Error404(string slug)
        {

            return View("Error404");
        }
    }
}
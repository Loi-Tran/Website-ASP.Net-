using MyClass.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace MyClass.DAO
{
    public class ProductDao
    {
        private ShopOnlineDbContext db = new ShopOnlineDbContext();
        //trả về danh sách các mẫu tin
        public List<ProductInfo> getListByListCatId(List<int> listcatid,int take)
        {
            List<ProductInfo> list = db.Products
                 .Join(
                            db.Categories,
                            p => p.CategoryID,
                            c => c.ID,
                            (p, c) => new ProductInfo
                            {
                                ID = p.ID,
                                Name = p.Name,
                                CategoryName = c.Name,
                                Code = p.Code,
                                Slug = p.Slug,
                                Description = p.Description,
                                Image = p.Image,
                                MoreImages = p.MoreImages,
                                Price = p.Price,
                                PromotionPrice = p.PromotionPrice,
                               
                                Quantity = p.Quantity,
                                CategoryID = p.CategoryID,
                                Detail = p.Detail,
                             
                                CreatedDate = p.CreatedDate,
                                CreatedBy = p.CreatedBy,
                                ModifieDate = p.ModifieDate,
                                ModifieBy = p.ModifieBy,
                                MetaKeywords = p.MetaKeywords,
                                MetaDescriptions = p.MetaDescriptions,
                                Status = p.Status,
                              
                                ViewCount = p.ViewCount,
                                SuppierId = p.SuppierId,
                            }
                            ).Where(m => m.Status == 1 && listcatid.Contains(m.CategoryID))
                            .OrderByDescending(m => m.CreatedDate)
                            .Take(take)
                            .ToList();
            return list;
        }
        public IPagedList<ProductInfo> getListByListCatId(List<int> listcatid, int pageSize, int pageNumber)
        {
            IPagedList<ProductInfo> list = db.Products
                 .Join(
                            db.Categories,
                            p => p.CategoryID,
                            c => c.ID,
                            (p, c) => new ProductInfo
                            {
                                ID = p.ID,
                                Name = p.Name,
                                CategoryName = c.Name,
                                Code = p.Code,
                                Slug = p.Slug,
                                Description = p.Description,
                                Image = p.Image,
                                MoreImages = p.MoreImages,
                                Price = p.Price,
                                PromotionPrice = p.PromotionPrice,
                         
                                Quantity = p.Quantity,
                                CategoryID = p.CategoryID,
                                Detail = p.Detail,
                             
                                CreatedDate = p.CreatedDate,
                                CreatedBy = p.CreatedBy,
                                ModifieDate = p.ModifieDate,
                                ModifieBy = p.ModifieBy,
                                MetaKeywords = p.MetaKeywords,
                                MetaDescriptions = p.MetaDescriptions,
                                Status = p.Status,
                               
                                ViewCount = p.ViewCount,
                                SuppierId = p.SuppierId,
                            }
                            ).Where(m => m.Status == 1 && listcatid.Contains(m.CategoryID))
                            .OrderByDescending(m => m.CreatedDate)
                            .ToPagedList(pageNumber, pageSize);
            return list;
        }
        public IPagedList<ProductInfo> getList(int pageSize, int pageNumber)
        {
            IPagedList<ProductInfo> list = db.Products
                 .Join(
                            db.Categories,
                            p => p.CategoryID,
                            c => c.ID,
                            (p, c) => new ProductInfo
                            {
                                ID = p.ID,
                                Name = p.Name,
                                CategoryName = c.Name,
                                Code = p.Code,
                                Slug = p.Slug,
                                Description = p.Description,
                                Image = p.Image,
                                MoreImages = p.MoreImages,
                                Price = p.Price,
                                PromotionPrice = p.PromotionPrice,
                     
                                Quantity = p.Quantity,
                                CategoryID = p.CategoryID,
                                Detail = p.Detail,
                              
                                CreatedDate = p.CreatedDate,
                                CreatedBy = p.CreatedBy,
                                ModifieDate = p.ModifieDate,
                                ModifieBy = p.ModifieBy,
                                MetaKeywords = p.MetaKeywords,
                                MetaDescriptions = p.MetaDescriptions,
                                Status = p.Status,
                            
                                ViewCount = p.ViewCount,
                                SuppierId = p.SuppierId,
                            }
                            ).Where(m => m.Status == 1 )
                            .OrderByDescending(m => m.CreatedDate)                          
                            .ToPagedList(pageNumber,pageSize);
            return list;
        }
        public List<ProductInfo> GetListJoin(string page = "All")
        {
            List<ProductInfo> list = null;
            switch (page)
            {
                case "Index":
                    {
                        list = db.Products
                            .Join(
                            db.Categories,
                            p => p.CategoryID,
                            c => c.ID,
                            (p, c) => new ProductInfo
                            {                               
                                ID = p.ID,
                                Name = p.Name,
                                CategoryName = c.Name,
                                Code = p.Code,
                                Slug = p.Slug,
                                Description = p.Description,
                                Image = p.Image,
                                MoreImages = p.MoreImages,
                                Price = p.Price,
                                PromotionPrice = p.PromotionPrice,
                           
                                Quantity = p.Quantity,
                                CategoryID = p.CategoryID,
                                Detail = p.Detail,
                  
                                CreatedDate = p.CreatedDate,
                                CreatedBy = p.CreatedBy,
                                ModifieDate = p.ModifieDate,
                                ModifieBy = p.ModifieBy,
                                MetaKeywords = p.MetaKeywords,
                                MetaDescriptions = p.MetaDescriptions,
                                Status = p.Status,
                              
                                ViewCount = p.ViewCount,
                                SuppierId = p.SuppierId,
                            }
                            ).Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Products
                            .Join(
                            db.Categories,
                            p => p.CategoryID,
                            c => c.ID,
                            (p, c) => new ProductInfo
                            {
                                ID = p.ID,
                                Name = p.Name,
                                CategoryName = c.Name,
                                Code = p.Code,
                                Slug = p.Slug,
                                Description = p.Description,
                                Image = p.Image,
                                MoreImages = p.MoreImages,
                                Price = p.Price,
                                PromotionPrice = p.PromotionPrice,
                          
                                Quantity = p.Quantity,
                                CategoryID = p.CategoryID,
                                Detail = p.Detail,
                         
                                CreatedDate = p.CreatedDate,
                                CreatedBy = p.CreatedBy,
                                ModifieDate = p.ModifieDate,
                                ModifieBy = p.ModifieBy,
                                MetaKeywords = p.MetaKeywords,
                                MetaDescriptions = p.MetaDescriptions,
                                Status = p.Status,
                               
                                ViewCount = p.ViewCount,
                                SuppierId = p.SuppierId,
                            }
                            ).Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Products
                            .Join(
                            db.Categories,
                            p => p.CategoryID,
                            c => c.ID,
                            (p, c) => new ProductInfo
                            {
                                ID = p.ID,
                                Name = p.Name,
                                CategoryName = c.Name,
                                Code = p.Code,
                                Slug = p.Slug,
                                Description = p.Description,
                                Image = p.Image,
                                MoreImages = p.MoreImages,
                                Price = p.Price,
                                PromotionPrice = p.PromotionPrice,
                   
                                Quantity = p.Quantity,
                                CategoryID = p.CategoryID,
                                Detail = p.Detail,
                      
                                CreatedDate = p.CreatedDate,
                                CreatedBy = p.CreatedBy,
                                ModifieDate = p.ModifieDate,
                                ModifieBy = p.ModifieBy,
                                MetaKeywords = p.MetaKeywords,
                                MetaDescriptions = p.MetaDescriptions,
                                Status = p.Status,
                               
                                ViewCount = p.ViewCount,
                                SuppierId = p.SuppierId,
                            }
                            ).ToList();
                        break;
                    }
            }
            return list;
        }
        //Trả về 1 mẫu tin
        public Product getRow(long? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Products.Find(id);
            }
        }
        public Product getRow(string slug)
        {            
            var row = db.Products
                .Where(m => m.Slug == slug && m.Status == 1)
                .FirstOrDefault();
            return row;        
        }
        //Thêm mẫu tin
        public int Insert(Product row)
        {
            db.Products.Add(row);
            return db.SaveChanges();
        }
        //Cập nhật mẫu tin
        public int Update(Product row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        //Xóa mẫu tin
        public int Delete(Product row)
        {
            db.Products.Remove(row);
            return db.SaveChanges();
        }
        public List<Product> getList(List<int> listcatid, int limit, int notid, bool check = true)
        {
            var list = db.Products
                .Where(m => m.Status == 1 && m.ID != notid && listcatid.Contains(m.CategoryID))
                .OrderByDescending(m => m.CreatedDate)
                .Take(limit)
                .ToList();
            return list;
        }
    }
}

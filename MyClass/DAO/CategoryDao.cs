using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyClass.Models;

namespace MyClass.DAO
{
    public class CategoryDao
    {
        private ShopOnlineDbContext db = new ShopOnlineDbContext();
        //trả về danh sách các mẫu tin
        public List<int> getListId(int parentid)
        {
            List<int> listcatid = new List<int>();
            List<Category> listcategory1 = this.getList(parentid);
            if (listcategory1.Count > 0)
            {
                foreach (var cat1 in listcategory1)
                {
                    listcatid.Add(cat1.ID);
                    List<Category> listcategory2 = this.getList(cat1.ID);
                    if (listcategory2.Count > 0)
                    {
                        foreach (var cat2 in listcategory2)
                        {
                            listcatid.Add(cat2.ID);
                            List<Category> listcategory3 = this.getList(cat2.ID);
                            if (listcategory3.Count > 0)
                            {
                                foreach (var cat3 in listcategory3)
                                {
                                    listcatid.Add(cat3.ID);
                                }
                            }
                        }
                    }
                }
            }

            listcatid.Add(parentid);
            return listcatid;
        }
        public List<Category> getListByParentId(int parentid=0)
        {
            return db.Categories.Where(m => m.ParentID == parentid && m.Status == 1)
                .OrderBy(m => m.DisplayOrder)
                .ToList();
        }
        public List<Category> getList(int? parentid = 0)
        {
            var list = db.Categories
                .Where(m => m.ParentID == parentid && m.Status == 1)
                .OrderBy(m => m.DisplayOrder)
                .ToList();
            return list;
        }
        public List<Category> getList(string status = "All")
        {
            List<Category> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list=db.Categories.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash":
                    {
                        list= db.Categories.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        list= db.Categories.ToList();
                        break;
                    }
            }
            return list;
        }
        //Trả về 1 mẫu tin
        public Category getRow(long? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Categories.Find(id);
            }
        }
        public Category getRow(string slug)
        {
            if (slug == null)
            {
                return null;
            }
            else
            {
                return db.Categories.Where(m => m.Slug == slug && m.Status == 1).FirstOrDefault();
            }
        }
        //Thêm mẫu tin
        public int Insert(Category row)
        {
            db.Categories.Add(row);
            return db.SaveChanges();
        }
        //Cập nhật mẫu tin
        public int Update(Category row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        //Xóa mẫu tin
        public int Delete(Category row)
        {
            db.Categories.Remove(row);
            return db.SaveChanges();
        }
    }
}

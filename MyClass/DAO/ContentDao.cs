using MyClass.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class ContentDao
    {
        private ShopOnlineDbContext db = new ShopOnlineDbContext();
        //trả về danh sách các mẫu tin
        public List<Content> getList(string status = "All")
        {
            List<Content> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Contents.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Contents.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Contents.ToList();
                        break;
                    }
            }
            return list;
        }
        //Trả về 1 mẫu tin
        public Content getRow(long? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Contents.Find(id);
            }
        }
        public Content getRow(string slug)
        {          
            return db.Contents.Where(m=>m.Slug==slug&&m.Status==1).FirstOrDefault();      
        }
        //Thêm mẫu tin
        public int Insert(Content row)
        {
            db.Contents.Add(row);
            return db.SaveChanges();
        }
        //Cập nhật mẫu tin
        public int Update(Content row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        //Xóa mẫu tin
        public int Delete(Content row)
        {
            db.Contents.Remove(row);
            return db.SaveChanges();
        }
    }
}

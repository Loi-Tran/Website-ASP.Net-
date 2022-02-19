using MyClass.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class FooterDao
    {
        private ShopOnlineDbContext db = new ShopOnlineDbContext();
        //trả về danh sách các mẫu tin
        public List<Footer> getList(string status = "All")
        {
            List<Footer> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Footers.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Footers.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Footers.ToList();
                        break;
                    }
            }
            return list;
        }
        //Trả về 1 mẫu tin
        public Footer getRow(long? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Footers.Find(id);
            }
        }
        //Thêm mẫu tin
        public int Insert(Footer row)
        {
            db.Footers.Add(row);
            return db.SaveChanges();
        }
        //Cập nhật mẫu tin
        public int Update(Footer row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        //Xóa mẫu tin
        public int Delete(Footer row)
        {
            db.Footers.Remove(row);
            return db.SaveChanges();
        }
    }
}

using MyClass.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class FeedbackDao
    {
        private ShopOnlineDbContext db = new ShopOnlineDbContext();
        //trả về danh sách các mẫu tin
        public List<Feedback> getList(string status = "All")
        {
            List<Feedback> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Feedbacks.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Feedbacks.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Feedbacks.ToList();
                        break;
                    }
            }
            return list;
        }
        //Trả về 1 mẫu tin
        public Feedback getRow(long? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Feedbacks.Find(id);
            }
        }
        //Thêm mẫu tin
        public int Insert(Feedback row)
        {
            db.Feedbacks.Add(row);
            return db.SaveChanges();
        }
        //Cập nhật mẫu tin
        public int Update(Feedback row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        //Xóa mẫu tin
        public int Delete(Feedback row)
        {
            db.Feedbacks.Remove(row);
            return db.SaveChanges();
        }
    }
}

using MyClass.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class MenuDao
    {
        private ShopOnlineDbContext db = new ShopOnlineDbContext();
        //trả về danh sách các mẫu tin
        public List<Menu> getListByParentId(string position, int parentid=0)
        {
            return db.Menus
                .Where(m=>m.ParentID==parentid&&m.Status==1&&m.Position==position)
                .OrderBy(m=>m.DisplayOrder)
                .ToList();
        }
        public List<Menu> getListByParentId(string position, string typemenu, int parentid = 0)
        {
            return db.Menus
                .Where(m => m.ParentID == parentid && m.Status == 1 && m.Position == position && m.TypeMenu == typemenu)
                .OrderBy(m => m.DisplayOrder)
                .ToList();
        }
        public List<Menu> getList(string status = "All")
        {
            List<Menu> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Menus.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Menus.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Menus.ToList();
                        break;
                    }
            }
            return list;
        }
        //Trả về 1 mẫu tin
        public Menu getRow(long? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Menus.Find(id);
            }
        }
        //Thêm mẫu tin
        public int Insert(Menu row)
        {
            db.Menus.Add(row);
            return db.SaveChanges();
        }
        //Cập nhật mẫu tin
        public int Update(Menu row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        //Xóa mẫu tin
        public int Delete(Menu row)
        {
            db.Menus.Remove(row);
            return db.SaveChanges();
        }
    }
}

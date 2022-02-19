using MyClass.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class PostDao
    {
        private ShopOnlineDbContext db = new ShopOnlineDbContext();
        //trả về danh sách các mẫu tin
        public List<Post> getListByTopicId(int? contenid,string type = "Post",int? notid=null)
        {
            List<Post> list = null;
            if(notid==null)
            {
                list = db.Posts.Where(m => m.Status == 1 && m.PostType == type && m.ContentId == contenid).ToList();
            }    
            else
            {
                list = db.Posts.Where(m => m.Status == 1 && m.PostType == type && m.ContentId == contenid&&m.ID!=notid).ToList();
            }    
            return list;
        }
        public List<Post> getList(string type="Post")
        {
            return db.Posts.Where(m => m.Status == 1 && m.PostType == type).ToList();
        }
        public List<Post> getList(string status = "All", string type="Post")
        {
            List<Post> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Posts.Where(m => m.Status != 0 && m.PostType == type).ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Posts.Where(m => m.Status == 0 && m.PostType == type).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Posts.Where(m=>m.PostType == type).ToList();
                        break;
                    }
            }
            return list;
        }
        //Trả về 1 mẫu tin
        public Post getRow(long? id)
        {               
             return db.Posts.Find(id);          
        }
        public Post getRow(string slug)
        {
            return db.Posts.Where(m =>  m.Slug == slug && m.Status == 1).FirstOrDefault();
        }
        public Post getRow(string slug,string posttype)
        {
            return db.Posts.Where(m => m.PostType == posttype && m.Slug == slug && m.Status == 1).FirstOrDefault();       
        }
        //Thêm mẫu tin
        public int Insert(Post row)
        {
            db.Posts.Add(row);
            return db.SaveChanges();
        }
        //Cập nhật mẫu tin
        public int Update(Post row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        //Xóa mẫu tin
        public int Delete(Post row)
        {
            db.Posts.Remove(row);
            return db.SaveChanges();
        }
    }
}

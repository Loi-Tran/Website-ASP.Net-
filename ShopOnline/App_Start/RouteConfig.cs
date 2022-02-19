using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopOnline
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{botdetect}",
                new {botdetect=@"(.*)BotDetectCaptcha\.ashx"});
            //Khai báo url = cố định
            routes.MapRoute(
                name: "TatCaSanPham",
                url: "tat-ca-san-pham",
                defaults: new { controller = "Home", action = "Product", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                  name: "TatCaBaiViet",
                  url: "tat-ca-bai-viet",
                  defaults: new { controller = "Home", action = "Post", id = UrlParameter.Optional }//Post=About
              );
            routes.MapRoute(
                  name: "LienHe",
                  url: "lien-he",
                  defaults: new { controller = "Lienhe", action = "LienHe", id = UrlParameter.Optional }
              );
            routes.MapRoute(
                  name: "GioHang",
                  url: "gio-hang",
                  defaults: new { controller = "Giohang", action = "Index", id = UrlParameter.Optional }
              );
            routes.MapRoute(
                 name: "ThanhToan",
                 url: "thanh-toan",
                 defaults: new { controller = "Giohang", action = "ThanhToan", id = UrlParameter.Optional }
             );
            routes.MapRoute(
                 name: "ThanhCong",
                 url: "thanh-cong",
                 defaults: new { controller = "Giohang", action = "ThanhCong", id = UrlParameter.Optional }
             );
            routes.MapRoute(
                  name: "TimKiem",
                  url: "tim-kiem",
                  defaults: new { controller = "Timkiem", action = "Index", id = UrlParameter.Optional }
              );
            routes.MapRoute(
                name: "DangNhap",
                url: "dang-nhap",
                defaults: new { controller = "Khachhang", action = "DangNhap", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "DangKi",
                url: "dang-ki",
                defaults: new { controller = "Khachhang", action = "DangKi", id = UrlParameter.Optional }
            );
            //Khai báo URL động- Nằm kế trên default
            routes.MapRoute(
                  name: "SiteSlug",
                  url: "{slug}",
                  defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
              );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

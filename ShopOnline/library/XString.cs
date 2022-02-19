using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ShopOnline
{
    public static class XString
    {
        public static string Str_slug(string s)
        {
            String[][] symbols = 
                {
                new String[]{ "/á/á|à|ả|ạ|ã|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ/gi", "a"},
                new String[]{"/é|é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ/gi","e"},
                new String[]{"/i|í|ì|ỉ|ĩ|ị/gi","i"},
                new String[]{"/ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ/gi","o"},
                new String[]{"/ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự/gi","u"},
                new String[]{"/ý|ỳ|ỷ|ỹ|ỵ/gi","y"},
                new String[]{"[\\s'\";,]","-"}
            };
            s = s.ToLower();
            foreach (var ss in symbols)
            {
                s = Regex.Replace(s, ss[0], ss[1]);
            }
            return s;
        }
        public static string Str_Limit(this string str,int? length)
        {
            int lengt = (length ?? 20);
            if(str.Length>lengt)
            {
                str = str.Substring(0, lengt) + "...";
            }
            return str;
        }
        public static string ToMD5(this string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sbHash = new StringBuilder();
            foreach (byte b in bHash)
            {
                sbHash.Append(String.Format("{0:x2}", b));
            }
            return sbHash.ToString();
        }
        public static string ToShortString(this string str,int? lenght)
        {
            int lengt = (lenght ?? 20);
            if(str.Length>lenght)
            {
                str = str.Substring(0, lengt) + "...";
            }
            return str;
        }
    }
}
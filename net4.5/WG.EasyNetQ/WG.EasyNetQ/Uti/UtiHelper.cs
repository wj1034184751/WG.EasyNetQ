using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WG.EasyNetQ.Uti
{
    public class UnitHelper
    {
        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <param name="name"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string GetVersion(string name, string json)
        {
            return MD5(string.Format("{0}{1}", name, json));
        }

        /// <summary>
        /// 生成MD5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MD5(string input)
        {
            System.Security.Cryptography.MD5 md5Hasher = System.Security.Cryptography.MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Unicode.GetBytes(input));
            StringBuilder sBuiler = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuiler.Append(data[i].ToString("X2"));
            }
            return sBuiler.ToString().ToLower();
        }



        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static T DeserializeObject<T>(string Json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Json);
        }
    }
}

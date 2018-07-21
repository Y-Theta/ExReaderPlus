using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using RestSharp;
using Newtonsoft.Json;

namespace ExReaderPlus.Baidu
{
    public class Translate
    {
        private string _appid = "20180717000186138";
        private string _key = "iklsyC4TNsJUZ9dx9j9V";
        private string _salt = "1435660288";
        private string _from = "auto";
        private string ParameterTo = "zh";//中文
        public string Text
        {
            get; set;
        }
        private string Sign
        {
            get { return string.Format("{0}{1}{2}{3}", _appid, Text, _salt, _key); }
        }
        /// <summary>
        /// 把属性变成了方法
        /// </summary>

        //MD5加密
        private string GetMd5()
        {
            var md5 = new MD5CryptoServiceProvider();
            var result = Encoding.UTF8.GetBytes(Sign);
            var output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "").ToLower();
        }

        public string GetJson()
        {
            var client = new RestClient("http://api.fanyi.baidu.com");
            var request = new RestRequest("/api/trans/vip/translate", Method.GET);
            request.AddParameter("q", Text);
            request.AddParameter("from", _from);
            request.AddParameter("to", ParameterTo);
            request.AddParameter("appid", _appid);
            request.AddParameter("salt", _salt);
            var sign = GetMd5();
            request.AddParameter("sign", GetMd5());
            IRestResponse response = client.Execute(request);
            return response.Content;
        }

        public string GetResult()
        {
            var lst = new List<string>();
            var content = GetJson();
            dynamic json = JsonConvert.DeserializeObject(content);
            foreach (var item in json.trans_result)
            {
                lst.Add(item.dst.ToString());
            }
            return string.Join(";", lst);
        }



    }
}

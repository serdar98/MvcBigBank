using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Formatting;

namespace MvcBigBank.Controllers
{
    public class TypeHolder
    {
        public int age { get; set; }
        public int creditCount { get; set; }
        public float creditSize { get; set; }
        public int homeState { get; set; }
        public int phoneState { get; set; }

        public string name { get; set; }

    }
    public class CreditController : Controller
    {
        static string result ="";
        static int temp = 2;
        public ActionResult Resulto()
        {
            if (temp==1)
            {
                ViewBag.value = "Krediniz Onaylanmıştır";
            }
            else if (temp == 0)
            {
                ViewBag.value = "Krediniz Başvurunuz Reddedilmiştir";
            }
            else
                ViewBag.value = "Opps!!";

            return View();
        }
        public ActionResult CreditApplication()
        {
            ViewBag.Message = "Alanların hepsini eksiksiz doldurunuz";
            return View();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> CreditApplication(int age,int creditCount,float creditSize,int homeState, int phoneState)
        {
            TypeHolder hello = new TypeHolder();
            hello.age = age;
            hello.creditCount= creditCount;
            hello.creditSize = creditSize;
            hello.homeState = homeState;
            hello.phoneState = phoneState;



            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://127.0.0.1:4000/krediTahmin/predict");

            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            var values = new Dictionary<string,string>
            {
                { "age", hello.age.ToString() },
                { "creditCount", hello.creditCount.ToString() },
                { "creditSize", hello.creditSize.ToString() },
                { "homeState", hello.phoneState.ToString() },
                { "phoneState", hello.homeState.ToString() }

            };
            HttpContent content = new StringContent(
            JsonConvert.SerializeObject(values),
            Encoding.UTF8,
            "application/json"
            );

            //var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(client.BaseAddress, content);
            result = response.Content.ReadAsStringAsync().Result;
            if (result.Contains("1.0"))
            {
                temp = 1;
            }
            else
                temp = 0;
            return RedirectToAction("Resulto", "Credit");
        }

    }
}
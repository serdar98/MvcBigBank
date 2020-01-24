using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBigBank.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Helpers;
using Newtonsoft.Json;
using System.Net.Http.Formatting;
using System.Data;
using System.Data.SqlClient;

namespace MvcBigBank.Controllers
{

    public class Holder
    {
        public int faturaID { get; set; }
        public int aboneID { get; set; }
        public decimal guncelBorc { get; set; }
        public DateTime sonOdemeTarihi { get; set; }

    }
    public class PaymentController : Controller
    {
        public static List<Holder> list;
        private db_BankProjectEntities2 db = new db_BankProjectEntities2();
        private faturaDBEntities1 fdb = new faturaDBEntities1();
        static decimal borc = 0;
        static string tarih = "";
        static int faturaID = 0;
        static string tel = "";
        static string hold = null;
        static string hold2 = null;

        public ActionResult PayTheBill()
        {
            ViewBag.Message = hold;
            ViewBag.Message2 = hold2;

            return View();

        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> PayTheBill(string telefon)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44304/api/Fatura/FaturaBilgi/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            HttpContent content = new StringContent(
            JsonConvert.SerializeObject(telefon),
            Encoding.UTF8,
            "application/json"
            );

            var response = await client.GetAsync(client.BaseAddress+telefon);
            string result = response.Content.ReadAsStringAsync().Result;
            list = JsonConvert.DeserializeObject<List<Holder>>(result);
            if (list.Count()>0)
            {
            tel = telefon;
            faturaID = Convert.ToInt32(list[0].faturaID);
            borc = Convert.ToDecimal(list[0].guncelBorc);
            tarih = list[0].sonOdemeTarihi.ToString("dd-MM-yyyy");
            return RedirectToAction("ShowTheBill", "Payment");
            }
            else
            {
                hold = "Girilen telefon numarasına ait borç bilgisi bulunamadı";
                ViewBag.Message = hold;
                return View();
            }

        }


        public ActionResult ShowTheBill()
        {
            list.Clear();
            ViewBag.value = borc;
            ViewBag.Message = tarih;
            return View();

        }


        public ActionResult SendingPayment()
        {
            ViewBag.Message = hold2;
            return View();

        }

        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> PayingTheBill(int id)
        {

            Hesap hesap = new Hesap();
            foreach (var item in db.Hesap)
            {
                if (item.hesapID==id)
                {
                    hesap = item;
                }
            }

            if (borc<=hesap.bakiye)
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:44304/api/Fatura/FaturaOde/");
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(
                JsonConvert.SerializeObject(tel),
                Encoding.UTF8,
                "application/json"
                );

                var response = await client.GetAsync(client.BaseAddress + tel);
                string result = response.Content.ReadAsStringAsync().Result;
                if (result=="true")
                {
                    db.sp_payTheBill(hesap.hesapNo, borc);
                    hold2 = "Odeme işleminiz başarıyla gerçekleştirilmiştir";
                    return RedirectToAction("PayTheBill", "Payment");
                }
                else
                {
                    hold2 = "Odeme işleminiz sırasında sorun ile karşılaştık. Lütfen daha sonra tekrar deneyiniz.";
                    return RedirectToAction("PayTheBill", "Payment");
                }
                    
            }
            else
            {
                hold2 = "Bakiyeniz Yetersiz Lütfen Başka Bir Hesap Seçiniz";
                return RedirectToAction("SendingPayment", "Payment");
            }


        }

        public int MusteriIdAlma(string tcKimlik)
        {

            int temp = 0;
            foreach (var item in db.Musteri)
            {
                if (item.tcKimlik == tcKimlik)
                    temp = item.musteriID;
            }
            return temp;
        }
    }
}
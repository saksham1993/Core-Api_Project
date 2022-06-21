using Core_Api_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Core_Api_Project.Controllers
{
    public class HomeController : Controller
    {
      

        

        Uri apiurl = new Uri("http://localhost:57782");
        HttpClient Client;
        public HomeController()
        {
            Client = new HttpClient();
            Client.BaseAddress = apiurl;
        }
        public IActionResult Index()    
        {
            List<Api_Class> list = new List<Api_Class>();
            HttpResponseMessage list_Detail = Client.GetAsync(Client.BaseAddress + "api/home/Get/Table").Result;
            if (list_Detail.IsSuccessStatusCode)
            {
                string data = list_Detail.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject<List<Api_Class>>(data);

                foreach (var item in res)
                {
                    list.Add(new Api_Class
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Mobile = item.Mobile,
                        School = item.School,
                        Address = item.Address,
                     
                    });


                }


            }
            return View(list);
        }
        [HttpGet]
        public ActionResult FormPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FormPage(Api_Class mvc)
        {

            string data = JsonConvert.SerializeObject(mvc);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage res = Client.PostAsync(Client.BaseAddress + "api/home/Post/Table", content).Result;
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");


        }
     
        public ActionResult Edit(int Id)
        {

            var res1 = Client.GetAsync(Client.BaseAddress + "api/home/Edit/Table" + '?' + "Id" + "=" + Id.ToString()).Result;
            string data = res1.Content.ReadAsStringAsync().Result;
            var v = JsonConvert.DeserializeObject<Api_Class>(data);
            return View("FormPage", v);


            
        }
        public ActionResult Delete(int Id)
        {
            var res2 = Client.DeleteAsync(Client.BaseAddress + "api/home/Delete/Table" + '?' + "Id" + "=" + Id.ToString()).Result;

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult LoginPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginPage(User obj)
        {

            string data = JsonConvert.SerializeObject(obj);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage res = Client.PostAsync(Client.BaseAddress + "api/home/Login/Table", content).Result;

            string data1 = res.Content.ReadAsStringAsync().Result;

            var v = JsonConvert.DeserializeObject<User>(data1);

            if (v.Email == "Email not found")
            {

                TempData["Email"] = "Email not found";

            }

            else
            {

                if (v.Id != 0)
                {

                    //Session["Email"] = v.Email;

                    //Session["Name"] = v.First_Name;

                    return RedirectToAction("Index");

                }

                else
                {
                    TempData["Passwrod"] = "Password is invalid";

                }


            }

            return View();

        }
    }
}

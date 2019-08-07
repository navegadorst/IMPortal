using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IMPortalEvaluation.Models;
using HtmlAgilityPack;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Net.Mail;

namespace IMPortalEvaluation.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            readLoadHMTL();

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #region methods
        private void readLoadHMTL()
        {


            var stream = readstringHtml();
            var document = new HtmlDocument();
            document.Load(stream);
            sendMail(document);
            //var node1 = getNode(document);
            //var node2 = getOtherNode(document);
        }

        private MemoryStream readstringHtml()
        {
            var input = getHtmlToURLAsync();
            byte[] array = Encoding.ASCII.GetBytes(input.Result);
            var ms = new MemoryStream(array);


            return ms;
        }
        private HtmlNode getNode(HtmlDocument document)
        {
            return document.DocumentNode.SelectSingleNode("html/head");
        }

        private async Task<string> getHtmlToURLAsync()
        {
            var result = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Get;
                    request.RequestUri = new Uri("https://www.chartjs.org/samples/latest/charts/doughnut.html", UriKind.Absolute);

                    using (HttpResponseMessage response = await client.SendAsync(request)) //await client.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.Content != null)
                            {
                                var _result = response.Content.ReadAsStringAsync();
                                result = _result.Result;
                                // write result to file
                            }
                        }
                    }
                }
            }
            return result;
        }

        internal void sendMail(HtmlDocument documentHTML)
        {
            try
            {

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("maile@gmail.com");
                mail.To.Add("mail2@gmail.com");
                mail.Subject = "Test Mail";
                mail.Body = documentHTML.ParsedText;//"This is for testing SMTP mail from GMAIL";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("mail@gmail.com", "pasword123");
                SmtpServer.EnableSsl = true;
                mail.IsBodyHtml = true;

                SmtpServer.Send(mail);
                //MessageBox.Show("mail Send");
            }
            catch (Exception ex)
            {
                string error = ex.Message;

            }

        }
        #endregion
    }
}

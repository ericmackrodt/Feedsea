using FeedseaWebSite.App_GlobalResources;
using FeedseaWebSite.Models;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace FeedseaWebSite.Controllers
{
    public class HomeController : Controller
    {
        protected string Language;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (filterContext.RouteData.Values.ContainsKey("lang"))
                Language = filterContext.RouteData.Values["lang"].ToString().ToLower();
            else
                Language = "en-us";

            ViewBag.Language = Language;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Language);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult JoinBeta()
        {
            ViewBag.Success = false;
            ViewBag.Error = false;
            return View();
        }

        [HttpPost]
        public ActionResult JoinBeta(JoinBetaViewModel viewModel)
        {
            string captchaMessage = "";
            if (!Captcha(out captchaMessage))
                return Json(new { success = false, isCaptchaInvalid = true, captchaMessage = captchaMessage }, JsonRequestBehavior.AllowGet);

            try
            {
                var sb = new StringBuilder();
                sb.Append("Name: ");
                sb.AppendLine(viewModel.Name);
                sb.Append("Email: ");
                sb.AppendLine(viewModel.Email);
                sb.AppendLine("Where did you hear about us?");
                sb.AppendLine(viewModel.WhereDidYouHear ?? "");

                SendEmail("support@feedsea.com", "[Join the Beta] " + viewModel.Name, sb.ToString());

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false}, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Feedback()
        {
            ViewBag.Success = false;
            ViewBag.Error = false;
            return View();
        }

        [HttpPost]
        public ActionResult Feedback(FeedbackViewModel viewModel)
        {
            ViewBag.Success = false;
            ViewBag.Error = false;

            string captchaMessage = "";
            if (!Captcha(out captchaMessage))
                return Json(new { success = false, isCaptchaInvalid = true,  captchaMessage = captchaMessage}, JsonRequestBehavior.AllowGet);

            try
            {
                var sb = new StringBuilder();
                sb.Append("Type: ");
                sb.AppendLine(GetType(viewModel.Type));
                sb.Append("Name: ");
                sb.AppendLine(viewModel.Name);
                sb.Append("Email: ");
                sb.AppendLine(viewModel.Email);
                sb.AppendLine("Description");
                sb.AppendLine("");
                sb.AppendLine(viewModel.Description);

                SendEmail(
                    viewModel.Type == "bug" ? "support@feedsea.com" : "feedback@feedsea.com",
                    string.Format("[{0}] {1}...", GetType(viewModel.Type), string.Join("", (viewModel.Description.Length > 20 ? viewModel.Description.Substring(0, 20) : viewModel.Description).Select(o => char.IsLetterOrDigit(o) ? o.ToString() : ""))),
                    sb.ToString());

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        private string GetType(string type)
        {
            switch (type)
            {
                case "bug":
                    return "Bug";
                case "suggestion":
                    return "Suggestion";
                case "other":
                default:
                    return "Other";
            }
        }

        private bool Captcha(out string captchaMessage)
        {
            var recaptchaHelper = this.GetRecaptchaVerificationHelper("6LdIYfESAAAAAIWvOrud2UdbvNQba-eyFCokCWk6");

            if (String.IsNullOrEmpty(recaptchaHelper.Response))
            {
                captchaMessage = Strings.CaptchaEmpty;
                return false;
            }

            RecaptchaVerificationResult recaptchaResult = recaptchaHelper.VerifyRecaptchaResponse();

            if (recaptchaResult != RecaptchaVerificationResult.Success)
            {
                captchaMessage = Strings.IncorrectCaptcha;
                return false;
            }

            captchaMessage = "";
            return true;
        }

        private void SendEmail(string to, string subject, string body)
        {

            //criação do objeto MailMessage
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("feedseaapp@gmail.com");
            mailMessage.To.Add(new MailAddress(to));
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = false; //para enviar mensagens no formato html

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new NetworkCredential("feedseaapp@gmail.com", "aloofer3311");
            smtpClient.EnableSsl = true; //True ou False dependendo se o seu servidor exige SSL

            //este código cria o gerenciador de evento que vai notificar se o email foi enviado ou não

            smtpClient.Send(mailMessage);
        }
    }
}

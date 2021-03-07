using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Notes__Marketplace.Models;
using Notes_MarketPlace.Db.Users_Login_Signup;
using System.Web.Security;
using System.Configuration;
using System.Net.Mail;
using System.Web.Services.Description;
using System.Net;
using System.Web.Hosting;
using System.Text;

namespace Notes__Marketplace.Controllers
{
  
    public class UserController : Controller
    {

        UsersRepository repository = null;
        private bool status;

        public string Subject { get; private set; }
        public bool IsBodyHtml { get; private set; }

        //Constructor
        public UserController() {

            repository = new UsersRepository();
        }

        // GET: User
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel lusr)
        {
            if (ModelState.IsValid)
            {
                int id = repository.Loginusr(lusr);
                if (id == 0 || id==1)
                {
                    FormsAuthentication.SetAuthCookie(lusr.EmailID,false);
                    if (id == 0)
                    {
                        //member 0
                        return RedirectToAction("Dashboard");

                    }
                    else {

                        //admin 1
                        //Return RedirectToAction("Action", "Controller", New With {.id = 99})
                        return RedirectToAction("AdminDashboard", "Admin");
                    }
                }
                else 
                {
                    ViewBag.msguser = "Invalid Username And Password";
                    ModelState.Clear();
                    return View();
                }
            }
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Signup(UsersModel usr)
        {
           
          
            usr.IsActive = true;
            
            if (ModelState.IsValid) {

                if (usr.Password == usr.ConfirmPassword)
                {
                    int id = repository.AddUser(usr);
                    if (id > 0)
                    {
                         ViewBag.msg = "Your account has been successfully created.";
                         TempData["data"] = id+","+usr.EmailID+","+usr.FirstName+ ","+usr.LastName;
                         ModelState.Clear();
                         return RedirectToAction("Emailvarification");
                    }
                }
                else {

                    ViewBag.ConfirmPassMsg = " Password Not Match";
                }
                
            }
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Home()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult SearchNotes() {

            return View();
        
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult FAQ()
        {

            return View();

        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult ContactUS()
        {

            return View();

        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ContactUS(ContactUSModel model) {

            String Body = " Hello," + model.Comment.ToString() +" Regards,  "+ model.FullName+"    "+model.EmailID+" ";
            String subjects =model.Subject;
            String recevermail = "kishankumardpatel17@gnu.ac.in";
            BuildEmailTEmplete(subjects, Body, recevermail);
            ViewBag.msg = "your request has been successfully submitted";
            return View();
        }



        [HttpGet]
        public ActionResult Dashboard()
        {

            return View();

        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Emailvarification()
        {
            string userdata = TempData["data"].ToString();
            string[] List = userdata.Split(',');
            String id = List[0];
            int ids = Convert.ToInt32(id);
            String email = List[1];
            String firstname = List[2];
            String lastname = List[3];
            ViewBag.id = ids;
            ViewBag.email = email;
            ViewBag.fname = firstname;
            ViewBag.lname = lastname;
            ViewBag.viewfname = firstname;
            ViewBag.viewlname = lastname;
            ViewBag.btntext = "VERIFY EMAIL ADDRESS";

            return View();

        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Emailvarification(EmailVerificationModel model)
        {
            String Body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "text" + ".cshtml");
            var url = "https://localhost:44342/" + "User/Verified?id=" + model.ID;
            Body = Body.Replace("@ViewBag.ConfirmationLink", url);
            String subjects = "Note Marketplace - Email Verification";
            String recevermail = model.EmailID;
            BuildEmailTEmplete(subjects, Body, recevermail);

            ViewBag.btntext = "Check Your Mailbox";
            return View();
        }

        [AllowAnonymous]
        private void BuildEmailTEmplete(string subjects, string bodytext, string recevermail)
        {
            String form, to, bcc, cc, subject, body;
            form = "macwin36@gmail.com";
            to = recevermail.Trim();
            bcc = "";
            cc = "";
            subject = subjects;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodytext);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(form);
            mail.To.Add(new MailAddress(to));
            if (!String.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }
            if (!String.IsNullOrEmpty(cc))
            {
                mail.Bcc.Add(new MailAddress(cc));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendEmail(mail);
        }

        [AllowAnonymous]
        private void SendEmail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.UseDefaultCredentials = false;
            //  client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("macwin36@gmail.com", "M@c36Win04");
            client.EnableSsl = true;
            client.Send(mail);
            try
            {
                //   client.Send(mail);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [AllowAnonymous]
        public ActionResult Verified(int id)
        {

            if (id > 0)
            {
                int ids = repository.UpdateUser(id);

            }
            else {

                return View("Emailvarification");
            }
         return RedirectToAction("Login");

        }

        //forgot pass
        [AllowAnonymous]
        public ActionResult Forgotpassword() {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Forgotpassword(ForgotPasswordModel fmodel)
        {
            if (ModelState.IsValid)
            {
                var dataofuser = repository.ForgotPassword(fmodel);
                int id = dataofuser.Item1;
                String randompassword = dataofuser.Item2;
                if (id == 1)
                {
                    ViewBag.erroRMsg = "Please Check Your Mail Box !";
                   
                    String Body = " Hello Dear User, Your New Password : "+randompassword;
                    String subjects = "New Temporary Password has been created for you";
                    String recevermail = fmodel.EmailID;
                    BuildEmailTEmplete(subjects, Body, recevermail);
                    return RedirectToAction("Login");

                }
                else
                {

                    ViewBag.erroRMsg = "Please enter a registered Email Address";
                    return View();
                }
            }
            else 
            {
                ViewBag.erroRMsg = "Enter Valid MailAddress";
                

            }

            return View();
        }
            
        
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("Home");
            
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult NoteDetails() 
        {


            return View();
        }
    }//class


}//namespace

//============================================================================================
//Split String Example


//        string authors = "Mahesh Chand, Henry He, Chris Love, Raj Beniwal, Praveen Kumar";
//        // Split authors separated by a comma followed by space  
//        string[] authorsList = authors.Split(", ");  
//foreach (string author in authorsList) 
//======================================================================================


//tempdata example
//====================
//public ActionResult GetMDN(string msisdn)
//{
//    int sngid = 10;

//    TempData["ID"] = sngid;

//    return RedirectToAction("IIndex");
//}

//public ActionResult IIndex()
//{

//    int id = Convert.ToInt32(TempData["ID"]);// id will be 10;
//}
//============================================================================================
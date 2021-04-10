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
using System.IO;
using PagedList.Mvc;
using PagedList;
using Notes_MarketPlace.Db;

namespace Notes__Marketplace.Controllers
{
  
    public class UserController : Controller
    {


        UsersRepository repository = null;
        private bool status;
        private FormsAuthenticationTicket authTicket;

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
            //method For Profile
            PartialOne();
            return RedirectToAction("Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            LoginModel model = new LoginModel() 
            {
                
            };
            if (Request.Cookies["Login"] != null)
            {
                model.EmailID = Request.Cookies["Login"].Values["EmailID"];
                model.Password = Request.Cookies["Login"].Values["Password"];
            }
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel lusr)
        {
            if (ModelState.IsValid)
            {
                int id = repository.Loginusr(lusr);
                if (id == 0 || id == 1 || id == 2)
                {


                         FormsAuthentication.SetAuthCookie(lusr.EmailID, lusr.Rem);
                         Session["EmailID"] = lusr.EmailID;
                         Session["UserID"] = repository.GetId(lusr.EmailID);
                        if (lusr.Rem)
                        {
                            HttpCookie cookie = new HttpCookie("Login");
                            cookie.Values.Add("EmailID", lusr.EmailID);
                            cookie.Values.Add("Password", lusr.Password);
                            cookie.Expires = DateTime.Now.AddDays(15);
                            Response.Cookies.Add(cookie);
                        }



                    if (id == 0)
                    {
                        //member 0

                        String mail = lusr.EmailID;
                        int myidss = repository.GetId(mail);
                        bool ISThere = repository.GetAnyUserProfileData(myidss);
                        if (ISThere)
                        {
                            //if(lusr.MyCheckBox=="remember")
                            //{
                            //    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,FormsAuthentication.Encrypt(F));
                            //    Response.Cookies.Add(cookie);
                            //}
              
                            return RedirectToAction("Dashboard");
                        }
                        else 
                        {
                            return RedirectToAction("MyProfile");
                        }
                        
                        
                    }
                    else if(id==1 || id==2)
                    {

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
                        TempData["data"] = id + "," + usr.EmailID + "," + usr.FirstName + "," + usr.LastName;
                        ModelState.Clear();
                        return RedirectToAction("Emailvarification");
                    }
                    else {

                        ViewBag.redmsg = "EmailAddress is already exists ";
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
            //method For Profile
            PartialOne();
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult SearchNotes(int? page,String Search,String Note_Type,String Category,String Country,String Course,String University) {

            //method For Profile
            PartialOne();


            var ReviewList = repository.GetReviewData();
            // var dataofuser
            ViewBag.ReviewList = ReviewList;
            //NoteType List
            var getcategory = repository.GetCategory();
            ViewBag.categorylist = getcategory;
            var getcountries = repository.GetCountries();
            ViewBag.countrieslist = getcountries;
            var getnotetype = repository.GetNoteType();
            ViewBag.notetypelist = getnotetype;


            //university List
            var getuniversity = repository.GetUniversity();
            ViewBag.unilist = getuniversity;
            //couse
            var getcourse = repository.GetCourse();
            ViewBag.courselist = getcourse;
            //rating
            var getrating = repository.GetRating();
            ViewBag.ratinglist = getrating;
       
            var MyList = repository.GetSearchNoteData(Search, Note_Type, Category, Country, Course, University);
            
            if(MyList.Count==0)
            {
                ViewBag.msg = "No Data Found";
            }
            return View(MyList.ToPagedList(page ?? 1, 9));
        
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult FAQ()
        {
            //method For Profile
            //method For Profile
            PartialOne();

            return View();

        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult ContactUS()
        {
            //method For Profile
            PartialOne();

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
        public ActionResult Dashboard(int? page,String IPNotes, int? page2,String PublishNotes)
        {
            String mail = User.Identity.Name;
            int loginid = repository.GetId(mail);
            //method For Profile
            PartialOne();
            var DashDataIPList = repository.GetDashboardDataForIPNotes(loginid,IPNotes);
            var DashDataPublishList = repository.GetDashboardDataForPublishNotes(loginid,PublishNotes);
            ViewBag.CategoryList = repository.GetCategoryList();
            ViewBag.RefDataList = repository.GetRefDataList();
            ViewBag.DashIPNotesData = DashDataIPList.ToPagedList(page?? 1,6);
            ViewBag.DashPublishNotesData = DashDataPublishList.ToPagedList(page2?? 1,6);
            if (TempData["mymsg"] != null)
            {
                ViewBag.mymsg = TempData["mymsg"].ToString();

            }

            //number of count mydownloads from id
            ViewBag.MytotalDownloads = repository.GetCountOfMyDownloads(loginid);
            //number of count myrejected from id
            ViewBag.MytotalRejected = repository.GetCountOfMyRejected(loginid);
            //number of count buyer req from id
            ViewBag.MytotalbuyerRequest = repository.GetCountofBuyerReqest(loginid);

            //number of sold count from id
            ViewBag.Mysold = repository.getnumberofsold(loginid);
            ViewBag.MytotalEarning = repository.getearning(loginid);

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
        public ActionResult NoteDetails(String id) 
        {
            //method For Profile
            PartialOne();
            try
            {
                if (TempData["msgdisplay"] != null)
                {
                    ViewBag.msgdisplay = TempData["msgdisplay"].ToString();
                }
                
                if (id!=null ) 
                {

                    int myid = Convert.ToInt32(id);
                    var ReviewData = repository.GetReviewDataFromID(myid);
                    ViewBag.reviewdata = ReviewData;
                    var MyNotesData = repository.GetSearchNoteDataFromID(myid);
                    ViewBag.NotesData = MyNotesData;
                    int idmycountry = (int)MyNotesData.Country;
                    String Country = repository.GetCountryFromID(idmycountry);
                    String category = repository.GetCategoryFromID(MyNotesData.Category);
                    ViewBag.Country = Country;
                    ViewBag.Category = category;
                    //get usernew details using sellernotes  sellerid
                    var Reviewfornoteid = repository.GetUserReviewFromID(myid);
                    ViewBag.UserReview = Reviewfornoteid;
                    var user = repository.GetUserFromReviewID();
                    ViewBag.User = user;




                }
                else 
                {

                    return RedirectToAction("SearchNotes");
                
                }
              
            }
            catch(Exception e) {

                throw e;
            
            }
             
                
           
            //return View(MyList.ToPagedList(page ?? 1, 9));

            return View();
        }
        //====================

        //var context = new SampleEntities();
        //var item = context.categories.ToList();
        //ViewBag.list = item;
        //    return View();

        //================

        [HttpGet]
        public ActionResult AddNotes(String EditNoteID)
        {
            var getcategory = repository.GetCategory();
            ViewBag.categorylist = getcategory;
            var getcountries = repository.GetCountries();
            ViewBag.countrieslist = getcountries;
            var getnotetype = repository.GetNoteType();
            ViewBag.notetypelist = getnotetype;
            ViewBag.disable = "true";

            if (EditNoteID != null)
            {

              
                ViewBag.EditDatamsg = EditNoteID;

                int NoteID = Convert.ToInt32(EditNoteID);
                var SellerNotesData = repository.GetSellerNotesDataFromIDdash(NoteID);
                //TempData["EditNoteData"]TempData["EditNoteData"];
                String path = SellerNotesData.NotesPath;
              
                ViewBag.Title = SellerNotesData.Title;
                ViewBag.Dp = SellerNotesData.DisplayPicture;
                ViewBag.NOFPage = SellerNotesData.NumberofPages;
                ViewBag.Description = SellerNotesData.Description;
                ViewBag.UniversityName = SellerNotesData.UniversityName;
                ViewBag.Course = SellerNotesData.Course;
                ViewBag.Professor = SellerNotesData.Professor;
                ViewBag.CourseCode = SellerNotesData.CourseCode;

                //type
                foreach (var X in getnotetype)
                {
                    if (X.ID == SellerNotesData.Note_Type)
                    {
                        ViewBag.NoteType = "Last : " + X.Name;
                    }
                }
                //ViewBag.Category
                foreach (var X in getcategory)
                {
                    if (X.ID == SellerNotesData.Category)
                    {
                        ViewBag.Category = "Last : " + X.Name;
                    }
                }
                //country ViewBag.Country
                foreach (var X in getcountries)
                {
                    if (X.ID == SellerNotesData.Country)
                    {
                        ViewBag.Country = "Last : " + X.Name;
                    }
                }
                //price data
                if (SellerNotesData.IsPaid == true)
                {
                    ViewBag.PriceData = "Paid";
                    ViewBag.price = SellerNotesData.SellingPrice;
                    @ViewBag.piad = "checked";
                }
                else
                {

                    ViewBag.PriceData = "Free";
                    ViewBag.free = "checked";
                    ViewBag.price = 0;
                }

            }
            

            return View();
        }
       

        [HttpPost]
        public ActionResult AddNotes(AddNotesModel model, System.Web.HttpPostedFileBase DisplayPicture, System.Web.HttpPostedFileBase NotesAttachements, System.Web.HttpPostedFileBase NotesPreview)
        { 
            var getcategory = repository.GetCategory();
            ViewBag.categorylist = getcategory;
            var getcountries = repository.GetCountries();
            ViewBag.countrieslist = getcountries;
            var getnotetype = repository.GetNoteType();
            ViewBag.notetypelist = getnotetype;
            //============================================
            // session id
            String mail = User.Identity.Name;
            int myidss=repository.GetId(mail);
            String dbpathpicture="";
            String dbpathnotes="";
            String dbpathpreview="";

            if(model.EditNoteID!=null)
            {
                if(DisplayPicture!=null)
                {
                    var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
                    // obj.img = model.img;
                    var fileName = Path.GetFileName(DisplayPicture.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(DisplayPicture.FileName); //getting the extension(ex-.jpg)  
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        var name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                        var myfile = name + "_" + myidss + "_" + ext; //appending the name with id                   
                        var path = Path.Combine(Server.MapPath("~/UploadData/"), myfile); // store the file inside ~/project folder(Img)  

                        DisplayPicture.SaveAs(path);
                        String temppath = "/UploadData//" + myfile;
                        dbpathpicture = temppath;
                    }

                }
                else 
                {
                    dbpathpicture = null;
                }


                if (NotesAttachements != null)
                {
                    //NotesAttchements

                    //Displaypicture
                    var allowedExtensionsnotes = new[] { ".txt", ".pdf", ".doc" };
                    // obj.img = model.img;
                    var fileNamenotes = Path.GetFileName(NotesAttachements.FileName); //getting only file name(ex-ganesh.jpg)  
                    var extnotes = Path.GetExtension(NotesAttachements.FileName); //getting the extension(ex-.jpg)  
                    if (allowedExtensionsnotes.Contains(extnotes)) //check what type of extension  
                    {
                        var namenotes = Path.GetFileNameWithoutExtension(fileNamenotes); //getting file name without extension  
                        var myfilenotes = namenotes + "_" + myidss + "_" + extnotes; //appending the name with id                   
                        var path = Path.Combine(Server.MapPath("~/UploadData/"), myfilenotes); // store the file inside ~/project folder(Img)  

                        NotesAttachements.SaveAs(path);
                        String temppath = "/UploadData/" + myfilenotes;
                        dbpathnotes = temppath;

                    }


                }
                else
                {

                    dbpathnotes = null;

                }

                if (NotesPreview != null)
                {

                    //NotesPreview 
                    var allowedExtensionspreview = new[] { ".txt", ".pdf", ".doc" };
                    // obj.img = model.img;
                    var fileNamepreview = Path.GetFileName(NotesPreview.FileName); //getting only file name(ex-ganesh.jpg)  
                    var extpreview = Path.GetExtension(NotesPreview.FileName); //getting the extension(ex-.jpg)  
                    if (allowedExtensionspreview.Contains(extpreview)) //check what type of extension  
                    {
                        var namepreview = Path.GetFileNameWithoutExtension(fileNamepreview); //getting file name without extension  
                        var myfilepreview = namepreview + "_" + myidss + "_" + extpreview; //appending the name with id                   
                        var path = Path.Combine(Server.MapPath("~/UploadData/"), myfilepreview); // store the file inside ~/project folder(Img)  
                        NotesPreview.SaveAs(path);
                        String temppath = "/UploadData/" + myfilepreview;
                        dbpathpreview = temppath;
                    }


                }
                else 
                {
                    dbpathpreview = null;
                }
                
                if (ModelState.IsValid)
                {
                    int id = repository.AddSellerNotes(model, myidss, dbpathpicture, dbpathnotes, dbpathpreview);
                    ViewBag.returnid = id;
                    ViewBag.e_msgs = "Data Submited , For Request To Publish Click On Publish Button ";
                    ViewBag.disable = "false";


                }
                else
                {

                    ViewBag.e_msg = "please enter valid information to proceed";

                }



            }
            else 
            {

                //add imgs/file in server
                //===========================================================

                if (DisplayPicture != null && NotesAttachements != null && NotesPreview != null)
                {
                    //Displaypicture
                    var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
                    // obj.img = model.img;
                    var fileName = Path.GetFileName(DisplayPicture.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(DisplayPicture.FileName); //getting the extension(ex-.jpg)  
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        var name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                        var myfile = name + "_" + myidss + "_" + ext; //appending the name with id                   
                        var path = Path.Combine(Server.MapPath("~/UploadData/"), myfile); // store the file inside ~/project folder(Img)  

                        DisplayPicture.SaveAs(path);
                        String temppath = "/UploadData//" + myfile;
                        dbpathpicture = temppath;
                    }

                    //NotesAttchements

                    //Displaypicture
                    var allowedExtensionsnotes = new[] { ".txt", ".pdf", ".doc" };
                    // obj.img = model.img;
                    var fileNamenotes = Path.GetFileName(NotesAttachements.FileName); //getting only file name(ex-ganesh.jpg)  
                    var extnotes = Path.GetExtension(NotesAttachements.FileName); //getting the extension(ex-.jpg)  
                    if (allowedExtensionsnotes.Contains(extnotes)) //check what type of extension  
                    {
                        var namenotes = Path.GetFileNameWithoutExtension(fileNamenotes); //getting file name without extension  
                        var myfilenotes = namenotes + "_" + myidss + "_" + extnotes; //appending the name with id                   
                        var path = Path.Combine(Server.MapPath("~/UploadData/"), myfilenotes); // store the file inside ~/project folder(Img)  

                        NotesAttachements.SaveAs(path);
                        String temppath = "/UploadData/" + myfilenotes;
                        dbpathnotes = temppath;

                    }

                    //NotesPreview 
                    var allowedExtensionspreview = new[] { ".txt", ".pdf", ".doc" };
                    // obj.img = model.img;
                    var fileNamepreview = Path.GetFileName(NotesPreview.FileName); //getting only file name(ex-ganesh.jpg)  
                    var extpreview = Path.GetExtension(NotesPreview.FileName); //getting the extension(ex-.jpg)  
                    if (allowedExtensionspreview.Contains(extpreview)) //check what type of extension  
                    {
                        var namepreview = Path.GetFileNameWithoutExtension(fileNamepreview); //getting file name without extension  
                        var myfilepreview = namepreview + "_" + myidss + "_" + extpreview; //appending the name with id                   
                        var path = Path.Combine(Server.MapPath("~/UploadData/"), myfilepreview); // store the file inside ~/project folder(Img)  
                        NotesPreview.SaveAs(path);
                        String temppath = "/UploadData/" + myfilepreview;
                        dbpathpreview = temppath;
                    }
                }
                else
                {
                    ViewBag.e_msg = "please enter valid information to proceed";
                    return View();
                }

                //add data in database
                if (ModelState.IsValid)
                {
                    int id = repository.AddSellerNotes(model, myidss, dbpathpicture, dbpathnotes, dbpathpreview);
                    ViewBag.returnid = id;
                    ViewBag.e_msgs = "Data Submited , For Request To Publish Click On Publish Button ";
                    ViewBag.disable = "false";


                }
                else
                {

                    ViewBag.e_msg = "please enter valid information to proceed";

                }


            }

            

            
            return View();
        }

        [HttpPost]
        public ActionResult Publish()
        {
            String id = Request["id"];
            int ids=Convert.ToInt32(id);
            int myid = repository.UpdateStatus(ids);
            return RedirectToAction("Dashboard");
        }

        public ActionResult DeleteNotesFromDashDelete(String ID)
        {
            int myid = repository.DeleteNotesFromID(ID);
            TempData["mymsg"] = "Notes Deleted successfully";
            return RedirectToAction("Dashboard");
        }


        //buyer Reqest Page

        [HttpGet]
        public ActionResult BuyerRequests(int? ByuerPageIndex, String BuyerDATA) {
            //method For Profile
            PartialOne();

            //userid
            String mail = User.Identity.Name;
            int loginuserid = repository.GetId(mail);


            //get sellernotes data from ID
            var dataList = repository.GetBuyerDataFromID(loginuserid,BuyerDATA);
            ViewBag.BuyerDataList = dataList.ToPagedList(ByuerPageIndex ?? 1, 6); 


            //UserProfile List 
            var UserProfileDataList = repository.GetUserProfileData();
            ViewBag.UserProfileDataList = UserProfileDataList;

            return View();
        }


        //My Profile Page

        [HttpGet]
        public ActionResult MyProfile()
        {
            //method For Profile
            PartialOne();
            String mail = User.Identity.Name;
            int myidss = repository.GetId(mail);
            ViewBag.UserEmail = mail;
            //From ID get FirstName And Last Name
            var dataofuser = repository.GetFnameLnameFromID(myidss);
            String Firstname = dataofuser.Item1;
            String Lastname = dataofuser.Item2;
            ViewBag.FName = Firstname;
            ViewBag.LName = Lastname;

            //==========================
            var getgender = repository.GetGender();
            ViewBag.genderlist = getgender;
            var getcode = repository.GetCode();
            ViewBag.Codelist = getcode;
            var getcountries = repository.GetCountries();
            ViewBag.countrieslist = getcountries;
            //===============================
            //if Record exist then Update
            var DataExist = repository.CheckUserProfileData(myidss);
            if (DataExist)
            {
                var Udata = repository.GetUserProfileDataFromID(myidss);
                
                ViewBag.OldData = Udata;
                if (Udata.Gender == 0)
                {
                    ViewBag.Gender = "Last : " + "Female";
                }
                else if(Udata.Gender == 1)
                {
                    ViewBag.Gender = "Last : " + "Male";
                }
                else
                {
                    ViewBag.Gender = "Last : " + "Other";
                }
                
                
                ViewBag.DOB ="Last : "+Udata.DOB;
                ViewBag.County = "Last : "+Udata.Country;
                ViewBag.Code = "Last : "+Udata.PhonenumberCountryCode;
                ViewBag.Profilepic = Udata.ProfilePicture;
                return View();
            }


            return View();
        }

        [HttpPost]
        public ActionResult MyProfile(UserProfileModel model, System.Web.HttpPostedFileBase ProfilePicture)
        {
            String mail = User.Identity.Name;
            int myidss = repository.GetId(mail);
            model.SecondaryEmailAddress = mail;

            //=======================Dropdown list===========================
            var getgender = repository.GetGender();
            ViewBag.genderlist = getgender;
            var getcode = repository.GetCode();
            ViewBag.Codelist = getcode;
            var getcountries = repository.GetCountries();
            ViewBag.countrieslist = getcountries;

            //==================================================
            if (ProfilePicture != null)
            {

                //Displaypicture
                var allowedExtensions = new[] {".Jpg", ".png", ".jpg", ".jpeg",".jpe" };
                // obj.img = model.img;
                var fileName = Path.GetFileName(ProfilePicture.FileName); //getting only file name(ex-ganesh.jpg)  
                var ext = Path.GetExtension(ProfilePicture.FileName); //getting the extension(ex-.jpg)  
                if (allowedExtensions.Contains(ext)) //check what type of extension  
                {
                    var name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                    var myfile = "DP_" + name + "_" + myidss + "_" + ext; //appending the name with id                   
                    var path = Path.Combine(Server.MapPath("~/UploadData/"), myfile); // store the file inside ~/project folder(Img)  

                    ProfilePicture.SaveAs(path);
                    String temppath = "/UploadData/" + myfile;
                    model.ProfilePicture = temppath;
                }
                else {

                    model.ProfilePicture = "/Content/Front_Content/images/User-Profile/profile.jpg";
                }
            }//end of dpnull
           

            if (ModelState.IsValid)
            {

                int id = repository.AddUserProfileData(model,myidss);
                ViewBag.msg = "data added successfully";
                return RedirectToAction("Dashboard");
            }
            else
            {

                return View();
            }

        }//end of my profile httpPOST  method

        //RequestForDownload

        [HttpGet]
        public ActionResult RequestForDownload(int noteid)
        {
            //userid
            String mail = User.Identity.Name;
            int loginuserid = repository.GetId(mail);
            int Roleid = repository.GetRoleId(loginuserid);

            //code for update Number of  download variable
            int add = repository.AddNumberOfDownloads(noteid);
            
         
            //get sellernotes data from ID
            var data = repository.GetSellerNotesDataFromID(noteid);
            SellerNotes obj = data;
            
            if(Roleid==1)
            {
                //download Code
                String myfile = obj.NotesPath;
                var path = Path.Combine(Server.MapPath("~"), myfile);
                return File(path, ".pdf");

            }
            // For paid Notes
            if (obj.IsPaid == true)
            {
                //check Record Exist OR NOT 
                //ifcondition  downloader meand
                //loginuserid and noteid is exist in download then rerun else enter new entrty
                var RecoedExist = repository.CheckForDownloadRecord(noteid,loginuserid);
                //item 1 for data Exist Or NOt
                bool isthere = RecoedExist.Item1;

                              
                if (isthere == true)
                {
                    //item 2 is For Downnlod Aloowed by seller Or NOT
                    bool alowed = RecoedExist.Item2;
                    if (obj.SellerID == loginuserid )
                    {

                        //download Code
                        String myfile = obj.NotesPath;
                        var path = Path.Combine(Server.MapPath("~"), myfile);
                        return File(path, ".pdf");

                    }
                    else if (alowed)
                    {
                        //download Code
                        String myfile = obj.NotesPath;
                        var path = Path.Combine(Server.MapPath("~"), myfile);
                        return File(path, ".pdf");

                    }
                    else
                    {
                        TempData["msgforRequested"] = "Wait For Seller Approval";
                        return RedirectToAction("MyDownload");
                    }

                }
                else 
                {
                    if (obj.SellerID == loginuserid)
                    {

                        //download Code
                        String myfile = obj.NotesPath;
                        var path = Path.Combine(Server.MapPath("~"), myfile);
                        return File(path, ".pdf");

                    }
                    else 
                    {
                        //add Download History Data IN Download Table
                        int val = repository.AddMyDownloadDataForPaid(noteid, loginuserid);
                        String Sellermail = repository.GetSellerEmailFromID(obj.SellerID);
                        if (val == 1)
                        {

                            String Body = " Hello, Please Check Your Buyer Request Portal   ";
                            String subjects = "someone wants to buy your Notes ";
                            String recevermail = Sellermail;
                            BuildEmailTEmplete(subjects, Body, recevermail);
                            TempData["msgdisplay"] = "Your request has been successfully submitted to Seller";

                        }

                        return RedirectToAction("NoteDetails", new { id = noteid });

                    }
                    
                }
                

            }
            else
            {
                //FreeNotes
                var RecoedExist = repository.CheckForDownloadRecord(noteid, loginuserid);
                //item 1 for data Exist Or NOt
                bool isthere = RecoedExist.Item1;
                //add Download History Data IN Download Table
                if (isthere==true)
                {
                    String myfile = obj.NotesPath;
                    var path = Path.Combine(Server.MapPath("~"), myfile);
                    return File(path, ".pdf");

                }
                else
                {

                    int val = repository.AddMyDownloadDataForFree(noteid, loginuserid);

                    if (val == 1)
                    {
                        //download Code
                        String myfile = obj.NotesPath;
                        var path = Path.Combine(Server.MapPath("~"), myfile);
                        return File(path, ".pdf");

                    }


                }

            }
            //download Code
            // String myfile = "nisarg_1005_.pdf";
            //var path = Path.Combine(Server.MapPath("~/UploadData/"), myfile);
            //return File(path,".pdf");
            return RedirectToAction("MyDownload");
        }//end of method


        [HttpGet]
        public ActionResult MyDownload(int? Downloadpage, String DownloadDATA)
        {
            //method For Profile
            PartialOne();
            if (TempData["msgforRequested"] != null)
            {
                ViewBag.msgforRequested = TempData["msgforRequested"].ToString();
            }

            if (TempData["msgFromReview"] != null)
            {
                ViewBag.msgFromReview = TempData["msgFromReview"].ToString();
            }

            if (TempData["msgReviewisthere"]!=null)
            {

                ViewBag.msgReviewisthere = TempData["msgReviewisthere"].ToString();


            }
            if (TempData["Reported"] != null)
            {
                ViewBag.msgFromReview = TempData["Reported"].ToString();
            }

            
            //userid
            String mail = User.Identity.Name;
            int loginuserid = repository.GetId(mail);


            //get sellernotes data from ID
            var dataList = repository.GetMyDownloadDataFromID(loginuserid,DownloadDATA);
            
            ViewBag.DownloadDataList = dataList.ToPagedList(Downloadpage ?? 1, 6); 
            ViewBag.LoginUserEmail = mail;

           
            return View();
        }//end of method


        ////send data from controller to shred view
        public PartialViewResult PartialOne()
        {
            String mail = User.Identity.Name;
            int loginuserid = repository.GetId(mail);
            var dataList = repository.GetUserProfileDataFromID(loginuserid);
            if (dataList != null)
            {
                ViewBag.UserProfileData = dataList.ProfilePicture;
            }
            else {

                ViewBag.UserProfileData = "~/Content/Front_Content/images/User-Profile/profile.jpg";
            }
            
            return PartialView("~/Views/Shared/__Custom_1_Layout.cshtml");
            //or return PartialView();
        }

        //allow Download To User From Seller  (BuyerPortal)
        public ActionResult AllowDownload(String id)
        {
            int ids = Convert.ToInt32(id);
            int X = repository.UpdateAllowMyDownloadData(ids);
            return RedirectToAction("BuyerRequests");
        }


        [HttpGet]
        //Add Review Of Notes From MyDownload Porta
        public ActionResult AddReviewOfNotes(String noteid)
        {
            ViewBag.MyNoteID = noteid;

            int NoteID = Convert.ToInt32(noteid);
            //get LoginUserID
            String mail = User.Identity.Name;
            int loginuserid = repository.GetId(mail);
            //getReviewData list
            var Isthere = repository.IsReviewExist(NoteID,loginuserid);
            if (Isthere == true)
            {
                TempData["msgReviewisthere"] = "You have already reviewed";
                return RedirectToAction("MyDownload");
            }
            else
            {
                return View();
            }
            
           
        }

        //noteID
        [HttpPost]
        //Add Review Of Notes From Review Porta
        public ActionResult AddReviewOfNotes(String noteID,String rate,String Comment)
        {
            //Login userid
            String mail = User.Identity.Name;
            int loginuserid = repository.GetId(mail);
            //int ids = Convert.ToInt32(id);
            int X = repository.AddReviewFromMyDownload(noteID,rate,Comment,loginuserid);
            if (X == 1)
            {
                TempData["msgFromReview"] = "Thank You For Your Review";
            }
            return RedirectToAction("MyDownload");
        }


        [HttpGet]
        public ActionResult ChangePassword()
        {


            return View();
        }


        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.NewPassword == model.ConfirmPassword)
                {
                    
                    //Login userid
                    String mail = User.Identity.Name;
                    int loginuserid = repository.GetId(mail);
                    int value = repository.ChangeUserPassword(model,loginuserid);
                    if (value == 1)
                    {
                        ViewBag.msg = "Password has been changed successfully";
                        return RedirectToAction("Login");

                    }
                    else {

                        ViewBag.msg = "Old Password Not Match";
                        return View();
                    }
                }
                else
                {

                    ViewBag.msg = "Both Password Not Match";
                    
                }
            
            }
            return View();
        }

        //MySoldNotes
        [HttpGet]
         public ActionResult MySoldNotes(int? SoldPageIndex, String SoldDATA)
         {
            //method For Profile
            PartialOne();

            //Login userid
            String mail = User.Identity.Name;
            int loginuserid = repository.GetId(mail);
            var SoldList = repository.GetMySoldNotesDataList(loginuserid,SoldDATA);
            //repo
            ViewBag.MySoldNoteList = SoldList.ToPagedList(SoldPageIndex ?? 1, 6); 

            //UserProfile List 
            var UserProfileDataList = repository.GetUserProfileData();
            ViewBag.UserProfileDataList = UserProfileDataList;
            return View();
         }


        //MyRejectedNotes
        [HttpGet]
        public ActionResult MyRejectedNotes(int? RejectedPageIndex, String RejectedDATA)
        {
            //method For Profile
            PartialOne();

            //Login userid
            String mail = User.Identity.Name;
            int loginuserid = repository.GetId(mail);

            //get  SellerNotesList
            var SellerNotesDataList = repository.GetMyRejectedNotesData(loginuserid,RejectedDATA);
            ViewBag.RejectedNotesDataList = SellerNotesDataList.ToPagedList(RejectedPageIndex ?? 1, 6); ;

            //get Category List
            ViewBag.CategoryList = repository.GetCategoryList();
            return View();
        }

        //Edit Notes From Dshboard 

        [HttpGet]
        public ActionResult EditNotesFromDash(String ID)
        {
            var getcategory = repository.GetCategory();
            ViewBag.categorylist = getcategory;
            var getcountries = repository.GetCountries();
            ViewBag.countrieslist = getcountries;
            var getnotetype = repository.GetNoteType();
            ViewBag.notetypelist = getnotetype;
            ViewBag.disable = "true";

            int NoteID = Convert.ToInt32(ID);
            var SellerNotesData = repository.GetSellerNotesDataFromIDdash(NoteID);
            //TempData["EditNoteData"]TempData["EditNoteData"];
            ViewBag.EditNoteData = SellerNotesData;
            TempData["MYEditNoteData"] = SellerNotesData;
            //type
            foreach (var X in getnotetype)
            {
                if (X.ID == SellerNotesData.Note_Type)
                {
                    ViewBag.NoteType = "Last : "+X.Name;
                }
            }
            //ViewBag.Category
            foreach (var X in getcategory)
            {
                if (X.ID == SellerNotesData.Category)
                {
                    ViewBag.Category = "Last : " + X.Name;
                }
            }
            //country ViewBag.Country
            foreach (var X in getcountries)
            {
                if (X.ID == SellerNotesData.Country)
                {
                    ViewBag.Country = "Last : "+ X.Name;
                }
            }
            //price data
            if (SellerNotesData.IsPaid == true)
            {
                ViewBag.PriceData = "Paid";
                ViewBag.price = SellerNotesData.SellingPrice;
                @ViewBag.piad = "checked";
            }
            else
            {

                ViewBag.PriceData = "Free";
                ViewBag.free = "checked";
                ViewBag.price = 0;
            }
            return RedirectToAction("AddNotes");
        
        }//end Of Method

        //report Notes ReportNotes
        public ActionResult ReportNotes(String NoteID, String Title, String SellerID)
        {
            String mail = User.Identity.Name;
            int loginuserid = repository.GetId(mail);
            ViewBag.NoteID = NoteID;
            ViewBag.Title=Title;
            ViewBag.SellerID = SellerID;
            //int X = repository.UpdateAllowMyDownloadData(ids);
            return View();
        }

        [HttpPost]
        public ActionResult ReportNotes(RejectNotesModel model)
        {
            String mail = User.Identity.Name;
            int loginuserid = repository.GetId(mail);
            int value = repository.AddReportData(loginuserid, model);
            if(value==1)
            {
                //senemail to admin  for report

                String Body = " Hello, Admin Note Reported   NoteID:"+model.noteID+"Title:"+model.NotesTitle;
                String subjects = "someone Report Notes ";
                String recevermail = "kishankumardpatel17@gnu.ac.in";
                BuildEmailTEmplete(subjects, Body, recevermail);

            }
            TempData["Reported"] = "Reported";
            return RedirectToAction("MyDownload");
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
//get loged user ID using this code
//String mail = User.Identity.Name;
//int myidss = repository.GetId(mail);
//============================================================================================
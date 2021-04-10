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
    [Authorize]
    public class AdminController : Controller
    {
        AdminRepository adminRepo = null;
       
        private FormsAuthenticationTicket authTicket;

        public string Subject { get; private set; }
        public bool IsBodyHtml { get; private set; }

        //Constructor
        public AdminController()
        {

            adminRepo = new AdminRepository();
           
        }

        UsersRepository UserRepo = new UsersRepository();

        //=====================================  methods  ========================================================
        public PartialViewResult PartialOne()
        {
            String mail = User.Identity.Name;
            int loginuserid = UserRepo.GetId(mail);
            int roleid= UserRepo.GetRoleId(loginuserid);
            var dataList = UserRepo.GetUserProfileDataFromID(loginuserid);
            if (dataList != null)
            {
                ViewBag.UserProfileData = dataList.ProfilePicture;
            }
            else
            {

                ViewBag.UserProfileData = "~/Content/Front_Content/images/User-Profile/profile.jpg";
            }

            ViewBag.Roleid = roleid;
            return PartialView("~/Views/Shared/__Custom_Admin_Layout.cshtml");
            //or return PartialView();
        }



        // GET: AdminDasBoard
        public ActionResult AdminDashboard(int? admindashindex, String AdminDashDATA,String MonthID)
        {
            //method For Profile
            PartialOne();

            if(TempData["unpublishmsg"]!=null)
            {
                ViewBag.unpublishmsg = TempData["unpublishmsg"].ToString();

            }
            //number of notes in review
            ViewBag.numberofnotesinreview = adminRepo.GetCountOfReviewNotes();
            ViewBag.numberofdownloadednotes= adminRepo.GetCountofDownloadedNotes();
            ViewBag.numberofregisterd = adminRepo.GetCountOfRegisterdNotes();
            //get all data from seller notes (status=6 Published Notes)
            var NotesList = adminRepo.GetSellerNotesData(AdminDashDATA,MonthID);
            if(NotesList.Count==0)
            {

                ViewBag.NoData = "No Data Found !!";
            }
            ViewBag.NoteList = NotesList.ToPagedList(admindashindex ?? 1, 6); 
            ViewBag.UserData = adminRepo.GetAllUserData();
            ViewBag.DownloadData = adminRepo.GetDownloadData();
            ViewBag.MonthList = adminRepo.GetMonthData();
            ViewBag.NotesCategories = UserRepo.GetCategoryList();
            return View();
        }

        private object ToPagedList(int v1, int v2)
        {
            throw new NotImplementedException();
        }

        //Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login","User");

        }

        [HttpGet]
        public ActionResult NotesUnderReview(int? notesReviewindex, String NotesUnderReviewDATA,String Seller)
        {
            //method For Profile
            PartialOne();
            var NotesList = adminRepo.GetNotesForNotesUnderReview(NotesUnderReviewDATA,Seller);
            if (NotesList.Count == 0)
            {

                ViewBag.NoData = "No Data Found !!";
            }
            ViewBag.ReviewNoteList = NotesList.ToPagedList(notesReviewindex ?? 1, 6); ;
            ViewBag.UserData = adminRepo.GetAllUserData();
            //ViewBag.DownloadData = adminRepo.GetDownloadData();
            //ViewBag.MonthList = adminRepo.GetMonthData();
            ViewBag.NotesCategories = UserRepo.GetCategoryList();

            if (TempData["approvemsg"] != null)
            {
                ViewBag.approvemsg = TempData["approvemsg"].ToString();

            }
            if (TempData["Rejectemsg"] != null)
            {
                ViewBag.Rejectemsg = TempData["Rejectemsg"].ToString();

            }

            if (TempData["Inreview"] != null)
            {
                ViewBag.Rejectemsg = TempData["Inreview"].ToString();

            }
            

            return View();
        }


        [HttpGet]
        public ActionResult ApproveNotes(String NoteID)
        {
            //method For Profile
            PartialOne();
            int id = adminRepo.ApproveNotes(NoteID);
            TempData["approvemsg"] = "Notes Published";
            return RedirectToAction("NotesUnderReview");
        }

        [HttpGet]
        public ActionResult RejectNotes(String RejectNoteID,String title)
        {
            //method For Profile
            PartialOne();
            ViewBag.Title = title;
            ViewBag.NoteID = RejectNoteID;
            
            return View();
        }

        [HttpPost]
        public ActionResult RejectNotes(RejectNotesModel Model)
        {
            if (ModelState.IsValid)
            {

                int id = adminRepo.RejectNotes(Model);
                TempData["Rejectemsg"] = "Note Rejected Successfully";
                return RedirectToAction("NotesUnderReview");
            }
            else
            {
                return View();
            }

          
        }

        [HttpGet]
        public ActionResult PublishedNotes(int? publishednotesindex, String publishedDATA,String Seller)
        {
            //method For Profile
            PartialOne();

            if (TempData["unpublishmsg"] != null)
            {
                ViewBag.unpublishmsg = TempData["unpublishmsg"].ToString();

            }
            //get all data from seller notes (status=6 Published Notes)
            var NotesList = adminRepo.GetPublishedNotes(publishedDATA,Seller);
            if (NotesList.Count == 0)
            {

                ViewBag.NoData = "No Data Found !!";
            }
            ViewBag.NoteList = NotesList.ToPagedList(publishednotesindex ?? 1, 6);
            ViewBag.UserData = adminRepo.GetAllUserData();
            ViewBag.DownloadData = adminRepo.GetDownloadData();
            ViewBag.MonthList = adminRepo.GetMonthData();
            ViewBag.NotesCategories = UserRepo.GetCategoryList();

            return View();
        }

        [HttpGet]
        public ActionResult DownloadedNotes(int? Downloadedntsindex, String DownloadedntsDATA,String Category, String Seller, String Buyer)
        {
            //method For Profile
            PartialOne();
            //all download data
            var DownloadedNotesList = adminRepo.GetAllDownloadedNotes(DownloadedntsDATA,Category,Seller,Buyer);
            ViewBag.UserData = adminRepo.GetAllUserData();
            ViewBag.AllSellerNotes = adminRepo.GetAllSellerNotesData();
            ViewBag.NotesListforDropdown =UserRepo.GetCategoryList();
            if (DownloadedNotesList.Count == 0)
            {

                ViewBag.NoData = "No Data Found !!";
            }
            ViewBag.DownloadedList = DownloadedNotesList.ToPagedList(Downloadedntsindex ?? 1, 6); 
            return View();
        }


        [HttpGet]
        public ActionResult RejectedNotes(int? Rejectedindex, String RejectedDATA, String Seller)
        {
            //method For Profile
            PartialOne();
            if (TempData["approvemsg"]!=null)
            {
                ViewBag.msg = TempData["approvemsg"].ToString();

            }
            var RejectedNotesList = adminRepo.GetAllRejectedNotes(RejectedDATA,Seller);
            if (RejectedNotesList.Count == 0)
            {

                ViewBag.NoData = "No Data Found !!";
            }
            ViewBag.RejectedList = RejectedNotesList.ToPagedList(Rejectedindex ?? 1, 6); 
            ViewBag.UserData = adminRepo.GetAllUserData();
            ViewBag.NotesCategories = UserRepo.GetCategoryList();
            return View();
        }
        [HttpGet]
        public ActionResult ApproveNotesFromRejectPortal(String NoteID)
        {
            //method For Profile
            PartialOne();
            int id = adminRepo.ApproveNotes(NoteID);
            TempData["approvemsg"] = "Notes Published";
            return RedirectToAction("RejectedNotes");
        }

        //NotesINReview
        [HttpGet]
        public ActionResult NotesINReview(String NoteID)
        {
            //method For Profile
            PartialOne();
            int id = adminRepo.InReviewNotes(NoteID);
            TempData["Inreview"] = "IN Review";
            return RedirectToAction("NotesUnderReview");
        }




        //members
        [HttpGet]
        public ActionResult MembersPage(int? memberindex, String memberDATA)
        {
            //method For Profile
            PartialOne();
            CalculateMemberDetailsData();
            var MemberList = adminRepo.GetAllMemberData(memberDATA);
            ViewBag.MemberList = MemberList.ToPagedList(memberindex ?? 1, 6);

            if(TempData["dactivmsg"]!=null)
            {

                ViewBag.Dactiv = TempData["dactivmsg"].ToString();
            }
            return View();
        }


        //calculate Memeber Details Page data 
        public void  CalculateMemberDetailsData()
        {
             
            List<Users_New> obj = adminRepo.GetAllUserData();

            foreach(var X in obj)
            {
                if(X.RoleID==0)
                {
                    //get totalnotes under review for this User
                    int TotalnoteunderReview = adminRepo.GetTotalNotesUnderReview(X.ID);

                    //get total Published for this User
                    int TotalPublishedNotes = adminRepo.GetTotalPublishedNotes(X.ID);

                    //get total Downloaded for this User
                    int TotalDownloadedNotes = adminRepo.GetTotalDownloadedNotes(X.ID);

                    //get total Expense EXPENSES for this User
                    decimal TotalExpensesNotes = adminRepo.GetTotalExpenses(X.ID);

                    //get total  Earning for this User
                    int TotalEarningNotes = adminRepo.GetTotalEarning(X.ID);

                    //add all data into memberrdetails Data table
                   
                    int Data = adminRepo.AddMemberData(X.ID, X.FirstName, X.LastName, X.EmailID, TotalnoteunderReview, TotalPublishedNotes, TotalDownloadedNotes, TotalExpensesNotes, TotalEarningNotes);




                }


            }

        }

        //Reports
        [HttpGet]
        public ActionResult ReportsPage(int? reportindex)
        {
            //method For Profile
            PartialOne();

            if(TempData["deletedvmsg"]!=null)
            {

                ViewBag.msg = TempData["deletedvmsg"].ToString();
            }
            //get data of reporttable
            var data = adminRepo.getReportData();
            ViewBag.reportList = data.ToPagedList(reportindex ?? 1, 6); ;

            ViewBag.UserData = adminRepo.GetAllUserData();
            ViewBag.NotesCategories = UserRepo.GetCategoryList();
            ViewBag.SellerNotes = adminRepo.GetAllSellerNotesData();
            return View();
        }



        //Settings
        //Manage System Configuration
        [HttpGet]
        public ActionResult ManageSystemConfigurations()
        {
            //method For Profile
            PartialOne();
            return View();
        }

        //Manage Administrator
        [HttpGet]
        public ActionResult ManageAdministrator(int? admindataindex, String adminDATA)
        {
            //method For Profile
            PartialOne();
            if (TempData["Deactiveadminmsg"]!=null)
            {
                ViewBag.adminmsg = TempData["Deactiveadminmsg"].ToString();

            }


            //get all admin here
            var adminList= adminRepo.GetAllAdminData(adminDATA);
            ViewBag.admin = adminList.ToPagedList(admindataindex ?? 1, 6); 
            //all UserProfile Data
            ViewBag.usrprofiledata = adminRepo.GetAllUserProfileData();

            return View();
        }


        //Manage Category
        [HttpGet]
        public ActionResult ManageCategory(int? categoryindex, String categoryDATA)
        {
            //method For Profile
            PartialOne();
            if (TempData["msg"] != null)
            {
                ViewBag.msg = TempData["msg"].ToString();

            }
            //get category List
              var data = adminRepo.GetAllCategoryList(categoryDATA);
            ViewBag.Categorylist = data.ToPagedList(categoryindex ?? 1, 6);
            return View();
        }


        public ActionResult AddCategory(String id)
        {
            //method For Profile
            PartialOne();
            if(id!=null)
            {
                int myid = Convert.ToInt32(id);
                NoteCategories data = adminRepo.GetCategoryFromID(myid);
                ViewBag.Name = data.Name;
                ViewBag.desc = data.Description;
                ViewBag.ID = myid;
            }
            
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(String Name,String Description,String ID)
        {
            //method For Profile
            PartialOne();

            String mail = User.Identity.Name;
            int loginuserid = UserRepo.GetId(mail);
            //add Category
            adminRepo.addCategory(loginuserid,Name,Description,ID);

            return RedirectToAction("ManageCategory");
        }

        //DeactiveCategory
        public ActionResult DeactiveCategory(String ID)
        {
            //method For Profile
            PartialOne();
            int id = Convert.ToInt32(ID);
            adminRepo.DeactiveCategoryFromID(id);
            TempData["msg"] = "Category Deactivated";

            return RedirectToAction("ManageCategory");
        }


        //Manage Typr
        [HttpGet]
        public ActionResult ManageType(int? typeindex, String typeDATA)
        {
            //method For Profile
            PartialOne();

            if (TempData["msg"] != null)
            {
                ViewBag.msg = TempData["msg"].ToString();

            }
            //get category List
            var data = adminRepo.GetAllTypeList(typeDATA);
            ViewBag.Categorylist = data.ToPagedList(typeindex ?? 1, 6);

            return View();
        }

        //AddType
        public ActionResult AddType(String id)
        {
            //method For Profile
            PartialOne();
            if (id != null)
            {
                int myid = Convert.ToInt32(id);
                NoteTypes data = adminRepo.GetNoteTypeFromID(myid);
                ViewBag.Name = data.Name;
                ViewBag.desc = data.Description;
                ViewBag.ID = myid;
            }

            return View();
        }



        [HttpPost]
        public ActionResult AddType(String Name, String Description, String ID)
        {
            //method For Profile
            PartialOne();

            String mail = User.Identity.Name;
            int loginuserid = UserRepo.GetId(mail);
            //add Category
            adminRepo.addType(loginuserid, Name, Description, ID);

            return RedirectToAction("ManageType");
        }


        //DeactiveType
        public ActionResult DeactiveType(String ID)
        {
            //method For Profile
            PartialOne();
            int id = Convert.ToInt32(ID);
            adminRepo.DeactiveTypeFromID(id);
            TempData["msg"] = "Type Deactivated";

            return RedirectToAction("ManageType");
        }




        //Manage Countries
        [HttpGet]
        public ActionResult ManageCountries(int? countryindex, String countryDATA)
        {
            //method For Profile
            PartialOne();

            if (TempData["msg"] != null)
            {
                ViewBag.mymsg = TempData["msg"].ToString();

            }
            //get category List
            var data = adminRepo.GetAllCountryList(countryDATA);
            ViewBag.Categorylist = data.ToPagedList(countryindex ?? 1, 6);

            return View();
        }

        //AddCountry
        public ActionResult AddCountry(String id)
        {
            //method For Profile
            PartialOne();
            if (id != null)
            {
                int myid = Convert.ToInt32(id);
                Countries data = adminRepo.GetCountryFromID(myid);
                ViewBag.Name = data.Name;
                ViewBag.desc = data.CountryCode;
                ViewBag.ID = myid;
            }

            return View();
        }


        [HttpPost]
        public ActionResult AddCountry(String Name, String Description, String ID)
        {
            //method For Profile
            PartialOne();

            String mail = User.Identity.Name;
            int loginuserid = UserRepo.GetId(mail);
            //add Category
            adminRepo.addCountry(loginuserid, Name, Description, ID);

            return RedirectToAction("ManageCountries");
        }

        public ActionResult DeactiveCountry(String ID)
        {
            //method For Profile
            PartialOne();
            int id = Convert.ToInt32(ID);
            adminRepo.DeactiveCountryFromID(id);
            TempData["msg"] = "Country Deactivated";

            return RedirectToAction("ManageCountries");
        }

        //change password
        [HttpGet]
        public ActionResult AdminChangePassword()
        {
            //method For Profile
            PartialOne();

            return View();
        }


        [HttpPost]
        public ActionResult AdminChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.NewPassword == model.ConfirmPassword)
                {

                    //Login userid
                    String mail = User.Identity.Name;
                    int loginuserid = adminRepo.GetId(mail);
                    int value = adminRepo.ChangeUserPassword(model, loginuserid);
                    if (value == 1)
                    {
                        ViewBag.msg = "Password has been changed successfully";
                        return RedirectToAction("Login", "User");

                    }
                    else
                    {

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


        //============================
        //My Profile Page

        [HttpGet]
        public ActionResult AdminProfile()
        {
            //method For Profile
            PartialOne();

            String mail = User.Identity.Name;
            int loginuserid = UserRepo.GetId(mail);
            //codelist
            var getcode = UserRepo.GetCode();
            ViewBag.Codelist = getcode;
            UserProfile up = adminRepo.GetAdminProfileDataFromID(loginuserid);
            Users_New obj = adminRepo.GetAdminDataFromID(loginuserid);
            ViewBag.fname =obj.FirstName;
            ViewBag.lname =obj.LastName;
            ViewBag.email = obj.EmailID;
            ViewBag.semail = up.SecondaryEmailAddress;
            ViewBag.phone = up.Phonenumber;
            ViewBag.code = up.PhonenumberCountryCode;
            ViewBag.Userid = loginuserid;
            ViewBag.Profilepic = up.ProfilePicture;


            return View();
        
        }



        [HttpPost]
        public ActionResult AdminProfile(AdminProfileModel model, System.Web.HttpPostedFileBase ProfilePicture)
        {
            //method For Profile
            PartialOne();

            String mail = User.Identity.Name;
            int loginuserid = UserRepo.GetId(mail);
            //codelist
            var getcode = UserRepo.GetCode();
            ViewBag.Codelist = getcode;

            model.UserID = loginuserid;
            if (ProfilePicture != null)
            {

                //Displaypicture
                var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", ".jpeg", ".jpe" };
                // obj.img = model.img;
                var fileName = Path.GetFileName(ProfilePicture.FileName); //getting only file name(ex-ganesh.jpg)  
                var ext = Path.GetExtension(ProfilePicture.FileName); //getting the extension(ex-.jpg)  
                if (allowedExtensions.Contains(ext)) //check what type of extension  
                {
                    var name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                    var myfile = "DP_" + name + "_" + loginuserid + "_" + ext; //appending the name with id                   
                    var path = Path.Combine(Server.MapPath("~/UploadData/"), myfile); // store the file inside ~/project folder(Img)  

                    ProfilePicture.SaveAs(path);
                    String temppath = "/UploadData/" + myfile;
                    model.ProfilePicture = temppath;
                }
                else
                {

                    model.ProfilePicture = "/Content/Front_Content/images/User-Profile/profile.jpg";
                }

                


            }//end of dpnull

            if(ModelState.IsValid)
            {
                int val= adminRepo.AddAdminProfileData(model);
                if(val==1)
                {

                    return RedirectToAction("AdminDashboard");
                }
            }

            return View();

        }
            [HttpGet]
        [AllowAnonymous]
        public ActionResult AdminNoteDetails(String id)
        {
            //method For Profile
            PartialOne();
            try
            {
                if (TempData["msgdisplay"] != null)
                {
                    ViewBag.msgdisplay = TempData["msgdisplay"].ToString();
                }

                if (id != null)
                {

                    int myid = Convert.ToInt32(id);
                    var ReviewData = UserRepo.GetReviewDataFromID(myid);
                    ViewBag.reviewdata = ReviewData;
                    var MyNotesData = UserRepo.GetSearchNoteDataFromID(myid);
                    ViewBag.NotesData = MyNotesData;
                    int idmycountry = (int)MyNotesData.Country;
                    String Country = UserRepo.GetCountryFromID(idmycountry);
                    String category = UserRepo.GetCategoryFromID(MyNotesData.Category);
                    ViewBag.Country = Country;
                    ViewBag.Category = category;
                    //get usernew details using sellernotes  sellerid
                    var Reviewfornoteid = UserRepo.GetUserReviewFromID(myid);
                    ViewBag.UserReview = Reviewfornoteid;
                    var user = UserRepo.GetUserFromReviewID();
                    ViewBag.User = user;




                }
                else
                {

                    return RedirectToAction("AdminDashboard");

                }

            }
            catch (Exception e)
            {

                throw e;

            }



            //return View(MyList.ToPagedList(page ?? 1, 9));

            return View();
        }

        [HttpGet]

        public ActionResult UnpublishNotes(String noteid,String Seller,String ntsTitle)
        {
            //method For Profile
            PartialOne();
            ViewBag.MyNoteID = noteid;
            ViewBag.SellerID = Seller;
            ViewBag.Title = ntsTitle;
            return View();
        }//end of method



        [HttpPost]
        public ActionResult UnpublishNotes(UnPublishModel model)
        {
            if(ModelState.IsValid)
            {

                String Email = adminRepo.UnPublishNotes(model);
                if(Email!=null)
                {

                    String Body = " Hello, Please Check Dashboard Portal We UnPublished Your Notes Named  "+model.NotesTitle;
                    String subjects = "Unpublish Notes ";
                    String recevermail = Email;
                    BuildEmailTEmplete(subjects, Body, recevermail);
                    TempData["unpublishmsg"]= "Note UnPublished";
                }
                return RedirectToAction("AdminDashboard");
            }
            else
            {
                return View();
            }
            
            
        }//end of method


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

        //file size code
        //FileInfo fi = new FileInfo(@"D:/ASP net MVC5__Projects/Notes__Marketplace/Notes__Marketplace/" + path);
        //var k = fi.Length;
        //ViewBag.size = k;


        //deactive User From Member Page
        public ActionResult DeactiveUser(String MyUserID)
        {
            //method For Profile
            PartialOne();
            int userid = Convert.ToInt32(MyUserID);
            adminRepo.DeactiveUserFromID(userid);
            TempData["dactivmsg"] = "User Deactivated";
            return RedirectToAction("MembersPage");
        }

        //about member   is will show all details of  pericular member 

        public ActionResult AboutMember(String MyUserID,int? aboutmemberindex)
        {
            //method For Profile
            PartialOne();
            int userid = Convert.ToInt32(MyUserID);
            var aboutdata=adminRepo.DataOFMemberProfile(userid);
            ViewBag.UserData = adminRepo.GetAllUserData();
            ViewBag.dataofmember = aboutdata;
            //get Notes List from Seller Notes Except Draft
            var NotesList = adminRepo.GetAllNotesFromUserID(userid);
            ViewBag.MYNoteList = NotesList.ToPagedList(aboutmemberindex ?? 1, 6);
            //all Seller Notes
            ViewBag.AllRefrence = adminRepo.GetAllRefrenceData();
            //all CategOry
            ViewBag.NotesCategories = UserRepo.GetCategoryList();
            //numbert of Downloads
            ViewBag.NofD = adminRepo.GetDownloadData();

            //ToPagedList(memberindex ?? 1, 6);
            return View();
        }//end of method



        //add Administrator
        public ActionResult AddAdministrator(String id)
        {
            //method For Profile
            PartialOne();
            if(id!=null)
            {
                int adminid = Convert.ToInt32(id);
                Users_New obj = adminRepo.GetAdminDataFromID(adminid);
                UserProfile up = adminRepo.GetAdminProfileDataFromID(adminid);
                ViewBag.fname =obj.FirstName;
                ViewBag.lname =obj.LastName;
                ViewBag.email = obj.EmailID;
                ViewBag.phone = up.Phonenumber;
                ViewBag.code = up.PhonenumberCountryCode;
                ViewBag.Userid = adminid;
            }
            var getcode =UserRepo.GetCode();
            ViewBag.Codelist = getcode;
            return View();
        }

        [HttpPost]
        //add Administrator
        public ActionResult AddAdministrator(AddAdministratorModel Model)
        {
            //method For Profile
            PartialOne();
            var getcode = UserRepo.GetCode();
            ViewBag.Codelist = getcode;
            //add data 
            if(ModelState.IsValid)
            {
                var insert = adminRepo.AddAdministratorData(Model);
                if(insert==1)
                {

                    ViewBag.MSG = "Admin User Added";
                }
            }
            


            return View();
        }

        // deactive admin user
        public ActionResult DeActiveAdmin(String id)
        {
            //method For Profile
            PartialOne();
            int adminid = Convert.ToInt32(id);
            adminRepo.DeactiveAdminFromID(adminid);
            TempData["Deactiveadminmsg"] = "Admin Deleted !!";
            return RedirectToAction("ManageAdministrator");
        }//end Of methods


        //DeleteRemark
        public ActionResult DeleteRemark(String ID)
        {
            //method For Profile
            PartialOne();
            int userid = Convert.ToInt32(ID);
            adminRepo.DeleteRemark(userid);
            TempData["deletedvmsg"] = "Remark Deleted !!";
            return RedirectToAction("ReportsPage");
        }




    }//end of controller
}//namespce
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notes__Marketplace.Models;
namespace Notes_MarketPlace.Db.Users_Login_Signup
{
    public class UsersRepository
    {
        public object V { get; private set; }

        public int AddUser(UsersModel usrobj) {
            //Notes_MarketplaceEntities
            using (var Context = new Notes_MarketplaceEntities()) {

                bool isthere = Context.Users_New.Where(u => u.EmailID == usrobj.EmailID).Any();
                if (isthere) {

                    return 0;
                }
                else {

                    Users_New newusr = new Users_New() {


                        IsEmailVerified = usrobj.IsEmailVerified,
                        IsActive = usrobj.IsActive,
                        FirstName = usrobj.FirstName,
                        LastName = usrobj.LastName,
                        EmailID = usrobj.EmailID,
                        Password = usrobj.Password

                    };

                    Context.Users_New.Add(newusr);
                    Context.SaveChanges();

                    return newusr.ID;
                }
            }

        }//adduser_Method


        public int Loginusr(LoginModel obj)
        {
            //var notify = (from s in db.AccountSettings
            //              where s.UserName == username
            //              select s.NotifyOnComment).DefaultIfEmpty(false);

            using (var Context = new Notes_MarketplaceEntities())
            {

                bool Isvalid = Context.Users_New.Any(X => X.EmailID == obj.EmailID && X.Password == obj.Password && X.IsEmailVerified == true);
                //= from v in Context.Users_New where v.EmailID == obj.EmailID select v.RoleID;
                var s = (from v in Context.Users_New where v.EmailID == obj.EmailID select v.RoleID).FirstOrDefault();

                int roleid = s;

                if (Isvalid)
                {
                    return roleid;
                }
                else {


                    return 10101;
                }
            }
            return 10101;
        }


        public int UpdateUser(int id)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {

                Users_New updateusr = Context.Users_New.Single(X => X.ID == id);
                updateusr.IsEmailVerified = true;
                Context.SaveChanges();


            }
            return 1;
        }


        public Tuple<int, String> ForgotPassword(ForgotPasswordModel obj)
        {
            Tuple<int, string> usrdata = new Tuple<int, string>(0, null);
            try {

                using (var Context = new Notes_MarketplaceEntities())
                {
                    //check email is register or not
                    bool isthere = Context.Users_New.Where(u => u.EmailID == obj.EmailID).Any();

                    if (isthere)
                    {
                        //get id of registed mail
                        var Idofuser = (from v in Context.Users_New where v.EmailID == obj.EmailID select v.ID).FirstOrDefault();
                        int id = Idofuser;

                        //genrate random pass
                        String Randompass = CreatePassword(8);

                        //updata user password
                        Users_New updateusr = Context.Users_New.Single(X => X.ID == id);
                        updateusr.Password = Randompass;
                        Context.SaveChanges();


                        //return data 1,random pass
                        Tuple<int, string> myusrdata = new Tuple<int, string>(1, Randompass);


                        return myusrdata;
                    }
                    else {


                        return usrdata;
                    }

                }//dbconnectionclose
                return usrdata;

            }
            catch (Exception e)
            {

                throw e;

            }
        }//mehtod

        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        //category
        public List<NoteCategories> GetCategory()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.NoteCategories.ToList();
                return data;

            }
        }

        //Countries
        public List<Countries> GetCountries()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.Countries.ToList();
                return data;

            }
        }

        //Type of Notes
        public List<NoteTypes> GetNoteType()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.NoteTypes.ToList();
                return data;

            }
        }

        //AddSellerNotes

        public int AddSellerNotes(AddNotesModel notesobj, int ids, String dppath, String notepath, String previewpath)
        {
            bool is_paid = false;
            Decimal price = 0;

            using (var Context = new Notes_MarketplaceEntities())
            {
                if (notesobj.IsPaid == "Free")
                {
                    is_paid = false;


                }
                else if (notesobj.IsPaid == "Paid") {

                    is_paid = true;
                    price = (decimal)notesobj.SellingPrice;
                }
                SellerNotes sobj = new SellerNotes()
                {
                    SellerID = ids,
                    Status = 3,
                    Title = notesobj.Title,
                    Category = notesobj.Category,
                    DisplayPicture = dppath,
                    Note_Type = notesobj.Note_Type,
                    NumberofPages = notesobj.NumberofPages,
                    Description = notesobj.Description,
                    UniversityName = notesobj.UniversityName,
                    Country = notesobj.Country,
                    Course = notesobj.Course,
                    CourseCode = notesobj.CourseCode,
                    Professor = notesobj.Professor,
                    IsPaid = is_paid,
                    SellingPrice = price,
                    NotesPreview = previewpath,
                    NotesPath = notepath,
                    CreatedDate = DateTime.Now,
                    IsActive = true,

                };
                Context.SellerNotes.Add(sobj);
                Context.SaveChanges();
                NotesRating();
                int returnid = sobj.ID;

                return returnid;
            }
        }


        //GetID
        public int GetId(String Email)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {

                var s = (from v in Context.Users_New where v.EmailID == Email select v.ID).FirstOrDefault();

                int id = s;

                return id;
            }
        }


        //update status draf to submited for review
        public int UpdateStatus(int id)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {
                SellerNotes sobj = Context.SellerNotes.Single(X => X.ID == id);
                sobj.Status = 4;
                Context.SaveChanges();

                return 1;
            }
        }

        public List<SellerNotes> GetSearchNoteData(String Search)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.SellerNotes.Where(X=>X.Title.StartsWith(Search) || Search == null).ToList();
                return data;

            }
        }

        public void NotesRating()
        {

            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.SellerNotes.ToList();


                //int noteid=Context.SellerNotes.g
                foreach (var Result in data)
                {
                    bool istherereview = Context.SellerNotesReviews.Where(u => u.NoteID == Result.ID).Any();
                    bool istherereport = Context.SellerNotesReportedIssues.Where(u => u.NoteID == Result.ID).Any();
                    if (istherereview == true && istherereport == true) {

                        int totalreview = Context.SellerNotesReviews.Count(X => X.NoteID == Result.ID);
                        int totalreported = Context.SellerNotesReportedIssues.Count(X => X.NoteID == Result.ID);
                        var sum = Context.SellerNotesReviews.Where(X => X.NoteID == Result.ID).Sum(dra => dra.Ratings);
                        int retingsum = Convert.ToInt32(sum);
                        int averegerating = retingsum / totalreview;

                        // record notesid present then update othewise creaete new
                        bool isthereRecord = Context.Notes_Review_Rating.Where(u => u.NoteID == Result.ID).Any();
                        if (isthereRecord)
                        {
                            Notes_Review_Rating obj = Context.Notes_Review_Rating.Single(X => X.NoteID == Result.ID);
                            obj.AverageRating = averegerating;
                            obj.TotalReview = totalreview;
                            obj.TotalReport = totalreported;
                            Context.SaveChanges();

                        }
                        else {

                            Notes_Review_Rating obj = new Notes_Review_Rating()
                            {
                                NoteID = Result.ID,
                                AverageRating = averegerating,
                                TotalReview = totalreview,
                                TotalReport = totalreported,

                            };
                            Context.Notes_Review_Rating.Add(obj);
                            Context.SaveChanges();
                        }

                    }
                }//foearch




            }//dbconnection close
        }//end of method


        public List<Notes_Review_Rating> GetReviewData()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.Notes_Review_Rating.ToList();
                return data;

            }
        }


        public Notes_Review_Rating GetReviewDataFromID(int id)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //var data = Context.Notes_Review_Rating.Single(X => X.NoteID == id);
                var D = (from v in Context.Notes_Review_Rating where v.NoteID == id select v).FirstOrDefault();
                return D;

            }
        }

        public SellerNotes GetSearchNoteDataFromID(int id)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //var data = Context.SellerNotes.Single(X => X.ID == id);
                var s = (from v in Context.SellerNotes where v.ID == id select v).FirstOrDefault();
                return s;
            }
        }

        //getcountryfromID
        public String GetCountryFromID(int id)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var s = (from v in Context.Countries where v.ID == id select v.Name).FirstOrDefault();

                String country = s;
                return country;

            }
        }

        //getcategoryfromid since/mba/type
        public String GetCategoryFromID(int id)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var s = (from v in Context.NoteCategories where v.ID == id select v.Name).FirstOrDefault();
                String cate = s;
                return cate;

            }
        }

        //Get data of Review table data from NoteID ID
        public List<SellerNotesReviews> GetUserReviewFromID(int id)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //var data = Context.SellerNotes.Single(X => X.ID == id);
                var s = (from v in Context.SellerNotesReviews where v.NoteID == id select v).ToList();
                return s;
            }
        }


        //Get data of Usernew table data from ReviewID
        public List<Users_New> GetUserFromReviewID()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //var data = Context.SellerNotes.Single(X => X.ID == id);
                var s = Context.Users_New.ToList();
                return s;
            }
        }

        //get data for dashboard tabl1 in progress Notes  GetDashboardDataForIPNotes 
        public List<SellerNotes> GetDashboardDataForIPNotes(int id,String IPNotesSearch)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //var data = Context.SellerNotes.Single(X => X.ID == id); .Where(X => X.Title.StartsWith(Search) || Search == null).
                var s = (from v in Context.SellerNotes where v.SellerID == id && (v.Status == 3|| v.Status==4 || v.Status==5) && (v.Title.StartsWith(IPNotesSearch)|| IPNotesSearch==null)select v).ToList();
                return s;
            }
        }


        //get data for dashboard tabl1 in progress Notes  GetDashboardDataForIPNotes 
        public List<SellerNotes> GetDashboardDataForPublishNotes(int id,String PublishNotes)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //var data = Context.SellerNotes.Single(X => X.ID == id); .Where(X => X.Title.StartsWith(Search) || Search == null).
                var s = (from v in Context.SellerNotes where v.SellerID == id && v.Status == 6 && (v.Title.StartsWith(PublishNotes) || PublishNotes == null) select v).ToList();
                return s;
            }
        }
        //get CategoryList GetCategoryList

        public List<NoteCategories> GetCategoryList()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //var data = Context.SellerNotes.Single(X => X.ID == id);
                var s = Context.NoteCategories.ToList();
                return s;
            }
        }

        //GetRefDataList from RefrenceData Table for Status ID
        public List<ReferenceData> GetRefDataList()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //var data = Context.SellerNotes.Single(X => X.ID == id);
                var s = Context.ReferenceData.ToList();
                return s;
            }
        }

        //DeleteNotesFromID delete Notes From Dashbord Delete Icon

        public int DeleteNotesFromID(String ID)
        {
            int id = Convert.ToInt32(ID);
            using (var Context = new Notes_MarketplaceEntities())
            {
                var obj = Context.SellerNotes.Where(X =>X.ID==id).FirstOrDefault();
                Context.SellerNotes.Remove(obj);
                Context.SaveChanges();
                return 1;
            }
        }

        //get Gender List  getgender
        public List<Gender> GetGender()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.Gender.ToList();
                return data;

            }
        }

        //get Country Calling Code GetCode
        public List<Code> GetCode()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.Code.ToList();
                return data;

            }
        }


        //get Firstname and Lastname From ID
        public Tuple<String, String> GetFnameLnameFromID(int id)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var s = (from v in Context.Users_New where v.ID == id select v.FirstName).FirstOrDefault();
                String fname = s;
                var y = (from v in Context.Users_New where v.ID == id select v.LastName).FirstOrDefault();
                String lname = y;

                Tuple<string, string> myusrdata = new Tuple<string, string>(fname,lname);
                return myusrdata;
            }
        }

        //===============================================================

        //AddUserProfileData(model, myidss);  add data of UserProfile
        public int AddUserProfileData(UserProfileModel Modelobj, int ids)
        {
            String DpPath = "";

            using (var Context = new Notes_MarketplaceEntities())
            {

                bool isthere = Context.UserProfile.Where(u => u.UserID == ids).Any();
                if (isthere)
                {
                    if (Modelobj.ProfilePicture != null)
                    {
                        DpPath = Modelobj.ProfilePicture;
                    }
                    else
                    {
                        DpPath = "/Content/Front_Content/images/User-Profile/profile.jpg";

                    }

                    UserProfile myUpobj = Context.UserProfile.Single(X => X.UserID == ids);
                    myUpobj.DOB = Modelobj.DOB;
                    myUpobj.Gender = Modelobj.Gender;
                    myUpobj.SecondaryEmailAddress = Modelobj.SecondaryEmailAddress;
                    myUpobj.PhonenumberCountryCode = Modelobj.PhonenumberCountryCode;
                    myUpobj.Phonenumber = Modelobj.Phonenumber;
                    myUpobj.ProfilePicture = DpPath;
                    myUpobj.AddressLine1 = Modelobj.AddressLine1;
                    myUpobj.AddressLine2 = Modelobj.AddressLine2;
                    myUpobj.City = Modelobj.City;
                    myUpobj.State = Modelobj.State;
                    myUpobj.ZipCode = Modelobj.ZipCode;
                    myUpobj.Country = Modelobj.Country;
                    myUpobj.University = Modelobj.University;
                    myUpobj.College = Modelobj.College;
                    Context.SaveChanges();

                    return 1;
                }
                else {


                    if (Modelobj.ProfilePicture != null)
                    {
                        DpPath = Modelobj.ProfilePicture;
                    }
                    else
                    {
                        DpPath = "/Content/Front_Content/images/User-Profile/profile.jpg";

                    }

                    UserProfile up = new UserProfile()
                    {
                        UserID = ids,
                        DOB = Modelobj.DOB,
                        Gender = Modelobj.Gender,
                        SecondaryEmailAddress = Modelobj.SecondaryEmailAddress,
                        PhonenumberCountryCode = Modelobj.PhonenumberCountryCode,
                        Phonenumber = Modelobj.Phonenumber,
                        ProfilePicture = DpPath,
                        AddressLine1 = Modelobj.AddressLine1,
                        AddressLine2 = Modelobj.AddressLine2,
                        City = Modelobj.City,
                        State = Modelobj.State,
                        ZipCode = Modelobj.ZipCode,
                        Country = Modelobj.Country,
                        University = Modelobj.University,
                        College = Modelobj.College,

                    };

                    Context.UserProfile.Add(up);
                    Context.SaveChanges();

                    return up.ID;

                }

                
            }

        }//end OF Method

        //check UserProfile Data Is there  GetAnyUserProfileData(myidss); 

        public bool GetAnyUserProfileData(int myidss)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //check email is register or not
                bool isthere = Context.UserProfile.Where(u => u.UserID == myidss).Any();

                return isthere;
            
            }


        }//end of method


        //get SellerNotes Data From ID GetSellerNotesDataFromID
        public SellerNotes GetSellerNotesDataFromID(int id)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //var data = Context.SellerNotes.Where(X => X.Title.StartsWith(Search) || Search == null).ToList();
                var s = (from v in Context.SellerNotes where v.ID == id select v).FirstOrDefault();
                return s;

            }
        }

        //add Download data into Download table AddMyDownloadData(noteid);

        public int AddMyDownloadDataForPaid(int noteid,int loginuserid)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //var data = Context.SellerNotes.Where(X => X.Title.StartsWith(Search) || Search == null).ToList();
                var s = (from v in Context.SellerNotes where v.ID == noteid select v).FirstOrDefault();
                SellerNotes obj = s;
                var categoty = (from v in Context.NoteCategories where v.ID == obj.Category select v.Name).FirstOrDefault();
                Downloads Dobj = new Downloads() {

                    NoteID = obj.ID,
                    Seller = obj.SellerID,
                    Downloader = loginuserid,
                    IsSellerHasAllowedDownload = false,
                    AttachmentPath = obj.NotesPath,
                    IsAttachmentDownloaded = false,
                    AttachmentDownloadedDate = DateTime.Now,
                    IsPaid = true,
                    PurchasedPrice=obj.SellingPrice,
                    NoteTitle=obj.Title,
                    NoteCategory=categoty,
                };

                Context.Downloads.Add(Dobj);
                Context.SaveChanges();
                return 1;

            }
        }


        //add Download data into Download table AddMyDownloadDataForFree(noteid);

        public int AddMyDownloadDataForFree(int noteid, int loginuserid)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //var data = Context.SellerNotes.Where(X => X.Title.StartsWith(Search) || Search == null).ToList();
                var s = (from v in Context.SellerNotes where v.ID == noteid select v).FirstOrDefault();
                SellerNotes obj = s;
                var categoty = (from v in Context.NoteCategories where v.ID == obj.Category select v.Name).FirstOrDefault();
                Downloads Dobj = new Downloads()
                {

                    NoteID = obj.ID,
                    Seller = obj.SellerID,
                    Downloader = loginuserid,
                    IsSellerHasAllowedDownload = true,
                    AttachmentPath = obj.NotesPath,
                    IsAttachmentDownloaded = true,
                    AttachmentDownloadedDate = DateTime.Now,
                    IsPaid = false,
                    PurchasedPrice = obj.SellingPrice,
                    NoteTitle = obj.Title,
                    NoteCategory = categoty,
                };
                Context.Downloads.Add(Dobj);
                Context.SaveChanges();
                return 1;

            }
        }


        //get sellerEmail From Seller ID GetSellerEmailFromID(obj.SellerID);
        public String GetSellerEmailFromID(int SellerID)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var s = (from v in Context.UserProfile where v.UserID == SellerID select v.SecondaryEmailAddress).FirstOrDefault();
                String email = s;
                return email;

            }
        }

        //check Data Exist or not in Download Table .CheckForDownloadRecord(noteid,loginuserid);

        public Tuple<bool,bool>  CheckForDownloadRecord(int noteid,int DownloderID)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //check email is register or not
                bool isthere = Context.Downloads.Where(u => u.NoteID == noteid && u.Downloader==DownloderID).Any();

                if (isthere)
                {
                    var allowed = (from v in Context.Downloads where v.NoteID == noteid && v.Downloader==DownloderID select v.IsSellerHasAllowedDownload).FirstOrDefault();
                    Tuple<bool, bool> myusrdata = new Tuple<bool, bool>(isthere, allowed);
                    return myusrdata;
                }
                else 
                {
                    Tuple<bool, bool> myusrdata = new Tuple<bool, bool>(isthere,false);
                    return myusrdata;
                }
                
               

            }


        }//end of method


        //Display My Download DAta FRom LoginuserID GetMyDownloadDataFromID(loginuserid);

        public List<Downloads> GetMyDownloadDataFromID(int LoginUserID,String SearchData)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //var data = Context.SellerNotes.Where(X => X.Title.StartsWith(Search) || Search == null).ToList();
                //var data = Context.SellerNotes.Single(X => X.ID == id); .Where(X => X.Title.StartsWith(Search) || Search == null).
  //              var s = (from v in Context.Downloads where v.Downloader == LoginUserID && (v.NoteTitle.StartsWith(SearchData) || SearchData == null) select v).ToList();
                var s=Context.Downloads.Where(v=>v.Downloader == LoginUserID && (v.NoteTitle.StartsWith(SearchData) || SearchData == null)).ToList();
                return s;
            }
        }
        //end OF MEthod



        //Get data From UserProfile From ID GetUserProfileDataFromID

        public UserProfile GetUserProfileDataFromID(int LoginUserID)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //var data = Context.SellerNotes.Single(X => X.ID == id); .Where(X => X.Title.StartsWith(Search) || Search == null).
                var s = (from v in Context.UserProfile where v.UserID == LoginUserID select v).FirstOrDefault();
                return s;
            }
        }//end OF Method


        //get BuyerRequest Data from download Table GetBuyerDataFromID(loginuserid);
        public List<Downloads> GetBuyerDataFromID(int LoginUserID,String BuyerDATA)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //var data = Context.SellerNotes.Single(X => X.ID == id); .Where(X => X.Title.StartsWith(Search) || Search == null).
               // var s = (from v in Context.Downloads where v.Seller == LoginUserID select v).ToList();
                var s = Context.Downloads.Where(v => v.Seller == LoginUserID && (v.NoteTitle.StartsWith(BuyerDATA) || BuyerDATA == null)).ToList();
                return s;
            }
        }
        //end OF MEthod

        //get UserProfile List ALL LIST
        public List<UserProfile> GetUserProfileData()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //var data = Context.SellerNotes.Single(X => X.ID == id); .Where(X => X.Title.StartsWith(Search) || Search == null).
                var s = Context.UserProfile.ToList();
                return s;
            }
        }
        //end OF MEthod

        //update mydownload data allow user to download From Buyer Reqest Portal UpdateAllowMyDownloadData
        public int UpdateAllowMyDownloadData(int id)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {
                Downloads dobj = Context.Downloads.Single(X => X.ID == id);
                dobj.IsSellerHasAllowedDownload = true;
                Context.SaveChanges();

                return 1;
            }
        }

        //AddReviewFromMyDownload
        public int AddReviewFromMyDownload(String noteid,String rate,String Comment,int LoginUserID)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                int mynoteid = Convert.ToInt32(noteid);
                int myrate = Convert.ToInt32(rate);
                var sellerID = (from v in Context.SellerNotes where v.ID == mynoteid select v.SellerID).FirstOrDefault();

                SellerNotesReviews Robj = new SellerNotesReviews() {

                    NoteID = mynoteid,
                    ReviewedByID=LoginUserID,
                    AgainstDownloadsID=sellerID,
                    Ratings=myrate,
                    Comments=Comment,
                    IsActive=true,
                
                };
                Context.SellerNotesReviews.Add(Robj);
                Context.SaveChanges();


                return 1;
            }
        }


            }//class Repositry


}//NameSpace














// retrive value from databse
//=====================================================

//public string GetCustomerName(int CustomerId)
//{
//    var results = (from var in CustomerNames
//                   where Var.CustomerId == CustomerId)
//                  select Var.FirstName);
//results.FirstOrDefault();
// }

//======================================================
//For Search Code
//public List<SellerNotes> GetSearchNoteData(String Search)
//{
//    using (var Context = new Notes_MarketplaceEntities())
//    {
//        var data = Context.SellerNotes.Where(X => X.Title.StartsWith(Search) || Search == null).ToList();
//        return data;

//    }
//}

//===================================================
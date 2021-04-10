using System;
using System.Collections.Generic;
using System.IO;
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
                        Password = usrobj.Password,
                        CreatedDate = DateTime.Now,
                        
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

                bool Isvalid = Context.Users_New.Any(X => X.EmailID == obj.EmailID && X.Password == obj.Password && X.IsEmailVerified == true && X.IsActive==true);
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

                //check Notes Exist OR NOT
                if(notesobj.EditNoteID!=null)
                {
                    //SetOldData
                    int Editnoteid = Convert.ToInt32(notesobj.EditNoteID);
                    bool Isthere = Context.SellerNotes.Where(u => u.ID == Editnoteid).Any();
                    if (Isthere) 
                    {
                        var old = (from v in Context.SellerNotes where v.ID == Editnoteid select v).FirstOrDefault();
                       
                        SellerNotes updateNotes = Context.SellerNotes.Single(X => X.ID == Editnoteid);
                        
                        updateNotes.Title=notesobj.Title;
                        updateNotes.Category = notesobj.Category;
                        if (dppath != null)
                        {
                            updateNotes.DisplayPicture = dppath;

                        }
                        if (notepath != null)
                        {
                            updateNotes.NotesPath = notepath;

                        }
                        if (previewpath != null)
                        {
                            updateNotes.NotesPreview = previewpath;

                        }
                        updateNotes.Note_Type = notesobj.Note_Type;
                        updateNotes.NumberofPages = notesobj.NumberofPages;
                        updateNotes.Description = notesobj.Description;
                        updateNotes.UniversityName = notesobj.UniversityName;
                        updateNotes.Country = notesobj.Country;
                        updateNotes.Course = notesobj.Course;
                        updateNotes.CourseCode = notesobj.CourseCode;
                        updateNotes.Professor = notesobj.Professor;
                        updateNotes.CreatedDate = DateTime.Now;
                        updateNotes.IsPaid = is_paid;
                        updateNotes.SellingPrice = price;
                        updateNotes.NumberofPages = notesobj.NumberofPages;
                        Context.SaveChanges();
                    }
                    //calculate Size oF File
                     AddSizeOfFile();

                    return Editnoteid;
                }
                else
                {
                    //add new
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
                    //calculate Size oF File
                    AddSizeOfFile();
                  
                    Context.SellerNotes.Add(sobj);
                    Context.SaveChanges();

                    int returnid = sobj.ID;
                    NumberofDownloads nobj = new NumberofDownloads()
                    {
                        NoteID = sobj.ID,
                        Downloads = 0,
                    };
                    Context.NumberofDownloads.Add(nobj);
                    Context.SaveChanges();
                   
                    return returnid;
                }

                

               
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

        public List<SellerNotes> GetSearchNoteData(String Search,String Note_Type, String Category, String Country, String Course, String University)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {

               var  data = Context.SellerNotes.Where(X => X.Status == 6 && (X.Title.StartsWith(Search) || Search == null)).ToList();
               
                if(Note_Type=="1")
                {
                    Note_Type = null;
                }

                if (Category=="11")
                {
                    Category = null;
                }

                if (Country=="2")
                {
                    Country = null;
                }

                if (Course == "1")
                {
                    Course = null;
                }

                if (University == "1")
                {
                    University = null;
                }
                



                if (Category!=null)
                {
                    int category = Convert.ToInt32(Category);
                    data = Context.SellerNotes.Where(X => X.Status == 6 && (X.Category == category)).ToList();

                }
                if(Note_Type!=null)
                {
                    int type = Convert.ToInt32(Note_Type);
                    data = Context.SellerNotes.Where(X => X.Status == 6 && (X.Note_Type == type)).ToList();
                }
               if (Country!=null)
                {
                    int country = Convert.ToInt32(Country);
                    data = Context.SellerNotes.Where(X => X.Status == 6 && (X.Country == country)).ToList();
                }

                if (Course != null)
                {
                    int cid = Convert.ToInt32(Course);
                    var s = (from v in Context.Course where v.CourseID == cid select v.CourseName).FirstOrDefault();
                    data = Context.SellerNotes.Where(X => X.Status == 6 && (X.Course.Equals(s))).ToList();
                }

                if (University != null)
                {
                    int cid = Convert.ToInt32(University);
                    var s = (from v in Context.University where v.UniversityID == cid select v.UniversityName).FirstOrDefault();
                    data = Context.SellerNotes.Where(X => X.Status == 6 && (X.Course.Equals(s))).ToList();
                }


                if (Category == null && Note_Type == null && Country == null && Course == null && University == null)
                {

                    data = Context.SellerNotes.Where(X => X.Status == 6 && (X.Title.StartsWith(Search) || Search == null)).ToList();
                }
               
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
                    UserProfile Udata = GetUserProfileDataFromID(ids);
                    if (Modelobj.ProfilePicture != null)
                    {
                        DpPath = Modelobj.ProfilePicture;
                    }
                    else
                    {
                        //set oldpath
                        DpPath = Udata.ProfilePicture;

                    }

                    UserProfile myUpobj = Context.UserProfile.Single(X => X.UserID == ids);
                    
                    if (Modelobj.DOB == null)
                    {
                        //setOld
                        myUpobj.DOB = Udata.DOB;
                    }
                    else
                    {
                        myUpobj.DOB = Modelobj.DOB;

                    }
                    //Gender
                    if (Modelobj.Gender == null)
                    {
                        myUpobj.Gender = Udata.Gender;
                    }
                    else
                    {
                        myUpobj.Gender = Modelobj.Gender;
                    }
                                           
                    myUpobj.SecondaryEmailAddress = Modelobj.SecondaryEmailAddress;

                   //Code
                    if (Modelobj.PhonenumberCountryCode == null)
                    {
                        myUpobj.PhonenumberCountryCode = Udata.PhonenumberCountryCode;
                    }
                    else
                    {
                        myUpobj.PhonenumberCountryCode = Modelobj.PhonenumberCountryCode;
                    }

                    myUpobj.Phonenumber = Modelobj.Phonenumber;
                    myUpobj.ProfilePicture = DpPath;
                    myUpobj.AddressLine1 = Modelobj.AddressLine1;
                    myUpobj.AddressLine2 = Modelobj.AddressLine2;
                    myUpobj.City = Modelobj.City;
                    myUpobj.State = Modelobj.State;
                    myUpobj.ZipCode = Modelobj.ZipCode;
                    
                    //Country
                    if (Modelobj.Country == null)
                    {
                        myUpobj.Country = Udata.Country;
                    }
                    else
                    {
                        myUpobj.Country = Modelobj.Country;
                    }


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
                //for calculate latest update of Rating 
                NotesRating();


                return 1;
            }
        }


        //Get ReviewList From DB GetReviewDataList
        public bool IsReviewExist(int NoteId,int loginUserID)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //var data = Context.SellerNotes.Single(X => X.ID == id); .Where(X => X.Title.StartsWith(Search) || Search == null).
                bool isthere = Context.SellerNotesReviews.Where(u => u.NoteID == NoteId && u.ReviewedByID==loginUserID).Any();
                return isthere;
            }
        }

        //Update Pawword Chnage password repository.ChangeUserPassword(model,loginuserid);
        public int ChangeUserPassword(ChangePasswordModel model,int LoginUserID)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {

                bool isthere = Context.Users_New.Where(u => u.ID == LoginUserID && u.Password == model.Password).Any();
                if (isthere)
                {
                    Users_New  obj = Context.Users_New.Single(X => X.ID == LoginUserID);
                    obj.Password = model.NewPassword;
                    Context.SaveChanges();
                    return 1;
                }
                else {


                    return 0;
                }
               
            }
        }


       // get MySold Notes Data From Mydwonload Table GetMySoldNotesDataList
        public List<Downloads> GetMySoldNotesDataList(int LoginUserID,String SoldData)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //  var data = Context.SellerNotes.Single(X => X.ID == id); .Where(X => X.Title.StartsWith(Search) || Search == null).
                 var Datas = Context.Downloads.Where(v => v.Seller == LoginUserID  &&  v.IsSellerHasAllowedDownload == true && (v.NoteTitle.StartsWith(SoldData) || SoldData == null)).ToList();             
                // var SoldList = s.Select(X=>X.NoteID).Distinct().ToList()
                 return Datas;
            }
        }


        //get my Rejected Notes data GetMyRejectedNotesData(loginuserid);


        public List<SellerNotes> GetMyRejectedNotesData(int loginuserid, String RejectedData)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //  var data = Context.SellerNotes.Single(X => X.ID == id); .Where(X => X.Title.StartsWith(Search) || Search == null). 
                // && (v.NoteTitle.StartsWith(SoldData) || SoldData == null)
                var Datas = Context.SellerNotes.Where(v => v.SellerID == loginuserid && v.Status == 7 && (v.Title.StartsWith(RejectedData) || RejectedData == null)).ToList();
                // var SoldList = s.Select(X=>X.NoteID).Distinct().ToList()
                return Datas;
            }
        }

        //Check UserProfile data Exist CheckUserProfileData(myidss);
        public bool CheckUserProfileData(int loginUserID)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //var data = Context.SellerNotes.Single(X => X.ID == id); .Where(X => X.Title.StartsWith(Search) || Search == null).
                bool isthere = Context.UserProfile.Where(u => u.UserID == loginUserID).Any();
                return isthere;
               
            }
        }

        ////CheckSeller Notes Data Exist Or Note
        //public bool CheckSellerNotesDataFromIDdash(int NoteId)
        //{
        //    using (var Context = new Notes_MarketplaceEntities())
        //    {
        //        //var data = Context.SellerNotes.Single(X => X.ID == id); .Where(X => X.Title.StartsWith(Search) || Search == null).
        //        bool isthere = Context.SellerNotes.Where(u => u.ID == NoteId).Any();
        //        return isthere;

        //    }
        //}

        //get Seller Noted data from Id Of dashboard Portal For Edit GetSellerNotesDataFromIDdash(NoteId);
        public SellerNotes GetSellerNotesDataFromIDdash(int NoteId)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //var data = Context.SellerNotes.Single(X => X.ID == id); .Where(X => X.Title.StartsWith(Search) || Search == null).
                var s = (from v in Context.SellerNotes where v.ID == NoteId select v).FirstOrDefault();
                return s;
            }
        }//end OF Method


        //CheckforDownloadedNotes(noteid);
        public bool CheckforDownloadedNotes(int noteid)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //var data = Context.SellerNotes.Single(X => X.ID == id); .Where(X => X.Title.StartsWith(Search) || Search == null).
                bool isthere = Context.NumberofDownloads.Where(u => u.NoteID == noteid).Any();
                return isthere;
            }
        }

        //repository.getNumberofDownloadFromNoteID(noteid);

        public int getNumberofDownloadFromNoteID(int noteid)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var s = (from v in Context.NumberofDownloads where v.NoteID == noteid select v.Downloads).FirstOrDefault();
               
                return (int)s;

            }
        }

        //AddNumberOfDownloads(numberofdownload)
      

        public int AddNumberOfDownloads(int noteid)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {

                bool isthere = Context.NumberofDownloads.Where(u => u.NoteID == noteid).Any();
                if (isthere)
                {
                    //update
                    int numberofdownload = getNumberofDownloadFromNoteID(noteid);
                    int newdata = numberofdownload + 1;
                    NumberofDownloads nobj = Context.NumberofDownloads.Single(X => X.NoteID == noteid);
                    nobj.Downloads = newdata;
                    Context.SaveChanges();
                }
                else 
                {
                    //addnew

                    NumberofDownloads obj = new NumberofDownloads() 
                    {
                        NoteID=noteid,
                        Downloads=1,
                    };
                    Context.NumberofDownloads.Add(obj);
                    Context.SaveChanges();
                }
               
                return 1;

            }
        }

        //file size code
        

        public int AddSizeOfFile()
        {

            using (var Context = new Notes_MarketplaceEntities())
            {

                var Datas = Context.SellerNotes.ToList();

                foreach(var X in Datas)
                {
                    String path = X.NotesPath;
                    FileInfo fi = new FileInfo(@"D:/ASP net MVC5__Projects/Notes__Marketplace/Notes__Marketplace/" + path);
                    var k = fi.Length;

                    SellerNotes dobj = Context.SellerNotes.Single(Y => Y.ID == X.ID);
                    dobj.Size = (k/1000).ToString();
                    Context.SaveChanges();

                }
              

                return 1;
            }
        }//end of method


        //get role id from login user id GetRoleId(loginuserid);
        public int GetRoleId(int loginuserid)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {

                var s = (from v in Context.Users_New where v.ID == loginuserid select v.RoleID).FirstOrDefault();

                int id = s;

                return id;
            }
        }


        //addreport data into table AddReportData
        public int AddReportData(int loginuserid, RejectNotesModel model)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {
                //var sum = Context.SellerNotesReviews.Where(X => X.NoteID == Result.ID).Sum(dra => dra.Ratings);

                int Noteid=Convert.ToInt32(model.noteID);
                int sellerid = Convert.ToInt32(model.SellerID);
                bool Isthere = Context.SellerNotesReportedIssues.Where(u => u.NoteID == Noteid && u.ReportedBYID==loginuserid).Any();
                if(Isthere)
                {
                    //UPDATE
                    SellerNotesReportedIssues nobj = Context.SellerNotesReportedIssues.Single(X => X.NoteID == Noteid && X.ReportedBYID==loginuserid);
                    nobj.Remarks = model.Remark;
                    nobj.CreatedDate = DateTime.Now;
                    Context.SaveChanges();


                }
                else
                {

                    //ADD NEW
                    SellerNotesReportedIssues obj = new SellerNotesReportedIssues() { 
                    
                          NoteID=Noteid,
                          ReportedBYID=loginuserid,
                          AgainstDownloadID=sellerid,
                          Remarks=model.Remark,
                         CreatedDate = DateTime.Now,



                };
                    Context.SellerNotesReportedIssues.Add(obj);
                    Context.SaveChanges();


                
                }

                return 1;
            }
        }

        //GetCountOfMyDownloads(loginid);

        public int GetCountOfMyDownloads(int loginuserid)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {


                var value = Context.Downloads.Where(X=>X.Downloader==loginuserid).Count();
                int count = value;

                return count;
            }
        }//end Of Method


        //GetCountOfMyRejected(loginid);
        public int GetCountOfMyRejected(int loginuserid)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {


                var value = Context.SellerNotes.Where(X => X.SellerID == loginuserid && X.Status == 7).Count();
                int count = value;

                return count;
            }
        }//end Of Method



        //GetCountofBuyerReqest(loginid)
        public int GetCountofBuyerReqest(int loginuserid)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {


                var value = Context.Downloads.Where(X => X.Seller == loginuserid).Count();
                int count = value;

                return count;
            }
        }//end Of Method


        //getnumberofsold(loginid);
        public int getnumberofsold(int loginuserid)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {


                var value = Context.Downloads.Where(X => X.Seller == loginuserid && X.IsSellerHasAllowedDownload==true).Count();
                int count = value;

                return count;
            }
        }//end Of Method


        //getnumberofsold(loginid);
        public int getearning(int loginuserid)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {

                var s = (from v in Context.MemberDetailsData where v.UserID == loginuserid select v.TotalEarning).FirstOrDefault();
                //var value = Context.Downloads.Where(X => X.Seller == loginuserid && X.IsSellerHasAllowedDownload == true).Count();
               if(s==null)
                {
                    s = 0;
                }
                int count = (int)s;

                return count;
            }
        }//end Of Method


        //GetUniversity
        public List<University> GetUniversity()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //  var data = Context.SellerNotes.Single(X => X.ID == id); .Where(X => X.Title.StartsWith(Search) || Search == null). 
                // && (v.NoteTitle.StartsWith(SoldData) || SoldData == null)
                var Datas = Context.University.ToList();
                // var SoldList = s.Select(X=>X.NoteID).Distinct().ToList()
                return Datas;
            }
        }

        //GetCourse
        public List<Course> GetCourse()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //  var data = Context.SellerNotes.Single(X => X.ID == id); .Where(X => X.Title.StartsWith(Search) || Search == null). 
                // && (v.NoteTitle.StartsWith(SoldData) || SoldData == null)
                var Datas = Context.Course.ToList();
                // var SoldList = s.Select(X=>X.NoteID).Distinct().ToList()
                return Datas;
            }
        }

        //getrating
        public List<Rating> GetRating()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //  var data = Context.SellerNotes.Single(X => X.ID == id); .Where(X => X.Title.StartsWith(Search) || Search == null). 
                // && (v.NoteTitle.StartsWith(SoldData) || SoldData == null)
                var Datas = Context.Rating.ToList();
                // var SoldList = s.Select(X=>X.NoteID).Distinct().ToList()
                return Datas;
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
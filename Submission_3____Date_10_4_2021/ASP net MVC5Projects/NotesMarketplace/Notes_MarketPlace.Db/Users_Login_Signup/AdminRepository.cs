using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notes__Marketplace.Models;

namespace Notes_MarketPlace.Db.Users_Login_Signup
{
   public class AdminRepository
    {
        //get Gender List  getgender
        public List<Gender> GetGender()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.Gender.ToList();
                return data;

            }
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

        //get Country Calling Code GetCode
        public List<Code> GetCode()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.Code.ToList();
                return data;

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


        //Update Pawword Chnage password repository.ChangeUserPassword(model,loginuserid);
        public int ChangeUserPassword(ChangePasswordModel model, int LoginUserID)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {

                bool isthere = Context.Users_New.Where(u => u.ID == LoginUserID && u.Password == model.Password).Any();
                if (isthere)
                {
                    Users_New obj = Context.Users_New.Single(X => X.ID == LoginUserID);
                    obj.Password = model.NewPassword;
                    Context.SaveChanges();
                    return 1;
                }
                else
                {


                    return 0;
                }

            }
        }//end Of Method



        // Admin dash data GetSellerNotesData()   (X.CreatedDate.Value.Month==MonthID)
        public List<SellerNotes> GetSellerNotesData(String AdminDashDATA,String Monthid)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {

                if(Monthid!=null)
                {
                    int month = Convert.ToInt32(Monthid);
                    var data = Context.SellerNotes.Where(X => X.Status == 6 && X.CreatedDate.Value.Month == month).ToList();
                     return data;
                }
                else
                {

                    var data = Context.SellerNotes.Where(X => X.Status == 6 && (X.Title.StartsWith(AdminDashDATA) || AdminDashDATA == null)).OrderBy(X=>X.CreatedDate).ToList();
                    return data;
                }
               
               

            }
        }

        //Get UserData
        public List<Users_New> GetAllUserData()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.Users_New.ToList();
                return data;

            }
        }

        //Get aLLData OF NUMBER of dOWNLODAS
        public List<NumberofDownloads> GetDownloadData()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.NumberofDownloads.ToList();
                return data;

            }
        }

        //Get Month List for Dropdown
        public List<Months> GetMonthData()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.Months.ToList();
                return data;

            }
        }

        //code for UnPublishNotes
        public String UnPublishNotes(UnPublishModel Model)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {

                int NoteId = Convert.ToInt32(Model.noteID);
                int SellerId = Convert.ToInt32(Model.SellerID);
                SellerNotes obj = Context.SellerNotes.Single(X => X.ID == NoteId);
                obj.Status = 8;
                obj.AdminRemarks=Model.Remark;
                Context.SaveChanges();

                var s = (from v in Context.UserProfile where v.UserID == SellerId select v.SecondaryEmailAddress).FirstOrDefault();
                String email = s;
                return email;
            }
        }

        //get Notes data From SellerNotes table for notes under Review GetNotesForNotesUnderReview
        public List<SellerNotes> GetNotesForNotesUnderReview(String NotesUnderReviewDATA,String Seller)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                if(Seller!=null)
                {
                    int slrid = Convert.ToInt32(Seller);
                    var data = Context.SellerNotes.Where(X => (X.Status == 4 || X.Status == 5) && (X.SellerID==slrid)).ToList();
                    return data;


                }
                else
                {
                    // && (X.Title.StartsWith(AdminDashDATA) || AdminDashDATA == null)
                    var data = Context.SellerNotes.Where(X => (X.Status == 4 || X.Status == 5) && (X.Title.StartsWith(NotesUnderReviewDATA) || NotesUnderReviewDATA == null)).OrderBy(X=>X.CreatedDate).ToList();
                    return data;


                }

            }
        }


        //approve Notes From Notes Under Review ApproveNotes(NoteID);
        public int ApproveNotes(String NoteID)
        {
             
            using (var Context = new Notes_MarketplaceEntities())
            {

                int noteid = Convert.ToInt32(NoteID);
                SellerNotes obj = Context.SellerNotes.Single(X => X.ID == noteid);
                obj.Status = 6;
                obj.PublishedDate = DateTime.Now;
                Context.SaveChanges();
                return 1;           
            }
        }//end Of Method

        //reject Notes from NoteUnder Review Poratal RejectNotes(Model)

        public int RejectNotes(RejectNotesModel Model)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {

                int noteid = Convert.ToInt32(Model.noteID);
                SellerNotes obj = Context.SellerNotes.Single(X => X.ID == noteid);
                obj.AdminRemarks = Model.Remark;
                obj.Status = 7;
                Context.SaveChanges();
                return 1;
            }
        }//end Of Method


        //published Notes

        public List<SellerNotes> GetPublishedNotes(String publishedDATA,String Seller)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {

                if (Seller != null)
                {
                    int slrid = Convert.ToInt32(Seller);
                    var data = Context.SellerNotes.Where(X => X.Status == 6 && (X.SellerID == slrid)).ToList();
                    return data;
                }
                else 
                {
                    var data = Context.SellerNotes.Where(X => X.Status == 6 && (X.Title.StartsWith(publishedDATA) || publishedDATA == null)).OrderBy(X => X.CreatedDate).ToList();
                    return data;
                }
               

            }
        }

        //get all downloaded Notes GetAllDownloadedNotes
        public List<Downloads> GetAllDownloadedNotes(String DownloadedntsDATA,String Category, String Seller, String Buyer)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                //var data = Context.Downloads.Where(X=>X.NoteTitle.StartsWith(DownloadedntsDATA) || DownloadedntsDATA == null).ToList();
                //return data;

                if (Category != null)
                {
                    int id = Convert.ToInt32(Category);
                    var s = (from v in Context.NoteCategories where v.ID == id select v.Name).FirstOrDefault();
                    var data = Context.Downloads.Where(X => X.NoteCategory.Equals(s)).ToList();
                    return data;
                }
                else if(Seller!=null)
                {
                    int slrid = Convert.ToInt32(Seller);
                    var data = Context.Downloads.Where(X=>X.Seller == slrid).ToList();
                    return data;
                }
                else if (Buyer!=null)
                {
                    int bid = Convert.ToInt32(Buyer);
                    var data = Context.Downloads.Where(X => X.Downloader == bid).ToList();
                    return data;
                }
                else
                {
                    var data = Context.Downloads.Where(X => X.NoteTitle.StartsWith(DownloadedntsDATA) || DownloadedntsDATA == null).ToList();
                    return data;
                }
            }
        }

        //.GetAllDownloadedNotesfordrop
        public List<Downloads> GetAllDownloadedNotesfordrop()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.Downloads.ToList();
                return data;

            }
        }
        // get all seller Notes data  GetAllSellerNotesData
        public List<SellerNotes> GetAllSellerNotesData()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.SellerNotes.ToList();
                return data;

            }
        }


        //rejected Notes List GetAllRejectedNotes();
        public List<SellerNotes> GetAllRejectedNotes(String RejectedDATA,String Seller)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
              

                //=======
                if (Seller != null)
                {
                    int slrid = Convert.ToInt32(Seller);
                    var data = Context.SellerNotes.Where(X => X.Status == 7 && (X.SellerID == slrid)).ToList();
                    return data;
                }
                else
                {
                    var data = Context.SellerNotes.Where(X => X.Status == 7 && (X.Title.StartsWith(RejectedDATA) || RejectedDATA == null)).OrderBy(X => X.CreatedDate).ToList();
                    return data;
                }



            }
        }

       

        //get total Notes Under Review For Perticular user GetTotalNotesUnderReview
        public int GetTotalNotesUnderReview(int UserID)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {

                int sellerid = Convert.ToInt32(UserID);
                var value = Context.SellerNotes.Where(X => X.SellerID == sellerid && X.Status==4).Count();
                int count = value;               
              
                return count;
            }
        }//end Of Method

        //GetTotalPublishedNotes

        public int GetTotalPublishedNotes(int UserID)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {

                int sellerid = Convert.ToInt32(UserID);
                var value = Context.SellerNotes.Where(X => X.SellerID == sellerid && X.Status == 6).Count();
                int count = value;

                return count;
            }
        }//end Of Method

        //get total Downloaded Notes GetTotalDownloadedNotes(X.ID)
        public int GetTotalDownloadedNotes(int UserID)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {

                int Userid = Convert.ToInt32(UserID);
                var value = Context.Downloads.Where(X => X.Downloader == Userid).Count();
                int count = value;

                return count;
            }
        }//end Of Method


        //GetTotalExpenses
        public Decimal GetTotalExpenses(int UserID)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {
                bool IsthereSeller = Context.Downloads.Where(u => u.Seller == UserID).Any();
                bool IsthereDownloader = Context.Downloads.Where(u => u.Downloader == UserID).Any();
                if (IsthereDownloader && IsthereSeller)
                {

                    //var sum = Context.SellerNotesReviews.Where(X => X.NoteID == Result.ID).Sum(dra => dra.Ratings);

                        int Userid = Convert.ToInt32(UserID);
                        var value = Context.Downloads.Where(X => X.Downloader == Userid && X.IsSellerHasAllowedDownload==true).Sum(S=>S.PurchasedPrice);
                        Decimal count = (decimal)value;

                       return count;
                }
                else
                {

                    return 0;
                }
            }
        }//end Of Method

        //GetTotalEarning(X.ID)

        public int GetTotalEarning(int UserID)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {
                //var sum = Context.SellerNotesReviews.Where(X => X.NoteID == Result.ID).Sum(dra => dra.Ratings);
                bool IsthereSeller = Context.Downloads.Where(u => u.Seller == UserID).Any();
                bool IsthereDownloader = Context.Downloads.Where(u => u.Downloader == UserID).Any();
                if(IsthereDownloader && IsthereSeller)
                {
                    int Userid = Convert.ToInt32(UserID);
                    var value = Context.Downloads.Where(X => X.Seller == Userid && X.IsSellerHasAllowedDownload == true).Sum(S => S.PurchasedPrice);
                    int count = (int)value;

                    return count;
                }
                else
                {

                    return 0;
                }
                
            }
        }//end Of Method

        //Add Memeber Detail data Into table
        public int AddMemberData(int userid,String firstname,String lastname,String email,int totalNUR,int totalpublished,int totaldownloaded,Decimal expense,Decimal earning)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {
                bool Isthere = Context.MemberDetailsData.Where(u =>u.UserID == userid).Any();
                var Date = (from v in Context.Users_New where v.ID == userid select v.CreatedDate).FirstOrDefault();
                if (Isthere)
                {
                    //UPDATE
                    MemberDetailsData obj = Context.MemberDetailsData.Single(X => X.UserID == userid);
                    obj.TotalNotesUnderReview =totalNUR;
                    obj.TotalPublishedNotes = totalpublished;
                    obj.TotalDownloadedNotes =totaldownloaded;
                    obj.TotalExpense = expense;
                    obj.TotalEarning = earning;
                    Context.SaveChanges();

                }
                else
                {

                    //add
                    MemberDetailsData Mobj = new MemberDetailsData()
                    {
                        UserID = userid,
                        FirstName = firstname,
                        LastName = lastname,
                        Email = email,
                        JoiningDate = Date,
                        TotalNotesUnderReview = totalNUR,
                        TotalPublishedNotes = totalpublished,
                        TotalDownloadedNotes = totaldownloaded,
                        TotalExpense = expense,
                        TotalEarning = earning,


                     };
                    Context.MemberDetailsData.Add(Mobj);
                    Context.SaveChanges();


                }


                return 1;
            }

        }//endof method

        //get all memberDetails data  GetAllMemberData from  MemberDetailsData table
        public List<MemberDetailsData> GetAllMemberData(String memberDATA)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.MemberDetailsData.Where(X=>X.FirstName.StartsWith(memberDATA) || memberDATA == null).ToList();
                return data;

            }
        }


        // DeactiveUserFromID(userid) deactive User From MemberPortal and Deactive Published Notes also

        public int DeactiveUserFromID(int UserID)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {

                Users_New obj = Context.Users_New.Single(X => X.ID == UserID);
                obj.IsActive = false;
                Context.SaveChanges();

                var data = Context.SellerNotes.Where(X=>X.SellerID==UserID).ToList();
                foreach(var D in data)
                {

                    D.Status = 8;

                }
                return 1;
            }
        }//end of method 


        public int ActiveUserFromID(int UserID)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {

                Users_New obj = Context.Users_New.Single(X => X.ID == UserID);
                obj.IsActive = true;
                Context.SaveChanges();

                var data = Context.SellerNotes.Where(X => X.SellerID == UserID).ToList();
                foreach (var D in data)
                {

                    D.Status = 4;

                }
                return 1;
            }
        }

        // data of DataOFMemberProfile(userid
        public UserProfile DataOFMemberProfile(int userid)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.UserProfile.Where(X=>X.UserID==userid).FirstOrDefault();
                return data;

            }
        }

        //get All Notes From Seller Notes Except draft GetAllNotesFromUserID(userid);

        public List<SellerNotes> GetAllNotesFromUserID(int userid)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.SellerNotes.Where(X => X.SellerID== userid && X.Status != 3).ToList();
                return data;

            }
        }// end off Mehtod


        //get all data of Refrnce data Tbale

        public List<ReferenceData> GetAllRefrenceData()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.ReferenceData.ToList();
                return data;

            }
        }// end off Mehtod


        //get all admin list  GetAllAdminData

        public List<Users_New> GetAllAdminData(String AdminData)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.Users_New.Where(X=>X.RoleID==1 && (X.FirstName.StartsWith(AdminData) || AdminData == null)).ToList();
                return data;

            }
        }// end off Mehtod

        public List<UserProfile> GetAllUserProfileData()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.UserProfile.ToList();
                return data;

            }
        }// end off Mehtod

        //add administartor data  AddAdministratorData(Model);
        public int AddAdministratorData(AddAdministratorModel Model)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {
                //var sum = Context.SellerNotesReviews.Where(X => X.NoteID == Result.ID).Sum(dra => dra.Ratings);

                bool Isthere = Context.Users_New.Where(u => u.ID == Model.UserID).Any();

                if(Isthere)
                {
                    //update
                    //MemberDetailsData obj = Context.MemberDetailsData.Single(X => X.UserID == userid);
                    Users_New updateObj = Context.Users_New.Single(X => X.ID== Model.UserID);
                    updateObj.RoleID = 1;
                    updateObj.FirstName = Model.FirstName;
                    updateObj.LastName = Model.LastName;
                    updateObj.EmailID = Model.EmailID;
                    updateObj.Password = "admin";
                    updateObj.IsEmailVerified = true;
                    updateObj.CreatedDate = DateTime.Now;
                    updateObj.IsActive = true;
                    Context.SaveChanges();

                    //updateProfile
                    UserProfile up = Context.UserProfile.Single(X => X.UserID == Model.UserID);
                                       
                       up.SecondaryEmailAddress = Model.EmailID;
                       up.Phonenumber = Model.Phonenumber;
                       up.PhonenumberCountryCode = Model.PhonenumberCountryCode;
                       Context.SaveChanges();
                }
                else
                {
                    //addnew
                    Users_New addObj = new Users_New()
                    {

                        RoleID = 1,
                        FirstName = Model.FirstName,
                        LastName = Model.LastName,
                        EmailID = Model.EmailID,
                        Password = "admin",
                        IsEmailVerified = true,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                       

                };
                    Context.Users_New.Add(addObj);
                    Context.SaveChanges();

                    UserProfile up = new UserProfile()
                    {
                        UserID = addObj.ID,
                        SecondaryEmailAddress = Model.EmailID,
                        Phonenumber = Model.Phonenumber,
                        PhonenumberCountryCode = Model.PhonenumberCountryCode,
                        AddressLine1 = "non",
                        AddressLine2 = "non",
                        City = "non",
                        State = "non",
                        ZipCode = "non",
                        Country = "non",

                    };
                    Context.UserProfile.Add(up);
                    Context.SaveChanges();
                }
                
                return 1;

            }
        }//end Of Method

        //deactive Admin From ID DeactiveAdminFromID(adminid)
        public void DeactiveAdminFromID(int adminid)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                Users_New Obj = Context.Users_New.Single(X => X.ID == adminid);
                Obj.IsActive = false;
                Context.SaveChanges();

            }
        }// end off Mehtod

        //get admin data from id  
        public Users_New GetAdminDataFromID(int userID)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.Users_New.Where(X=>X.ID==userID).FirstOrDefault();
                return data;

            }
        }// end off Mehtod


        //GetAdminProfileDataFromID
        public UserProfile GetAdminProfileDataFromID(int userID)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.UserProfile.Where(X => X.UserID == userID).FirstOrDefault();
                return data;

            }
        }// end off Mehtod

        //add AdminProfile Data  AddAdminProfileData(model)

        public int AddAdminProfileData(AdminProfileModel  model)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {
                //var sum = Context.SellerNotesReviews.Where(X => X.NoteID == Result.ID).Sum(dra => dra.Ratings);

                bool Isthere = Context.UserProfile.Where(u => u.UserID == model.UserID).Any();

                if(Isthere)
                {

                    UserProfile up = Context.UserProfile.Single(X => X.UserID == model.UserID);

                    up.SecondaryEmailAddress = model.SecondaryEmailAddress;
                    up.Phonenumber = model.Phonenumber;
                    up.PhonenumberCountryCode =model.PhonenumberCountryCode;
                    up.ProfilePicture = model.ProfilePicture;
                    Context.SaveChanges();

                }

                return 1;
            }
        }
        //get report Data  getReportData();

        public List<SellerNotesReportedIssues> getReportData()
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.SellerNotesReportedIssues.ToList();
                return data;

            }
        }// end off Mehtod


        //delete remark
        public void DeleteRemark(int id)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                SellerNotesReportedIssues Obj = Context.SellerNotesReportedIssues.Single(X => X.ID == id);

                Context.SellerNotesReportedIssues.Remove(Obj);
                Context.SaveChanges();

            }
        }// end off Mehtod














        //get all category list GetAllCategoryList();
        public List<NoteCategories> GetAllCategoryList(String SearchData)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.NoteCategories.Where(X=>X.Name.StartsWith(SearchData) || SearchData == null).ToList();
                return data;

            }
        }// end off Mehtod

        //addcategory


        
        public void addCategory(int userid,String name, String description,String ID)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
            
                if(ID != null)
                {
                    int id = Convert.ToInt32(ID);
                    //update
                    NoteCategories Obj = Context.NoteCategories.Single(X => X.ID == id);
                    Obj.Name = name;
                    Obj.Description = description;
                    Obj.CreatedDate = DateTime.Now;
                    Obj.CreatedBy = userid;
                    Obj.IsActive = true;
                    
                    Context.SaveChanges();
                }
                else 
                {

                    // addnew
                    NoteCategories Obj = new NoteCategories()
                    {
                        Name = name,
                        Description = description,
                        CreatedDate = DateTime.Now,
                        CreatedBy = userid,
                        IsActive = true,

                    };
                    Context.NoteCategories.Add(Obj);
                    Context.SaveChanges();

                }

               

            }
        }// end off Mehtod

        //DeactiveCategoryFromID(id);
        public void DeactiveCategoryFromID(int id)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                NoteCategories Obj = Context.NoteCategories.Single(X => X.ID == id);

                Obj.IsActive = false;
                Context.SaveChanges();

            }
        }// end off Mehtod


        //get category from id GetCategoryFromID(myid)
        public NoteCategories GetCategoryFromID(int myid)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.NoteCategories.Where(X => X.ID == myid).FirstOrDefault();
                return data;

            }
        }// end off Mehtod

        //GetAllTypeList(typeDATA);
        public List<NoteTypes> GetAllTypeList(String SearchData)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.NoteTypes.Where(X => X.Name.StartsWith(SearchData) || SearchData == null).ToList();
                return data;

            }
        }// end off Mehtod

        //addType
        public void addType(int userid, String name, String description, String ID)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
               
                if (ID != null)
                {
                    int id = Convert.ToInt32(ID);
                    //update
                    NoteTypes Obj = Context.NoteTypes.Single(X => X.ID == id);
                    Obj.Name = name;
                    Obj.Description = description;
                    Obj.CreatedDate = DateTime.Now;
                    Obj.CreatedBy = userid;
                    Obj.IsActive = true;

                    Context.SaveChanges();
                }
                else
                {

                    // addnew
                    NoteTypes Obj = new NoteTypes()
                    {
                        Name = name,
                        Description = description,
                        CreatedDate = DateTime.Now,
                        CreatedBy = userid,
                        IsActive = true,

                    };
                    Context.NoteTypes.Add(Obj);
                    Context.SaveChanges();

                }



            }
        }// end off Mehtod


        //DeactiveTypeFromID(id)
        public void DeactiveTypeFromID(int id)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                NoteTypes Obj = Context.NoteTypes.Single(X => X.ID == id);

                Obj.IsActive = false;
                Context.SaveChanges();

            }
        }// end off Mehtod


        //GetNoteTypeFromID(myid);
        public NoteTypes GetNoteTypeFromID(int myid)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.NoteTypes.Where(X => X.ID == myid).FirstOrDefault();
                return data;

            }
        }// end off Mehtod


        //GetAllCountryList(countryDATA);
        public List<Countries> GetAllCountryList(String SearchData)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.Countries.Where(X => X.Name.StartsWith(SearchData) || SearchData == null).ToList();
                return data;

            }
        }// end off Mehtod

        //GetCountryFromID(myid)
       
        public Countries GetCountryFromID(int myid)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                var data = Context.Countries.Where(X => X.ID == myid).FirstOrDefault();
                return data;

            }
        }// end off Mehtod


        //addCountry
        public void addCountry(int userid, String name, String description, String ID)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {

                if (ID != null)
                {
                    int id = Convert.ToInt32(ID);
                    //update
                    Countries Obj = Context.Countries.Single(X => X.ID == id);
                    Obj.Name = name;
                    Obj.CountryCode = description;
                    Obj.CreatedDate = DateTime.Now;
                    Obj.CreatedBy = userid;
                    Obj.IsActive = true;

                    Context.SaveChanges();
                }
                else
                {

                    // addnew
                    Countries Obj = new Countries()
                    {
                        Name = name,
                        CountryCode = description,
                        CreatedDate = DateTime.Now,
                        CreatedBy = userid,
                        IsActive = true,

                    };
                    Context.Countries.Add(Obj);
                    Context.SaveChanges();

                }



            }
        }// end off Mehtod

        //DeactiveCountryFromID(id)
        public void DeactiveCountryFromID(int id)
        {
            using (var Context = new Notes_MarketplaceEntities())
            {
                Countries Obj = Context.Countries.Single(X => X.ID == id);

                Obj.IsActive = false;
                Context.SaveChanges();

            }
        }// end off Mehtod


        //InReviewNotes
        public int InReviewNotes(String NoteID)
        {

            using (var Context = new Notes_MarketplaceEntities())
            {

                int noteid = Convert.ToInt32(NoteID);
                SellerNotes obj = Context.SellerNotes.Single(X => X.ID == noteid);
                obj.Status = 5;
                obj.PublishedDate = DateTime.Now;
                Context.SaveChanges();
                return 1;
            }
        }//end Of Method


        //GetCountOfReviewNotes()
        public int GetCountOfReviewNotes()
        {

            using (var Context = new Notes_MarketplaceEntities())
            {

                
                var value = Context.SellerNotes.Where(X => X.Status==4 || X.Status==5).Count();
                int count = value;

                return count;
            }
        }//end Of Method

        //GetCountofDownloadedNotes()
        public int GetCountofDownloadedNotes()
        {

            using (var Context = new Notes_MarketplaceEntities())
            {


                var value = Context.Downloads.Count();
                int count = value;

                return count;
            }
        }//end Of Method


        //GetCountOfRegisterdNotes();
        public int GetCountOfRegisterdNotes()
        {

            using (var Context = new Notes_MarketplaceEntities())
            {


                var value = Context.SellerNotes.Count();
                int count = value;

                return count;
            }
        }//end Of Method

    }//class
}//end of namespace

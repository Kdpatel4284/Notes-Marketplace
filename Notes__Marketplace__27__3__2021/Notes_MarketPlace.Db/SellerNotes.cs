//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Notes_MarketPlace.Db
{
    using System;
    using System.Collections.Generic;
    
    public partial class SellerNotes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SellerNotes()
        {
            this.SellerNotesReportedIssues = new HashSet<SellerNotesReportedIssues>();
            this.SellerNotesReviews = new HashSet<SellerNotesReviews>();
            this.Downloads = new HashSet<Downloads>();
        }
    
        public int ID { get; set; }
        public int SellerID { get; set; }
        public int Status { get; set; }
        public Nullable<int> ActionedBy { get; set; }
        public string AdminRemarks { get; set; }
        public Nullable<System.DateTime> PublishedDate { get; set; }
        public string Title { get; set; }
        public int Category { get; set; }
        public string DisplayPicture { get; set; }
        public Nullable<int> Note_Type { get; set; }
        public string NotesPath { get; set; }
        public Nullable<int> NumberofPages { get; set; }
        public string Description { get; set; }
        public string UniversityName { get; set; }
        public Nullable<int> Country { get; set; }
        public string Course { get; set; }
        public string CourseCode { get; set; }
        public string Professor { get; set; }
        public bool IsPaid { get; set; }
        public Nullable<decimal> SellingPrice { get; set; }
        public string NotesPreview { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Countries Countries { get; set; }
        public virtual NoteCategories NoteCategories { get; set; }
        public virtual NoteTypes NoteTypes { get; set; }
        public virtual ReferenceData ReferenceData { get; set; }
        public virtual SellerNotes SellerNotes1 { get; set; }
        public virtual SellerNotes SellerNotes2 { get; set; }
        public virtual SellerNotes SellerNotes11 { get; set; }
        public virtual SellerNotes SellerNotes3 { get; set; }
        public virtual Users_New Users_New { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SellerNotesReportedIssues> SellerNotesReportedIssues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SellerNotesReviews> SellerNotesReviews { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Downloads> Downloads { get; set; }
    }
}

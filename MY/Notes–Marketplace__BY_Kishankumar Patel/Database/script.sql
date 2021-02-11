USE [master]
GO
/****** Object:  Database [Notes_Marketplace]    Script Date: 11-02-2021 21:10:38 ******/
CREATE DATABASE [Notes_Marketplace]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Notes_Marketplace', FILENAME = N'D:\sql 2017 server\MSSQL14.MSSQLSERVER\MSSQL\DATA\Notes_Marketplace.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Notes_Marketplace_log', FILENAME = N'D:\sql 2017 server\MSSQL14.MSSQLSERVER\MSSQL\DATA\Notes_Marketplace_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Notes_Marketplace] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Notes_Marketplace].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Notes_Marketplace] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Notes_Marketplace] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Notes_Marketplace] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Notes_Marketplace] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Notes_Marketplace] SET ARITHABORT OFF 
GO
ALTER DATABASE [Notes_Marketplace] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Notes_Marketplace] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Notes_Marketplace] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Notes_Marketplace] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Notes_Marketplace] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Notes_Marketplace] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Notes_Marketplace] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Notes_Marketplace] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Notes_Marketplace] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Notes_Marketplace] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Notes_Marketplace] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Notes_Marketplace] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Notes_Marketplace] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Notes_Marketplace] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Notes_Marketplace] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Notes_Marketplace] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Notes_Marketplace] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Notes_Marketplace] SET RECOVERY FULL 
GO
ALTER DATABASE [Notes_Marketplace] SET  MULTI_USER 
GO
ALTER DATABASE [Notes_Marketplace] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Notes_Marketplace] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Notes_Marketplace] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Notes_Marketplace] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Notes_Marketplace] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Notes_Marketplace', N'ON'
GO
ALTER DATABASE [Notes_Marketplace] SET QUERY_STORE = OFF
GO
USE [Notes_Marketplace]
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 11-02-2021 21:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[ID] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[CountryCode] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Downloads]    Script Date: 11-02-2021 21:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Downloads](
	[ID] [int] NOT NULL,
	[NoteID] [int] NOT NULL,
	[Seller] [int] NOT NULL,
	[Downloader] [int] NOT NULL,
	[IsSellerHasAllowedDownload] [bit] NOT NULL,
	[AttachmentPath] [varchar](max) NULL,
	[IsAttachmentDownloaded] [bit] NOT NULL,
	[AttachmentDownloadedDate] [datetime] NULL,
	[IsPaid] [bit] NOT NULL,
	[PurchasedPrice] [decimal](18, 0) NULL,
	[Note Title] [varchar](100) NOT NULL,
	[Note Category] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_Downloads] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteCategories]    Script Date: 11-02-2021 21:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteCategories](
	[ID] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_NoteCategories] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteTypes]    Script Date: 11-02-2021 21:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteTypes](
	[ID] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_NoteTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReferenceData]    Script Date: 11-02-2021 21:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReferenceData](
	[ID] [int] NOT NULL,
	[Value] [varchar](100) NOT NULL,
	[Datavalue] [varchar](100) NOT NULL,
	[RefCategory] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotes]    Script Date: 11-02-2021 21:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotes](
	[ID] [int] NOT NULL,
	[SellerID] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[ActionedBy] [int] NULL,
	[AdminRemarks] [varchar](max) NULL,
	[PublishedDate] [datetime] NULL,
	[Title] [varchar](100) NOT NULL,
	[Category] [int] NOT NULL,
	[DisplayPicture] [varchar](500) NULL,
	[Note Type] [int] NULL,
	[NumberofPages] [int] NULL,
	[Description] [varchar](max) NOT NULL,
	[UniversityName] [varchar](200) NULL,
	[Country] [int] NULL,
	[Course] [varchar](100) NULL,
	[CourseCode] [varchar](100) NULL,
	[Professor] [varchar](100) NULL,
	[IsPaid] [bit] NOT NULL,
	[SellingPrice] [decimal](18, 0) NULL,
	[NotesPreview] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SellerNotes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesAttachements]    Script Date: 11-02-2021 21:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesAttachements](
	[ID] [int] NOT NULL,
	[NoteID] [int] NOT NULL,
	[FileName] [varchar](100) NOT NULL,
	[FilePath] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SellerNotesAttachements] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesReportedIssues]    Script Date: 11-02-2021 21:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesReportedIssues](
	[ID] [int] NOT NULL,
	[NoteID] [int] NOT NULL,
	[ReportedBYID] [int] NOT NULL,
	[AgainstDownloadID] [int] NOT NULL,
	[Remarks] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_SellerNotesReportedIssues] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesReviews]    Script Date: 11-02-2021 21:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesReviews](
	[ID] [int] NOT NULL,
	[NoteID] [int] NOT NULL,
	[ReviewedByID] [int] NOT NULL,
	[AgainstDownloadsID] [int] NOT NULL,
	[Ratings] [decimal](18, 0) NOT NULL,
	[Comments] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModififedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SellerNotesReviews] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemConfigurations]    Script Date: 11-02-2021 21:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemConfigurations](
	[ID] [int] NOT NULL,
	[Key] [varchar](100) NOT NULL,
	[Value] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SystemConfigurations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProfile]    Script Date: 11-02-2021 21:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfile](
	[ID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[DOB] [datetime] NULL,
	[Gender] [int] NULL,
	[SecondaryEmailAddress] [varchar](100) NULL,
	[Phone number-Country Code] [varchar](5) NOT NULL,
	[Phone number] [varchar](20) NOT NULL,
	[Profile Picture] [varchar](500) NULL,
	[Address Line 1] [varchar](100) NOT NULL,
	[Address Line 2] [varchar](100) NOT NULL,
	[City] [varchar](50) NOT NULL,
	[State] [varchar](50) NOT NULL,
	[Zip Code] [varchar](50) NOT NULL,
	[Country] [varchar](50) NOT NULL,
	[University] [varchar](100) NULL,
	[College] [varchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_UserProfile] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 11-02-2021 21:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[ID] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 11-02-2021 21:10:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[EmailID] [varchar](100) NOT NULL,
	[Password] [varchar](24) NOT NULL,
	[IsEmailVerified] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1000, N'india', N'+91', CAST(N'2021-02-11T00:00:00.000' AS DateTime), 1000, CAST(N'2021-02-11T00:00:00.000' AS DateTime), 1000, 1)
GO
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [Note Title], [Note Category], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1000, 1001213, 100, 1234, 1, N'path', 0, CAST(N'2021-02-11T00:00:00.000' AS DateTime), 1, CAST(500 AS Decimal(18, 0)), N'Python', N'Computer Science', CAST(N'2021-02-11T00:00:00.000' AS DateTime), 1000, CAST(N'2021-02-11T00:00:00.000' AS DateTime), 1000)
GO
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1000, N'kishan', N'good', CAST(N'2021-02-11T00:00:00.000' AS DateTime), 10001, CAST(N'2021-02-11T00:00:00.000' AS DateTime), 10001, 1)
GO
INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1000, N'kishan', N'good', CAST(N'2021-02-11T00:00:00.000' AS DateTime), 1000, CAST(N'2021-02-11T00:00:00.000' AS DateTime), 1100, 0)
GO
INSERT [dbo].[ReferenceData] ([ID], [Value], [Datavalue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (10001, N'10', N'12', N'gender', CAST(N'2021-02-11T00:00:00.000' AS DateTime), 1000, CAST(N'2021-02-11T00:00:00.000' AS DateTime), 1000, 1)
GO
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [Note Type], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1001, 10001, 101, 101, N'great', CAST(N'2021-02-10T00:00:00.000' AS DateTime), N'WorkTime', 10101, N'img/cover.jpg', 1, 100, N'good', N'gujarat', 91, NULL, NULL, NULL, 1, CAST(100 AS Decimal(18, 0)), N'path', CAST(N'2021-02-10T00:00:00.000' AS DateTime), 1001, CAST(N'2021-02-10T00:00:00.000' AS DateTime), 101, 1)
GO
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1001, 100, N'kd', N'path', CAST(N'2020-12-10T00:00:00.000' AS DateTime), 100, CAST(N'2020-12-10T00:00:00.000' AS DateTime), 100, 1)
GO
INSERT [dbo].[SellerNotesReportedIssues] ([ID], [NoteID], [ReportedBYID], [AgainstDownloadID], [Remarks], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1000, 10121, 10112, 102120, N'Not Good', CAST(N'2021-02-11T00:00:00.000' AS DateTime), 1010, CAST(N'2021-02-11T00:00:00.000' AS DateTime), 10001)
GO
INSERT [dbo].[SellerNotesReviews] ([ID], [NoteID], [ReviewedByID], [AgainstDownloadsID], [Ratings], [Comments], [CreatedDate], [CreatedBy], [ModifiedDate], [ModififedBy], [IsActive]) VALUES (1000, 1001023, 21231, 12312, CAST(5 AS Decimal(18, 0)), N'good', CAST(N'2021-02-11T00:00:00.000' AS DateTime), 1000, CAST(N'2021-02-11T00:00:00.000' AS DateTime), 1000, 0)
GO
INSERT [dbo].[SystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (10001, N'  key', N'value', CAST(N'2021-02-11T00:00:00.000' AS DateTime), 1001, CAST(N'2021-02-11T00:00:00.000' AS DateTime), 1001, 0)
GO
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [Phone number-Country Code], [Phone number], [Profile Picture], [Address Line 1], [Address Line 2], [City], [State], [Zip Code], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1000, 10001, CAST(N'1999-09-04T00:00:00.000' AS DateTime), 1, N'kdpatel428436@gmail.com', N'+91', N'8140137967', N'img/kishan-1.jpg', N'Sonasan', N'Prantij', N'himmatnagar', N'gujarat', N'383210', N'india', N'ganpat university', N'UVPCE', CAST(N'2021-02-10T00:00:00.000' AS DateTime), 10001, CAST(N'2021-02-10T00:00:00.000' AS DateTime), 1001)
GO
INSERT [dbo].[UserRoles] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1000, N'Admin', N'updated record', CAST(N'2021-02-10T00:00:00.000' AS DateTime), 1001, CAST(N'2021-02-10T00:00:00.000' AS DateTime), 1001, 1)
GO
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1000, 1000, N'Kishan', N'Patel', N'pk814013@gmail.com', N'kishan', 1, CAST(N'2021-02-10T00:00:00.000' AS DateTime), 1, CAST(N'2021-02-10T00:00:00.000' AS DateTime), 1, 1)
GO
USE [master]
GO
ALTER DATABASE [Notes_Marketplace] SET  READ_WRITE 
GO

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Notes__Marketplace
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
             name: "AllowDawnload",
             url: "User/AllowDownload/{id}",
             defaults: new { controller = "User", action = "AllowDownload", id = UrlParameter.Optional }
         );


            routes.MapRoute(
             name: "NotesDeleteByID",
             url: "User/DeleteNotesFromDashDelete/{id}",
             defaults: new { controller = "User", action = "DeleteNotesFromDashDelete", id = UrlParameter.Optional }
         );

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
             name: "NotesDetaisByID",
             url: "User/NoteDetails/{id}",
             defaults: new { controller = "User", action = "NoteDetails" , id = UrlParameter.Optional}
         );

            routes.MapRoute(
               name: "AdminDedault",
               url: "Admin/{action}/{id}",
               defaults: new { controller = "Admin", action = "AdminDashboard", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

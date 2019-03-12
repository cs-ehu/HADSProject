﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using DataBaseAccess;
using EmailLib;
using WebApplication.Framework;
using WebApplication.Utils;

namespace WebApplication {

	public class Global : HttpApplication {

		// Global.asax handlers and events: http://sandblogaspnet.blogspot.com/2008/03/methods-in-globalasax.html

		#region ASP.NET Handlers

		protected void Application_Start(object sender, EventArgs e) {

			ScriptResource.RegisterJQuery(WebConfigurationManager.AppSettings["jquery"]);
			ScriptResource.RegisterBoostrap(WebConfigurationManager.AppSettings["boostrap"]);

			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			SetupDataAccess();

#if DEBUG
			DataAccessService dataService;
			Application.Lock();
			dataService = Application["DataAccess"] as DataAccessService;
			Application.UnLock();
			dataService.NonQuery("DELETE FROM Usuarios WHERE email = 'tudelaigarcia@gmail.com'");
#endif
		}

		protected void Application_BeginRequest(object sender, EventArgs e) { }

		protected void Application_AuthenticateRequest(object sender, EventArgs e) { }

		protected void Session_Start(object sender, EventArgs e) {

			Lazy<DataAccessService> dataAccessService = new Lazy<DataAccessService>(() => (DataAccessService)Application.Get("DataAccess"));
			Session["DataAccess"] = dataAccessService.Value;

			Session["IsLogged"] = false;
			Session["Email"] = "";
			Session["Name"] = "";
			Session["LastName"] = "";
			Session["UserType"] = "";

		}

		protected void Application_EndRequest(object sender, EventArgs e) { }

		protected void Session_End(object sender, EventArgs e) {
		}

		protected void Application_End(object sender, EventArgs e) { }

		protected void Application_Error(object sender, EventArgs e) { }

		#endregion

		protected void SetupDataAccess() {

			DataAccessService dataAccess = new DataAccessService(ConfigurationManager.ConnectionStrings["HADS18-DB"].ConnectionString);
			Application.Lock();
			Application.Set("DataAccess", dataAccess);
			Application.UnLock();

		}

	}

}

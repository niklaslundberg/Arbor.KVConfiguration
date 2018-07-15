using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Arbor.KVConfiguration.Core;

namespace Arbor.KVConfiguration.Samples.Web
{
    public class Global : HttpApplication
    {
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
            if (StaticKeyValueConfigurationManager.AppSettings is IDisposable disposable)
            {
                disposable.Dispose();
            }

            StaticKeyValueConfigurationManager.Release();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            Bootstrapper.Start();

            RouteTable.Routes.MapMvcAttributeRoutes();
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }
    }
}

﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AutoResponse.Sample.WebApi2.Startup))]

namespace AutoResponse.Sample.WebApi2
{
    using System.Web.Http;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {            
            new Bootstrapper(app).Run();         
        }     
    }
}

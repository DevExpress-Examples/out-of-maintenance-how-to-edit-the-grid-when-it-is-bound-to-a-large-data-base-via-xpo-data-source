using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SampleCore.Utils
{
    public class ConnectionStringsProvider {
        public ConnectionStringsProvider(IConfiguration configuration, string contentRootPath) {
            Configuration = configuration;
            ContentRootPath = contentRootPath;
            DatabasesPath = Path.Combine(ContentRootPath, "App_Data");
        }

        protected IConfiguration Configuration { get; }
        protected string ContentRootPath { get; }
        protected string DatabasesPath { get; }

        public string GetConnectionString(string name) {
            return Configuration.GetConnectionString(name).Replace("%DataDirectory%", DatabasesPath);
        }
    }
}

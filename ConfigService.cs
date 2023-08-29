using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIQnALab1
{
    internal class ConfigService
    {
        public IConfiguration LoadConfiguration()
        {
            //A Frustrating issue where I need to specify the dir of the file because it's a console app.
            //otherwise the appsettings.json can not load in the environment (because it's -
            //expecting a default folder that doesn't exist). But this should solve the issue.
            var projectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            // Configurations to load the appsettings.json and make it required (otherwise will throw an exception).
            var builder = new ConfigurationBuilder()
                .SetBasePath(projectDir)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }
    }
}

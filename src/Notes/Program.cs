using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Notes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            String url = "";
            foreach (string arg in args)
            {
                if (arg.ToString().StartsWith("http"))
                {
                    url = arg.ToString();
                }
            }
            if (url.Length < 1)
            {
                url = "http://0.0.0.0:5000";
            }

        var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls(url)
                .Build();

            host.Run();
        }
    }
}

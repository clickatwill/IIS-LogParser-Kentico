using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = @"C:\IIS-ParsedLog.csv";
            int count = 0;

            using (var stream = File.CreateText(file))
            {
                stream.WriteLine("Date, Time, Client IP, URL, Referring URL");

                string[] files = Directory.GetFiles(@"E:\Testing\LogParser\logs");
                count = files.Count();

                foreach (var s in files)
                {
                    StreamReader reader = File.OpenText(s);
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (MeetsReportConditions(line))
                        {
                            string[] items = line.Split(' ');

                            string date = items[0];
                            string time = items[1];
                            string ip = items[8];
                            string url = items[4];
                            string referer = items[10];

                            string csvRow = string.Format("{0},{1},{2},{3},{4}", date, time, ip, url, referer);

                            stream.WriteLine(csvRow);
                            
                        }
                    }
                   
                }
              
            }

            Console.WriteLine("Finished parsing " + count + " files...");
            Console.ReadLine();
        }

        private static bool MeetsReportConditions(string textLine)
        {
            //Comments
            if (textLine.StartsWith("#"))
            {
                return false;
            }


            //Pingdom Bot
            if (textLine.Contains("Pingdom.com_bot"))
            {
                return false;
            }

            //Google Bot
            if (textLine.Contains("AdsBot-Google"))
            {
                return false;
            }

            //Baiduspider Bot
            if (textLine.ToLower().Contains("baiduspider"))
            {
                return false;
            }

            //Kentico Stylesheets
            if (textLine.Contains("/CMSPages/Get") || textLine.Contains(".css"))
            {
                return false;
            }


            //Service calls
            if (textLine.Contains("/ScriptResource.axd") || textLine.Contains("/WebResource.axd") || textLine.Contains("getattachment/"))
            {
                return false;
            }

            //Images
            if (textLine.Contains(".jpg") || textLine.Contains(".png") || textLine.Contains(".gif") || textLine.Contains(".svg") || textLine.Contains("favicon.ico"))
            {
                return false;
            }

            //Fonts
            if (textLine.Contains(".woff") || textLine.Contains(".eot"))
            {
                return false;
            }

            //Javascript
            if (textLine.Contains(".js"))
            {
                return false;
            }

            //Kentico folder
            if (textLine.Contains("/CMSTemplates/") || textLine.Contains("/CMSFormControls/") || textLine.Contains("/CMSAdminControls/") || textLine.Contains("/CMSModules/") || textLine.Contains("/CMSPages/") || textLine.Contains("/Admin/CMSAdmin") || textLine.Contains("/Admin/cmsadmin"))
            {
                return false;
            }





            return true;
        }
    }
}

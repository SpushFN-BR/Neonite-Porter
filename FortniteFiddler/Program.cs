using Fiddler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace FortniteFiddler
{

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green; // console colors
            Console.WriteLine("NeonitePorter is a tool made by @BiruFN & @SpushFNBR that redirects you to neonite backend on Fortnite.\n \n"); // console write line
            Console.WriteLine("Instructions:\n 1. In the next 5 seconds, a screen will open, press yes \n 2. Launch neonite.bat https://github.com/HybridFNBR/Neonite \n 3. Launch Fortnite in Epic Games Store. \n This shouldnt be bannable since it redirects all profile stuff to the backend, But as always do it at your own risk. ");
            if (CertMaker.createRootCert())
            {
                if (CertMaker.trustRootCert())
                {
                    FiddlerCoreStartupSettings settings = new FiddlerCoreStartupSettingsBuilder()
                        .ListenOnPort(9999)
                        .OptimizeThreadPool()
                        .DecryptSSL()
                        .RegisterAsSystemProxy()
                        .Build();
                    
                    FiddlerApplication.BeforeRequest += BeforeReq;
                    FiddlerApplication.BeforeResponse += BeforeRes;

                    FiddlerApplication.Startup(settings);

                    Console.ReadKey();

                    FiddlerApplication.Shutdown();
                }
            }
        }
        // fiddler stuff
        public static void BeforeReq(Session session)
        {
            if (session.RequestHeaders["User-Agent"].StartsWith("Fortnite"))
            {
                if (session.PathAndQuery.Contains("/game/v2/profile/") || session.PathAndQuery.Contains("/api/locker/v3") || session.PathAndQuery.Contains("/lightswitch/api/service/") || session.PathAndQuery.Contains("/waitingroom/api/waitingroom"))
                {
                    if (session.HTTPMethodIs("connect"))
                    {
                        session["x-replywithtunnel"] = "FortniteTunnel";
                        return;
                    }

                    session.fullUrl = "http://localhost:5595" + session.PathAndQuery;
                }
            }
        }

        public static void BeforeRes(Session session)
        {
            if (session.RequestHeaders["User-Agent"].StartsWith("Fortnite"))
            {

                
            }
        }
    }
}

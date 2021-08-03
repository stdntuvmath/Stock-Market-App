using System;
using System.Threading.Tasks;
using Alpaca.Markets;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using Stock_Paper_Trading_App;
using System.IO;

namespace Stock_Trading_App
{
    internal static class REGINAListener
    {
        /*
        
            This method will
             
        */


        private static string API_KEY = "PKHNXT0M3ZBTEKBEH1LM";
        private static string API_SECRET_KEY = "3EjNkXEQt7s4WceVGdmPslJ6oYo65uIzfHE09yPS";
        private static string windowsUserName = System.Environment.UserName;//gives windows username
        private static string CurrentEquityFile = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\CurrentEquity.txt";
        private static string CurrentCashMade = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\CurrentCashMade.txt";
        private static string InitialEquityFile = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\InitialEquityFile.txt";


        private static string logFileLine;

        private static bool OKtoBuy = true;
        private static bool Sold = false;
        private static decimal buyPrice;
        private static decimal buyInEquity;
        private static decimal NewMarketPrice;



        public static async Task REGINAListenerMethod(string stockListing, int numberOfShares)
        {

        }



    }
}

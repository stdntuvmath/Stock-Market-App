using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Alpaca.Markets;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using Stock_Paper_Trading_App;
using System.IO;

namespace Stock_Trading_App
{
    internal static class Test1_TestingValues
    {


        private static string API_KEY = "PKHNXT0M3ZBTEKBEH1LM";
        private static string API_SECRET_KEY = "3EjNkXEQt7s4WceVGdmPslJ6oYo65uIzfHE09yPS";
        private static string windowsUserName = System.Environment.UserName;//gives windows username
        private static string CurrentEquityFile = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\CurrentEquity.txt";
        private static string CurrentCashMade = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\CurrentCashMade.txt";
        private static string logFileLine;

        private static bool OKtoBuy = true;
        private static bool Sold = false;
        private static decimal buyPrice;
        private static decimal NewMarketPrice;

        private static decimal oldEquity = 0;
        private static decimal equity = 0;
        private static decimal initialEquity = equity;
        private static decimal buyInEquity = 0;
        private static decimal incrementEquity = 0;
        private static decimal cashMadeToday = 0;
        private static decimal longAmount = 0;
        private static decimal shortAmount = 0;

        public static async Task Test1_TestingValuesMethod(string stockListing, int numberOfShares)
        {
            TimeSpan start = new TimeSpan(8, 29, 0); //8:30 AM
            TimeSpan end = new TimeSpan(14, 59, 0); //3 o'clock military time
            TimeSpan now = DateTime.Now.TimeOfDay;
            DayOfWeek theDayToday = DateTime.Today.DayOfWeek;

            DateTime todaysDateOnly = DateTime.Today;


            string today = theDayToday.ToString();
            string todaysDate = todaysDateOnly.ToString("yyyyMMdd_");

            string logFilePath = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\Logs\" + todaysDate + @"log.txt";



            //ready the getLastTrade web call (DataClient)

            var clientData = Alpaca.Markets.Environments.Paper.GetAlpacaDataClient(new SecretKey(API_KEY, API_SECRET_KEY));


            //ready the getLastEquity web call (TradingClient)
            var client = Alpaca.Markets.Environments.Paper.GetAlpacaTradingClient(new SecretKey(API_KEY, API_SECRET_KEY));

            // Get our account information.
            var account = await client.GetAccountAsync();

           


            


            initialEquity = account.TradableCash;
            longAmount = account.LongMarketValue;
            shortAmount = account.ShortMarketValue;
            




            

            now = DateTime.Now.TimeOfDay;
            logFileLine = now + "|" + "initialEquity: " + initialEquity + "\r";
            File.AppendAllText(logFilePath, logFileLine);

            now = DateTime.Now.TimeOfDay;
            logFileLine = now + "|" + "LongMarketValue: " + longAmount + "\r";
            File.AppendAllText(logFilePath, logFileLine);

            now = DateTime.Now.TimeOfDay;
            logFileLine = now + "|" + "ShortMarketValue: " + shortAmount + "\r";
            File.AppendAllText(logFilePath, logFileLine);

            

            //buy something
            



            now = DateTime.Now.TimeOfDay;
            logFileLine = "*******************************************\r"+now + "|" + "BUY and get everything again...\r********************************************\r\r";
            File.AppendAllText(logFilePath, logFileLine);

            BuyPaperStock.BuyPaperStockMethod(stockListing, numberOfShares);



            


            Thread.Sleep(10000);

            //MessageBox.Show(equity.ToString());
            //account = null;
            equity = decimal.Zero;
            //MessageBox.Show(equity.ToString());

            account = await client.GetAccountAsync();
            //longAmount = account.LongMarketValue;
            equity = account.TradableCash;// + buyPrice;
                                          //MessageBox.Show("Get New Equity: " + equity.ToString());




            longAmount = account.LongMarketValue;
            shortAmount = account.ShortMarketValue;





            now = DateTime.Now.TimeOfDay;
            logFileLine = now + "|" + "initialEquity: " + initialEquity + "\r";
            File.AppendAllText(logFilePath, logFileLine);

            now = DateTime.Now.TimeOfDay;
            logFileLine = now + "|" + "newEquity: " + equity + "\r";
            File.AppendAllText(logFilePath, logFileLine);

            now = DateTime.Now.TimeOfDay;
            logFileLine = now + "|" + "LongMarketValue: " + longAmount + "\r";
            File.AppendAllText(logFilePath, logFileLine);

            now = DateTime.Now.TimeOfDay;
            logFileLine = now + "|" + "ShortMarketValue: " + shortAmount + "\r";
            File.AppendAllText(logFilePath, logFileLine);

            now = DateTime.Now.TimeOfDay;
            logFileLine = now + "|" + "buyPrice: " + buyPrice + "\r";
            File.AppendAllText(logFilePath, logFileLine);

            decimal buyPricePlusInitialEquity = decimal.Add(buyPrice,equity);

            now = DateTime.Now.TimeOfDay;
            logFileLine = now + "|" + "buyPrice + equity = initialEquity: " +buyPrice+ " + " + equity + " = " +buyPricePlusInitialEquity + "\r\r";
            File.AppendAllText(logFilePath, logFileLine);

            decimal longAmountAndEquity = decimal.Add(longAmount, equity);


            now = DateTime.Now.TimeOfDay;
            logFileLine = now + "|" + "LongMarketValue + equity = initialEquity?: " + longAmount + " + " + equity + " = " + longAmountAndEquity + "\r\r";
            File.AppendAllText(logFilePath, logFileLine);



            




            now = DateTime.Now.TimeOfDay;
            logFileLine = "*******************************************\r" + now + "|" + "SELL and get everything again...\r********************************************\r\r\r\r";
            File.AppendAllText(logFilePath, logFileLine);

            SellPaperStock.SellPaperStockMethod(stockListing);

            





            //MessageBox.Show(equity.ToString());
            //account = null;
            equity = decimal.Zero;
            //MessageBox.Show(equity.ToString());

            account = await client.GetAccountAsync();
            //longAmount = account.LongMarketValue;
            equity = account.TradableCash;// + buyPrice;
                                          //MessageBox.Show("Get New Equity: " + equity.ToString());


            initialEquity = account.TradableCash;
            longAmount = account.LongMarketValue;
            shortAmount = account.ShortMarketValue;
            




            

            now = DateTime.Now.TimeOfDay;
            logFileLine = now + "|" + "initialEquity: " + initialEquity + "\r";
            File.AppendAllText(logFilePath, logFileLine);

            now = DateTime.Now.TimeOfDay;
            logFileLine = now + "|" + "LongMarketValue: " + longAmount + "\r";
            File.AppendAllText(logFilePath, logFileLine);

            now = DateTime.Now.TimeOfDay;
            logFileLine = now + "|" + "ShortMarketValue: " + shortAmount + "\r";
            File.AppendAllText(logFilePath, logFileLine);

            

        }

    }
}

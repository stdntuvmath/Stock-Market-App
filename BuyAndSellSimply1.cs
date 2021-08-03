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
    internal static class BuyAndSellSimply1
    {
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


        public static async Task BuyAndSellSimply1Method(string stockListing, int numberOfShares)
        {
            //ready the getLastTrade web call (DataClient)

            var clientData = Alpaca.Markets.Environments.Paper.GetAlpacaDataClient(new SecretKey(API_KEY, API_SECRET_KEY));


            //ready the getLastEquity web call (TradingClient)
            var client = Alpaca.Markets.Environments.Paper.GetAlpacaTradingClient(new SecretKey(API_KEY, API_SECRET_KEY));

            // Get our account information.
            var account = await client.GetAccountAsync();


            //while within stock market hours
            TimeSpan start = new TimeSpan(8, 29, 0); //8:30 AM
            TimeSpan end = new TimeSpan(14, 59, 0); //3 o'clock military time
            TimeSpan now = DateTime.Now.TimeOfDay;
            DayOfWeek theDayToday = DateTime.Today.DayOfWeek;
            string today = theDayToday.ToString();

            DateTime todaysDateOnly = DateTime.Today;
            string todaysDate = todaysDateOnly.ToString("yyyyMMdd_");

            string logFilePath = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\Logs\" + todaysDate + @"log.txt";



            decimal initialEquity = account.Equity;// the equity when everything is sold.

            decimal oldEquity = 0;
            decimal equity = 0;
            decimal buyInEquity = 0;
            decimal incrementEquity = 0;
            decimal cashMadeToday = 0;
            decimal oldEquityOffset = 0;

            string[] logFileArray = { };



            if ((start > now) || (now >= end) || today == "Saturday" || today == "Sunday")
            {


                MessageBox.Show("The Stock Market is closed. Please try again Monday thru Friday 8:30 AM to 3:00 PM CST.", "The Stock Market is Closed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                now = DateTime.Now.TimeOfDay;
                logFileLine = "************************\r" + now + "|" + "Attempted access: The Stock Market is closed. Please try again Monday thru Friday 8:30 AM to 3:00 PM CST.\r************************\r\r";
                File.AppendAllText(logFilePath, logFileLine);

                File.WriteAllText(CurrentEquityFile, initialEquity.ToString());
            }
            else
            {
                while ((start <= now) && (now < end) && today != "Saturday" && today != "Sunday")
                {
                    now = DateTime.Now.TimeOfDay;
                    logFileLine = now + "_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-\rSTART FINDING SLOPE\r\r\r";
                    File.AppendAllText(logFilePath, logFileLine);




                    //get slope
                    //get price from Alpaca and convert to decimal number
                    var getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                    var newPriceObject = getLastTradeObject.Price;
                    decimal oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                    newPriceObject = decimal.Zero;
                    //MessageBox.Show("old price: "+oldMarketPrice);                        
                    //wait 10 seconds
                    Thread.Sleep(10000);
                    //MessageBox.Show("new price object: " + newPriceObject);
                    //get another price from Alpaca and convert to decimal number
                    getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                    newPriceObject = getLastTradeObject.Price;
                    decimal newMarketPrice = decimal.Parse(newPriceObject.ToString());
                    newPriceObject = decimal.Zero;
                    //MessageBox.Show("new price: " + newMarketPrice);
                    //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                    decimal divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                    decimal slope1 = Decimal.Divide(divisor, 10);
                    //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                    //MessageBox.Show("slope at " + now.ToString() + " is " + slope);
                }
            }
        }
    }
}

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
    internal static class BuyAndSellUsingEquity
    {
        
        private static string API_KEY = "removed";
        private static string API_SECRET_KEY = "removed";
        private static string windowsUserName = System.Environment.UserName;//gives windows username
        private static string CurrentEquityFile = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\CurrentEquity.txt";
        private static string CurrentCashMade = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\CurrentCashMade.txt";
        private static string logFileLine;

        private static bool OKtoBuy = true;
        private static bool Sold = false;
        private static decimal buyPrice;
        private static decimal NewMarketPrice;

        public static async Task BuyAndSellPaperSlopeAvg60AndEquityMethod(string stockListing, int numberOfShares)
        {
            try
            {
                //ready the getLastTrade web call (DataClient)

                var clientData = Alpaca.Markets.Environments.Paper.GetAlpacaDataClient(new SecretKey(API_KEY, API_SECRET_KEY));


                //ready the getLastEquity web call (TradingClient)
                var client = Alpaca.Markets.Environments.Paper.GetAlpacaTradingClient(new SecretKey(API_KEY, API_SECRET_KEY));

                // Get our account information.
                var account = await client.GetAccountAsync();               

                //test to see if buying changes without the null
                //decimal equity = account.TradableCash;
                //MessageBox.Show(equity.ToString());
                //BuyPaperStock.BuyPaperStockMethod(stockListing, numberOfShares);
                //account = await client.GetAccountAsync();
                //equity = account.TradableCash;
                //MessageBox.Show("After buy: "+equity.ToString());




                decimal oldEquity = 0;
                decimal equity = account.TradableCash;
                decimal initialEquity = equity;
                decimal buyInEquity = 0;
                decimal incrementEquity = 0;
                decimal cashMadeToday = 0;
                decimal longAmount = 0;

                string[] logFileArray = { };

                //while within stock market hours
                TimeSpan start = new TimeSpan(8, 29, 0); //8:30 AM
                TimeSpan end = new TimeSpan(14, 59, 0); //3 o'clock military time
                TimeSpan now = DateTime.Now.TimeOfDay;
                DayOfWeek theDayToday = DateTime.Today.DayOfWeek;
                string today = theDayToday.ToString();

                DateTime todaysDateOnly = DateTime.Today;
                string todaysDate = todaysDateOnly.ToString("yyyyMMdd_");

                string logFilePath = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\Logs\"+ todaysDate + @"log.txt";

                //if (!File.Exists(logFilePath))
                //{
                //    File.CreateText(logFilePath);
                //}
                //else
                //{
                //    //do nothing
                //}
                

                //write equity to file

                //File.WriteAllText(CurrentEquityFile, equity.ToString());

                //write cash made to file

                //cashMadeToday = equity - initialEquity;

                //File.WriteAllText(CurrentCashMade, cashMadeToday.ToString());


                if ((start > now) || (now >= end) || today == "Saturday" || today == "Sunday")
                {
                    MessageBox.Show("The Stock Market is closed. Please try again Monday thru Friday 8:30 AM to 3:00 PM CST.", "The Stock Market is Closed",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    now = DateTime.Now.TimeOfDay;
                    logFileLine = now + "|" + "Attempted access: The Stock Market is closed. Please try again Monday thru Friday 8:30 AM to 3:00 PM CST.\r";
                    File.AppendAllText(logFilePath, logFileLine);
                }
                else
                {
                    while ((start <= now) && (now < end) && today != "Saturday" && today != "Sunday")
                    {
                        //MessageBox.Show(now.Minutes.ToString());
                        //MessageBox.Show(now.Seconds.ToString());
                        now = DateTime.Now.TimeOfDay;

                        if ((now.Minutes == 00 && now.Seconds == 0) || (now.Minutes == 02 && now.Seconds == 30) ||
                            (now.Minutes == 05 && now.Seconds == 0) || (now.Minutes == 07 && now.Seconds == 30) ||
                            (now.Minutes == 10 && now.Seconds == 0) || (now.Minutes == 12 && now.Seconds == 30) ||
                            (now.Minutes == 15 && now.Seconds == 0) || (now.Minutes == 17 && now.Seconds == 30) ||
                            (now.Minutes == 20 && now.Seconds == 0) || (now.Minutes == 22 && now.Seconds == 30) ||
                            (now.Minutes == 25 && now.Seconds == 0) || (now.Minutes == 27 && now.Seconds == 30) ||
                            (now.Minutes == 30 && now.Seconds == 0) || (now.Minutes == 32 && now.Seconds == 30) ||
                            (now.Minutes == 30 && now.Seconds == 0) || (now.Minutes == 37 && now.Seconds == 30) ||
                            (now.Minutes == 40 && now.Seconds == 0) || (now.Minutes == 42 && now.Seconds == 30) ||
                            (now.Minutes == 45 && now.Seconds == 0) || (now.Minutes == 47 && now.Seconds == 30) ||
                            (now.Minutes == 50 && now.Seconds == 0) || (now.Minutes == 52 && now.Seconds == 30) ||
                            (now.Minutes == 55 && now.Seconds == 0) || (now.Minutes == 57 && now.Seconds == 30) 
                            )//every 2.5 minutes synced to the clock
                        {

                            
                            //get slope
                            //get price from Alpaca and convert to decimal number
                            var getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            var newPriceObject = getLastTradeObject.Price;
                            decimal oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("old price: "+oldMarketPrice);                        
                            //wait 10 seconds
                            Thread.Sleep(5000);
                            //MessageBox.Show("new price object: " + newPriceObject);
                            //get another price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            decimal newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("new price: " + newMarketPrice);
                            //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            decimal divisor = newMarketPrice - oldMarketPrice;
                            decimal slope1 = Decimal.Divide(divisor, 5);
                            //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //MessageBox.Show("slope at " + now.ToString() + " is " + slope);





                            //get price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("old price: "+oldMarketPrice);                        
                            //wait 10 seconds
                            Thread.Sleep(5000);
                            //MessageBox.Show("new price object: " + newPriceObject);
                            //get another price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("new price: " + newMarketPrice);
                            //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            divisor = newMarketPrice - oldMarketPrice;
                            decimal slope2 = Decimal.Divide(divisor, 5);
                            //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //MessageBox.Show("slope at " + now.ToString() + " is " + slope);




                            //get price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("old price: "+oldMarketPrice);                        
                            //wait 10 seconds
                            Thread.Sleep(5000);
                            //MessageBox.Show("new price object: " + newPriceObject);
                            //get another price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("new price: " + newMarketPrice);
                            //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            divisor = newMarketPrice - oldMarketPrice;
                            decimal slope3 = Decimal.Divide(divisor, 5);
                            //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //MessageBox.Show("slope at " + now.ToString() + " is " + slope);


                            //get price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("old price: "+oldMarketPrice);                        
                            //wait 10 seconds
                            Thread.Sleep(5000);
                            //MessageBox.Show("new price object: " + newPriceObject);
                            //get another price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("new price: " + newMarketPrice);
                            //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            divisor = newMarketPrice - oldMarketPrice;
                            decimal slope4 = Decimal.Divide(divisor, 5);
                            //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //MessageBox.Show("slope at " + now.ToString() + " is " + slope);


                            //get price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("old price: "+oldMarketPrice);                        
                            //wait 10 seconds
                            Thread.Sleep(5000);
                            //MessageBox.Show("new price object: " + newPriceObject);
                            //get another price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("new price: " + newMarketPrice);
                            //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            divisor = newMarketPrice - oldMarketPrice;
                            decimal slope5 = Decimal.Divide(divisor, 5);
                            //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //MessageBox.Show("slope at " + now.ToString() + " is " + slope);


                            //get price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("old price: "+oldMarketPrice);                        
                            //wait 10 seconds
                            Thread.Sleep(5000);
                            //MessageBox.Show("new price object: " + newPriceObject);
                            //get another price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("new price: " + newMarketPrice);
                            //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            divisor = newMarketPrice - oldMarketPrice;
                            decimal slope6 = Decimal.Divide(divisor, 5);
                            //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //MessageBox.Show("slope at " + now.ToString() + " is " + slope);
                           


                            //get price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("old price: "+oldMarketPrice);                        
                            //wait 10 seconds
                            Thread.Sleep(5000);
                            //MessageBox.Show("new price object: " + newPriceObject);
                            //get another price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("new price: " + newMarketPrice);
                            //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            divisor = newMarketPrice - oldMarketPrice;
                            decimal slope7 = Decimal.Divide(divisor, 5);
                            //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //MessageBox.Show("slope at " + now.ToString() + " is " + slope);

                            //get price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("old price: "+oldMarketPrice);                        
                            //wait 10 seconds
                            Thread.Sleep(5000);
                            //MessageBox.Show("new price object: " + newPriceObject);
                            //get another price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("new price: " + newMarketPrice);
                            //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            divisor = newMarketPrice - oldMarketPrice;
                            decimal slope8 = Decimal.Divide(divisor, 5);
                            //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //MessageBox.Show("slope at " + now.ToString() + " is " + slope);

                            //get price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("old price: "+oldMarketPrice);                        
                            //wait 10 seconds
                            Thread.Sleep(5000);
                            //MessageBox.Show("new price object: " + newPriceObject);
                            //get another price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("new price: " + newMarketPrice);
                            //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            divisor = newMarketPrice - oldMarketPrice;
                            decimal slope9 = Decimal.Divide(divisor, 5);
                            //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //MessageBox.Show("slope at " + now.ToString() + " is " + slope);

                            //get price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("old price: "+oldMarketPrice);                        
                            //wait 10 seconds
                            Thread.Sleep(5000);
                            //MessageBox.Show("new price object: " + newPriceObject);
                            //get another price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("new price: " + newMarketPrice);
                            //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            divisor = newMarketPrice - oldMarketPrice;
                            decimal slope10 = Decimal.Divide(divisor, 5);
                            //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //MessageBox.Show("slope at " + now.ToString() + " is " + slope);

                            //get price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("old price: "+oldMarketPrice);                        
                            //wait 10 seconds
                            Thread.Sleep(5000);
                            //MessageBox.Show("new price object: " + newPriceObject);
                            //get another price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("new price: " + newMarketPrice);
                            //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            divisor = newMarketPrice - oldMarketPrice;
                            decimal slope11 = Decimal.Divide(divisor, 5);
                            //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //MessageBox.Show("slope at " + now.ToString() + " is " + slope);

                            //get price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("old price: "+oldMarketPrice);                        
                            //wait 10 seconds
                            Thread.Sleep(5000);
                            //MessageBox.Show("new price object: " + newPriceObject);
                            //get another price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("new price: " + newMarketPrice);
                            //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            divisor = newMarketPrice - oldMarketPrice;
                            decimal slope12 = Decimal.Divide(divisor, 5);
                            //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //MessageBox.Show("slope at " + now.ToString() + " is " + slope);




                            NewMarketPrice = newMarketPrice;
                            decimal divisor2 = slope1 + slope2 + slope3 + slope4 + slope5 + slope6
                                             + slope7 + slope8 + slope9 + slope10 + slope11 + slope12;
                            decimal avgSlope = Decimal.Divide(divisor2, 12);
                            //MessageBox.Show("avg Slope is: " + avgSlope);


                            now = DateTime.Now.TimeOfDay;
                            logFileLine = now + "|" + "avgSlope: "+avgSlope+ "\r";                            
                            File.AppendAllText(logFilePath, logFileLine);


                            if (avgSlope > 0 && OKtoBuy == true)
                            {
                                buyInEquity = equity;
                                oldEquity = equity;
                                //MessageBox.Show("Old Equity: "+oldEquity.ToString());
                                //MessageBox.Show("New Equity: " + equity.ToString());
                                getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                                newPriceObject = getLastTradeObject.Price;
                                newMarketPrice = decimal.Parse(newPriceObject.ToString());
                                
                                BuyPaperStock.BuyPaperStockMethod(stockListing, numberOfShares);
                                
                                OKtoBuy = false;
                                //Sold = false;
                                buyPrice = newMarketPrice;
                                newPriceObject = decimal.Zero;

                                decimal trueEquity = decimal.Add(equity, buyPrice);


                                now = DateTime.Now.TimeOfDay;
                                logFileLine = now + "|" + "avgSlope is positive: BUY " +"|"+"buyPrice: " +buyPrice.ToString()+"|"+"buyInEquity: "+buyInEquity.ToString() + "|" + "equity: " + trueEquity.ToString() + "|" + "oldEquity: " + oldEquity.ToString() + "\r";
                                File.AppendAllText(logFilePath, logFileLine);

                                while (Sold == false)//while we have not sold
                                {
                                    
                                    Thread.Sleep(1000);

                                    //MessageBox.Show(equity.ToString());
                                    //account = null;
                                    equity = decimal.Zero;
                                    //MessageBox.Show(equity.ToString());

                                    account = await client.GetAccountAsync();
                                    //longAmount = account.LongMarketValue;
                                    equity = account.TradableCash;// + buyPrice;
                                    //MessageBox.Show("Get New Equity: " + equity.ToString());

                                    trueEquity = decimal.Add(equity, buyPrice);

                                    now = DateTime.Now.TimeOfDay;
                                    logFileLine = now + "|" + "old equity amount: " + oldEquity + "|" + "new equity amount: " + trueEquity + "\r";
                                    File.AppendAllText(logFilePath, logFileLine);

                                    //incrementEquity = equity - buyInEquity;
                                    //oldEquity = oldEquity + incrementEquity;

                                    //MessageBox.Show(equity.ToString());

                                    


                                    if (oldEquity > trueEquity)// || buyInEquity > equity)
                                    {
                                        SellPaperStock.SellPaperStockMethod(stockListing);
                                        OKtoBuy = true;
                                        Sold = true;

                                        now = DateTime.Now.TimeOfDay;
                                        logFileLine = now + "|" + "old equity > new equity: SOLD" + "\r";
                                        File.AppendAllText(logFilePath, logFileLine);





                                    }



                                    else if (oldEquity < trueEquity)
                                    {

                                        while (oldEquity < trueEquity)
                                        {
                                            now = DateTime.Now.TimeOfDay;
                                            logFileLine = now + "|" + "old equity < new equity: STAY IN" + "\r";
                                            File.AppendAllText(logFilePath, logFileLine);

                                            oldEquity = equity;
                                            Thread.Sleep(1000);


                                            now = DateTime.Now.TimeOfDay;
                                            logFileLine = now + "|" + "Make old equity = new equity and STAY IN: oldEquity: " + oldEquity + "|" + "newEquity: " + equity + "\r";
                                            File.AppendAllText(logFilePath, logFileLine);

                                            //get new equity

                                            Thread.Sleep(9000);

                                            //MessageBox.Show(equity.ToString());
                                            //account = null;
                                            equity = decimal.Zero;
                                            //MessageBox.Show(equity.ToString());

                                            account = await client.GetAccountAsync();
                                            //longAmount = account.LongMarketValue;
                                            equity = account.TradableCash;// + buyPrice;
                                                                          //MessageBox.Show("Get New Equity: " + equity.ToString());

                                            trueEquity = decimal.Add(equity, buyPrice);

                                            now = DateTime.Now.TimeOfDay;
                                            logFileLine = now + "|" + "old equity amount: " + oldEquity + "|" + "new equity amount: " + trueEquity + "\r";
                                            File.AppendAllText(logFilePath, logFileLine);


                                        }

                                        now = DateTime.Now.TimeOfDay;
                                        logFileLine = now + "|" + "Get out of the top near the high point: SOLD" + "\r";
                                        File.AppendAllText(logFilePath, logFileLine);

                                        SellPaperStock.SellPaperStockMethod(stockListing);
                                        OKtoBuy = true;

                                        //MessageBox.Show("oldEquity is: "+oldEquity.ToString()+"\r\rnewEquity is: " + equity.ToString());
                                        //incrementEquity = equity - oldEquity;
                                        //oldEquity = oldEquity + incrementEquity;
                                        //while (oldEquity < equity)
                                        //{



                                        //    Thread.Sleep(5000);

                                        //    MessageBox.Show(equity.ToString());
                                        //    account = null;
                                        //    equity = decimal.Zero;
                                        //    MessageBox.Show(equity.ToString());

                                        //    account = await client.GetAccountAsync();
                                        //    longAmount = account.LongMarketValue;
                                        //    equity = account.TradableCash;// + buyPrice;
                                        //    MessageBox.Show("Get New Equity: " + equity.ToString());
                                        //    MessageBox.Show(now.ToString() + "\r\rAfter Equity changed: \r\roldEquity is: " + oldEquity.ToString() + "\r\rnewEquity is: " + equity.ToString());


                                        //    if (oldEquity < equity)// || buyInEquity > equity)
                                        //    {
                                        //        SellPaperStock.SellPaperStockMethod(stockListing);
                                        //        OKtoBuy = true;
                                        //        Sold = true;
                                        //    }

                                        //}

                                    }
                                    //oldEquity = equity;
                                }
                                //write equity to file
                                decimal actualAmount = equity + buyPrice;

                                now = DateTime.Now.TimeOfDay;
                                logFileLine = now + "|" + "equity + buyPrice: " + trueEquity + "\r";
                                File.AppendAllText(logFilePath, logFileLine);

                                File.WriteAllText(CurrentEquityFile, equity.ToString());

                                //write cash made to file
                                Thread.Sleep(250);
                                cashMadeToday = trueEquity - buyInEquity;

                                File.WriteAllText(CurrentCashMade, cashMadeToday.ToString());

                                //equity = decimal.Zero;
                                //oldEquity = decimal.Zero;

                            }
                            else
                            {
                                Thread.Sleep(1000);

                                //MessageBox.Show(equity.ToString());
                                //account = null;
                                equity = decimal.Zero;
                                //MessageBox.Show(equity.ToString());

                                account = await client.GetAccountAsync();
                                //longAmount = account.LongMarketValue;
                                equity = account.TradableCash;// + buyPrice;
                                                              //MessageBox.Show("Get New Equity: " + equity.ToString());

                                decimal trueEquity = decimal.Add(equity, buyPrice);


                                now = DateTime.Now.TimeOfDay;
                                logFileLine = now + "|" + "old equity amount: " + oldEquity + "|" + "new equity amount: " + equity+buyPrice + "\r";
                                File.AppendAllText(logFilePath, logFileLine);

                                if (oldEquity > trueEquity)
                                {
                                    SellPaperStock.SellPaperStockMethod(stockListing);
                                    OKtoBuy = true;

                                    now = DateTime.Now.TimeOfDay;
                                    logFileLine = now + "|" + "Sell on a high: SOLD" + "\r";
                                    File.AppendAllText(logFilePath, logFileLine);
                                }
                                else if(oldEquity < trueEquity)
                                {

                                    oldEquity = trueEquity;

                                    now = DateTime.Now.TimeOfDay;
                                    logFileLine = now + "|" + "STAY IN" + "\r";
                                    File.AppendAllText(logFilePath, logFileLine);
                                    ////do nothing
                                }

                            }
                            
                        }
                        //Thread.Sleep(150000);
                        //now = DateTime.Now.TimeOfDay;
                        //logFileLine = now + "|" + "Startover and wait for the next 5 minute interval: oldEquity: " + oldEquity + "|" + "newEquity: " + equity + "\r";
                        //File.AppendAllText(logFilePath, logFileLine);
                    }

                    now = DateTime.Now.TimeOfDay;
                    logFileLine = now + "|" + "End of Day: SELL EVERYTHING" + "\r";
                    File.AppendAllText(logFilePath, logFileLine);

                    SellPaperStock.SellPaperStockMethod(stockListing);
                    OKtoBuy = true;
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show("Could not access Alpaca Network through pluggable protocol.\r\r" + ex, "WebException", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (HttpListenerException ex)
            {
                MessageBox.Show("Could not access Alpaca Network through pluggable protocol.\r\r" + ex, "HttpListenerException", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Alpaca.Markets.RestClientErrorException ex)
            {
                //MessageBox.Show("Something went wrong with the BuyAndSellPaperMethod() because: \r\r" + ex, "General Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                //SellPaperStock.SellPaperStockMethod(stockListing);

            }
        }

        
    }
}

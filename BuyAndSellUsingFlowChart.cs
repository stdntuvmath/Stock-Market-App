using System;
using System.Threading.Tasks;
using Alpaca.Markets;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using Stock_Paper_Trading_App;
using System.IO;
using System.Collections.Generic;
using System.Linq;


namespace Stock_Trading_App
{
    internal static class BuyAndSellUsingFlowChart
    {
        
        private static string API_KEY = "PKHNXT0M3ZBTEKBEH1LM";
        private static string API_SECRET_KEY = "3EjNkXEQt7s4WceVGdmPslJ6oYo65uIzfHE09yPS";
        private static string windowsUserName = System.Environment.UserName;//gives windows username
        private static string CurrentEquityFile = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\CurrentEquity.txt";
        private static string CurrentCashMade = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\CurrentCashMade.txt";
        private static string InitialEquityFile = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\InitialEquityFile.txt";


        private static string logFileLine;

        private static bool OKtoBuy = true;
        private static bool FindOldEquity = true;
        private static decimal buyPrice;
        private static decimal oldEquity;
        private static decimal differenceBetweenEquities;
        private static decimal maxDifference;

        private static List<Decimal> differenceBetweenEquitiesArray = new List<decimal>();

        private static decimal buyInEquity;
        private static decimal NewMarketPrice;

        private static List<Decimal> slopeArray = new List<decimal>();
        private static List<Decimal> AVGSlopeArray = new List<decimal>();


        public static async Task BuyAndSellPapeUsingFlowChartMethod(string stockListing, int numberOfShares)
        {
            try
            {
                //ready the getLastTrade web call (DataClient)

                var clientData = Alpaca.Markets.Environments.Paper.GetAlpacaDataClient(new SecretKey(API_KEY, API_SECRET_KEY));


                //ready the getLastEquity web call (TradingClient)
                var client = Alpaca.Markets.Environments.Paper.GetAlpacaTradingClient(new SecretKey(API_KEY, API_SECRET_KEY));

                // Get our account information.
                var account = await client.GetAccountAsync();

                

                decimal initialEquity = account.Equity;// the equity when everything is sold.
                
                decimal oldEquity = 0;
                decimal equity=0;
                decimal buyInEquity = 0;
                decimal incrementEquity = 0;
                decimal cashMadeToday = 0;
                decimal oldEquityOffset = 0;

                string[] logFileArray = { };

                //stock market hours
                TimeSpan start = new TimeSpan(8, 29, 0); //8:30 AM
                TimeSpan end = new TimeSpan(14, 59, 0); //3 o'clock military time
                TimeSpan now = DateTime.Now.TimeOfDay;
                DayOfWeek theDayToday = DateTime.Today.DayOfWeek;
                string today = theDayToday.ToString();

                DateTime todaysDateOnly = DateTime.Today;
                string todaysDate = todaysDateOnly.ToString("yyyyMMdd_");

                string logFilePath = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\Logs\"+ todaysDate + @"log.txt";

                

                if ((start > now) || (now >= end) || today == "Saturday" || today == "Sunday")
                {

                   
                    MessageBox.Show("The Stock Market is closed. Please try again Monday thru Friday 8:30 AM to 3:00 PM CST.", "The Stock Market is Closed",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    now = DateTime.Now.TimeOfDay;
                    logFileLine = "************************\r" + now + "|" + "Attempted access: The Stock Market is closed. Please try again Monday thru Friday 8:30 AM to 3:00 PM CST.\r************************\r\r";
                    File.AppendAllText(logFilePath, logFileLine);

                    File.WriteAllText(CurrentEquityFile, initialEquity.ToString());
                }
                else
                {
                    //while within stock market hours

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
                        Thread.Sleep(5000);
                        //MessageBox.Show("new price object: " + newPriceObject);
                        //get another price from Alpaca and convert to decimal number
                        getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                        newPriceObject = getLastTradeObject.Price;
                        decimal newMarketPrice = decimal.Parse(newPriceObject.ToString());
                        newPriceObject = decimal.Zero;
                        //MessageBox.Show("new price: " + newMarketPrice);
                        //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                        decimal divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                        decimal slope1 = Decimal.Divide(divisor, 5);
                        slopeArray.Add(slope1);
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
                        divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                        decimal slope2 = Decimal.Divide(divisor, 5);
                        slopeArray.Add(slope2);

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
                        divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                        decimal slope3 = Decimal.Divide(divisor, 5);
                        slopeArray.Add(slope3);


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
                        divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                        decimal slope4 = Decimal.Divide(divisor, 5);
                        slopeArray.Add(slope4);

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
                        divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                        decimal slope5 = Decimal.Divide(divisor, 5);
                        slopeArray.Add(slope5);

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
                        divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                        decimal slope6 = Decimal.Divide(divisor, 5);
                        slopeArray.Add(slope6);

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
                        divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                        decimal slope7 = Decimal.Divide(divisor, 5);
                        slopeArray.Add(slope7);

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
                        divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                        decimal slope8 = Decimal.Divide(divisor, 5);
                        slopeArray.Add(slope8);

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
                        divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                        decimal slope9 = Decimal.Divide(divisor, 5);
                        slopeArray.Add(slope9);

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
                        divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                        decimal slope10 = Decimal.Divide(divisor, 5);
                        slopeArray.Add(slope10);

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
                        divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                        decimal slope11 = Decimal.Divide(divisor, 5);
                        slopeArray.Add(slope11);

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
                        divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                        decimal slope12 = Decimal.Divide(divisor, 5);
                        slopeArray.Add(slope12);

                        //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                        //MessageBox.Show("slope at " + now.ToString() + " is " + slope);




                        //calcualte avg slope

                        NewMarketPrice = newMarketPrice;

                        

                        decimal sumOfAllSlopes = slopeArray.Sum();

                        

                        

                        decimal avgSlope = Decimal.Divide(sumOfAllSlopes, 12);
                        //MessageBox.Show("avg Slope is: " + avgSlope);



                        now = DateTime.Now.TimeOfDay;
                        logFileLine = now + "|" + "avgSlope: " + avgSlope + "\r\r\r";
                        File.AppendAllText(logFilePath, logFileLine);

                        AVGSlopeArray.Add(avgSlope);



                        if (avgSlope > 0  && OKtoBuy == true)//this is where we buy
                        {
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            buyPrice = newMarketPrice;
                            //oldEquity = initialEquity;
                            //decimal trueEquity = decimal.Add(equity, buyPrice);
                            buyInEquity = account.Equity;

                            BuyPaperStock.BuyPaperStockMethod(stockListing, numberOfShares);

                            OKtoBuy = false;


                            if (FindOldEquity == true)//99.35
                            {
                                oldEquity = decimal.Zero;
                                account = await client.GetAccountAsync();
                                oldEquity = account.Equity;
                            }


                            Thread.Sleep(10000);


                            equity = decimal.Zero;
                            account = await client.GetAccountAsync();
                            equity = account.Equity;//99.10

                            now = DateTime.Now.TimeOfDay;
                            logFileLine = "************************\r" + now + "|" + "AUTOBUY " + "\r" + "buyPrice: " + buyPrice.ToString() + "\r" + "oldEquity: " + oldEquity.ToString() + "\rnew equity: " + equity.ToString() + "\r************************\r\r";
                            File.AppendAllText(logFilePath, logFileLine);

                            //get the last four digits of both oldEquity and equity

                            string oldEquityString = oldEquity.ToString();
                            char[] oldEquityCharArray = oldEquityString.ToCharArray();
                            oldEquityString = oldEquityString.Substring(oldEquityString.Length - 4, oldEquityString.Length);
                            MessageBox.Show(oldEquityString);

                            decimal oldEquityLastFourDigits = Convert.ToDecimal(oldEquityString);

                            string equityString = equity.ToString();
                            char[] equityCharArray = equityString.ToCharArray();
                            equityString = equityString.Substring(equityString.Length - 4, equityString.Length);
                            MessageBox.Show(equityString);

                            decimal equityLastFourDigits = Convert.ToDecimal(equityString);

                            decimal PercentOfoldEquity = Decimal.Multiply(oldEquityLastFourDigits, 0.02M);//2% of oldEquity
                            decimal oldEquityLastFourDigitsMinusPercentOfoldEquity = Decimal.Subtract(oldEquityLastFourDigits, PercentOfoldEquity);

                            if (equity > oldEquity)//goes high 99.35 > 99.33
                            {
                                oldEquity = equity;//99.33=99.33
                                FindOldEquity = false;
                                logFileLine = now + "|\r" + "*******************************STAY IN\r\roldEquity: " + oldEquity + "\rnewEquity: " + equity + "\r************************************\r\r";
                                File.AppendAllText(logFilePath, logFileLine);
                            }
                            else if ((equity < oldEquity && equityLastFourDigits <= oldEquityLastFourDigitsMinusPercentOfoldEquity) || AVGSlopeArray[AVGSlopeArray.Count - 2] < AVGSlopeArray[AVGSlopeArray.Count - 1] && AVGSlopeArray[AVGSlopeArray.Count - 1] < AVGSlopeArray[AVGSlopeArray.Count])
                            {
                                SellPaperStock.SellPaperStockMethod(stockListing);
                                OKtoBuy = true;
                                FindOldEquity = true;

                                now = DateTime.Now.TimeOfDay;
                                logFileLine = "************************\r" + now + "|" + "AUTOSELL " + "\r" + "equityLastFourDigits: " + equityLastFourDigits.ToString() + "\r" + "oldEquityLastFourDigits: " + oldEquityLastFourDigits.ToString() + "\roldEquity: " + oldEquity + "\rnewEquity: " + equity + "\r ************************\r\r";
                                File.AppendAllText(logFilePath, logFileLine);
                            }


                            //while (OKtoBuy == false)//while in the buy
                            //{
                            //    /**1. We could analyze the slope and try to predict when it is low so that we can 
                            //     * avoid the low slopes. If we do this, it should be another program that predicts 
                            //     * the slope and tells this program when to stay out.
                            //     * 
                            //     * 2. We could ask Alpaca if we can get a more precise value of our account.Equity
                            //     * value. And then see if the below module will get out more quickly, with the more
                            //     * precise value, when equity is goes low.
                            //     * 
                            //     * 3. We could predict when the slope will change with linear regression.
                            //     * 
                            //     * 4. We could predict when the next positive run would be (5 minute or longer positive run)
                            //     *    by analyzing all the positive runs within a certain period of time.Yrthsa12@@ 
                            //     * 
                            //     * 5. We could use the difference between the new equity and old equity**/


                            //    if (FindOldEquity == true)//99.35
                            //    {
                            //        oldEquity = decimal.Zero;
                            //        account = await client.GetAccountAsync();
                            //        oldEquity = account.Equity;
                            //    }
                                

                            //    Thread.Sleep(10000);


                            //    equity = decimal.Zero;
                            //    account = await client.GetAccountAsync();
                            //    equity = account.Equity;//99.10

                            //    now = DateTime.Now.TimeOfDay;
                            //    logFileLine = "************************\r" + now + "|" + "AUTOBUY " + "\r" + "buyPrice: " + buyPrice.ToString() + "\r" + "oldEquity: " + oldEquity.ToString() + "\rnew equity: " + equity.ToString() + "\r************************\r\r";
                            //    File.AppendAllText(logFilePath, logFileLine);

                            //    //get the last four digits of both oldEquity and equity

                            //    string oldEquityString = oldEquity.ToString();
                            //    char[] oldEquityCharArray = oldEquityString.ToCharArray();
                            //    oldEquityString = oldEquityString.Substring(oldEquityString.Length - 4, oldEquityString.Length);
                            //    MessageBox.Show(oldEquityString);

                            //    decimal oldEquityLastFourDigits = Convert.ToDecimal(oldEquityString);

                            //    string equityString = equity.ToString();
                            //    char[] equityCharArray = equityString.ToCharArray();
                            //    equityString = equityString.Substring(equityString.Length - 4, equityString.Length);
                            //    MessageBox.Show(equityString);

                            //    decimal equityLastFourDigits = Convert.ToDecimal(equityString);

                            //    decimal PercentOfoldEquity = Decimal.Multiply(oldEquityLastFourDigits, 0.02M);//2% of oldEquity
                            //    decimal oldEquityLastFourDigitsMinusPercentOfoldEquity = Decimal.Subtract(oldEquityLastFourDigits, PercentOfoldEquity);

                            //    if (equity > oldEquity)//goes high 99.35 > 99.33
                            //    {
                            //        oldEquity = equity;//99.33=99.33
                            //        FindOldEquity = false;
                            //        logFileLine = now + "|\r" + "*******************************STAY IN\r\roldEquity: " + oldEquity + "\rnewEquity: " + equity + "\r************************************\r\r";
                            //        File.AppendAllText(logFilePath, logFileLine);
                            //    }
                            //    else if ((equity < oldEquity && equityLastFourDigits <= oldEquityLastFourDigitsMinusPercentOfoldEquity)|| AVGSlopeArray[AVGSlopeArray.Count - 2] < AVGSlopeArray[AVGSlopeArray.Count - 1] && AVGSlopeArray[AVGSlopeArray.Count - 1] < AVGSlopeArray[AVGSlopeArray.Count])
                            //    {
                            //        SellPaperStock.SellPaperStockMethod(stockListing);
                            //        OKtoBuy = true;
                            //        FindOldEquity = true;

                            //        now = DateTime.Now.TimeOfDay;
                            //        logFileLine = "************************\r" + now + "|" + "AUTOSELL " + "\r" + "equityLastFourDigits: " + equityLastFourDigits.ToString() + "\r" + "oldEquityLastFourDigits: " + oldEquityLastFourDigits.ToString() + "\roldEquity: " + oldEquity + "\rnewEquity: " + equity + "\r ************************\r\r";
                            //        File.AppendAllText(logFilePath, logFileLine);
                            //    }



                            //    ////get slope
                            //    ////get price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("old price: "+oldMarketPrice);                        
                            //    ////wait 10 seconds
                            //    //Thread.Sleep(5000);
                            //    ////MessageBox.Show("new price object: " + newPriceObject);
                            //    ////get another price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("new price: " + newMarketPrice);
                            //    ////slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            //    //divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                            //    //slope1 = Decimal.Divide(divisor, 5);
                            //    //slopeArray.Add(slope1);
                            //    ////MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //    ////MessageBox.Show("slope at " + now.ToString() + " is " + slope);





                            //    ////get price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("old price: "+oldMarketPrice);                        
                            //    ////wait 10 seconds
                            //    //Thread.Sleep(5000);
                            //    ////MessageBox.Show("new price object: " + newPriceObject);
                            //    ////get another price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("new price: " + newMarketPrice);
                            //    ////slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            //    //divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                            //    //slope2 = Decimal.Divide(divisor, 5);
                            //    //slopeArray.Add(slope2);

                            //    ////MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //    ////MessageBox.Show("slope at " + now.ToString() + " is " + slope);




                            //    ////get price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("old price: "+oldMarketPrice);                        
                            //    ////wait 10 seconds
                            //    //Thread.Sleep(5000);
                            //    ////MessageBox.Show("new price object: " + newPriceObject);
                            //    ////get another price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("new price: " + newMarketPrice);
                            //    ////slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            //    //divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                            //    //slope3 = Decimal.Divide(divisor, 5);
                            //    //slopeArray.Add(slope3);


                            //    ////MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //    ////MessageBox.Show("slope at " + now.ToString() + " is " + slope);


                            //    ////get price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("old price: "+oldMarketPrice);                        
                            //    ////wait 10 seconds
                            //    //Thread.Sleep(5000);
                            //    ////MessageBox.Show("new price object: " + newPriceObject);
                            //    ////get another price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("new price: " + newMarketPrice);
                            //    ////slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            //    //divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                            //    //slope4 = Decimal.Divide(divisor, 5);
                            //    //slopeArray.Add(slope4);

                            //    ////MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //    ////MessageBox.Show("slope at " + now.ToString() + " is " + slope);


                            //    ////get price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("old price: "+oldMarketPrice);                        
                            //    ////wait 10 seconds
                            //    //Thread.Sleep(5000);
                            //    ////MessageBox.Show("new price object: " + newPriceObject);
                            //    ////get another price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("new price: " + newMarketPrice);
                            //    ////slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            //    //divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                            //    //slope5 = Decimal.Divide(divisor, 5);
                            //    //slopeArray.Add(slope5);

                            //    ////MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //    ////MessageBox.Show("slope at " + now.ToString() + " is " + slope);


                            //    ////get price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("old price: "+oldMarketPrice);                        
                            //    ////wait 10 seconds
                            //    //Thread.Sleep(5000);
                            //    ////MessageBox.Show("new price object: " + newPriceObject);
                            //    ////get another price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("new price: " + newMarketPrice);
                            //    ////slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            //    //divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                            //    //slope6 = Decimal.Divide(divisor, 5);
                            //    //slopeArray.Add(slope6);

                            //    ////MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //    ////MessageBox.Show("slope at " + now.ToString() + " is " + slope);



                            //    ////get price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("old price: "+oldMarketPrice);                        
                            //    ////wait 10 seconds
                            //    //Thread.Sleep(5000);
                            //    ////MessageBox.Show("new price object: " + newPriceObject);
                            //    ////get another price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("new price: " + newMarketPrice);
                            //    ////slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            //    //divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                            //    //slope7 = Decimal.Divide(divisor, 5);
                            //    //slopeArray.Add(slope7);

                            //    ////MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //    ////MessageBox.Show("slope at " + now.ToString() + " is " + slope);

                            //    ////get price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("old price: "+oldMarketPrice);                        
                            //    ////wait 10 seconds
                            //    //Thread.Sleep(5000);
                            //    ////MessageBox.Show("new price object: " + newPriceObject);
                            //    ////get another price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("new price: " + newMarketPrice);
                            //    ////slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            //    //divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                            //    //slope8 = Decimal.Divide(divisor, 5);
                            //    //slopeArray.Add(slope8);

                            //    ////MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //    ////MessageBox.Show("slope at " + now.ToString() + " is " + slope);

                            //    ////get price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("old price: "+oldMarketPrice);                        
                            //    ////wait 10 seconds
                            //    //Thread.Sleep(5000);
                            //    ////MessageBox.Show("new price object: " + newPriceObject);
                            //    ////get another price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("new price: " + newMarketPrice);
                            //    ////slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            //    //divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                            //    //slope9 = Decimal.Divide(divisor, 5);
                            //    //slopeArray.Add(slope9);

                            //    ////MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //    ////MessageBox.Show("slope at " + now.ToString() + " is " + slope);

                            //    ////get price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("old price: "+oldMarketPrice);                        
                            //    ////wait 10 seconds
                            //    //Thread.Sleep(5000);
                            //    ////MessageBox.Show("new price object: " + newPriceObject);
                            //    ////get another price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("new price: " + newMarketPrice);
                            //    ////slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            //    //divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                            //    //slope10 = Decimal.Divide(divisor, 5);
                            //    //slopeArray.Add(slope10);

                            //    ////MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //    ////MessageBox.Show("slope at " + now.ToString() + " is " + slope);

                            //    ////get price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("old price: "+oldMarketPrice);                        
                            //    ////wait 10 seconds
                            //    //Thread.Sleep(5000);
                            //    ////MessageBox.Show("new price object: " + newPriceObject);
                            //    ////get another price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("new price: " + newMarketPrice);
                            //    ////slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            //    //divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                            //    //slope11 = Decimal.Divide(divisor, 5);
                            //    //slopeArray.Add(slope11);

                            //    ////MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //    ////MessageBox.Show("slope at " + now.ToString() + " is " + slope);

                            //    ////get price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("old price: "+oldMarketPrice);                        
                            //    ////wait 10 seconds
                            //    //Thread.Sleep(5000);
                            //    ////MessageBox.Show("new price object: " + newPriceObject);
                            //    ////get another price from Alpaca and convert to decimal number
                            //    //getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            //    //newPriceObject = getLastTradeObject.Price;
                            //    //newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            //    //newPriceObject = decimal.Zero;
                            //    ////MessageBox.Show("new price: " + newMarketPrice);
                            //    ////slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            //    //divisor = Decimal.Subtract(newMarketPrice, oldMarketPrice);
                            //    //slope12 = Decimal.Divide(divisor, 5);
                            //    //slopeArray.Add(slope12);

                            //    ////MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //    ////MessageBox.Show("slope at " + now.ToString() + " is " + slope);




                            //    ////calcualte avg slope

                            //    //NewMarketPrice = newMarketPrice;



                            //    //sumOfAllSlopes = slopeArray.Sum();





                            //    //avgSlope = Decimal.Divide(sumOfAllSlopes, 12);
                            //    ////MessageBox.Show("avg Slope is: " + avgSlope);



                            //    //now = DateTime.Now.TimeOfDay;
                            //    //logFileLine = now + "|" + "avgSlope: " + avgSlope + "\r\r\r";
                            //    //File.AppendAllText(logFilePath, logFileLine);

                            //    //AVGSlopeArray.Add(avgSlope);

                            //    //now = DateTime.Now.TimeOfDay;
                            //    //logFileLine = now + "|" + "Slope is Negative: STAY OUT" + "\r";
                            //    //File.AppendAllText(logFilePath, logFileLine);





                            //}
                            //write internal data to files
                            File.WriteAllText(CurrentEquityFile, equity.ToString());

                            //write cash made to file
                            Thread.Sleep(250);
                            cashMadeToday = equity - initialEquity;

                            File.WriteAllText(CurrentCashMade, cashMadeToday.ToString());

                            //oldEquity = equity;
                        }
                        else if (avgSlope < 0 && OKtoBuy == true)//if avgSlope is low and we haven't bought yet
                        {
                            now = DateTime.Now.TimeOfDay;
                            logFileLine = now + "|" + "Slope is Negative: STAY OUT" + "\r";
                            File.AppendAllText(logFilePath, logFileLine);
                        }
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

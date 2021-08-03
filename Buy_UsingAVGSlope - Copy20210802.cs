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
    internal static class Buy_UsingAVGSlope
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
        private static decimal sellPrice;

        private static decimal oldEquity;
        private static decimal differenceBetweenEquities;
        private static decimal differenceBetweenPrices;

        private static decimal maxDifference;
        private static decimal secondToLastAVGSlope;


        private static List<Decimal> differenceBetweenEquitiesArray = new List<decimal>();

        private static decimal buyInEquity;
        private static decimal NewMarketPrice;
        private static decimal maxEquity;

        private static TimeSpan now = DateTime.Now.TimeOfDay;
        private static DateTime todaysDateOnly = DateTime.Today;

        private static string todaysDate = todaysDateOnly.ToString("yyyyMMdd_");


        private static List<Decimal> slopeArray = new List<decimal>();
        private static List<Decimal> AVGSlopeArray = new List<decimal>();
        private static List<Decimal> equityArray = new List<decimal>();


        public static async Task Buy_UsingAVGSlopeMethod(string stockListing, int numberOfShares)
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
                decimal equity = 0;
                decimal buyInEquity = 0;
                decimal incrementEquity = 0;
                decimal cashMadeToday = 0;
                decimal oldEquityOffset = 0;

                string[] logFileArray = { };

                //stock market hours
                TimeSpan start = new TimeSpan(8, 29, 0); //8:30 AM
                TimeSpan end = new TimeSpan(14, 59, 0); //3 o'clock military time
                
                DayOfWeek theDayToday = DateTime.Today.DayOfWeek;
                string today = theDayToday.ToString();


                string logFilePath = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\Logs\" + todaysDate + @"log.txt";



                if ((start > now) || (now >= end) || today == "Saturday" || today == "Sunday")//In production, these should have OR's "||" not AND's "&&"
                {


                    MessageBox.Show("The Stock Market is closed. Please try again Monday thru Friday 8:30 AM to 3:00 PM CST.", "The Stock Market is Closed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    now = DateTime.Now.TimeOfDay;
                    logFileLine = "************************\r" + now + "|" + "Attempted access: The Stock Market is closed. Please try again Monday thru Friday 8:30 AM to 3:00 PM CST.\r************************\r\r";
                    File.AppendAllText(logFilePath, logFileLine);

                    //File.WriteAllText(CurrentEquityFile, initialEquity.ToString());

                    Application.Exit();
                }
                else
                {
                    //while within stock market hours

                    while ((start <= now) && (now < end) && today != "Saturday" && today != "Sunday")//In production, these should have AND's "&&" not OR's "||"
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



                        //calcualte avg slope

                        //NewMarketPrice = newMarketPrice;



                        decimal sumOfAllSlopes = slopeArray.Sum();





                        decimal avgSlope = Decimal.Divide(sumOfAllSlopes, 3);
                        //MessageBox.Show("avg Slope is: " + avgSlope);



                        now = DateTime.Now.TimeOfDay;
                        logFileLine = now + "|" + "avgSlope: " + avgSlope + "\r\r\r";
                        File.AppendAllText(logFilePath, logFileLine);

                        AVGSlopeArray.Add(avgSlope);

                        //MessageBox.Show(AVGSlopeArray.Count.ToString());

                        decimal lastAVGSlope = AVGSlopeArray[AVGSlopeArray.Count-1];

                        equity = decimal.Zero;
                        account = await client.GetAccountAsync();
                        equity = account.Equity;
                        //differenceBetweenEquities = decimal.Subtract(equity, buyInEquity);
                        
                        //store equity into array
                        equityArray.Add(equity);
                        maxEquity = equityArray.Max();

                        //differenceBetweenPrices = decimal.Subtract(newMarketPrice, buyPrice);

                        if (!(AVGSlopeArray.Count - 1.00M <=0))
                        {
                            secondToLastAVGSlope = AVGSlopeArray[AVGSlopeArray.Count - 2];

                        }

                        now = DateTime.Now.TimeOfDay;
                        logFileLine = now + "|" + "lastAVGSlope: " + lastAVGSlope+"\rsecondToLastAVGSlope: "+ secondToLastAVGSlope+"\r\r";
                        File.AppendAllText(logFilePath, logFileLine);

                        if (avgSlope == decimal.Zero)
                        {
                            //start at top of while loop
                            //MessageBox.Show("avgSlope = 0");
                            //return;

                        }
                        //else if ((differenceBetweenPrices > decimal.Zero) || (differenceBetweenEquities > decimal.Zero) || (equity <= maxEquity - 0.35M) && OKtoBuy == false)//this one didn't work as good 

                        else if ((secondToLastAVGSlope < lastAVGSlope && equity > buyInEquity || equity <= maxEquity - 1.00M) && OKtoBuy == false)//this one worked the best for selling - please don't delete!!
                        //else if (((equity > buyInEquity) || (newMarketPrice > buyPrice) || (equity <= maxEquity - 0.35M)) && OKtoBuy == false)// I tried to fix the issue where it buys and sells on the same price according to alpaca.This one didn't work as good

                        {
                                    //equity = decimal.Zero;
                                    //account = await client.GetAccountAsync();
                                    //equity = account.Equity;


                                    getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            
                            newPriceObject = getLastTradeObject.Price;
                            
                            newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            sellPrice = newMarketPrice;
                            //oldEquity = initialEquity;
                            //decimal trueEquity = decimal.Add(equity, buyPrice);
                            

                            SellPaperStock.SellPaperStockMethod(stockListing);
                            OKtoBuy = true;

                            now = DateTime.Now.TimeOfDay;
                            logFileLine = "************************\r" + now + "|" + "AUTOSELL " +
                                            "\r" + "buyPrice: " + buyPrice.ToString() +
                                            "\r" + "sellPrice: " + sellPrice.ToString() +
                                            "\r" + "buyInEquity: " + buyInEquity.ToString() +
                                            "\r" + "sellOutEquity: " + equity.ToString() + "\r************************\r\r";
                            File.AppendAllText(logFilePath, logFileLine);

                            equityArray.Clear();
                            maxEquity = decimal.Zero;
                        }
                        else if ((secondToLastAVGSlope > lastAVGSlope) && OKtoBuy == true)//this is where we buy - 
                        {
                            equity = decimal.Zero;
                            account = await client.GetAccountAsync();
                            buyInEquity = account.Equity;

                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            buyPrice = newMarketPrice;


                            BuyPaperStock.BuyPaperStockMethod(stockListing, numberOfShares);

                            OKtoBuy = false;

                            
                            now = DateTime.Now.TimeOfDay;
                            logFileLine = "************************\r" + now + "|" + "AUTOBUY " + 
                                            "\r" + "buyPrice: " + buyPrice.ToString() + 
                                            "\r" + "buyInEquity: " + buyInEquity.ToString() +
                                            "\r" + "secondToLastAVGSlope > lastAVGSlope:\r" + secondToLastAVGSlope.ToString() +"\r"+ lastAVGSlope.ToString()+

                                            "\r************************\r\r";
                            File.AppendAllText(logFilePath, logFileLine);


                            

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
                    SellPaperStock.SellPaperStockMethod(stockListing);
                    Thread.Sleep(1000);
                    //OKtoBuy = true;
                    Application.Exit();
                }
            }
            catch (WebException ex)
            {
                string logErrorPath = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\Errors\" + todaysDate + @"errorLog.txt";


                now = DateTime.Now.TimeOfDay;
                logFileLine = now + "|" + "Web Exception Error - is thrown when an error occurs while accessing the network through a pluggable protocol." + "\r" + ex.ToString();
                File.AppendAllText(logErrorPath, logFileLine);
            }
            catch (HttpListenerException ex)
            {
                string logErrorPath = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\Errors\" + todaysDate + @"errorLog.txt";


                now = DateTime.Now.TimeOfDay;
                logFileLine = now + "|" + "Http Listener Exception - is thrown when an error occurs processing an HTTP request" + "\r" + ex.ToString();
                File.AppendAllText(logErrorPath, logFileLine);
            }
            catch (Alpaca.Markets.RestClientErrorException ex)
            {
                string logErrorPath = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\Errors\" + todaysDate + @"errorLog.txt";


                now = DateTime.Now.TimeOfDay;
                logFileLine = now + "|" + "Alpaca.Markets.RestClientErrorException - Represents Alpaca/Polygon Rest API specific error information" + "\r" + ex.ToString();
                File.AppendAllText(logErrorPath, logFileLine);
            }
            catch (Exception ex)
            {

                string logErrorPath = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\Errors\" + todaysDate + @"errorLog.txt";


                now = DateTime.Now.TimeOfDay;
                logFileLine = now + "|" + "General Exception - is thrown for errors that occur during application execution. See the Reference Source for more details." + "\r"+ex.ToString();
                File.AppendAllText(logErrorPath, logFileLine);


                //MessageBox.Show("Something broke somewheres.\r\r" + ex, "General Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //SellPaperStock.SellPaperStockMethod(stockListing);
                //Application.Exit();

            }
        }
    }
}

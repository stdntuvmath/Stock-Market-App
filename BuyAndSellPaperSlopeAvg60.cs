using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Alpaca.Markets;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using Stock_Paper_Trading_App;

namespace Stock_Trading_App
{
    internal static class BuyAndSellPaperSlopeAvg60
    {
        private static string API_KEY = "removed";
        private static string API_SECRET_KEY = "removed";

        private static bool ToggleCancel = false;
        private static bool OKtoBuy = true;
        private static bool OKtoSell = false;
        private static decimal buyPrice;
        private static decimal NewMarketPrice = 0;


        public static async Task BuyAndSellPaperSlopeAvg60Method(string stockListing, int numberOfShares)
        {
            try
            {
                //ready the getLastTrade web call

                var clientData = Alpaca.Markets.Environments.Paper.GetAlpacaDataClient(new SecretKey(API_KEY, API_SECRET_KEY));



                //while within stock market hours
                TimeSpan start = new TimeSpan(8, 30, 0); //8:30 AM
                TimeSpan end = new TimeSpan(14, 59, 0); //3 o'clock military time
                TimeSpan now = DateTime.Now.TimeOfDay;
                DayOfWeek theDayToday = DateTime.Today.DayOfWeek;
                string today = theDayToday.ToString();


                if ((start > now) || (now >= end) || today == "Saturday" || today == "Sunday")
                {
                    MessageBox.Show("Stock Market is closed. Please try again tomorrow.");
                }
                else
                {
                    while ((start <= now) && (now < end) && today != "Saturday" && today != "Sunday")
                    {
                        //MessageBox.Show(now.Minutes.ToString());
                        //MessageBox.Show(now.Seconds.ToString());
                        now = DateTime.Now.TimeOfDay;

                        if ( (now.Minutes == 00 && now.Seconds == 0) || (now.Minutes == 05 && now.Seconds == 0)||
                             (now.Minutes == 10 && now.Seconds == 0) || (now.Minutes == 15 && now.Seconds == 0)||
                             (now.Minutes == 20 && now.Seconds == 0) || (now.Minutes == 25 && now.Seconds == 0)||
                             (now.Minutes == 30 && now.Seconds == 0) || (now.Minutes == 35 && now.Seconds == 0)||
                             (now.Minutes == 40 && now.Seconds == 0) || (now.Minutes == 45 && now.Seconds == 0)||
                             (now.Minutes == 50 && now.Seconds == 0) || (now.Minutes == 55 && now.Seconds == 0)
                             )//every 5 minutes synced to the clock
                        {

                            if ((NewMarketPrice > buyPrice + buyPrice * 0.0001M)|| (NewMarketPrice < buyPrice - buyPrice * 0.0001M))
                            {
                                SellPaperStock.SellPaperStockMethod(stockListing);
                                OKtoBuy = true;

                            }

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
                            decimal divisor = newMarketPrice - oldMarketPrice;
                            decimal slope1 = Decimal.Divide(divisor, 10);
                            //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //MessageBox.Show("slope at " + now.ToString() + " is " + slope);





                            //get price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("old price: "+oldMarketPrice);                        
                            //wait 10 seconds
                            Thread.Sleep(10000);
                            //MessageBox.Show("new price object: " + newPriceObject);
                            //get another price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("new price: " + newMarketPrice);
                            //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            divisor = newMarketPrice - oldMarketPrice;
                            decimal slope2 = Decimal.Divide(divisor, 10);
                            //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //MessageBox.Show("slope at " + now.ToString() + " is " + slope);




                            //get price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("old price: "+oldMarketPrice);                        
                            //wait 10 seconds
                            Thread.Sleep(10000);
                            //MessageBox.Show("new price object: " + newPriceObject);
                            //get another price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("new price: " + newMarketPrice);
                            //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            divisor = newMarketPrice - oldMarketPrice;
                            decimal slope3 = Decimal.Divide(divisor, 10);
                            //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //MessageBox.Show("slope at " + now.ToString() + " is " + slope);


                            //get price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("old price: "+oldMarketPrice);                        
                            //wait 10 seconds
                            Thread.Sleep(10000);
                            //MessageBox.Show("new price object: " + newPriceObject);
                            //get another price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("new price: " + newMarketPrice);
                            //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            divisor = newMarketPrice - oldMarketPrice;
                            decimal slope4 = Decimal.Divide(divisor, 10);
                            //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //MessageBox.Show("slope at " + now.ToString() + " is " + slope);


                            //get price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("old price: "+oldMarketPrice);                        
                            //wait 10 seconds
                            Thread.Sleep(10000);
                            //MessageBox.Show("new price object: " + newPriceObject);
                            //get another price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("new price: " + newMarketPrice);
                            //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            divisor = newMarketPrice - oldMarketPrice;
                            decimal slope5 = Decimal.Divide(divisor, 10);
                            //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //MessageBox.Show("slope at " + now.ToString() + " is " + slope);


                            //get price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("old price: "+oldMarketPrice);                        
                            //wait 10 seconds
                            Thread.Sleep(10000);
                            //MessageBox.Show("new price object: " + newPriceObject);
                            //get another price from Alpaca and convert to decimal number
                            getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                            newPriceObject = getLastTradeObject.Price;
                            newMarketPrice = decimal.Parse(newPriceObject.ToString());
                            newPriceObject = decimal.Zero;
                            //MessageBox.Show("new price: " + newMarketPrice);
                            //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                            divisor = newMarketPrice - oldMarketPrice;
                            decimal slope6 = Decimal.Divide(divisor, 10);
                            //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                            //MessageBox.Show("slope at " + now.ToString() + " is " + slope);
                            NewMarketPrice = newMarketPrice;
                            decimal divisor2 = slope1 + slope2 + slope3 + slope4 + slope5 + slope6;
                            decimal avgSlope = Decimal.Divide(divisor2, 6);
                            //MessageBox.Show("avg Slope is: " + avgSlope);


                            

                            if (avgSlope > 0 && OKtoBuy == true)
                            {
                                BuyPaperStock.BuyPaperStockMethod(stockListing, numberOfShares);
                                buyPrice = newMarketPrice;
                                decimal slope = 0;
                                while (avgSlope > 0 && slope >= 0)
                                {
                                    oldMarketPrice = newMarketPrice;

                                    //get price from Alpaca and convert to decimal number
                                    getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                                    newPriceObject = getLastTradeObject.Price;
                                    oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                                    newPriceObject = decimal.Zero;
                                    //MessageBox.Show("old price: "+oldMarketPrice);                        
                                    //wait 10 seconds
                                    Thread.Sleep(1000);
                                    //MessageBox.Show("new price object: " + newPriceObject);
                                    //get another price from Alpaca and convert to decimal number
                                    getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                                    newPriceObject = getLastTradeObject.Price;
                                    newMarketPrice = decimal.Parse(newPriceObject.ToString());
                                    newPriceObject = decimal.Zero;
                                    //MessageBox.Show("new price: " + newMarketPrice);
                                    //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                                    divisor = newMarketPrice - oldMarketPrice;
                                    slope = Decimal.Divide(divisor, 2);

                                    if (newMarketPrice < oldMarketPrice - oldMarketPrice*0.0001M)
                                    {
                                        SellPaperStock.SellPaperStockMethod(stockListing);
                                        OKtoBuy = true;
                                    }

                                }


                                OKtoBuy = false;
                            }
                            else
                            {
                                //do nothing
                            }

                        }
                        
                    }

                    if ((NewMarketPrice > buyPrice + buyPrice * 0.0001M))
                    {
                        SellPaperStock.SellPaperStockMethod(stockListing);
                        OKtoBuy = true;
                    }

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
                SellPaperStock.SellPaperStockMethod(stockListing);
                
            }
        }
    }
}

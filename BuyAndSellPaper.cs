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
    internal static class BuyAndSellPaper
    {
        private static string API_KEY = "PKHNXT0M3ZBTEKBEH1LM";
        private static string API_SECRET_KEY = "3EjNkXEQt7s4WceVGdmPslJ6oYo65uIzfHE09yPS";

        private static bool ToggleCancel = false;
        private static bool OKtoBuy = true;
        private static bool OKtoSell = false;
        private static decimal buyPrice;



        public static async Task BuyAndSellPaperMethod(string stockListing, int numberOfShares, bool toggleCancel)
        {
            try
            {
                //ready the getLastTrade web call
                
                var clientData = Alpaca.Markets.Environments.Paper.GetAlpacaDataClient(new SecretKey(API_KEY, API_SECRET_KEY));



                //while within stock market hours
                TimeSpan start = new TimeSpan(8, 30, 0); //8:30 AM
                TimeSpan end = new TimeSpan(14, 45, 0); //3 o'clock military time
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

                        //get slope

                        
                        //get price from Alpaca and convert to decimal number
                        var getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                        var newPriceObject = getLastTradeObject.Price;
                        decimal oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                        newPriceObject = decimal.Zero;
                        //MessageBox.Show("old price: "+oldMarketPrice);                        
                        //wait 10 seconds
                        Thread.Sleep(20000);
                        //MessageBox.Show("new price object: " + newPriceObject);
                        //get another price from Alpaca and convert to decimal number
                        getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                        newPriceObject = getLastTradeObject.Price;
                        decimal newMarketPrice = decimal.Parse(newPriceObject.ToString());
                        newPriceObject = decimal.Zero;
                        //MessageBox.Show("new price: " + newMarketPrice);
                        //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                        decimal divisor = newMarketPrice - oldMarketPrice;
                        decimal slope1 = Decimal.Divide(divisor, 20);
                        //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                        //MessageBox.Show("slope at " + now.ToString() + " is " + slope);




                        //get price from Alpaca and convert to decimal number
                        getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                        newPriceObject = getLastTradeObject.Price;
                        oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                        newPriceObject = decimal.Zero;
                        //MessageBox.Show("old price: "+oldMarketPrice);                        
                        //wait 10 seconds
                        Thread.Sleep(20000);
                        //MessageBox.Show("new price object: " + newPriceObject);
                        //get another price from Alpaca and convert to decimal number
                        getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                        newPriceObject = getLastTradeObject.Price;
                        newMarketPrice = decimal.Parse(newPriceObject.ToString());
                        newPriceObject = decimal.Zero;
                        //MessageBox.Show("new price: " + newMarketPrice);
                        //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                        divisor = newMarketPrice - oldMarketPrice;
                        decimal slope2 = Decimal.Divide(divisor, 20);
                        //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                        //MessageBox.Show("slope at " + now.ToString() + " is " + slope);




                        //get price from Alpaca and convert to decimal number
                        getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                        newPriceObject = getLastTradeObject.Price;
                        oldMarketPrice = decimal.Parse(newPriceObject.ToString());
                        newPriceObject = decimal.Zero;
                        //MessageBox.Show("old price: "+oldMarketPrice);                        
                        //wait 10 seconds
                        Thread.Sleep(20000);
                        //MessageBox.Show("new price object: " + newPriceObject);
                        //get another price from Alpaca and convert to decimal number
                        getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                        newPriceObject = getLastTradeObject.Price;
                        newMarketPrice = decimal.Parse(newPriceObject.ToString());
                        newPriceObject = decimal.Zero;
                        //MessageBox.Show("new price: " + newMarketPrice);
                        //slope = dy/dx = change in price/change in time = (newPrice - oldPrice)/(10secs) =(newPrice - oldPrice)/(0.16666hours) 
                        divisor = newMarketPrice - oldMarketPrice;
                        decimal slope3 = Decimal.Divide(divisor, 20);
                        //MessageBox.Show("new - old: " + newMarketPrice+" - "+oldMarketPrice);
                        //MessageBox.Show("slope at " + now.ToString() + " is " + slope);



                        decimal divisor2 = slope1 + slope2 + slope3;
                        decimal avgSlope = Decimal.Divide(divisor2, 3);





                        if ((avgSlope > 0 && newMarketPrice > oldMarketPrice && OKtoBuy == true))
                        {
                            BuyPaperStock.BuyPaperStockMethod(stockListing, numberOfShares);
                            OKtoBuy = false;
                            buyPrice = newMarketPrice;

                            while (newMarketPrice > oldMarketPrice || newMarketPrice < oldMarketPrice)
                            {
                                oldMarketPrice = newMarketPrice;

                                Thread.Sleep(10000);

                                //check newMarketPrice again
                                //get another price from Alpaca and convert to decimal number
                                getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                                newPriceObject = getLastTradeObject.Price;
                                newMarketPrice = decimal.Parse(newPriceObject.ToString());
                                newPriceObject = decimal.Zero;

                                divisor = newMarketPrice - oldMarketPrice;
                                decimal slope = Decimal.Divide(divisor, 10);

                                while (buyPrice < newMarketPrice)
                                {
                                    oldMarketPrice = newMarketPrice;

                                    Thread.Sleep(10000);

                                    //check newMarketPrice again
                                    //get another price from Alpaca and convert to decimal number
                                    getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                                    newPriceObject = getLastTradeObject.Price;
                                    newMarketPrice = decimal.Parse(newPriceObject.ToString());
                                    newPriceObject = decimal.Zero;

                                    divisor = newMarketPrice - oldMarketPrice;
                                    slope = Decimal.Divide(divisor, 10);
                                    if (newMarketPrice > buyPrice + 0.01M)
                                    {
                                        SellPaperStock.SellPaperStockMethod(stockListing);
                                        OKtoBuy = true;

                                        break;
                                    }
                                }

                                if (newMarketPrice <= buyPrice - buyPrice*0.01M)
                                {
                                    
                                    SellPaperStock.SellPaperStockMethod(stockListing);
                                    OKtoBuy = true;

                                    break;
                                }

                            }
                        }
                        //else
                        //{
                        //    while (newMarketPrice < oldMarketPrice)
                        //    {
                        //        oldMarketPrice = newMarketPrice;
                        //        Thread.Sleep(10000);

                        //        //check newMarketPrice again
                        //        //get another price from Alpaca and convert to decimal number
                        //        getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                        //        newPriceObject = getLastTradeObject.Price;
                        //        newMarketPrice = decimal.Parse(newPriceObject.ToString());
                        //        newPriceObject = decimal.Zero;

                        //        if (newMarketPrice > oldMarketPrice)
                        //        {
                                    

                        //            while (newMarketPrice >= oldMarketPrice)
                        //            {
                        //                oldMarketPrice = newMarketPrice;
                        //                Thread.Sleep(10000);

                        //                //check newMarketPrice again
                        //                //get another price from Alpaca and convert to decimal number
                        //                getLastTradeObject = await clientData.GetLastTradeAsync(stockListing);
                        //                newPriceObject = getLastTradeObject.Price;
                        //                newMarketPrice = decimal.Parse(newPriceObject.ToString());
                        //                newPriceObject = decimal.Zero;

                        //                divisor = newMarketPrice - oldMarketPrice;
                        //                decimal slope = Decimal.Divide(divisor, 30);

                        //                if (buyPrice < newMarketPrice && slope < 0 || buyPrice - 0.03M > newMarketPrice)
                        //                {
                        //                    SellPaperStock.SellPaperStockMethod(stockListing);
                        //                    OKtoBuy = true;

                        //                    break;
                        //                }

                        //            }
                        //            break;
                        //        }

                        //    }
                        //}  

                        newPriceObject = decimal.Zero;
                        oldMarketPrice = decimal.Zero;
                        newMarketPrice = decimal.Zero;
                    }
                    SellPaperStock.SellPaperStockMethod(stockListing);
                    //MessageBox.Show("Skipping everything!!!");

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
                MessageBox.Show("Something went wrong with the BuyAndSellPaperMethod() because: \r\r" + ex, "General Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
            finally
            {
                SellPaperStock.SellPaperStockMethod(stockListing);
                OKtoBuy = true;
            }
        }
        


    }
}

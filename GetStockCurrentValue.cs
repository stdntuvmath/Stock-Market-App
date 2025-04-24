using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alpaca.Markets;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace Stock_Trading_App
{
    internal class GetStockCurrentValue
    {
        private static string API_KEY = "removed";
        private static string API_SECRET_KEY = "removed";

        private static string windowsUserName = System.Environment.UserName;//gives windows username
        private static string hiddenPSVFile = @"C:\Users\" + windowsUserName + @"\Documents\Current Value.psv";
        private static string DocumentsFolder = @"C:\Users\" + windowsUserName + @"\Documents\";

        public static string CurrentValue;



        public static async Task GetStockCurrentValueMethod(string stockListing)
        {
            //var timeUtc = DateTime.UtcNow;
            
            //TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            //DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);


            try
            {              
                var clientData = Alpaca.Markets.Environments.Paper.GetAlpacaDataClient(new SecretKey(API_KEY, API_SECRET_KEY));

                var currentPrice = await clientData.GetLastTradeAsync(stockListing);

                var marketPrice = currentPrice.Price;
                //MessageBox.Show("Get current stock value in BuySell Algorithm: "+marketPrice.ToString());

                if (!Directory.Exists(DocumentsFolder))
                {
                    try
                    {
                        Directory.CreateDirectory(DocumentsFolder);


                        File.WriteAllText(hiddenPSVFile, marketPrice.ToString());

                        //FileInfo hiddenFile = new FileInfo(hiddenPSVFile);
                        //hiddenFile.Attributes = FileAttributes.Hidden;

                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("Could not create directory because \r\r" + ex, "Could Not Create Directory", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    File.WriteAllText(hiddenPSVFile, marketPrice.ToString());
                    //MessageBox.Show("Writes current value to text file.." + marketPrice.ToString());

                }

                //else if (now > end)
                //{

                //    // First, open the API connection
                //    var clientData = Alpaca.Markets.Environments.Paper.GetAlpacaDataClient(new SecretKey(API_KEY, API_SECRET_KEY));

                //    // Get daily price data for stockListing over the last 1 trading days.
                //    var bars = await clientData.GetBarSetAsync(
                //      new BarSetRequest(stockListing, TimeFrame.Day) { Limit = 1 });

                //    // See how much stockListing moved in that timeframe.
                //    //var startPrice = bars[stockListing].First().Open;
                //    var endPrice = bars[stockListing].Last().Close;

                //    //var percentChange = (endPrice - startPrice) / startPrice * 100;
                //    //Console.WriteLine($"AAPL moved {percentChange} over the last 1 days.");

                //    //Console.Read();
                //    MessageBox.Show(endPrice.ToString());
                //    string marketPrice = endPrice.ToString();

                //    if (!Directory.Exists(DocumentsFolder))
                //    {
                //        try
                //        {
                //            Directory.CreateDirectory(DocumentsFolder);


                //            File.WriteAllText(hiddenPSVFile, marketPrice);
                //            //FileInfo hiddenFile = new FileInfo(hiddenPSVFile);
                //            //hiddenFile.Attributes = FileAttributes.Hidden;

                //        }
                //        catch (IOException ex)
                //        {
                //            MessageBox.Show("Could not create directory for the student because \r\r" + ex, "Could Not Create Directory", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        }
                //    }
                //    else
                //    {
                //        File.WriteAllText(hiddenPSVFile, marketPrice);
                //    }


                //}







            }
            catch (WebException ex)
            {
                MessageBox.Show("Could not access Alpaca Network through pluggable protocol.\r\r"+ex, "WebException", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            catch (HttpListenerException ex)
            {
                MessageBox.Show("Could not access Alpaca Network through pluggable protocol.\r\r" + ex, "HttpListenerException", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not get price\r\r" + ex, "General Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




        }
    }
}

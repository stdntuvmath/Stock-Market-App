using System;
using System.Threading.Tasks;
using Alpaca.Markets;
using System.Windows.Forms;
using System.Net;

namespace Stock_Trading_App
{
    internal static class InstantPrice
    {
        private static string API_KEY = "removed";
        private static string API_SECRET_KEY = "removed";





        public static async Task InstantStockPriceMethod(string stockListing)
        {

            try
            {
                var clientData = Alpaca.Markets.Environments.Paper.GetAlpacaDataClient(new SecretKey(API_KEY, API_SECRET_KEY));

                var currentPrice = await clientData.GetLastTradeAsync(stockListing);

                var marketPrice = currentPrice.Price;
                MessageBox.Show(marketPrice.ToString());

                clientData = null;
                currentPrice = null;
                marketPrice = decimal.Zero;

            }
            catch (WebException ex)
            {
                MessageBox.Show("Could not access Alpaca Network through pluggable protocol.\r\r" + ex, "WebException", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

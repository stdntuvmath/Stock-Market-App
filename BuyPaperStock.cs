using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alpaca.Markets;

namespace Stock_Paper_Trading_App
{
    internal static class BuyPaperStock
    {


        private static string API_KEY = "removed";
        private static string API_SECRET_KEY = "removed";




        public static async Task BuyPaperStockMethod(string stockListing, int quantity)
        {
            // First, open the API connection
            var client = Alpaca.Markets.Environments.Paper
                .GetAlpacaTradingClient(new SecretKey(API_KEY, API_SECRET_KEY));

            // Get our account information.
            var account = await client.GetAccountAsync();

            // Check if our account is restricted from trading.
            if (account.IsTradingBlocked)
            {
                MessageBox.Show("Account is currently restricted from trading.");
                
            }
            else
            {
                // Submit a market order to buy x amount of shares of y stocklisting at market price

                try
                {
                    var order = await client.PostOrderAsync(new NewOrderRequest(stockListing, quantity, OrderSide.Buy, OrderType.Market, TimeInForce.Day));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                
            }

            


        }



        //public void BuyPaparStockMethod(string stockListing, int quantity)
        //{
        //    PrivateSellStockMethod(stockListing, quantity);
        //}

        //private void PrivateSellStockMethod(string stockListing, int quantity)
        //{
        //    var client = new RestClient("https://api.alpaca.markets/v2/orders");
        //    client.Timeout = -1;
        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("APCA-API-KEY-ID", "PKHNXT0M3ZBTEKBEH1LM");
        //    request.AddHeader("APCA-API-SECRET-KEY", "3EjNkXEQt7s4WceVGdmPslJ6oYo65uIzfHE09yPS");
        //    request.AddHeader("Content-Type", "text/plain");
        //    request.AddParameter("text/plain", "{\r\n    \"symbol\": \"" + stockListing + "\",\r\n    \"qty\": " + quantity + ",\r\n    \"side\": \"buy\",\r\n    \"type\": \"market\",\r\n    \"time_in_force\": \"day\"\r\n}", ParameterType.RequestBody);
        //    IRestResponse response = client.Execute(request);
        //    MessageBox.Show(response.Content);
        //}
    }
}

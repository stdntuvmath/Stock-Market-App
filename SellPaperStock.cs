using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using Alpaca.Markets;


namespace Stock_Paper_Trading_App
{
    internal static class SellPaperStock
    {


        private static string API_KEY = "removed";
        private static string API_SECRET_KEY = "removed";




        public static async Task SellPaperStockMethod(string stockListing)
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
                    var order = await client.DeletePositionAsync(
                          new DeletePositionRequest(stockListing));
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                }

            }




        }

        //public void SellPaperStockMethod(string stockListing)
        //{
        //    PrivateSellPaperStockMethod( stockListing);
        //}



        //private void PrivateSellPaperStockMethod(string stockListing)
        //{
        //    var client = new RestClient("https://paper-api.alpaca.markets/v2/positions/"+stockListing);
        //    client.Timeout = -1;
        //    var request = new RestRequest(Method.DELETE);
        //    request.AddHeader("APCA-API-KEY-ID", "PKHNXT0M3ZBTEKBEH1LM");
        //    request.AddHeader("APCA-API-SECRET-KEY", "3EjNkXEQt7s4WceVGdmPslJ6oYo65uIzfHE09yPS");
        //    request.AddHeader("Content-Type", "text/plain");
        //    IRestResponse response = client.Execute(request);
        //    Console.WriteLine(response.Content);
        //}
    }
}

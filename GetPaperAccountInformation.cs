using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestSharp;
using Alpaca.Markets;

namespace Stock_Paper_Trading_App
{
    internal static class GetPaperAccountInformation
    {

        private static string API_KEY = "removed";
        private static string API_SECRET_KEY = "removed";




        public static async Task GetPaperAccountInformationMethod()
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

            MessageBox.Show(account.BuyingPower + " is available as buying power.\r" +
                            account.TradableCash+" is cash on hand.");

            
        }





        //private void PrivateGetPaperAccountInformationMethod()
        //{
        //    string methodType = "Method.GET";
        //    var client = new RestClient("https://paper-api.alpaca.markets/v2/account");
        //    client.Timeout = -1;
        //    var request = new RestRequest(methodType);
        //    request.AddHeader("APCA-API-KEY-ID", "PKHNXT0M3ZBTEKBEH1LM");
        //    request.AddHeader("APCA-API-SECRET-KEY", "3EjNkXEQt7s4WceVGdmPslJ6oYo65uIzfHE09yPS");
            
            
        //    try
        //    {
        //        IRestResponse response = client.Execute(request);
        //        //Console.WriteLine(response.Content);
        //        //Console.Read();
        //        MessageBox.Show(response.Content);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Could not execute "+ methodType+" because of the following errors: \r\r"+ex, "Cannot Execute Command",MessageBoxButtons.OK,MessageBoxIcon.Error);
        //    }
            
        //}
    }
}

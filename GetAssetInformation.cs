using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alpaca.Markets;

namespace Stock_Trading_App
{
    internal static class GetAssetInformation
    {
        private static string API_KEY = "removed";
        private static string API_SECRET_KEY = "removed";

        public static string CompanyName;



        public static async Task GetAssetInformationMethod(string stockListing)
        {
            // First, open the API connection
            var client = Alpaca.Markets.Environments.Paper
                .GetAlpacaTradingClient(new SecretKey(API_KEY, API_SECRET_KEY));

            // Get our account information.
            var stockInformation = await client.GetAssetAsync(stockListing);

            CompanyName = stockInformation.Symbol;

            MessageBox.Show(CompanyName);

        }
    }
}

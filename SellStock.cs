using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace Stock_Paper_Trading_App
{
    class SellStock
    {

        public void SellStockMethod(string stockListing, int quantity)
        {
            var client = new RestClient("https://api.alpaca.markets/v2/positions/");
            client.Timeout = -1;
            var request = new RestRequest(Method.DELETE);
            request.AddHeader("APCA-API-KEY-ID", "removed");
            request.AddHeader("APCA-API-SECRET-KEY", "removed");
            request.AddHeader("Content-Type", "text/plain");
            request.AddParameter("text/plain", "{\r\n    \"symbol\": \""+ stockListing +"\",\r\n    \"qty\": "+ quantity + ",\r\n    \"side\": \"buy\",\r\n    \"type\": \"market\",\r\n    \"time_in_force\": \"day\"\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }

    }
}

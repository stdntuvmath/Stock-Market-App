using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Trading_App
{
    class Buy_DifferenceInEquity
    {
        ////get the difference between new equity and old equity which is the profit

        ////store value of profit
        //differenceBetweenEquities = decimal.Subtract(equity, oldEquity);
        ////store into array
        //differenceBetweenEquitiesArray.Add(differenceBetweenEquities);

        ////store the max difference
        //maxDifference = differenceBetweenEquitiesArray.Max();

        ////check value
        //if (differenceBetweenEquities > 0)
        //{                                    

        //    //check if difference is the greatest difference
        //    if (differenceBetweenEquities == maxDifference)
        //    {
        //        oldEquity = equity;
        //        FindOldEquity = false;
        //    }
        //    //reset difference to zero
        //    differenceBetweenEquities = decimal.Zero;

        //}
        //else if (differenceBetweenEquities < 0)
        //{
        //    //get max*%

        //    decimal maxDifferenceTimesPercent = decimal.Multiply(maxDifference, 0.02M);//2%

        //    //get max - max*%
        //    decimal lowDifference = decimal.Subtract(maxDifference, maxDifferenceTimesPercent);

        //    //check if difference is equal to max - max*2%

        //    if (differenceBetweenEquities <= lowDifference)
        //    {
        //        now = DateTime.Now.TimeOfDay;
        //        logFileLine = "************************\r" + now + "|" + "AUTOSELL " + "\r" + "buyPrice: " + buyPrice.ToString() + "\r" + "oldEquity: " + oldEquity.ToString() + "\rnew equity: " + equity.ToString() + "\rbuyInEquity: " + buyInEquity + "\r************************\r\r";
        //        File.AppendAllText(logFilePath, logFileLine);

        //        SellPaperStock.SellPaperStockMethod(stockListing);
        //        OKtoBuy = true;
        //        FindOldEquity = true;

        //    }
        //    differenceBetweenEquities = decimal.Zero;
        //}
    }
}

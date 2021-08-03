using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Trading_App
{
    class Buy_LastFourDigitsOfEquity
    {
        ////get the last four digits of both oldEquity and equity

        //string oldEquityString = oldEquity.ToString();
        //char[] oldEquityCharArray = oldEquityString.ToCharArray();
        //oldEquityString = oldEquityString.Substring(oldEquityString.Length - 4, oldEquityString.Length);
        //MessageBox.Show(oldEquityString);

        //decimal oldEquityLastFourDigits = Convert.ToDecimal(oldEquityString);

        //string equityString = equity.ToString();
        //char[] equityCharArray = equityString.ToCharArray();
        //equityString = equityString.Substring(equityString.Length - 4, equityString.Length);
        //MessageBox.Show(equityString);

        //decimal equityLastFourDigits = Convert.ToDecimal(equityString);

        //decimal PercentOfoldEquity = Decimal.Multiply(oldEquityLastFourDigits, 0.02M);//2% of oldEquity
        //decimal oldEquityLastFourDigitsMinusPercentOfoldEquity = Decimal.Subtract(oldEquityLastFourDigits, PercentOfoldEquity);

        //while (equityLastFourDigits > oldEquityLastFourDigitsMinusPercentOfoldEquity)
        //{

        //    logFileLine = now + "|oldEquityLastFourDigits < equityLastFourDigits\r" + "oldEquityLastFourDigits: " + oldEquityLastFourDigits + "\requityLastFourDigits: " + equityLastFourDigits + "\r\rStayed In the Buy";
        //    File.AppendAllText(logFilePath, logFileLine);

        //    oldEquity = equity;

        //    Thread.Sleep(10000);


        //    equity = decimal.Zero;
        //    account = await client.GetAccountAsync();
        //    equity = account.Equity;

        //    //get the last four digits of both oldEquity and equity

        //    oldEquityString = oldEquity.ToString();
        //    oldEquityCharArray = oldEquityString.ToCharArray();
        //    oldEquityString = oldEquityString.Substring(oldEquityString.Length - 4, oldEquityString.Length);
        //    MessageBox.Show(oldEquityString);

        //    oldEquityLastFourDigits = Convert.ToDecimal(oldEquityString);

        //    equityString = equity.ToString();
        //    equityCharArray = equityString.ToCharArray();
        //    equityString = equityString.Substring(equityString.Length - 4, equityString.Length);
        //    MessageBox.Show(equityString);

        //    equityLastFourDigits = Convert.ToDecimal(equityString);

        //    PercentOfoldEquity = Decimal.Multiply(oldEquityLastFourDigits, 0.02M);//2% of oldEquity
        //    oldEquityLastFourDigitsMinusPercentOfoldEquity = Decimal.Subtract(oldEquityLastFourDigits, PercentOfoldEquity);

        //}
    }
}

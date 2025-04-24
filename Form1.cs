using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alpaca.Markets;
using Stock_Trading_App;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Stock_Paper_Trading_App
{
    public partial class Form1 : Form
    {
        private static Int32 quantity;
        private static string StockListing;
        private static string windowsUserName = System.Environment.UserName;//gives windows username
        private static string CurrentEquityFile = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\CurrentEquity.txt";
        private static string CurrentCashFile = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\CurrentCashMade.txt";

        private static string API_KEY = "removed";
        private static string API_SECRET_KEY = "removed";
        private static bool toggleCancel = false;
        private static string WithdrawlableCash;

        public static string CurrentStockPrice;


        private static TimeSpan start = new TimeSpan(8, 29, 55); //8:30 military time
        private static TimeSpan now = DateTime.Now.TimeOfDay;//whatever time it is now
        private static DateTime TodaysDate = DateTime.Today;

        public Form1()
        {
            InitializeComponent();

            textBox1.TabIndex = 0;
            textBox2.TabIndex = 1;            
            button4.TabIndex = 2;
            button5.TabIndex = 3;
            button2.TabIndex = 4;
            button3.TabIndex = 5;


        }

        private void Form1_Load(object sender, EventArgs e)
        {

            while (true)
            {
                //get new time
                now = DateTime.Now.TimeOfDay;
                if (start <= now)
                {
                    


                    button1.BackColor = Color.LightGreen;
                    // TODO: This line of code loads data into the 'lootLoaderDataSet.lootloader_PriceData_2021' table. You can move, or remove it, as needed.
                    backgroundWorker1.RunWorkerAsync();
                    

                    break;

                }
            }



            //GetPaperAccountInformation.GetPaperAccountInformationMethod();
            //string equity = File.ReadAllText(CurrentEquityFile);
            //string cashMade = File.ReadAllText(CurrentCashFile);

            //this.Text = "Paper Trader  " + equity;
            //label3.Text = cashMade;


            //backgroundWorker2.RunWorkerAsync();

            DateTime todaysDateOnly = DateTime.Today;
            string todaysDate = todaysDateOnly.ToString("yyyyMMdd_");

            string logFilePath = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\Logs\" + todaysDate + @"log.txt";
            now = DateTime.Now.TimeOfDay;

            string logFileLine = "************************\r***\r***\r***\r***\r" + now + "\r" + "Paper Trader Program Initiated.\r***\r***\r***\r***\r************************\r\r";
            File.AppendAllText(logFilePath, logFileLine);


            textBox1.Text = "AMD";
            textBox2.Text = "1";
            button4.PerformClick();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                if (button1.Text == "Paper Trader")
                {

                    DialogResult result = MessageBox.Show("Are you sure you want to switch to live trading with real money?", "Switch to Using Real Money?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        button1.BackColor = Color.Yellow; //symbolizes light turned on

                        button1.Text = "Pattern Day Trader";

                        this.Text = "Stock Market Day Trading";
                    }

                      
                }

                else if (button1.Text == "Pattern Day Trader")
                {

                    DialogResult result = MessageBox.Show("Are you sure you want to switch to fake money?", "Switch to Using Fake Money?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        button1.BackColor = Color.LightGreen; //symbolizes light turned off

                        button1.Text = "Paper Trader";

                        this.Text = "Paper Trader";
                    }

                    
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)//Buy button
        {

            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                textBox1.ForeColor = Color.White;
                textBox1.BackColor = Color.Salmon;
                textBox2.ForeColor = Color.White;
                textBox2.BackColor = Color.Salmon;
                // Move the selection pointer to the end of the text of the control.

                textBox1.Select(textBox1.Text.Length, 0);
            }
            else
            {
                try
                {
                    quantity = Int32.Parse(textBox2.Text);

                }
                catch (ArgumentNullException ex)
                {
                    MessageBox.Show("Could not change the quantity from a string to an integer and because of this a null was passed to the method Int32.Parse().\r\r" + ex, "ArgumentNullException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("The format of the argument of Int32.Parse() is invalid.\r\r" + ex, "FormatException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (OverflowException ex)
                {
                    MessageBox.Show("The arguement variable is being overloaded with data for method Int32.Parse(). The number is too big.\r\r" + ex, "OverflowException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                StockListing = textBox1.Text;

                BuyPaperStock.BuyPaperStockMethod(StockListing, quantity);

                DateTime todaysDateOnly = DateTime.Today;
                string todaysDate = todaysDateOnly.ToString("yyyyMMdd_");

                string logFilePath = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\Logs\" + todaysDate + @"log.txt";
                TimeSpan now = DateTime.Now.TimeOfDay;

                string logFileLine = now + "| BUY - Manually Clicked Buy Button.\r\r";
                File.AppendAllText(logFilePath, logFileLine);
            }

           

        }

        private void button3_Click(object sender, EventArgs e)//Sell button
        {
            StockListing = textBox1.Text;
            SellPaperStock.SellPaperStockMethod(StockListing);

            DateTime todaysDateOnly = DateTime.Today;
            string todaysDate = todaysDateOnly.ToString("yyyyMMdd_");

            string logFilePath = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\Logs\" + todaysDate + @"log.txt";
            TimeSpan now = DateTime.Now.TimeOfDay;

            string logFileLine = now + "| SOLD - Manually Clicked Sell Button.\r\r";
            File.AppendAllText(logFilePath, logFileLine);
        }

       

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    textBox1.ForeColor = Color.White;
                    textBox1.BackColor = Color.Yellow;
                    
                    // Move the selection pointer to the end of the text of the control.

                    textBox1.Select(textBox1.Text.Length, 0);
                }
                else
                {
                    string stockListing = textBox1.Text;
                    InstantPrice.InstantStockPriceMethod(stockListing);

                }
            }
        }

        private void button4_Click(object sender, EventArgs e)//begin mining button
        {

            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                textBox1.ForeColor = Color.White;
                textBox1.BackColor = Color.Salmon;
                textBox2.ForeColor = Color.White;
                textBox2.BackColor = Color.Salmon;
                // Move the selection pointer to the end of the text of the control.

                textBox1.Select(textBox1.Text.Length, 0);
            }
            else
            {
                try
                {
                    backgroundWorker1.RunWorkerAsync();

                }
                catch
                {
                    //backgroundWorker1.CancelAsync();
                    backgroundWorker1.Dispose();
                }

                //DialogResult result = MessageBox.Show("Are you sure you want to mine this stock?", "Begin Mining Stock", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);


                //if (result == DialogResult.Yes)
                //{







                //}
                //else if (result == DialogResult.No)
                //{
                //    //do nothing
                //}


            }


           
        }

        private void button5_Click(object sender, EventArgs e)//cancel button
        {
            
            backgroundWorker1.Dispose();
            textBox1.Clear();
            textBox2.Clear();

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            string stockListing = textBox1.Text;
            string numOfShares = textBox2.Text;
            int numberOfShares = int.Parse(numOfShares);
            //BuyAndSellUsingEquity.BuyAndSellPaperSlopeAvg60AndEquityMethod(stockListing, numberOfShares);

            //BuyAndSellUsingFlowChart.BuyAndSellPapeUsingFlowChartMethod(stockListing, numberOfShares);

            Buy_UsingAVGSlope.Buy_UsingAVGSlopeMethod(stockListing, numberOfShares);

            //BuyAndSellSimply1.BuyAndSellSimply1Method(stockListing,numberOfShares);

            //Test1_TestingValues.Test1_TestingValuesMethod(stockListing, numberOfShares);




        }

        private void textBox1_BackColorChanged(object sender, EventArgs e)
        {
            Thread.Sleep(500);
            textBox1.BackColor = Color.Salmon;
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            //while (true)
            //{
            //    try
            //    {
            //        Thread.Sleep(10000);
                    
            //        string equity = File.ReadAllText(CurrentEquityFile);
            //        string cashMade = File.ReadAllText(CurrentCashFile);

            //        this.Text = "Paper Trader  " + equity;
            //        label3.Text = cashMade;
            //    }
            //    catch (IOException ex)
            //    {

            //    }
            //}
        }

        private void button6_Click(object sender, EventArgs e)//Refresh
        {
            string equity = File.ReadAllText(CurrentEquityFile);
            string cashMade = File.ReadAllText(CurrentCashFile);


            if (button1.Text == "Paper Trader")
            {

                this.Text = "Paper Trader  " + equity;

            }

            else if (button1.Text == "Pattern Day Trader")
            {
                this.Text = "Stock Market DT  " + equity;

            }

            label3.Text = cashMade;




            //Process.Start(@"C:\Users\14025\source\repos\Stock Trading App\Stock Trading App\bin\Debug\Stock Trading App.exe");
            //Application.Exit();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

            DateTime todaysDateOnly = DateTime.Today;
            string todaysDate = todaysDateOnly.ToString("yyyyMMdd_");

            string logFilePath = @"C:\Users\" + windowsUserName + @"\Documents\Paper Trader\Logs\" + todaysDate + @"log.txt";
            TimeSpan now = DateTime.Now.TimeOfDay;

            now = DateTime.Now.TimeOfDay;
            string logFileLine = "************************\r***\r***\r***\r***\r" + now + "\r" + "Paper Trader Program Deactivated.\r***\r***\r***\r***\r************************\r\r";
            File.AppendAllText(logFilePath, logFileLine);
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox1.BackColor = Color.White;
            textBox1.ForeColor = Color.Black;
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            textBox2.BackColor = Color.White;
            textBox2.ForeColor = Color.Black;


        }

        private void button7_Click(object sender, EventArgs e)//Log button
        {
            Process.Start(@"C:\Users\stdnt\Documents\Paper Trader\Logs");
        }
    }
}

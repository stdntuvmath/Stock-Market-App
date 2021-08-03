using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stock_Trading_App
{
    class Test1
    {
        public void method1()
        {
            int[] anArray = { 1, 9, 5, 2, 7, 3 };
            // Finding max
            int max = anArray.Max();

            // Positioning max
            int index = Array.IndexOf(anArray, max);

            MessageBox.Show("Largest number is "+ max+" located in index "+ index);
        }

    }
}

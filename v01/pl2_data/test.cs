using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Windows.Forms;
using System.Diagnostics;

namespace Test
{
    static class Test_all
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] parameters)
        {
            Console.WriteLine("Проверка");
            HTML_DBF_tester d = new HTML_DBF_tester();
	    d.Test_all();
            Console.WriteLine("Проверка завершена");
        }
    }
}

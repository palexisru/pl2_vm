using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Windows.Forms;
using System.Diagnostics;
using pl2.Data.HTML;
using System.IO;
using System.Text;

namespace Test
{
    public class HTML_DBF_tester
    {
        public const string xml_path = @"C:\propusk\local.xml";
        public const string database_path = @"C:\propusk\";
        public const string database_name = "PROPUSK";

        public pl2.Data.HTML.Config vm;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        public HTML_DBF_tester()
        {
        }

        public bool Test_all()
        {
            System.Text.Encoding e = System.Text.Encoding.Default;
            Console.WriteLine(e.WindowsCodePage);

            create_xml(xml_path);
            create_database(database_name);
            test_read("database");
            create_fio();
            test_read("fio");
            update_fio();
	    return false;
        }

        public void create_xml(string path)
        {
            vm = new pl2.Data.HTML.Config(path);
            // vm.Create_config();
        }

        public void create_database(string db_name)
        {
            // Config c 
            vm = new pl2.Data.HTML.Config(xml_path);
            vm.Read_config();
            HTML_Data_base d = vm.Databases.databases["PROPUSK"];
        }

        public void create_fio()
        {
            Console.WriteLine("create_fio");
            HTML_Data_base d = vm.Databases.databases["PROPUSK"];

            string full_name = d.ConnectionString+"fio.html";
            // long fstart;
            StreamWriter s;
            s = new StreamWriter(full_name, false, Encoding.GetEncoding(1200));
            s.WriteLine("<HTML>");
            s.WriteLine("<head>");
            s.WriteLine("<meta http-equiv=Content-Type content=\"text/html; charset=windows-1200\">");
            s.WriteLine("</head>");
            s.WriteLine("<body lang=RU>");

            s.WriteLine("<table border=1>");
            s.WriteLine("<thead><td>field_count</td><td>fields_start</td><td>record_start</td><td>record_len</td><td>record_count</td></thead>");
            s.WriteLine("<tr><td>000</td><td>0000000000</td><td>0000000000</td><td>0000000000</td><td>0000000000</td></tr>");
            //s.WriteLine("<tr>                                                                                        </tr>");
            s.WriteLine("</table>");

            s.WriteLine("<table border=1>");
            s.WriteLine("<tr><td>deleted         </td><td>D</td><td>08</td><td>0</td></tr>");
            s.WriteLine("<tr><td>kod             </td><td>A</td><td>10</td><td>0</td></tr>");
            s.WriteLine("<tr><td>f               </td><td>C</td><td>15</td><td>0</td></tr>");
            s.WriteLine("<tr><td>i               </td><td>C</td><td>15</td><td>0</td></tr>");
            s.WriteLine("<tr><td>o               </td><td>C</td><td>15</td><td>0</td></tr>");
            s.WriteLine("<tr><td>rab             </td><td>C</td><td>40</td><td>0</td></tr>");
            s.WriteLine("<tr><td>dol             </td><td>C</td><td>40</td><td>0</td></tr>");
            s.WriteLine("<tr><td>perm            </td><td>L</td><td> 1</td><td>0</td></tr>");
            s.WriteLine("<tr><td>otd             </td><td>C</td><td> 4</td><td>0</td></tr>");
            s.WriteLine("<tr><td>data            </td><td>C</td><td> 8</td><td>0</td></tr>");
            s.WriteLine("<tr><td>cc              </td><td>C</td><td> 2</td><td>0</td></tr>");
            s.WriteLine("<tr><td>mm              </td><td>C</td><td> 2</td><td>0</td></tr>");
            s.WriteLine("<tr><td>end_data        </td><td>C</td><td> 8</td><td>0</td></tr>");
            s.WriteLine("<tr><td>end_cc          </td><td>C</td><td> 2</td><td>0</td></tr>");
            s.WriteLine("<tr><td>end_mm          </td><td>C</td><td> 2</td><td>0</td></tr>");
            s.WriteLine("<tr><td>corpus          </td><td>C</td><td>20</td><td>0</td></tr>");
            s.WriteLine("<tr><td>vp_data         </td><td>D</td><td> 8</td><td>0</td></tr>");
            s.WriteLine("<tr><td>vp              </td><td>L</td><td> 1</td><td>0</td></tr>");
            s.WriteLine("</table>");

            s.WriteLine("<table border=1>");

            s.Write("<thead><td>deleted</td>");
            s.Write("<td>kod</td>");
            s.Write("<td>f</td>");
            s.Write("<td>i</td>");
            s.Write("<td>o</td>");
            s.Write("<td>rab</td>");
            s.Write("<td>dol</td>");
            s.Write("<td>perm</td>");
            s.Write("<td>otd</td>");
            s.Write("<td>data</td>");
            s.Write("<td>mm</td>");
            s.Write("<td>cc</td>");
            s.Write("<td>end_data</td>");
            s.Write("<td>end_mm</td>");
            s.Write("<td>end_cc</td>");
            s.Write("<td>corpus</td>");
            s.Write("<td>vp_data</td>");
            s.Write("<td>vp</td>");
            s.WriteLine("</thead>");


            s.WriteLine("</table></body></html>");
            s.Close();
            d.Recover_data_file(full_name);
            
        }
        
        public void  test_read(string name)
        {
            HTML_Data_base d = vm.Databases.databases["PROPUSK"];

            string full_name = d.ConnectionString+name+d.data_base_file_extension;
            Console.WriteLine("TODO - read table - " + full_name);
            
        }

        public void update_fio()        
        {
            Console.WriteLine("TODO - update table fio");
        }

    }
}

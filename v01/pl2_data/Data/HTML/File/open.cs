using System.Collections.Generic;
using System.Diagnostics;
using System.Data.Common;
using System.Data;
using System.Data.OleDb;
using System;
using System.IO;
using System.Text;
// using Microsoft.VisualBasic.FileIO;
using pl2.Data.HTML.Field;

namespace pl2.Data.HTML
{
    public partial class HTML_table
    {
        public HTML_Data_base data_base;
        string table_name;
        string full_name;
        public FileStream file_stream;
        System.Collections.Generic.Dictionary<string, HTML_field> fields = new System.Collections.Generic.Dictionary<string, HTML_field>();
        int rc, v, data_end_pos, data_start_pos;
        int r;
        HTML_File_header header = new HTML_File_header();

        // public int field_count, fields_start, records_start, record_len, records_count;
        public string record_value;
        public byte[] record_buffer;

        public List<Dictionary<HTML_field, object>> records;
        public DataTable data_table;


        public HTML_table(HTML_Data_base master_data_base, string master_table_name)
        {
            data_base = master_data_base;
            table_name = master_table_name;
            full_name = data_base.ConnectionString + table_name + data_base.extension;
            Console.WriteLine(HTML_field.empty_field_definition.Length);
            open();

        }

        public void open()
        {
            if (File.Exists(full_name) == false)
               throw new FileNotFoundException();
            int to_read = 65536 ; // 16384;
            byte[] buffer = new byte[to_read];
            char[] to_buffer = new char[to_read/2];
            // char[] replace;
            string text ;
            // int charsUsed, bytesUsed;
            // bool completed;

            // System.Text.UnicodeEncoding dd = new System.Text.UnicodeEncoding();
            // Decoder d=dd.GetDecoder();
            // Encoder e=dd.GetEncoder();

            Console.WriteLine("Restory_data_file()");

            file_stream = new FileStream(full_name, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
            rc=file_stream.Read(buffer, 0, to_read);

            r = data_base.decoder.GetChars(buffer, 0, rc, to_buffer, 0);
            Console.WriteLine(r);
            text = new String(to_buffer);
            header.description_table_pos = text.IndexOf("<table");
            Console.WriteLine(header.description_table_pos );

            header.description_table_tr_pos = text.IndexOf("<tr>", header.description_table_pos);
            Console.WriteLine(header.description_table_tr_pos);

            header.field_table_pos = text.IndexOf("<table", header.description_table_tr_pos);
            Console.WriteLine("field_table_pos "+header.field_table_pos );

            header.field_table_tr_pos = text.IndexOf("<tr>", header.field_table_pos);
            Console.WriteLine("field_table_tr_pos " + header.field_table_tr_pos);

            header.data_table_pos = text.IndexOf("<table", header.field_table_tr_pos);
            Console.WriteLine("data_table_pos " + header.data_table_pos );

            header.data_table_tr_pos = text.IndexOf("<tr>", header.data_table_pos);
            Console.WriteLine(header.data_table_tr_pos);

            header.pos_t4 = text.IndexOf("</table>", header.data_table_tr_pos);

            /*
            Console.WriteLine(text.Substring(description_table_tr_pos+8,86));
            Console.WriteLine(text.Substring(description_table_tr_pos+8, 3));
            Console.WriteLine(text.Substring(description_table_tr_pos+20, 10));
            Console.WriteLine(text.Substring(description_table_tr_pos+39, 10));
            Console.WriteLine(text.Substring(description_table_tr_pos+58,10));
            Console.WriteLine(text.Substring(description_table_tr_pos+77,10));
            */

            header.field_count = int.Parse(text.Substring(header.description_table_tr_pos+8,3)); 
            header.fields_start = int.Parse(text.Substring(header.description_table_tr_pos+20, 10));
            header.records_start = int.Parse(text.Substring(header.description_table_tr_pos+39, 10)); 
            header.record_length = int.Parse(text.Substring(header.description_table_tr_pos+58, 10));
            header.records_count = int.Parse(text.Substring(header.description_table_tr_pos+77, 10));
            // Console.WriteLine(field_count + ", " + fields_start + ", " + records_start + ", " + record_len + ", " +records_count);

            fields.Clear();
            
            int field_offset = 9;
            int fields_counter = 0;

            for (int field_pos = 0; field_pos*HTML_field.field_definition_chars + header.field_table_tr_pos < header.data_table_pos-10; ++ field_pos)
            {
                HTML_field f = new HTML_field(this);
                string field_def = text.Substring(field_pos*HTML_field.field_definition_chars + header.field_table_tr_pos, HTML_field.field_definition_chars-7);
                string field_name = field_def.Substring(8, 16);
                string field_type = field_def.Substring(33, 1);
                string field_len = field_def.Substring(43, 2);
                string field_dec = field_def.Substring(54, 1);
                
                // Console.WriteLine(field_def + "\r\n" + field_name+", " + field_type + ", " + field_len + ", " + field_dec);

                f.field_name = field_name.Trim();
                f.field_type = field_type;
                f.field_length = int.Parse(field_len);
                f.field_dec = int.Parse(field_dec);
                f.field_offset = field_offset;
                fields.Add(f.field_name.Trim(), f);
                ++fields_counter;

                Console.WriteLine(f.field_name+", " + f.field_type + ", " + f.field_length + ", " + f.field_dec + ", " +f.field_offset);
                field_offset = field_offset + f.field_length + 9;

            }
            file_stream.Seek(header.records_start, SeekOrigin.Begin);
            Debug.Assert (fields_counter==header.field_count);
            //read_field_definition();

        }

        public void read_field_definition(int field_pos)
        {
        }

        public static Type ToDbType(string type)
        {
            switch (type)
            {
                case "F":
                    return typeof(float);

                case "N":
                    return typeof(int);

                case "C":
                    return typeof(string);

                case "D":
                    return typeof(DateTime);

                case "L":
                    return typeof(bool);

                default:
                    return null;
            }
        }

        // выравнивание до определенной длины
        public static string Padr(string s, int len)
        {
           s = s.PadRight(len);
           s = s.Substring(0, len);
           return s;
        }

        // выравнивание до определенной длины
        public static string Padl(string s, int len)
        {
           s = s.PadLeft(len);
           s = s.Substring(len-s.Length, len);
           return s;
        }
    }
}
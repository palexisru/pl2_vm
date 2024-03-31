using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;

namespace pl2.Data.HTML
{
    public partial class HTML_table
    {
        public IEnumerable<Dictionary<string, object>> ReadToDictionary()
        {
            ReadRecords();
            return records.Select(record => record.ToDictionary(r => r.Key.field_name, r => r.Value)).ToList();
        }

        private void ReadRecords()
        {
            // TraceString("ReadRecords() - " + header.NumberOfRecords + " ");
            // Trace.Indent();
             
            records = new List<Dictionary<HTML_field, object>>();
            byte[] std_buffer = new byte[header.record_lenght];

            // Skip back to the end of the header. 
            file_stream.Seek(header.records_start, SeekOrigin.Begin);

            for (int i = 0; i < header.number_of_records; ++i)
            {
                // if (file_stream.PeekChar() == '*') // DELETED
                // {
                //     file_stream.ReadBytes(header.RecordLenght);
                //     continue;
                // }

                var record = new Dictionary<HTML_field, object>(); // 
                var row = file_stream.Read(std_buffer, 0, header.record_lenght);

                foreach (var field in fields)
                {
                    byte[] buffer = new byte[field.Value.field_length];
                    Array.Copy(std_buffer, field.Value.field_offset, buffer, 0, field.Value.field_length);
                    string text = (data_base.encoding.GetString(buffer) ?? String.Empty).Trim();

                    switch (field.Value.field_type)
                    {
                        case "C":
                            record[field.Key] = text;
                            break;
                        default:
                            record[field.Key] = buffer;
                            break;
                    } 
                    // TraceString(text + ":" + (DBFFieldType)field.FieldType + " ");
                }

                records.Add(record);
                // TraceString("");
            }
            // Trace.Unindent();
        }

        public DataTable ReadToDataTable()
        {
            ReadRecords();

            var table = new DataTable();

            // Columns
            foreach (var field in fields)
            {
                var colType = ToDbType(field.Value.field_type);
                var column = new DataColumn(field.Value.field_name, colType ?? typeof(String));
                table.Columns.Add(column);
            }

            // Rows
            foreach (var record in records)
            {
                var row = table.NewRow();
                foreach (var column in record.Keys)
                {
                    row[column.field_name] = record[column] ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            return table;
        }


        public IEnumerable<T> ReadToObject<T>()
            where T : new()
        {
            ReadRecords();

            var type = typeof(T);
            var list = new List<T>();

            foreach (var record in records)
            {
                T item = new T();
                foreach (var pair in record.Select(s => new { Key = s.Key.FieldName, Value = s.Value }))
                {
                    var property = type.GetProperty(pair.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (property != null)
                    {
                        if (property.PropertyType == pair.Value.GetType())
                        {
                            property.SetValue(item, pair.Value, null);
                        }
                        else
                        {
                            if (pair.Value != DBNull.Value)
                            {
                                property.SetValue(item, System.Convert.ChangeType(pair.Value, property.PropertyType), null);
                            }
                        }
                    }
                }
                list.Add(item);
            }

            return list;
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing == false) return;
            if (reader != null)
            {
                reader.Close();
                reader.Dispose();
                reader = null;
            }
        }

        ~HTML_table()
        {
            Dispose(false);
        } 

        #endregion

         /// <summary>
        /// Convert a Julian Date as long to a .NET DateTime structure
        /// Implemented from pseudo code at http://en.wikipedia.org/wiki/Julian_day
        /// </summary>
        /// <param name="julianDateAsLong">Julian Date to convert (days since 01/01/4713 BC)</param>
        /// <returns>DateTime</returns>
        private static DateTime JulianToDateTime(long julianDateAsLong)
         {
             if (julianDateAsLong == 0) return DateTime.MinValue;
            double p = Convert.ToDouble(julianDateAsLong);
            double s1 = p + 68569;
            double n = Math.Floor(4 * s1 / 146097);
            double s2 = s1 - Math.Floor(((146097 * n) + 3) / 4);
            double i = Math.Floor(4000 * (s2 + 1) / 1461001);
            double s3 = s2 - Math.Floor(1461 * i / 4) + 31;
            double q = Math.Floor(80 * s3 / 2447);
            double d = s3 - Math.Floor(2447 * q / 80);
            double s4 = Math.Floor(q / 11);
            double m = q + 2 - (12 * s4);
            double j = (100 * (n - 49)) + i + s4;
            return new DateTime(Convert.ToInt32(j), Convert.ToInt32(m), Convert.ToInt32(d));
        }

        ///  Отладочная трассировка с выводом номера строки 
        ///  и имени файла вызвавшей процедуры
        public static void TraceString(string trace_message)
        {
            System.Diagnostics.StackFrame sf
                = new System.Diagnostics.StackFrame(1, true);
            // string fnShort = sf.GetFileName();
            Trace.WriteLine(trace_message + "  @ - " + sf.GetFileLineNumber()
                   + " : " +  System.IO.Path.GetFileName(sf.GetFileName()));

            Trace.Flush();
        }

    }
}
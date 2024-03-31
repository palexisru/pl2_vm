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
using System.Data.Common;

namespace pl2.Data.HTML.Field
{
    public partial class Field
    {
        public const int field_definition_chars = 67; // символов в описании поля
        public const string empty_field_definition  = "<tr><td>                </td><td> </td><td>  </td><td> </td></tr>\r\n";

        public HTML_table data_table;
        public string field_name;
        public string field_type;
        public int field_length;
        public int field_dec;
        public int field_offset;

        public Field(HTML_table tbl)
        {
            data_table = tbl;
        }
    }
}



namespace System.IO.HTML.DBF
{
    /// <summary>
    /// This is the field descriptor structure. There will be one of these for each column in the table.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct HTML_DBF_Field_Descriptor
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public readonly string trtd0; // = "<tr><td>"

        /// <summary>The field name.</summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public readonly string field_name;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public readonly string etdtd1; //  = "</td><td>"

        /// <summary>The field type.</summary>
        public readonly char field_type; //  = "U"


        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public readonly string etdtd2; //  = "</td><td>"

        /// <summary>The field length in bytes.</summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public readonly string field_length; //  = " 0"

        /// <summary>The field address.</summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public readonly string etdtd4; //  = "</td><td>"


        /// <summary>The field precision.</summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public readonly string field_decimal; //  = "0"

        /// <summary>The field address.</summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public readonly string etdetr; //  = "</td></tr>"

        /// <summary>The field address.</summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public readonly string ecrlf; //  = "\r\n"


        public override string ToString()
        {
            return String.Format("{0} {1}({2},{3})", field_name, field_type, field_length, field_decimal);
        }
    }
}
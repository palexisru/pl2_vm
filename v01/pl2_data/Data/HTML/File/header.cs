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

namespace System.IO
{

    public partial struct HTML_File_header{

        public int description_table_pos, description_table_tr_pos, field_table_pos, field_table_tr_pos, data_table_pos, data_table_tr_pos, pos_t4;
        public int number_of_records, record_lenght;
        public int field_count; 
        public int fields_start;
        public int records_start; 
        public int record_length;
        public int records_count;
    }


        public enum DBFFieldType : ushort
        {
            Auto = 'A',
            Character = 'C',
            Numeric = 'N',
            Float = 'F',
            Date = 'D',
            DateTime = 'T',
            Logical = 'L',
        }

}

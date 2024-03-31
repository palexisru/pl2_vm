using System;
using System.Text;

namespace pl2.Data.HTML.Record
{
    public class Read_me
    {
    }
    public class Data
    {
        public const string charset_string = "charset=";
        public const int std_codepage = 1521;
        public int codepage;             // активная кодовая страница
        public long field_description_len; // длина описания одного поля
        public long field_description_position; // начало описания полей
        public long field_count;          // количество полей
        public long field_count_position; // позиция считывания количества полей
        public long header_size;          // размер заголовка до начала записей (до <table>)
        public long header_size_position; // позиция считывания размера заголовка
        public long reclength;            // размер записи
        public long reclength_position;   // позиция считывания размера записи
        public long reccount;             // количество записей
        public long reccount_position;    // позиция количества записей
        public long current_record_position;
        public Encoding e;
    }
}


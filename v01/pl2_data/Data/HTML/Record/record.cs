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
        public int codepage;             // �������� ������� ��������
        public long field_description_len; // ����� �������� ������ ����
        public long field_description_position; // ������ �������� �����
        public long field_count;          // ���������� �����
        public long field_count_position; // ������� ���������� ���������� �����
        public long header_size;          // ������ ��������� �� ������ ������� (�� <table>)
        public long header_size_position; // ������� ���������� ������� ���������
        public long reclength;            // ������ ������
        public long reclength_position;   // ������� ���������� ������� ������
        public long reccount;             // ���������� �������
        public long reccount_position;    // ������� ���������� �������
        public long current_record_position;
        public Encoding e;
    }
}


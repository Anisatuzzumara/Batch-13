// See https://aka.ms/new-console-template for more information

using System;

namespace PartialClass
{
    public partial class Karyawan
    {
        public string Nama { get; set; }
        public int Umur { get; set; }

        public void TampilkanIdentitas()
        {
            Console.WriteLine($"Nama: {Nama}, Umur: {Umur}");
        }
    }

    // Bagian kedua dari kelas (masih Mahasiswa)
    public partial class Karyawan
    {
        public string Status()
        {
            return Umur >= 18 ? "Dewasa" : "Belum Dewasa";
        }
    }

    class Kantor
    {
        static void Main(string[] args)
        {
            Karyawan spg = new Karyawan();
            spg.Nama = "Dina";
            spg.Umur = 20;

            spg.TampilkanIdentitas();
            Console.WriteLine($"Status: {spg.Status()}");
        }
    }
}
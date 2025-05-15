// See https://aka.ms/new-console-template for more information

using System;
namespace Mahasiswa
{
    public class NilaiMhs
    {
        public string Nama { get; set; }
        public int Nilai { get; set; }

        public NilaiMhs(string nama, int nilai)
        {
            Nama = nama;
            Nilai = nilai;
        }

        public string NilaiNya()
        {
            if (Nilai >= 85) return "A";
            else if (Nilai >= 75) return "B";
            else if (Nilai >= 65) return "C";
            else return "D";
        }


        public void TampilkanHasil()
        {
            Console.WriteLine($"Hallo Nama: {Nama} , Kamu mendapatkan nilai: {Nilai} , Dengan Grade: {NilaiNya()}");

        }
    }
    class Hasil
    {
        static void Main(string[] args)
        {
            NilaiMhs nilaimhs1 = new NilaiMhs("Sherina", 89);
            NilaiMhs nilaimhs2 = new NilaiMhs("Agung", 76);
            NilaiMhs nilaimhs3 = new NilaiMhs("Anindita", 70);
            NilaiMhs nilaimhs4 = new NilaiMhs("Anugerah", 60);

            nilaimhs1.TampilkanHasil();
            nilaimhs2.TampilkanHasil();
            nilaimhs3.TampilkanHasil();
            nilaimhs4.TampilkanHasil();
        }

                
    }
}












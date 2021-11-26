using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace transponowanko
{
    class Excel
    {
        public static void ExcelCreateFile(List<string> headerList, List<string> dataList, string nameOfFile, string directoryName )
        {
            DateTime localDate = DateTime.Now;


            nameOfFile = Regex.Replace(nameOfFile, @"\s+", string.Empty);
            //  string sciezka = @"C:\\TEST_LOG\\logi_gotowe\\";
            string sciezka = directoryName + "\\";
            if (sciezka.Length < 2)
            {
                sciezka = @"C:\\TEST_LOG\\logi_gotowe\\";
            }


            string sourceFile = @sciezka + @nameOfFile + @localDate.ToString("_yyyy'.'MM'.'dd'_['HH'.'mm'.'ss]") + @".xlsx";


            if (Directory.Exists(sciezka))       //sprawdzanie czy  istnieje
            {
                ;
            }
            else
                System.IO.Directory.CreateDirectory(sciezka); //jeśli nie to ją tworzy


            var workbook = new XLWorkbook();
            workbook.AddWorksheet("sheetName");
            var ws = workbook.Worksheet("sheetName");
            

            headerList = headerList.Select(x => Regex.Replace(x, @"\s+", string.Empty)).ToList();
            var col = 1;
            foreach (var item in headerList)
            {

                var sub3 = item.Split(',');

                for (int q = 1; q <= sub3.Length; q++)
                {
                    ws.Cell(ExcelColumnFromNumber(q) + col.ToString()).Value = Regex.Replace(sub3[q - 1], @"\s+", string.Empty);
                }

                col++;
            }

            col = 10;
            foreach (var item in dataList)
            {

                var sub3 = item.Split(',');

                for (int q = 1; q <= sub3.Length; q++)
                {
                    ws.Cell(ExcelColumnFromNumber(col) + q.ToString()).Value = Regex.Replace(sub3[q - 1], @"\s+", string.Empty);
                }

                col++;
            }

            workbook.SaveAs(@sourceFile);
            MessageBox.Show("Sprawdź lokalizację: " + sciezka + "wprowadzonynumer_yyyy.mm.dd_[hh.mm.ss].xsls");
            MessageBox.Show("Zapisano plik: " + @sourceFile);
        }

        public static string ExcelColumnFromNumber(int column)
        {
            string columnString = "";
            int columnNumber = column;
            while (columnNumber > 0)
            {
                int currentLetterNumber = (columnNumber - 1) % 26;
                char currentLetter = (char)(currentLetterNumber + 65);
                columnString = currentLetter + columnString;
                columnNumber = (columnNumber - (currentLetterNumber + 1)) / 26;
            }
            return columnString;
        }

    }
}

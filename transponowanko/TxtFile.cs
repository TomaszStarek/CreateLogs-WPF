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
    class TxtFile
    {
        public static void ToTxtFile(List<string> headerList, List<string> dataList, string nameOfFile, string directoryName)
        {
            headerList = headerList.Select(x => Regex.Replace(x, @"\s+", string.Empty)).ToList();

            for (int p = 0; p < 5; p++)
            {
                headerList.Add(",");
            }

            var findedListSplitted = dataList.Select(x => x.Split(',')).ToList();

            DateTime localDate = DateTime.Now;

            nameOfFile = Regex.Replace(nameOfFile, @"\s+", string.Empty);

            string sciezka = directoryName + "\\";
            if (sciezka.Length < 2)
            {
                sciezka = @"C:\\TEST_LOG\\logi_gotowe\\";
            }


            string sourceFile = @sciezka + @nameOfFile + @localDate.ToString("_yyyy'.'MM'.'dd'_['HH'.'mm'.'ss]") + @".txt";


            if (Directory.Exists(sciezka))  
            {
                ;
            }
            else
                System.IO.Directory.CreateDirectory(sciezka); 


            using (StreamWriter sw = new StreamWriter(sourceFile))
            {

                var k = 0;
                headerList.ForEach(i =>
                {

                    sw.Write(i + ",,,");

                    int count = i.Split(',').Length - 1;

                    for (int q = count; q < 5; q++)
                    {
                        sw.Write(",");
                    }


                    for (int q = 0; q < findedListSplitted.Count; q++)
                    {
                        if (findedListSplitted[q].Length > k)
                            sw.Write(findedListSplitted[q][k] + ",");
                        else
                            sw.Write(",");
                    }
                    sw.Write("\r\n");
                    k++;
                });
            }
            MessageBox.Show("Sprawdź lokalizację: " + @sciezka + "wprowadzonynumer_yyyy.mm.dd_[hh.mm.ss].txt");
            MessageBox.Show("Zapisano plik: " + @sourceFile);
        }

    }
}

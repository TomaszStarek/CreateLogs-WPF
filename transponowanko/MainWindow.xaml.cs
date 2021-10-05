using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Path = System.IO.Path;
using ExcelLibrary;
using ExcelLibrary.SpreadSheet;
using SwiftExcel;

namespace transponowanko
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        string myString = "noname";

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                myString = textBox1.Text;
                int i = 0;
                foreach (string disp in data_array)
                {
                    string bufor;
                    bufor = Regex.Replace(disp, @"\s+", string.Empty);
                    //  MessageBox.Show(disp);
                    if (bufor.Contains(myString))
                    {
                        //  MessageBox.Show(bufor);
                        finded_list.Add(bufor);
                        i++;
                    }
                }

                if (i <= 0)
                {
                    finded_string = false;
                    MessageBox.Show("Nie znaleziono podanego ciągu znaków!");

                    Dispatcher.Invoke(new Action(() => label_state.Content = "3. Podaj numer który występuje w pliku \r\n i wciśnij enter!"));
                }
                else
                {
                    finded_string = true;
                //    MessageBox.Show("Znaleziono: " + i.ToString() + " logów");


                   // Dispatcher.Invoke(new Action(() => label_sciezka_dane.Content = filename));
                    Dispatcher.Invoke(new Action(() => button4.Visibility = Visibility.Visible));
                    Dispatcher.Invoke(new Action(() => button3.Visibility = Visibility.Visible));
                    Dispatcher.Invoke(new Action(() => label_state.Content = "4. Wygeneruj plik!"));
                }
                    
                //var match = data_list
                //            .Where(stringToCheck => stringToCheck.Contains(myString));
                //// textBox1.Text = match[0];


                //MessageBox.Show(match.Count().ToString());
                //int i = 0;

            }
        }



        bool finded_string;
        string[][] subs;
        List<string> header_list = new List<string>();
        List<string> data_list = new List<string>();
        List<string> finded_list = new List<string>();

        string[] header_array = new string[] { };
        string[] data_array = new string[] { };
        //          string[] data2_array = new string[] { };

        string fileContent = string.Empty;
        string filePath = string.Empty;
        string directoryName;

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            Button btnSender = (Button)sender;
            if (btnSender == button1)
            {
                select_fill_arr(1);
            }
            else if (btnSender == button2)
                select_fill_arr(2);

        }


        private void select_fill_arr(int mode)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            if (mode == 1)
                openFileDialog.Filter = "Text files (*.txt;*.TST)|*.txt;*.TST|All files (*.*)|*.*";
            else
                openFileDialog.Filter = "Text files (*.txt;*.DAT;)|*.txt;*.DAT;|All files (*.*)|*.*";

            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            //     openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            
            filePath = openFileDialog.FileName;
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    if (mode == 1)
                    {
                        Dispatcher.Invoke(new Action(() => label_sciezka_naglowek.Content = filename));
                        Dispatcher.Invoke(new Action(() => button2.Visibility = Visibility.Visible));
                        Dispatcher.Invoke(new Action(() => label3_Copy.Visibility = Visibility.Visible));
                        Dispatcher.Invoke(new Action(() => label_sciezka_dane.Visibility = Visibility.Visible));

                        Dispatcher.Invoke(new Action(() => textBox1.Visibility = Visibility.Hidden));
                        Dispatcher.Invoke(new Action(() => label.Visibility = Visibility.Hidden));

                        Dispatcher.Invoke(new Action(() => button4.Visibility = Visibility.Hidden));
                        Dispatcher.Invoke(new Action(() => button3.Visibility = Visibility.Hidden));

                        Dispatcher.Invoke(new Action(() => label_state.Content = "2. Wybierz plik z danymi!"));
                    }
                    else
                    {
                        directoryName = Path.GetDirectoryName(filename);
                        Dispatcher.Invoke(new Action(() => label_sciezka_dane.Content = filename));
                        Dispatcher.Invoke(new Action(() => textBox1.Visibility = Visibility.Visible));
                        Dispatcher.Invoke(new Action(() => label.Visibility = Visibility.Visible));

                        Dispatcher.Invoke(new Action(() => button4.Visibility = Visibility.Hidden));
                        Dispatcher.Invoke(new Action(() => button3.Visibility = Visibility.Hidden));

                        Dispatcher.Invoke(new Action(() => label_state.Content = "3. Wprowadź numer seryjny \r\n i wciśnij enter!"));
                    }
                      //  label_sciezka_dane.Content = filename;

                    //  lbFiles.Items.Add(Path.GetFileName(filename));
                    //  lbFiles.Items.Add(filename);
                }


                //                string[] subs = to_split.Split(',');

                var fileStream = openFileDialog.OpenFile();



                using (StreamReader reader = new StreamReader(fileStream))
                {

                    fileContent = reader.ReadToEnd();

                    reader.Close();
                }

                if (mode == 1)
                    header_array = fileContent.Split('\r');
                if (mode == 2)
                    data_array = fileContent.Split('\r');

            }

        }

        private void tworzenie_loga()
        {
            DateTime localDate = DateTime.Now;



             myString = Regex.Replace(myString, @"\s+", string.Empty);
            //  string sciezka = @"C:\\TEST_LOG\\logi_gotowe\\";
            string sciezka = directoryName + "\\";
            if (sciezka.Length < 2)
            {
                sciezka = @"C:\\TEST_LOG\\logi_gotowe\\";
            }


            string sourceFile = @sciezka + @myString + localDate.ToString("_yyyy'.'MM'.'dd'_['HH'.'mm'.'ss]") + @".txt";


            if (Directory.Exists(sciezka))       //sprawdzanie czy  istnieje
            {
                ;
            }
            else
                System.IO.Directory.CreateDirectory(sciezka); //jeśli nie to ją tworzy


            using (StreamWriter sw = new StreamWriter(sourceFile))
            {
                string[][] tofile = new string[finded_list.Count][];
                int i = 0;
                foreach (string disp in finded_list)
                {
                    tofile[i] = disp.Split(',');
                    i++;
                }

                header_array = header_array.Concat(new string[] { "" }).ToArray();
                header_array = header_array.Concat(new string[] { "" }).ToArray();
                header_array = header_array.Concat(new string[] { "" }).ToArray();

                int rozmiar = 0;
                int k = 0;
                foreach (string disp in header_array)
                {

                    string bufor;
                    bufor = Regex.Replace(disp, @"\s+", string.Empty);

                    sw.Write(bufor + ",,,");

                    int count = bufor.Split(',').Length - 1;

                    for (int q = count; q < 5; q++)
                    {
                        sw.Write(",");
                    }



                    for (int j = 0; j < finded_list.Count; j++)
                    {
                        if (rozmiar < tofile[j].GetLength(0))
                        {
                            rozmiar = tofile[j].GetLength(0) - 1;
                        }

                        if (k < tofile[j].GetLength(0))
                            sw.Write(tofile[j][k] + ",");
                        else
                            sw.Write(",");
                    }
                    sw.Write("\r\n");


                    if (k < rozmiar)
                        k++;

                }





            }

                MessageBox.Show("Sprawdź lokalizację: " + sciezka + "wprowadzonynumer_yyyy.mm.dd_[hh.mm.ss].txt");


        }





        private void button3_Click(object sender, RoutedEventArgs e)
        {
            string elo = "eloelo";
            if (elo.StartsWith("elo"))
            {
                MessageBox.Show("elo");
            }
            else
            {
                MessageBox.Show("nieelo");
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (finded_string)
                tworzenie_loga();
            else
                MessageBox.Show("Wprowadź poprawny numer do wyszukania!");

            

            //foreach (string disp in header_array)
            //    MessageBox.Show(disp);

            //foreach (string disp in data_array)
            //    MessageBox.Show(disp);

            ////////////////////////////foreach (string disp in finded_list)
            ////////////////////////////    MessageBox.Show(disp);


            //foreach (string disp2 in subs[0])
            //    MessageBox.Show(disp2);
            //foreach (string disp2 in subs[1])
            //    MessageBox.Show(disp2);
            //foreach (string disp2 in subs[2])
            //    MessageBox.Show(disp2);
        }

        string[] bufor_to_excel1;
        string[] bufor_to_excel2;
        

        private void save_file_excel()
        {
            int array_ln = header_array.GetLength(0);
            if (array_ln < 10)
            {
                array_ln = 120;
            }


            string[,] bufor_to_excel3 = new string[finded_list.Count + 8, array_ln + 25];

            //     string file = "C:/logi/newdoc.xls";
            //     Workbook workbook = new Workbook();
            //     Worksheet worksheet = new Worksheet("First Sheet");
            //     worksheet.Cells[0, 1] = new Cell((short)1);
            //    worksheet.Cells[2, 0] = new Cell(9999999);
            //    worksheet.Cells[3, 3] = new Cell((decimal)3.45);
            //   worksheet.Cells[2, 2] = new Cell("Text string");
            //   worksheet.Cells[2, 4] = new Cell("Second string");
            //   worksheet.Cells[4, 0] = new Cell(32764.5, "#,##0.00");
            //  worksheet.Cells[5, 1] = new Cell(DateTime.Now, @"YYYY-MM-DD");
            //  worksheet.Cells.ColumnWidth[0, 1] = 3000;
            //   workbook.Worksheets.Add(worksheet);
            //   workbook.Save(file);

            //header_array

            string file_name;

            if (textBox1.Text.Length > 2)
                file_name = textBox1.Text;
            else
                file_name = "noname";


                for (var row = 1; row < header_array.GetLength(0); row++)
                {

                    bufor_to_excel1 = header_array[row - 1].Split(',');

                    for (var col = 1; col < bufor_to_excel1.GetLength(0); col++)
                    {
                        bufor_to_excel3[col, row] = bufor_to_excel1[col - 1];
                        //  ew.Write(bufor_to_excel1[col - 1], col, row);
                    }

                    //  sw.Write(tofile[j][k] + ",");
                }


                for (var index = 0; index < finded_list.Count; index++)
                {
                    bufor_to_excel2 = finded_list[index].Split(',');
                    for (var row = 1; row < bufor_to_excel2.Length; row++)
                    {
                        bufor_to_excel3[7 + index, row] = bufor_to_excel2[row - 1];
                        //  ew.Write(bufor_to_excel2[row - 1], 17+index, row);
                    }
                }

            DateTime localDate = DateTime.Now;

            string sciezka = directoryName + "\\";
            if (sciezka.Length < 2)
            {
                sciezka = @"C:\\TEST_LOG\\logi_gotowe\\";
            }


           



            if (Directory.Exists(sciezka))       //sprawdzanie czy  istnieje
            {
                ;
            }
            else
                System.IO.Directory.CreateDirectory(sciezka); //jeśli nie to ją tworzy




            using (var ew = new ExcelWriter(sciezka + file_name + localDate.ToString("_yyyy'.'MM'.'dd'_['HH'.'mm'.'ss]") + ".xlsx"))
                {

                    for (var row = 1; row < bufor_to_excel3.GetLength(1); row++)
                    {
                        for (var col = 1; col < bufor_to_excel3.GetLength(0); col++)
                        {


                            ew.Write(bufor_to_excel3[col, row], col, row);
                        }
                    }
                }
            MessageBox.Show("Sprawdź lokalizację: " + sciezka + "wprowadzonynumer_yyyy.mm.dd_[hh.mm.ss].txt");

        }

        private void button3_Click_1(object sender, RoutedEventArgs e)
        {
            if (finded_string)
                save_file_excel();
            else
                MessageBox.Show("Wprowadź poprawny numer do wyszukania!");
            
            //if (finded_string)
            //    MessageBox.Show("Sprawdź folder E:\\TEST_LOG\\logi_gotowe\\szukany_numer.txt");
            //else
            //    MessageBox.Show("Wprowadź poprawny numer do wyszukania!");


            
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            finded_string = false;
        }




        private void testowanie_dousuniecia()
        {
            Dispatcher.Invoke(new Action(() => textBox1.Text = "" ));
        }




    }
}

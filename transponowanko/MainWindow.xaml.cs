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
using ClosedXML.Excel;

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

        private bool _finded_string;

        private List<string> finded_list = new List<string>();

        private List<string> header_list = new List<string>();
        private List<string> data_list = new List<string>();

        private string[] header_array = new string[] { };
        private string[] data_array = new string[] { };

        private string _directoryName;
        private string _myString = "noname";
        private int _findedListCount;

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Return)
            {
                if (textBox1.Text.Length > 2)
                    _myString = textBox1.Text;
                textBox1.Text = string.Empty;

                //Func<string, string> finded_list2 = (x) => Regex.Replace(x, @"\s+", string.Empty);
                //  finded_list = data_list.Where(x => x.Contains(myString)).Select(x => Regex.Replace(x, @"\s+", string.Empty)).ToList();

                finded_list.AddRange(data_list.Where(x => x.Contains(_myString)).Select(x => Regex.Replace(x, @"\s+", string.Empty)));

                if (_findedListCount < finded_list.Count)
                {
                    _findedListCount = finded_list.Count;
                    Dispatcher.Invoke(new Action(() => labelNapisZnalezioneNumery.Visibility = Visibility.Visible));
                    Dispatcher.Invoke(new Action(() => labelZnalezioneNumery.Content += _myString + "\r\n"));
                }
                else
                    MessageBox.Show("Nie znaleziono podanego ciągu znaków!");


                if (finded_list.Count == 0)
                {
                    
                    _finded_string = false;
                    MessageBox.Show("Nie znaleziono podanego ciągu znaków!");

                    Dispatcher.Invoke(new Action(() => label_state.Content = "3. Podaj numer i wciśnij enter! \r\n Lub ESC żeby usunąć \r\n znalezione numery"));
                    Dispatcher.Invoke(new Action(() => labelNapisZnalezioneNumery.Visibility = Visibility.Hidden));
                    Dispatcher.Invoke(new Action(() => labelZnalezioneNumery.Content = string.Empty));
                }
                else
                {
                    _finded_string = true;

                    Dispatcher.Invoke(new Action(() => button4.Visibility = Visibility.Visible));
                    Dispatcher.Invoke(new Action(() => button3.Visibility = Visibility.Visible));
                    Dispatcher.Invoke(new Action(() => label_state.Content = "4. Wygeneruj plik!"));
                }

            }
            else if (e.Key == Key.Escape)
            {
                _finded_string = false;
                _findedListCount = 0;
                Dispatcher.Invoke(new Action(() => label_state.Content = "3. Podaj numer i wciśnij enter! \r\n Lub ESC żeby usunąć \r\n żeby wyczyścić znalezione numery"));
                finded_list.Clear();
                _myString = string.Empty;

                Dispatcher.Invoke(new Action(() => textBox1.Visibility = Visibility.Visible));
                Dispatcher.Invoke(new Action(() => label.Visibility = Visibility.Visible));

                Dispatcher.Invoke(new Action(() => button4.Visibility = Visibility.Hidden));
                Dispatcher.Invoke(new Action(() => button3.Visibility = Visibility.Hidden));
                Dispatcher.Invoke(new Action(() => labelZnalezioneNumery.Content = string.Empty));
                
            }
        }

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
            //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //filePath = openFileDialog.FileName;
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
                        _directoryName = Path.GetDirectoryName(filename);
                        Dispatcher.Invoke(new Action(() => label_sciezka_dane.Content = filename));
                        Dispatcher.Invoke(new Action(() => textBox1.Visibility = Visibility.Visible));
                        Dispatcher.Invoke(new Action(() => label.Visibility = Visibility.Visible));

                        Dispatcher.Invoke(new Action(() => button4.Visibility = Visibility.Hidden));
                        Dispatcher.Invoke(new Action(() => button3.Visibility = Visibility.Hidden));

                        Dispatcher.Invoke(new Action(() => label_state.Content = "3. Podaj numer i wciśnij enter! \r\n Lub ESC żeby usunąć \r\n znalezione numery"));
                    }

                }

                var fileStream = openFileDialog.OpenFile();

                var fileContent = string.Empty;

                using (StreamReader reader = new StreamReader(fileStream))
                {

                    fileContent = reader.ReadToEnd();
                    reader.Close();
                }

                if (mode == 1)
                {
                    header_array = @fileContent.Split('\r');
                    header_list = fileContent.Split('\r').ToList(); //.Select(x => x + '>').ToList(); 
                }
                if (mode == 2)
                {
                    data_array = @fileContent.Split('\r');
                    data_list = fileContent.Split('\r').ToList();
                }

            }

        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            //tworzenie_loga();

            if (_finded_string)
                TxtFile.ToTxtFile(header_list, finded_list, _myString, _directoryName);
            else
                MessageBox.Show("Wprowadź poprawny numer do wyszukania!");

        }

        private void button3_Click_1(object sender, RoutedEventArgs e)
        {
            if (_finded_string)
                Excel.ExcelCreateFile(header_list, finded_list,_myString,_directoryName);
            else
                MessageBox.Show("Wprowadź poprawny numer do wyszukania!");

        }


    }
    }



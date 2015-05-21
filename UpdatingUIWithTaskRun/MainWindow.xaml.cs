using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace UpdatingUIWithTaskRun
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

        private async void StartProcess(object sender, RoutedEventArgs e)
        {
            var listOfStrings = await GenerateItems();

            _listBox.ItemsSource = listOfStrings;
        }

        Task<List<String>> GenerateItems()
        {
            IProgress<double> progress = new Progress<double>(UpdateProgressText);

            return Task.Run(() =>
            {
                int maxRecords = 1000000;

                List<String> listOfStrings = new List<string>();
                for (int i = 0; i < maxRecords; i++)
                {
                    //we don't want to flood the UI message loop so do periodic updates
                    if (i % 1500 == 0)
                    {
                        double percentage = (double)i / maxRecords;
                        progress.Report(percentage);
                    }

                    listOfStrings.Add(String.Format("Item: {0}", i));
                }

                return listOfStrings;
            });
        }

        public void UpdateProgressText(double percentage)
        {
            _progressBar.Value = percentage;
            _progressText.Text = (percentage).ToString("0%");
        }
    }
}

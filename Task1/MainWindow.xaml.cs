using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Task1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int totalPages = 0;
        int openPages = 0;
        int pageSize = 5;
        int currentPage = 1;
        bool isStart = true;

        public MainWindow()
        {
            InitializeComponent();
            GetNewFilterFiles();
        }

        private void GetNewFilterFiles()
        {
            DirectoryInfo directory = new DirectoryInfo(@"C:\Temp\ispp21\LabWork33");//любой путь к файлам
            FileInfo[] files = directory.GetFiles("*", SearchOption.AllDirectories);
            var result = files.Select(file => new { file.Name, file.Extension, file.FullName, file.Length, file.CreationTime }).OrderBy(file => file.FullName).Take(openPages + pageSize).ToList();
            openPages = result.Count;
            if(pageSize!=0)
                totalPages = Convert.ToInt32(Math.Ceiling((double)files.Count() / pageSize));
            ResultDataGrid.ItemsSource = result;

            if (result.Count == files.Count())
            {
                MoreButton.IsEnabled = false;
                goRightButton.IsEnabled = false;
                goEndButton.IsEnabled = false;
            }
            else
            {
                MoreButton.IsEnabled = true;
                goRightButton.IsEnabled = true;
                goEndButton.IsEnabled = true;
            }

            if (result.Count < 2)
            {
                goLeftButton.IsEnabled = false;
                goStartButton.IsEnabled = false;
            }
            else
            {
                goLeftButton.IsEnabled = true;
                goStartButton.IsEnabled = true;
            }

            CountFilesLabel.Content = $"Показано: {result.Count} из {files.Length}";
        }

        private void MoreButton_Click(object sender, RoutedEventArgs e)
        {
            GetNewFilterFiles();
            isStart = false;
            currentPage++;
            nowCountPagesTextBox.Text = currentPage.ToString();
        }

        private void CountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(CountTextBox.Text, out int newCount);
            pageSize = newCount;
            if (isStart)
            {
                openPages = 0;
                GetNewFilterFiles();
            }
        }

        private void NowCountPagesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(nowCountPagesTextBox.Text, out int newPage);
            currentPage = newPage;
            openPages = pageSize * currentPage - pageSize;
            GetNewFilterFiles();
        }

        private void goStartButton_Click(object sender, RoutedEventArgs e)
        {
            openPages = 0;
            GetNewFilterFiles();
            nowCountPagesTextBox.Text = "1";
        }

        private void goLeftButton_Click(object sender, RoutedEventArgs e)
        {
            openPages = openPages - 2 * pageSize;
            GetNewFilterFiles();
            int.TryParse(nowCountPagesTextBox.Text, out int newPage);
            nowCountPagesTextBox.Text = (newPage - 1).ToString();
        }

        private void goRightButton_Click(object sender, RoutedEventArgs e)
        {
            GetNewFilterFiles();
            int.TryParse(nowCountPagesTextBox.Text, out int newPage);
            nowCountPagesTextBox.Text = (newPage + 1).ToString();
        }

        private void goEndButton_Click(object sender, RoutedEventArgs e)
        {
            openPages = totalPages * pageSize - pageSize;
            GetNewFilterFiles();
            nowCountPagesTextBox.Text = openPages.ToString();
        }
    }
}
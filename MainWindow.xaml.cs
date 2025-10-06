using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tabella;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    
    public class Udp
    {
        public string name {  get; set; }
        public List<int> D { get; set; }
        public int total { get; set; }
    };
    
    public MainWindow()
    {

        InitializeComponent();
        
        List<Udp> people = new List<Udp>()
        {
            new Udp { name = "Alice", D = {1, 2}, total = 3 },
            new Udp { name = "Bob", D = {4, 5, 6}, total = 15 }
        };
        table.ItemsSource = people;
    }

    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void btnGenerate_Click(object sender, RoutedEventArgs e)
    {
        table.Items.Add(new { Name = "John", Age = 30 });
    }
}
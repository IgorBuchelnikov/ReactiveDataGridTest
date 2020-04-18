using DynamicData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
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

namespace ReactiveDataGridTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // For mutation of incoming bus messages
        private readonly SourceList<DataGridViewModel> _dataList;

        private readonly MainViewModel mainViewModel;
        private readonly DataSource dataSource;

        public MainWindow()
        {
            InitializeComponent();

            _dataList = new SourceList<DataGridViewModel>();
            dataSource = new DataSource();

            mainViewModel = new MainViewModel(_dataList);

            DataContext = mainViewModel;

        }

        private void DataSource_IncomingData(DataGridViewModel dataGridViewModel)
        {
            _dataList.Add(dataGridViewModel);
        }

        private void Button_Click(object sender, RoutedEventArgs evt)
        {
            dataSource.StartProducingData();

            var observableEvent = Observable.FromEventPattern<EventHandler<DataReceived>, DataReceived>(
                        e => e.Invoke,
                        e => dataSource.IncomingData += e,
                        e => dataSource.IncomingData -= e)
                        .ObserveOn(SynchronizationContext.Current)
                        .Subscribe(e => DataSource_IncomingData(e.EventArgs.DataGridViewModel));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}

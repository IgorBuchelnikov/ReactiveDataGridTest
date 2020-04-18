using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDataGridTest
{
	public class MainViewModel : ReactiveObject
	{
		[Reactive] public string SearchPhrase { get; set; }

		// For binding to UI
		private readonly ReadOnlyObservableCollection<DataGridViewModel> _itemsBinding;

        public ReadOnlyObservableCollection<DataGridViewModel> TargetCollection => _itemsBinding;

        public MainViewModel(SourceList<DataGridViewModel> _dataList)
        {

            AddSomeRandomData(_dataList);

            var _observableSearchPhrase = this
                .WhenPropertyChanged(x => x.SearchPhrase, false)
                .Where(x => !string.IsNullOrEmpty(x.Value) || !string.IsNullOrWhiteSpace(x.Value))
                .Select(x => FilterOnId(x.Value));

            _dataList.Connect()
                .ObserveOnDispatcher()
                .Filter(_observableSearchPhrase)
                .Bind(out _itemsBinding)
                .DisposeMany()
                .Subscribe();

        }

        private static void AddSomeRandomData(SourceList<DataGridViewModel> _dataList)
        {
            var random = new Random();
            for (int i = 0; i < 300_000; i++)
            {

                var buffer = new byte[random.Next(8, 100)];

                random.NextBytes(buffer);
                string hexData = BitConverter.ToString(buffer);

                var dataGridViewModel = new DataGridViewModel
                {
                    TimeStamp = DateTime.Now.ToString("HH:mm:ss.ffffff"),
                    Id = random.Next(1, 10).ToString(),
                    HexData = hexData
                };

                _dataList.Add(dataGridViewModel);
            }
        }

        private Func<DataGridViewModel, bool> FilterOnId(string id)
        {
            int idAsInt = ParseToInt(id);

            if (idAsInt == 0) return n => true;

            return n => ParseToInt(n.Id) == idAsInt;
        }

        private int ParseToInt(string id) => int.Parse(id);
	}
}

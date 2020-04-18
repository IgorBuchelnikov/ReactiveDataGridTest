using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ReactiveDataGridTest
{
    public class DataGridViewModel : ReactiveObject
    {
        [Reactive]
        public string TimeStamp { get; set; }

        [Reactive]
        public string HexData { get; set; }

        [Reactive]
        public string Id { get; set; }
    }
}

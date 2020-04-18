using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDataGridTest
{
    public class DataSource
    {
        public event EventHandler<DataReceived> IncomingData;

        public void StartProducingData()
        {
            Task.Run(async () =>
            {
                for (int i = 0; i < 100_000; i++)
                {
                    var random = new Random();
                    var buffer = new byte[random.Next(8, 100)];

                    random.NextBytes(buffer);
                    string hexData = BitConverter.ToString(buffer);

                    var dataGridViewModel = new DataGridViewModel
                    {
                        TimeStamp = DateTime.Now.ToString("HH:mm:ss.ffffff"),
                        Id = random.Next(1, 10).ToString(),
                        HexData = hexData
                    };

                    var dataReceived = new DataReceived { DataGridViewModel = dataGridViewModel };

                    OnDataReceived(dataReceived);

                    await Task.Delay(10);
                }
            });
        }

        protected virtual void OnDataReceived(DataReceived eventArgs)
        {
            IncomingData?.Invoke(this, eventArgs);
        }

    }

    public class DataReceived
    {
        public DataGridViewModel DataGridViewModel { get; set; }
    }

}

using System.Net.WebSockets;
using System.Text;

namespace ApplicationB
{
    public partial class MainPage : ContentPage
    {
        private ClientWebSocket _webSocket;

        public MainPage()
        {
            InitializeComponent();
            _webSocket = new ClientWebSocket();
        }

        private async void OnConnectClicked(object sender, EventArgs e)
        {
            // Call the REST API from Application A to start streaming
            var client = new HttpClient();
            var response = await client.GetAsync("http://localhost:9696/api/start");
            if (response.IsSuccessStatusCode) 
            {
                // Connect to the web socket in Application A
                await _webSocket.ConnectAsync(new Uri("ws://localhost:9696/data"), CancellationToken.None);
                await ReadDataAsync();
            }
        }

        private async Task ReadDataAsync()
        {
            var buffer = new byte[1024];
            while (_webSocket.State == WebSocketState.Open)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var receivedData = Encoding.UTF8.GetString(buffer, 0, result.Count);
                DataLabel.Text = "Data: " + receivedData;
            }
        }
    }
}
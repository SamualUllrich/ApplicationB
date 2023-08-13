using System.Net.WebSockets;
using System.Text;

namespace ApplicationB
{
    public partial class MainPage : ContentPage
    {
        private ClientWebSocket _webSocket;
        private bool _isConnected = false;

        public MainPage()
        {
            InitializeComponent();
            _webSocket = new ClientWebSocket();
        }

        private async void OnConnectClicked(object sender, EventArgs e)
        {

            if (_webSocket != null && _webSocket.State == WebSocketState.Open)
            {
                await DisplayAlert("Connection Error", "Already connected to Application A. Please disconnect before attempting to reconnect.", "OK");
                return;
            }
            
            // Call the REST API from Application A to start streaming
            _webSocket = new ClientWebSocket();
            var client = new HttpClient();
            var response = await client.GetAsync("http://localhost:9696/api/start");

            if (response.IsSuccessStatusCode) 
            {
                _isConnected = true;
                ConnectBtn.Text = "Streaming Started";
                // Connect to the web socket in Application A
                await _webSocket.ConnectAsync(new Uri("ws://localhost:9696/data"), CancellationToken.None);
                await ReadDataAsync();
            }
        }

        private async void OnTerminateClickedAsync(object sender, EventArgs e)
        {
            if (!_isConnected)
            {
                DisplayAlert("Disconnect Error", "Not currently connected to Application A.", "OK");
                return;
            }

            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);

            // Dispose of the WebSocket client
            _webSocket.Dispose();
            _webSocket = null;

            _isConnected = false;
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
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
            try
            {
                if (_webSocket != null && _webSocket.State == WebSocketState.Open)
                {
                    await DisplayAlert("Connection Error", "Already connected to Application A. Please disconnect before attempting to reconnect.", "OK");
                    return;
                }

                string baseUrl;
                Uri webSocketUri;

                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    baseUrl = "http://10.0.2.2:9696/api/start";
                    webSocketUri = new Uri("ws://10.0.2.2:9696/data");
                }
                else
                {
                    baseUrl = "http://localhost:9696/api/start";
                    webSocketUri = new Uri("ws://localhost:9696/data");
                }

                _webSocket = new ClientWebSocket();
                var client = new HttpClient();

                var response = await client.GetAsync(baseUrl);

                if (response.IsSuccessStatusCode)
                {
                    _isConnected = true;
                    ConnectBtn.Text = "Streaming Started";
                    await _webSocket.ConnectAsync(webSocketUri, CancellationToken.None);
                    await ReadDataAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert("Connection Error", $"Failed to connect to Application A: {ex.Message}", "OK");
            }
            catch (WebSocketException ex)
            {
                await DisplayAlert("Connection Error", $"WebSocket connection failed: {ex.Message}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Unexpected Error", $"An unexpected error occurred: {ex.Message}", "OK");
            }
        }

        private async void OnTerminateClickedAsync(object sender, EventArgs e)
        {
            if (!_isConnected)
            {
                _ = DisplayAlert("Disconnect Error", "Not currently connected to Application A.", "OK");
                return;
            }

            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);

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
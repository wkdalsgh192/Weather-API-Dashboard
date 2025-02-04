// Required Namespaces
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace WeatherAPIDashboard
{
    public partial class MainForm : Form
    {
        private readonly string apiKey = "YOUR_API_KEY"; // Replace with your API key
        private readonly string apiUrl = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric";

        public MainForm()
        {
            InitializeComponent();
        }

        private async void btnGetWeather_Click(object sender, EventArgs e)
        {
            string city = txtCity.Text;
            if (!string.IsNullOrEmpty(city))
            {
                await GetWeatherDataAsync(city);
            }
            else
            {
                MessageBox.Show("Please enter a city name.");
            }
        }

        private async Task GetWeatherDataAsync(string city)
        {
            using (HttpClient client = new HttpClient())
            {
                string requestUrl = string.Format(apiUrl, city, apiKey);
                try
                {
                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject weatherData = JObject.Parse(responseBody);

                    lblTemperature.Text = $"Temperature: {weatherData["main"]["temp"]} °C";
                    lblHumidity.Text = $"Humidity: {weatherData["main"]["humidity"]}%";
                    lblDescription.Text = $"Condition: {weatherData["weather"][0]["description"]}";
                }
                catch (HttpRequestException httpEx)
                {
                    MessageBox.Show($"Request error: {httpEx.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeatherApi.Models
{
    [Serializable]
    public class Forecast
    {
        private string _feels = "";

        public string? Condition { get; set; }
        public string? Description { get; set; }

        public string Feels { get { return _feels; } }

        public string? Temperature { get; set; }
        
        public List<Alert> alerts = new List<Alert>();

        public static Forecast? Create(string result)
        {
            try
            {
                var json = (JObject?)JsonConvert.DeserializeObject(result);

                if (json != null)
                {
                    Forecast forecast = new Forecast();
                    if(json.SelectToken("current.weather[0].main") != null) 
                        forecast.Condition = json.SelectToken("current.weather[0].main")?.Value<string>();

                    if (json.SelectToken("current.temp") != null)
                    {
#pragma warning disable CS8604 // Possible null reference argument.
                        double temp = json.SelectToken("current.temp").Value<double>();
#pragma warning restore CS8604 // Possible null reference argument.
                        forecast.SetFeels(temp);
                        forecast.Temperature = temp.ToString() + " °F";
                    }

                    if(json.SelectToken("current.weather[0].description") != null)
                        forecast.Description = json.SelectToken("current.weather[0].description")?.Value<string>();

                    if (json.SelectToken("alerts") != null)
                    {
                        var alert = new Alert();
                        alert.SenderName = json.SelectToken("alerts[0].sender_name")?.Value<string>();
                        alert.Event = json.SelectToken("alerts[0].event")?.Value<string>();
                        alert.Description = json.SelectToken("alerts[0].description")?.Value<string>();
#pragma warning disable CS8604 // Possible null reference argument.
                        double start = json.SelectToken("alerts[0].start").Value<double>();
#pragma warning restore CS8604 // Possible null reference argument.
                        alert.Start = Alert.UnixTimeStampToDateTime(start);
#pragma warning disable CS8604 // Possible null reference argument.
                        double end = json.SelectToken("alerts[0].end").Value<double>();
#pragma warning restore CS8604 // Possible null reference argument.
                        alert.End = Alert.UnixTimeStampToDateTime(end);

                        forecast.alerts.Add(alert);

                    }


                    return forecast;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }


        public void SetFeels(double temp)
        {
            if (temp <= 15)
            {
                _feels = "Frigid";
            }
            else if (temp <= 32)
            {
                _feels = "Freezing";
            }
            else if (temp <= 50)
            {
                _feels = "Cold";
            }
            else if (temp < 65)
            {
                _feels = "Cool";
            }
            else if (temp < 75)
            {
                _feels = "Comfortable";
            }
            else if (temp < 85)
            {
                _feels = "Warm";
            }
            else if (temp < 100)
            {
                _feels = "Hot";
            }
            else if (temp > 95)
            {
                _feels = "Sweltering";
            }
        }
    }
}

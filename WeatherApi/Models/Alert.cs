namespace WeatherApi.Models
{
    [Serializable]
    public class Alert
    {
        public string? SenderName { get; set; }
        public string? Event { get; set; }
        public string? Description { get; set; }
        public DateTime Start { get; set; }

        public DateTime End { get; set; }



        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

    }


}
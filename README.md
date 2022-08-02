# WeatherApi

clone folder from github 
  git clone https://github.com/dvisaria/WeatherApi.git

open folder weatherapi in visual studio code

open new terminal window in vs code and run following command
  > cd weatherapi
  weatherapi\weatherapi> dotnet build
  weatherapi\weatherapi> dotnet run

https://localhost:7121/weatherForecast?lat={lat}&lon={lon}
Valid Ranges:
  -90.0 <= lat <= 90.0
  -180.0 <- lon <= 180.0
  
https://localhost:7121/weatherForecast?lat=37.839333&lon=-84.270020
Output:
  {
    "alerts": [
        {
            "senderName": "NWS Louisville (Central Kentucky)",
            "event": "Flood Watch",
            "description": "...FLOOD WATCH REMAINS IN EFFECT FROM 7 AM CDT /8 AM EDT/ THIS\nMORNING THROUGH MONDAY MORNING...\n* WHAT...Flooding caused by excessive  rainfall continues to be\npossible.\n* WHERE...Portions of east central Kentucky, north central Kentucky,\nnorthwest Kentucky and south central Kentucky, including the\nfollowing counties, in east central Kentucky, Boyle, Garrard,\nJessamine, Madison and Mercer. In north central Kentucky,\nBreckinridge, Hardin, Larue, Nelson and Washington. In northwest\nKentucky, Ohio. In south central Kentucky, Adair, Allen, Barren,\nButler, Casey, Clinton, Cumberland, Edmonson, Grayson, Green,\nHart, Lincoln, Logan, Marion, Metcalfe, Monroe, Russell, Simpson,\nTaylor and Warren.\n* WHEN...From 7 AM CDT /8 AM EDT/ this morning through Monday\nmorning.\n* IMPACTS...Excessive runoff may result in flooding of rivers,\ncreeks, streams, and other low-lying and flood-prone locations.\nCreeks and streams may rise out of their banks.\n* ADDITIONAL DETAILS...\n- Period of showers and thunderstorms are expected across the\nwatch area.  Rainfall rates of 1-2 inches per hour will be\npossible and bring a flash flood threat to portions of\ncentral and southern Kentucky Sunday and Sunday night.  Areas\nthat see repeated rounds of thunderstorm activity will be\nmost susceptible to flash flooding.\n- http://www.weather.gov/safety/flood",
            "start": "2022-07-31T11:10:00-04:00",
            "end": "2022-08-04T14:00:00-04:00"
        }
    ],
    "lat": 37.8393,
    "lon": -84.27,
    "condition": "Clear",
    "description": "clear sky",
    "feels": "Warm",
    "temperature": "80.87 °F"
}

url for swagger document and UI
  https://localhost:7121/swagger/index.html

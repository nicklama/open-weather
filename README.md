# Nick's Open Weather

## Introduction

This application allows users to fetch the current weather description from the OpenWeather API by providing a city and country. The project consists of a ReactJS + Vite front-end which handles the user inputs and presentation, combined with a ASP.NET backend service which manages all the external API calls.

### Front-end

-   [x] Fetches from the backend via a REST URL
-   [x] Allows user to enter city name and country name
-   [x] Presents the result to user
-   [x] Handles any errors/exceptions

### Back-end

-   [x] Created an API Key scheme
-   [x] Enforced rate limiting for each key to 5 requests/hour
-   [x] Feedback is provided when the limit is exceeded
-   [x] Takes inputs from the front-end and calls the external OpenWeather API

## How to Build and Run

1. Install the latest Node.js and npm versions
    - Versions developed with: Node.js (v22.12.0), npm (10.9.2)
1. Install the latest .NET 8 SDK
    - Versions developed with: SDK 8.0.400 (.NET Runtime 8.0.8)
1. Clone this repo to your local machine
1. Run the command npm install in the _open-weather.client_ directory to download the required node packages
1. Once all setup has completed, execute the command dotnet run in the _open-weather.Server_ directory to launch the application
1. Both the back-end and front-end should start but if not, execute npm run dev in the client directory

## Resources

[Open Weather Documentation](https://openweathermap.org/current)

[amCharts Weather Icons](https://www.amcharts.com/free-animated-svg-weather-icons/)

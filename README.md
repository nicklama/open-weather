# Nick's Open Weather

## Introduction

This application allows users to fetch the current weather description from the OpenWeather API by providing a city and country. The project consists of a ReactJS + Vite front-end which handles the user inputs and presentation, combined with a ASP.NET 8 back-end service (hosted in Azure) which manages all the external API calls, authentication and rate-limiting.

<img width="1024" alt="OpenWeather Frontpage" src="https://github.com/user-attachments/assets/a1a600e6-3dd5-41e3-9f7a-ca06dbda83fb">

### Front-end

-   [x] Fetches data from the back-end via a REST URL
-   [x] Allows user to enter city name and country name
-   [x] Presents the result to user
-   [x] Handles any errors/exceptions

### Back-end

-   [x] Created an API Key scheme
-   [x] Enforced rate limiting for each key to 5 requests/hour
-   [x] Feedback is provided when the limit is reached
-   [x] Takes inputs from the front-end and calls the external OpenWeather API

### Azure Functions (HTTP trigger)
-   [x] Hosted in Azure via Function App (consumption plan)
-   [x] Protected with custom API key authentication
-   [x] Rate-limited via Azure API Management policies
-   [x] Access restricted to APIM IP ranges only
-   [x] Supports CORS and referrer validation for enhanced frontend security

## How to Build and Run Manually

1. Install the latest Node.js and npm versions [here](https://nodejs.org).
    - Versions developed with: Node.js (v22.12.0), npm (10.9.2)
1. Install the latest .NET 8 SDK [here](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).
    - Versions developed with: SDK 8.0.400 (.NET Runtime 8.0.8)
1. Clone this repository to your local machine.
1. Open command prompt and run the command _npm install_ in the _open-weather.client_ directory to download the required node modules.
1. Execute the command _dotnet run_ in the _open-weather.Server_ directory to build and launch the application.
1. The front-end and back-end should start up together, but if not then execute _npm run dev_ in the _client_ directory to start the front-end manually.
1. Finally, navigate to the application URL https://localhost:5173/ as specified in the Vite command window.

## Resources

[Open Weather Documentation](https://openweathermap.org/current)

[amCharts Weather Icons](https://www.amcharts.com/free-animated-svg-weather-icons/)

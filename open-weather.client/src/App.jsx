import { useEffect, useState } from 'react';
import './App.css';
import WeatherInput from './components/WeatherInput';
import WeatherResponse from './components/WeatherResponse';

function App() {
    const [search, setSearch] = useState({ country: "", city: "" });
    const [weather, setWeather] = useState({});

    // Call populateWeatherData when the search variable is set
    useEffect(() => {
        if (search.city == "" && search.country == "") return; // Don't execute if search is empty
        populateWeatherData(search.city, search.country);
    }, [search]);

    return (
        <div>
            <h1 id="tableLabel">Nick&apos;s Open Weather</h1>
            <p>Type in a city and country to check the current weather</p>
            <WeatherInput setSearch={setSearch}></WeatherInput>
            <WeatherResponse weather={weather} search={search}></WeatherResponse>
        </div>
    );
    
    async function populateWeatherData(city, country) {
        try {
            //console.log(city + ", " + country);
            // Call the back-end service with the input parameters and API key
            const response = await fetch(`openweather?city=${city}&country=${country}`, {
                headers: {
                    "ApiKey": "myValidApiKey1",
                }
            });
            // Resolve the response promise in json format
            const data = await response.json();
            console.log(data);

            // Check for HTTP errors in the response and notify the user
            if (!response.ok) {
                if (data.message) setWeather(data);
                // Handle error for bad requests
                else if (data.status == 400) {
                    setWeather(
                    {
                        error: true,
                        message: "Please enter both a city and country. "
                    });
                }
                else setWeather(
                    {
                        error: true,
                        message: "An unexpected error has occured: " + response.statusText
                    });
                return;
            }

            //console.log(data.description);
            setWeather(data);
        }
        catch (error) {
            alert(error);
        }
    }
}

export default App;
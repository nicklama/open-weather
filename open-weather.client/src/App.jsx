import { useEffect, useState } from 'react';
import './App.css';
import WeatherInput from './components/WeatherInput';

function App() {
    const [search, setSearch] = useState({ country: "", city: "" });
    const [weather, setWeather] = useState({});

    useEffect(() => {
        if (search.city == "" && search.country == "") return; // don't execute if search is empty
        populateWeatherData(search.city, search.country);
    }, [search]);

    return (
        <div>
            <h1 id="tableLabel">Open Weather</h1>
            <p>Input a city and country to check the current weather</p>
            <WeatherInput setSearch={setSearch}></WeatherInput>
            <p>{weather?.description}</p>
        </div>
    );
    
    async function populateWeatherData(city, country) {
        try {
            console.log(city + ", " + country);
            const response = await fetch(`openweather?city=${city}&country=${country}`, {
                headers: {
                    "ApiKey": "myValidApiKey1",
                }
            });
            const data = await response.json();
            console.log(data);

            // Check for HTTP errors in the response and alert the user
            if (!response.ok) {
                if (data.message) alert(data.message);
                else alert("An unextected error has occured: " + response.statusText);
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
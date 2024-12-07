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
        console.log(city + ", " + country);
        const response = await fetch('openweather?location=' + city + "," + country);
        const data = await response.json();
        //let data = { description: "cloudy" };
        console.log(data.description);
        setWeather(data);
    }
}

export default App;
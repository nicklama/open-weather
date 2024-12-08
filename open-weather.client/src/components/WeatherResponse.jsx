import './WeatherResponse.css';
import cloudy from '../assets/cloudy.svg';
import day from '../assets/day.svg';
import rainy from '../assets/rainy-5.svg';
import snowy from '../assets/snowy-5.svg';
import thunder from '../assets/thunder.svg';

function WeatherResponse({ weather, search }) {
    function properCase(str) {
        if (!str) return str; // Handle empty strings
        return str.charAt(0).toUpperCase() + str.slice(1).toLowerCase();
    }

    // Set the icon based on the description response
    let icon;
    if (Object.keys(weather).length != 0 && weather.description) {
        switch (true) {
            case weather.description.includes("cloud"):
                icon = <img src={cloudy} alt="cloudy"></img>;
                break;
            case weather.description.includes("rain") || weather.description.includes("drizzle"):
                icon = <img src={rainy} alt="rainy"></img>;
                break;
            case weather.description.includes("snow"):
                icon = <img src={snowy} alt="snowy"></img>;
                break;
            case weather.description.includes("thunder"):
                icon = <img src={thunder} alt="thunder"></img>;
                break;
            default:
                icon = <img src={day} alt="day"></img>;
            }
    }

    // Build the output based on the response from the API
    const apiResponse = Object.keys(weather).length != 0 ?
        weather.error ? <h3 className="message">{weather.message}</h3> :
            weather.description ?
            <div className="success">
                    <p>The weather in {properCase(search.city)}, {properCase(search.country)} is currently:</p>
                    {icon}
                    <h3>{properCase(weather.description)}</h3>
                </div>
            : <h3>Sorry I cannot find the weather for that location at the moment, please try again later!</h3>
        : <h3 className="message">Give it a shot!</h3>;

    return (
        <div>
            {apiResponse}
        </div>
  );
}

export default WeatherResponse;
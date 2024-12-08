import { useState } from "react";
import './WeatherInput.css';

function WeatherInput({ setSearch }) {
    // Inputs stored in state variables
    const [city, setCity] = useState("");
    const [country, setCountry] = useState("");

    // Handlers used to update the state variables when an input is entered
    const handleCity = (e) => {
        setCity(e.target.value);
    };
    const handleCountry = (e) => {
        setCountry(e.target.value);
    };
    const handleSubmit = () => {
        //console.log(city + ', ' + country);
        setSearch({ city, country });
    }

  return (
      <form onSubmit={(e) => e.preventDefault()}>
          <label htmlFor="city">City:</label>
          <input type="text" id="city" onChange={handleCity} placeholder="Enter a city" ></input>

          <label htmlFor="country">Country:</label>
          <input type="text" id="country" onChange={handleCountry} placeholder="Enter a country" ></input>

          <button onClick={handleSubmit}>Go!</button>
      </form>
  );
}

export default WeatherInput;
import { useState, useEffect } from 'react';
import axios from 'axios';
import styled from 'styled-components';
import FavoriteCity from './FavoriteCity';
import snowImage from '/snow.jpg';
import coldImage from '/cold.jpg';
import sunnyImage from '/sunny.jpg';
import defaultImage from '/default.jpg';

const getBackgroundColor = (weatherData) => {
    if (!weatherData) return '#f8f8f8'; // Default background color

    const { temperature, humidity } = weatherData;

    if (temperature < 0) {
        return '#99ccff'; // Blue for freezing temperatures
    } else if (temperature >= 0 && temperature < 15) {
        return '#ccff66'; // Light green for cold temperatures
    } else if (temperature >= 15 && temperature < 25 && humidity < 60) {
        return '#ffcc66'; // Yellow for warm temperatures and lower humidity
    } else {
        return '#ff9999'; // Red for other conditions
    }
};

const getImage = (weatherData) => {
    if (!weatherData) return defaultImage; // Default image if no weather data

    const { temperature} = weatherData;

    if (temperature < 0) {
        return snowImage; // Image for freezing temperatures
    } else if (temperature >= 0 && temperature < 15) {
        return coldImage; // Image for cold temperatures
    } else if (temperature >= 15) {
        return sunnyImage; // Image for warm temperatures and lower humidity
    } else {
        return defaultImage; // Default image for other conditions
    }
};

const WeatherContainer = styled.div`
   margin: 20px auto;
    padding: 20px;
    width: 80%;
    max-width: 600px;
    border: 1px solid #ccc;
    border-radius: 8px;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
    background-color: #f8f8f8;
    background-color: ${(props) => getBackgroundColor(props.weatherData)};
    text-align: center;
    display: flex;
    flex-direction: column;
    align-items: center;
    

    h1 {
        font-size: 28px;
        margin-bottom: 20px;
    }

    label {
        font-size: 18px;
        margin-right: 10px;
    }

    input[type='text'] {
        padding: 8px;
        margin-right: 10px;
        border: 1px solid #ccc;
        border-radius: 4px;
    }

    button {
        padding: 8px 16px;
        background-color: #007bff;
        color: white;
        border: none;
        border-radius: 4px;
        cursor: pointer;
    }

    p {
        font-size: 18px;
        margin: 10px 0;
    }
`;


const WeatherComponent = () => {
    const [weatherData, setWeatherData] = useState(null);
    const [cityName, setCityName] = useState('Stockholm');
    const [errorMessage, setErrorMessage] = useState('');

    const fetchWeatherData = async () => {
        try {
            let endpoint = `https://localhost:7186/weatherdata/${cityName}`;
            if (cityName === 'Stockholm') {
                endpoint = 'https://localhost:7186/weatherdata';
            }

            const response = await axios.get(endpoint);

            if (!response.data || response.data.cod === '404') {
                setWeatherData(null);
                setErrorMessage('City not found');
            } else {
                setErrorMessage('');
                setWeatherData(response.data);
            }
        } catch (error) {
            console.error('Error fetching weather data:', error);
            setWeatherData(null);
            setErrorMessage('Error fetching data');
        }
    };

    const handleCityChange = (e) => {
        setCityName(e.target.value); // Update the cityName state with the input value
    };

    const handleCitySelect = (selectedCity) => {
        setCityName(selectedCity); // Update the cityName state with the selected city
    };

    const handleSearch = async () => {
        try {
            await fetchWeatherData();
            setCityName(''); // Clear the input field after fetching data
        } catch (error) {
            console.error('Error fetching weather data:', error);
            setWeatherData(null);
            setErrorMessage('Error fetching data');
        }
    };

    const handleFavoriteCitySelection = (selectedCity) => {
        setCityName(selectedCity);
        setCityName('');
    };

    useEffect(() => {
        if (cityName) {
            fetchWeatherData(cityName);
        }
    }, [cityName]);

    return (
       
        <div>
            
            <WeatherContainer weatherData={weatherData}>
                <img src={getImage(weatherData)} alt="Weather" style={{ width: 'oslopx', height: '200px' }} />
            <h1>Weather Information</h1>
            <div>
                <label htmlFor="cityInput">Enter City:</label>
                <input
                    type="text"
                    id="cityInput"
                    value={cityName}
                    onChange={handleCityChange}
                />
                <button onClick={handleSearch}>Search</button>
            </div>
            {errorMessage && <p>{errorMessage}</p>}
            {weatherData && !errorMessage && (
                <div>
                        <h2>{weatherData.cityName}</h2>
                        <p>Temperature: {weatherData && `${weatherData.temperature.toString()}\u00B0C`}</p>
                    <p>Humidity: {weatherData.humidity}%</p>
                    <p>Wind Speed: {weatherData.windSpeed} m/s</p>
                </div>
            )}
            </WeatherContainer>
            <FavoriteCity
                onFavoriteSave={handleFavoriteCitySelection}
                onCitySelect={handleCitySelect} // Pass the callback function to handle city selection
            />
            </div>
       
    );
};

export default WeatherComponent;

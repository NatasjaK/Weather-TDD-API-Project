import { useState } from 'react';

const FavoriteCity = ({ onFavoriteSave, onCitySelect }) => {
    const [favoriteCity, setFavoriteCity] = useState('');
    const [savedCities, setSavedCities] = useState([]);

    const handleFavoriteSave = () => {
        if (favoriteCity.trim() !== '') {
            setSavedCities([...savedCities, favoriteCity]);
            onFavoriteSave(favoriteCity); // Pass the city back to the parent component
            setFavoriteCity('');
        }
    };

    const handleRemoveFavorite = (city) => {
        const updatedCities = savedCities.filter((c) => c !== city);
        setSavedCities(updatedCities);
        // Add logic here to remove city from saved cities in storage or backend
    };

    const handleCitySelect = (city) => {
        onCitySelect(city); // Pass the selected city back to the parent component
    };

    return (
        <div>
            <h3>Save as Favorite City</h3>
            <input
                type="text"
                value={favoriteCity}
                onChange={(e) => setFavoriteCity(e.target.value)}
                placeholder="Enter city name"
            />
            <button onClick={handleFavoriteSave}>Save as Favorite</button>
            <div>
                <h3>Saved Cities</h3>
                <ul>
                    {savedCities.map((city, index) => (
                        <li key={index}>
                            <button onClick={() => handleCitySelect(city)}>{city}</button>
                            <button onClick={() => handleRemoveFavorite(city)}>Remove</button>
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    );
};

export default FavoriteCity;

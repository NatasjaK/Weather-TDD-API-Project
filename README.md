# Weather_TDD_API 🌦️

## Overview 📚

This repository contains a simple weather information API implemented using ASP.NET Core Web Api with a focus on Test-Driven Development (TDD). The API provides weather data for a specified city using the OpenWeatherMap API. The project includes a set of tests for the API functionality.

The front-end component, built with React using Vite, consumes the API to display weather information

## Getting Started 🚀

### Installation ⚙️

### Prerequisites 🛠️
 - npm create vite@latest my-react-app -- --template react
 - npm install
 - npm run dev

1. **Clone the repository:**

    ```bash
    git clone https://github.com/NatasjaK/Weather_TDD_API.git
    cd Weather_TDD_API
    or
    cd Client-React\clientreact
    ```

2. **Run the API:**

    ```bash
    Start debugging
    ```

    The API will be available at `https://localhost:7068` by default.

4. **(Optional) Run the front-end component:**

    ```bash
    cd Client-React\clientreact
    npm run dev
    ```

    The front-end will be available at `http://localhost:5173` by default.

## API Endpoints 🚀

- **GET /healthcheck**: Returns "OK" to indicate the health status of the API.

- **GET /weatherdata**: Returns weather data for the default city (Stockholm).

- **GET /weatherdata/{cityName}**: Returns weather data for the specified city.

- **GET /statistics**: Returns the total number of API calls made since the start.

## Testing 🧪

The project includes a set of tests in the `ApiWeatherTests` directory, implemented using xUnit. To run the tests:

```bash
cd ApiWeatherTests
run tests


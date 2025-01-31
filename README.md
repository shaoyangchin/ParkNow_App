![parknowlogo](images/parknow_logo.png)

# Welcome to ParkNow ! 

ParkNow is a web application designed to streamline parking management for vehicle owners in Singapore. It simplifies the process of locating, booking, and paying for parking spaces, helping to reduce the time and fuel wasted in searching for parking spots. With features like real-time parking availability, reservations, and navigation assistance, ParkNow aims to improve urban mobility and reduce congestion.

## Technology Stack

- ASP.Net Core Blazor (Front End and Back End)

- MudBlazor (Component Library)

- Leaflet (For Maps)

- MSSql (For Database Storage)

- Docker (For Easier Deployment)

- Nginx (Reverse Proxy to proxy requests to Blazor)

## Features

- **User Account Management**: 
  - Register and log in using Google or email.
  - Manage user profiles and update account information.
  - Securely reset passwords.

- **Vehicle Details Management**: 
  - Add, update, and manage vehicle information.
  - Support for different vehicle types including private, commercial, and electric vehicles.

- **Parking Space Locator**: 
  - Search for available parking spaces based on location, carpark number, or postal code.
  - Real-time updates on parking availability and rates.

- **Navigation Assistance**: 
  - Get turn-by-turn directions to the selected parking spot using Google Maps integration.
  - Real-time location tracking and route updates.

- **Reservation System**: 
  - Book parking spots in advance to secure availability.
  - Modify or cancel reservations as needed within a specified time frame.

- **Carpark Payment**: 
  - Make secure payments for parking reservations through integrated payment gateways.
  - Apply promo codes or discounts during the payment process.
  - View transaction history for past parking sessions.

- **Parking History**: 
  - View detailed records of past parking sessions, including location, time, and cost.
  - Filter history by carpark name, date range, or frequency of visits.

- **In-app Guide**: 
  - Step-by-step instructions for new users to get started with the application.
  - Provides guidance on managing accounts, booking parking, and making payments.

## Environment Setup
### 1. Install dotnet 8.0
After installation, check by entering "dotnet" into cmd, u should see some help functions

### 2. Install docker
After installation, check by entering "docker" into cmd, u should see some help functions

### 3. Run in your cmd the following
```
docker pull mcr.microsoft.com/mssql/server:2022-latest
```

### 4. Pull the latest parknow repo with 
```
git clone https://github.com/MetaSlave/ParkNow
```

### 5. Add a new file called "appsettings.json" in the main dir

### 6. inside the file, paste the following
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=localhost,1433;User ID=sa;Password=123456a@;Database=ParkNow;MultipleActiveResultSets=true;TrustServerCertificate=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### 7. Run the following in cmd
```
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=123456a@" -p 1433:1433 --name sa -d mcr.microsoft.com/mssql/server:2022-latest
```

### 8. Go to your cli in the main parknow dir, and run 
```
dotnet run
```

### 9. Visit "http://localhost:5121/" and make sure the site is up

### 11. Run the following command to install the pyodbc module (optional)
```
pip install pyodbc
```

### 12. Install ODBC Driver 18 for SQL Server online
https://learn.microsoft.com/en-us/sql/connect/odbc/download-odbc-driver-for-sql-server?view=sql-server-ver16

### 12. Run the following command in the main dir
```
python populateCarpark.py
```

# [Documentation](https://github.com/MetaSlave/ParkNow/blob/main/documentation.md)


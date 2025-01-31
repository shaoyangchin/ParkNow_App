import csv
import pyodbc  # Use pyodbc for SQL Server connection

# SQL Server connection parameters
server = 'localhost'
database = 'ParkNow'
username = 'sa'
password = '123456a@'

# Establish connection using pyodbc
try:
    conn = pyodbc.connect(
        f'DRIVER={{ODBC Driver 18 for SQL Server}};'
        f'SERVER={server},1433;'
        f'DATABASE={database};'
        f'UID={username};'
        f'PWD={password};'
        'TrustServerCertificate=yes'
    )
    cursor = conn.cursor()
except Exception as e:
    print(f"Connection error: {e}")

# SQL query to insert data
insert_query = '''
INSERT INTO Carparks (
    CarparkId, Address, XCord, YCord, Type, 
    SystemType, ShortTermParkingType, FreeParking, 
    NightParking, 
    GantryHeight, CarparkBasement, CentralCharge
) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
'''

# Open and read the CSV file
with open('hdb.csv', 'r') as file:
    csv_reader = csv.DictReader(file)
    
    for row in csv_reader:
        print(row)
        values = (
            row['car_park_no'],
            row['address'],
            row['x_coord'],
            row['y_coord'],
            row['car_park_type'],
            row['type_of_parking_system'],
            row['short_term_parking'],
            row['free_parking'],
            # Convert night_parking to boolean
            True if row['night_parking'] == "YES" else False,
            row['gantry_height'],
            True if row['car_park_basement'] == "Y" else False,
            True if row['central'] == "Y" else False
        )
        
        # Execute the insert query
        cursor.execute(insert_query, values)

# Commit the changes and close the connection
conn.commit()
conn.close()

print("Data import completed successfully.")
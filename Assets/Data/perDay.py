import pandas as pd
import json

# Assuming 'data.json' is your dataset file, replace 'data.json' with the actual path to your dataset file
file_path = 'export_Jazu_Oskar_Felsa.json'

# Load JSON data into a DataFrame
with open(file_path, 'r') as file:
    data = json.load(file)

# Assuming your JSON structure is a list of records under a 'data' key
df = pd.DataFrame(data['Felsa']['locations'])

# Convert the 'timestamp' column to datetime
df['timestamp'] = pd.to_datetime(df['timestamp'])

# Extract date from timestamp
df['date'] = df['timestamp'].dt.date

# Group the data by date and calculate the mean for 'longitude' and 'latitude'
daily_averages = df.groupby('date').agg({'longitude': 'mean', 'latitude': 'mean'}).reset_index()

# Convert the daily averages DataFrame back to JSON
daily_averages_json = daily_averages.to_json(orient="records")

# Save the daily averages JSON to a file
with open('daily_averages_Felsa.json', 'w') as file:
    file.write(daily_averages_json)

print("Daily averages have been calculated and saved to 'daily_averages.json'.")

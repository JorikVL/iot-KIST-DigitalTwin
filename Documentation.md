# Unity Documentation for IOT-KIST-DigitalTwin

## The map

The Bing maps sdk is used to display the world map. The sdk handles streaming and rendering of 3D terrain data with world-wide coverage.
https://github.com/microsoft/MapsSDK-Unity

## Scripts

### JSONReader

The JSONReader script contains a class "Sensor" that contains all the variables that the json file contains.  
To be able to use JsonUtility with this class it is required that the class is supported by **[Serializable]**.

    [Serializable]
    public class Sensor
    {
        public string _id;
        public string Name;
        public double Latitude;
        public double Longtitude;
        public int battery;
        public int CO2;
        public int humidity;
        public int pm10;
        public int pm25;
        public int pressure;
        public int salinity;
        public int temp;
        public int tvox;
        public string time;
        public string speciality;
    }
    
<br>
When the script is first initiated it creates a list "sensors" of the object "Sensor".

    public List<Sensor> sensors = new List<Sensor>(); 
    
<br>
The "AddSensor" method creates a new sensor called "newSensor" it uses the JsonUtility.FromJson method to create a Sensor object from the json string argument.  
The if structure checks if the "newSensor" object contains data. If true it adds the sensor to the list "sensors".  
If there is a error in the method the try catch will show an error message in the debug console.

    public void AddSensor(string json){
        try{
            Sensor newSensor = JsonUtility.FromJson<Sensor>(json);
            if (newSensor != null) {
                sensors.Add(newSensor);
                Debug.Log("Sensor added to list");
            } else {
                Debug.Log("Sensor returned null");
            }
        }
        catch {
            Debug.Log("Add sensor to list failed!");
        }
    }

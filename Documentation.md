# The application

![image](https://user-images.githubusercontent.com/25724406/164173286-3aab6e9d-0f99-4645-9ee8-883bb29f3ee6.png)

...

# The code behind the application

## Unity

Unity is a game engine that supports desktop, mobile, console and virtual reality platforms.  
The engine supports development of both 2D and 3D applications and offers a primarly scripting API in C#.

## The map

The Bing maps sdk is used to display the world map in unity. The sdk handles streaming and rendering of 3D terrain data with world-wide coverage.
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

<br>

### ShowDataSensor

When the user clicks on a mappin the text field on the left of the screen will show the selected sensors data.  
The sensor has 2 public Text field, here we put the 2 Text objects where we want to display our data.  
When the Start() method is called it will find the "Manager" object and the "JSONReader" component.  
The DisplayData method is called from the OnMousePin script that is attached to each mappin, this will call the method with the id of the sensor as argument.  
The foreach loop will find the sensor with the same id and populate the text fields with its data.

    public class ShowDataSensor : MonoBehaviour
    {
    public Text DataDisplay;
    public Text SensorName;

    private GameObject manager;
    private JSONReader jsonReader;

    void Start(){
        manager = GameObject.Find("Manager");
        jsonReader = manager.GetComponent<JSONReader>();
    }

    public void DisplayData(string id){
        foreach (JSONReader.Sensor sensor in jsonReader.sensors){
            if (sensor._id == id){
                SensorName.text = sensor.Name;
                System.DateTime dateTime = System.DateTime.Parse(sensor.time);
                DataDisplay.text = "ID: " + sensor._id + "\nBattery: " + sensor.battery + "\nCO2: " + sensor.CO2 + "\nHumidity: " + sensor.humidity + "\nPm10: " + sensor.pm10 + "\nPm2.5: " + sensor.pm25 + "\nPressure: " + sensor.pressure + "\nSalinity: " + sensor.salinity + "\ntemp= " + sensor.temp + "\nTvox: " + sensor.tvox + "\nDate: " + dateTime;
            }   
        }

    }
    }
    
<br>


# The application

![image](https://user-images.githubusercontent.com/25724406/164180330-14465f97-7314-4565-8d1b-02834fef5743.png)

The application shows a map on which Sensors are shown, when you click on a sensor it shows it's data. The application gets the sensor's data by performing an API call to a node red server and dynamically places pins on the map according to their coordinates.

# The code behind the application

## Unity

Unity is a game engine that supports desktop, mobile, console and virtual reality platforms.  
The engine supports development of both 2D and 3D applications and offers a primarly scripting API in C#.

## The map

The Bing maps sdk is used to display the world map in unity. The sdk handles streaming and rendering of 3D terrain data with world-wide coverage.
https://github.com/microsoft/MapsSDK-Unity

## GameObjects

In the hierarchy you can see the different game objects.

![image](https://user-images.githubusercontent.com/25724406/164205282-1f46a16e-070b-4f31-9035-a31935cfb859.png)

- The Map object contains the Bing maps scripts and has the DynamicallyCreatePins Script attached to it.
- The Manager object contains the JSONReader, APICall, ShowDataSensor and Emailer script.
- The canvas is used for the UI and contains all the images, textfields and buttons.

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
The "AddSensor" method creates a new sensor called "newSensor" it uses the JsonUtility.FromJson method to create a Sensor object from the json text that was passed trough the string argument of the method.  
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
This alows us to access the sensor list that was created in the JSONReader script.  
The DisplayData method is called from the OnMousePin script that is attached to each mappin, this will call the method with the id of the sensor as argument.  
The foreach loop will loop over all the sensors in the list and find the sensor with the same id and populate the text fields with its data.

    public class ShowDataSensor : MonoBehaviour
    {
    public Text DataDisplay;
    public Text SensorName;

    private GameObject manager;
    private JSONReader jsonReader;

    void Start()
    {
        manager = GameObject.Find("Manager");
        jsonReader = manager.GetComponent<JSONReader>();
    }

    public void DisplayData(string id)
    {
        foreach (JSONReader.Sensor sensor in jsonReader.sensors)
        {
            if (sensor._id == id)
            {
                SensorName.text = sensor.Name;
                System.DateTime dateTime = System.DateTime.Parse(sensor.time);
                DataDisplay.text = "ID: " + sensor._id + "\nBattery: " + sensor.battery + "\nCO2: " + sensor.CO2 + "\nHumidity: " + sensor.humidity + "\nPm10: " + sensor.pm10 + "\nPm2.5: " + sensor.pm25 + "\nPressure: " + sensor.pressure + "\nSalinity: " + sensor.salinity + "\ntemp= " + sensor.temp + "\nTvox: " + sensor.tvox + "\nDate: " + dateTime;
            }   
        }
    }
    }
    
<br>

### OnMousePin

The OnMousePin script is attached to each mapPin object. It contains the sensor id of the sensor it represents.  
The start method will find the Manager and the showDataSensor script that is a component of the manager so that we can use its methods.  
When the user clicks on a mapPin object the OnMouseDown method will detect this and call the method DisplayData of the showDataSensor script and pass along the sensor Id argument.

    public class OnMousePin : MonoBehaviour
    {
    public string sensorId;
    private GameObject manager;
    private ShowDataSensor showDataSensor;

    void Start(){
        //Get manager object and JSONReader component
        manager = GameObject.Find("Manager");
        showDataSensor = manager.GetComponent<ShowDataSensor>();
    }

    private void OnMouseDown() {
        showDataSensor.DisplayData(sensorId);
    }
    }
    
<br>

### DynamicallyCreatePins

The DynamicallyCreatePins script automatically places the mapPins from the sensor list on the map.  
In the Start method it finds the manager script and the jsonReader and emailer component so it access the variables and methods, the InvokeRepeating function which will execute te PlacePins method after 5 seconds and execute it every 60 seconds.  

The PlacePins method start by deleting all existing mapPins so that it does not create duplicates.  
Then it loop over all te sensors in the sensor list from the JSONReader script.
if the sensor and its position in not equal to null it can be placed on the map.  
It uses the mapPin prefab to create a new GameObject with the OnMousePin script on it, this will include te sensor id.  
The pin gets set as a child of the map object and the coordinates get assigned.  


    public class DynamicallyCreatePins : MonoBehaviour
    {
    public GameObject pinPrefab;
    public GameObject _Reader;
    public List<GameObject> mapPins = new List<GameObject>(); 

    private GameObject manager;
    private JSONReader jsonReader;
    private Emailer emailer;

    void Start(){
        manager = GameObject.Find("Manager");
        jsonReader = manager.GetComponent<JSONReader>();
        emailer = manager.GetComponent<Emailer>();
        InvokeRepeating("PlacePins", 5, 60);
    }

    public void PlacePins(){
        Debug.Log("Start DynamicallyCreatePins");
        foreach (GameObject mapPin in mapPins){
            Destroy(mapPin);
        }

        foreach (JSONReader.Sensor sensor in jsonReader.sensors)
        {
            if (sensor._id != null && sensor.Longtitude != null && sensor.Latitude != null){
                Debug.Log("Place sensor: " + sensor._id + " with position: " + sensor.Longtitude + ", " + sensor.Latitude);
                //Instantiate Pin & assign pin number
                var mapPin = Instantiate(pinPrefab);
                mapPins.Add(mapPin);
                OnMousePin onMousePin = mapPin.GetComponent<OnMousePin>();
                onMousePin.sensorId = sensor._id;

                //Set pin as child of map
                mapPin.transform.parent = gameObject.transform;
                var mapPinComponent = mapPin.GetComponent<MapPin>();
                LatLon _pos = new LatLon(sensor.Latitude, sensor.Longtitude);
                mapPinComponent.Location = _pos;
                    
                //Get object
                var Root = FindObject(mapPin, "Root");
                var Sphere = FindObject(Root, "Sphere");
                var Stem = FindObject(Root, "MapPinStem");

                //Set color objects
                var mapPinRenderer = Sphere.GetComponent<Renderer>();
                var stemRenderer = Stem.GetComponent<Renderer>();
                float val = 0f;
                ///TODO: Add convert to float and add extreme values for alerts
                switch (sensor.speciality)
                {
                    case "temp":
                        val = (float)sensor.temp;
                        /*if (false){
                            SendAlert("High temperature");
                        }*/
                        break;
                    case "pressure":
                        val = (float)sensor.pressure;
                        /*if (false){
                            SendAlert("High pressure");
                        }*/
                        break;
                    case "pm25":
                        val = (float)sensor.pm25;
                        /*if (false){
                            SendAlert("High pm2.5");
                        }*/
                        break;
                    case "pm10":
                        val = (float)sensor.pm10;
                        /*if (false){
                            SendAlert("High pm10");
                        }*/
                        break;
                    case "humidity":
                        val = (float)sensor.humidity;
                        /*if (false){
                            SendAlert("High humidity");
                        }*/
                        break;
                    case "CO2":
                        val = (float)sensor.CO2;
                        /*if (false){
                            SendAlert("High CO2");
                        }*/
                        break;
                    case "tvox":
                        val = (float)sensor.tvox;
                        /*if (false){
                            SendAlert("High tvox");
                        }*/
                        break;
                    case "salinity":
                        val = (float)sensor.salinity;
                        /*if (false){
                            SendAlert("High salinity");
                        }*/
                        break;
                    default:
                        Debug.Log("No important value found!");
                        break;
                }
                Color lerpedColor = Color.Lerp(Color.red, Color.green, val);

                mapPinRenderer.material.color = lerpedColor;
                stemRenderer.material.color = lerpedColor;
            }
        }
    }

    public void SendAlert(string message){
        emailer.SendAnEmail( message );
    }

    private GameObject FindObject(GameObject obj, string objToFind){
        return obj.transform.Find(objToFind).gameObject;
    }
    }

## Notification

The notification script allows us to display notifications on screen.  
When te Notify method is called it start a coroutine.  
A coroutine can be paused with the yield statement and allows us to only display the notification for 3 seconds.

    public class Notification : MonoBehaviour
    {
    public GameObject notificationPanel;
    public TMP_Text notificationText;

    public void Notify(string text){
        StartCoroutine(showPanel(text));
    }

    IEnumerator showPanel(string text){
        notificationPanel.SetActive(true);
        notificationText.text = text;
        yield return new WaitForSeconds(3);
        notificationPanel.SetActive(false);
    }
    }
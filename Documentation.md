# Introduction

This goal of this project was to visualizes sensors and their data on a interactive map.  

The application shows a map on which Sensors are placed, when you click on a sensor it shows it's data.  
The application gets the sensor's data by performing an API call and according to their response, it places the sensor on the correct coordinates.  
*(The sensors data shown in the following pictures is test data)*

![image](https://user-images.githubusercontent.com/25724406/164618212-585a9d6a-9f8c-4bee-910c-0720bd78f3fb.png)

<br>

![image](https://user-images.githubusercontent.com/25724406/164618421-365a8711-f5ae-4bd0-9634-10978b35d3c0.png)

<br>

The application can send email alerts when the sensors value drops below a set value. It is possible to change the sender and receiver adres with a pop-up panel.

![image](https://user-images.githubusercontent.com/25724406/164619914-e3d559a4-6721-4b57-bfb1-c8e1c50db118.png)

<br>

![SensorAlert](https://user-images.githubusercontent.com/25724406/165459096-6e37597b-b05e-4da8-8a4f-c044685f2699.png)

<br>

The application also has the ability to show messages and alerts on screen.

![image](https://user-images.githubusercontent.com/25724406/164621015-ec31ed77-9679-4288-a3ca-f5fcbb54c9ea.png)

<br>

# Code

## Unity

This project was made with unity, unity is a game engine that supports desktop, mobile, console and virtual reality platforms.  
The engine supports development of both 2D and 3D applications and offers a primarly scripting API in C#.  
https://unity.com/

## The map

The Bing maps sdk is used to display the world map in unity. The sdk handles streaming and rendering of 3D terrain data with world-wide coverage.
https://github.com/microsoft/MapsSDK-Unity

## GameObjects

In the Unity project hierarchy you can see the different game objects.

![image](https://user-images.githubusercontent.com/25724406/164205282-1f46a16e-070b-4f31-9035-a31935cfb859.png)

- The Map object contains the Bing maps scripts and has the DynamicallyCreatePins Script attached to it.
- The canvas is used for the UI and contains all the images, textfields, buttons,... .
- The Manager object contains the JSONReader, APICall, ShowDataSensor, Emailer and Notification script.
    ![image](https://user-images.githubusercontent.com/25724406/164406340-16cc05fb-09f5-44fb-8bd4-4baa7baffdb2.png)

## Scripts

### JSONReader

The JSONReader script is used to convert a json file containing the sensors data to sensor objects that we can work with.  
The script has a class "Sensor" that contains all the variables from the json file.  
To be able to use [JsonUtility](https://docs.unity3d.com/ScriptReference/JsonUtility.html) with this class it is required that the class is supported by *[Serializable]*.

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

When the script is first initiated it creates a list "sensors" of the class "Sensor" to store all the sensors.  

    public List<Sensor> sensors = new List<Sensor>(); 
    
<br>

The "AddSensor" method creates a new sensor called "newSensor" it uses the [JsonUtility.FromJson](https://docs.unity3d.com/ScriptReference/JsonUtility.FromJson.html) method to create a Sensor object from the json text that was passed trough the string argument of the method.  
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
The sensor has two public Text field, here we put the two Text objects where we want to display our data. 

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

In the Start method it finds the manager script and the jsonReader and emailer component so it can access their variables and methods, the [InvokeRepeating](https://docs.unity3d.com/ScriptReference/MonoBehaviour.InvokeRepeating.html) function which will execute te PlacePins method after 5 seconds and execute it every 60 seconds.  

    void Start(){
        manager = GameObject.Find("Manager");
        jsonReader = manager.GetComponent<JSONReader>();
        emailer = manager.GetComponent<Emailer>();
        InvokeRepeating("PlacePins", 5, 60);
    }
  
 <br>

The PlacePins method start by deleting all existing mapPins so that it does not create duplicates.  
Then it loop over all te sensors in the sensor list from the JSONReader script.  
If the sensor and its position is not equal to null it can be placed on the map.  

To place the pin it uses the mapPin prefab to create a new GameObject with the OnMousePin script component on it, this will include the sensor id.  
The pin gets set as a child of the map object and the coordinates get assigned.  

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
                    
<br>
                    
The sensor pin will be given a color between green and red depending on its primary value.  
The color will be calculated using the [Color.Lerp](https://docs.unity3d.com/ScriptReference/Color.Lerp.html) function, this function linearly interpolates between 2 colors by a float value.  

To assign the color to the pin we first get the different parts of the MapPin prefab object, Then we get the renderer component and assign the color to the material.color.  

*The case stucture to convert the integer to float and to check if the sensor reaches a critical value is not yet filled with the final data*  
                    
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

<br>

### Notification

The notification script allows us to display notifications on screen. The notificationPanel and notificationText are assigned using the inspector.  

When the Notify method is called it start a coroutine.  
A [coroutine](https://docs.unity3d.com/ScriptReference/Coroutine.html) can be paused with the [yield](https://docs.unity3d.com/ScriptReference/YieldInstruction.html) statement and allows us to only display the notification for 3 seconds.

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

<br>

### Emailer

The Emailer script allows us to send emails to notify users.  
The private strings will contain the emailaddreses and password to send a email. 

In the Start() method it will run SetEmail(), this sets up the adress and password with the default value when starting the application.

The SetEmail() method reads the data from the inputfields and assigns it to the private variables, the inputfield are assigned to the public variables using the inspector in the Unity editor. This method will run when we change the email using the gui.

 
    public class Emailer : MonoBehaviour
    {

    private string kSenderEmailAddress;
    private string kSenderPassword;
    private string kReceiverEmailAddress;

    public Notification notification;
    public InputField senderEmail;
    public InputField senderPassword;
    public InputField receiverEmail;

    private void Start() {
        SetEmail();
    }

    public void SetEmail() {
        kSenderEmailAddress = senderEmail.text;
        kSenderPassword = senderPassword.text;
        kReceiverEmailAddress = receiverEmail.text;
        Debug.Log("Email Set");
    }

<br>

The SendAnEmail method uses the [System.Net.Mail Namespace](https://docs.microsoft.com/en-us/dotnet/api/system.net.mail?view=net-6.0) to send an email using a smtp server.  
It will create a new MailMessage object, then we add the message, sender, receiver to the mail object.  

To setup the server we create a new [smtpClient](https://docs.microsoft.com/en-us/dotnet/api/system.net.mail.smtpclient?view=net-6.0) object called smtpServer, we assign it the port, credentials and enable ssl.  
Then we use ServicePointManager to setup a http connection.

    public void SendAnEmail(string message) {
        // Create mail
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(kSenderEmailAddress);
        mail.To.Add(kReceiverEmailAddress);
        mail.Subject = "Sensor Alert";
        mail.Body = message;

        // Setup server 
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new NetworkCredential(
            kSenderEmailAddress, kSenderPassword ) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate ( object s, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors ) {
                Debug.Log("Email success!");
                return true;
            };

<br>

Now we will try to send the email if it fails we will show an error, if succesfull we log *Email sent*.

        // Send mail to server, print results
        try {
            smtpServer.Send(mail);
        }
        catch ( System.Exception e ) {
            Debug.Log("Email error: " + e.Message);
            notification.Notify("Email error: " + e.Message);
        }
        finally {
            Debug.Log("Email sent!");
        }
    }

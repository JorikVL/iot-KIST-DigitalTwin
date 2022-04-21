using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;

public class DynamicallyCreatePins : MonoBehaviour
{
    public GameObject pinPrefab;
    public GameObject _Reader;
    public List<GameObject> mapPins = new List<GameObject>(); 

    private GameObject manager;
    private JSONReader jsonReader;
    private Emailer emailer;
    private Notification notification;

    void Start(){
        manager = GameObject.Find("Manager");
        jsonReader = manager.GetComponent<JSONReader>();
        emailer = manager.GetComponent<Emailer>();
        notification = manager.GetComponent<Notification>();
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
                notification.Notify("Place sensor: " + sensor._id + " with position: " + sensor.Longtitude + ", " + sensor.Latitude);

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
                    
                //Get mappin objects
                var Root = FindObject(mapPin, "Root");
                var Sphere = FindObject(Root, "Sphere");
                var Stem = FindObject(Root, "MapPinStem");

                //Set color mappin objects
                var mapPinRenderer = Sphere.GetComponent<Renderer>();
                var stemRenderer = Stem.GetComponent<Renderer>();
                float val = 0f;

                ///TODO: Add convert to float and add extreme values for alerts
                switch (sensor.speciality)
                {
                    case "temp":
                        val = (float)sensor.temp;
                        /*if (val > ){
                            SendAlert("High temperature");
                        }*/
                        break;
                    case "pressure":
                        val = (float)sensor.pressure;
                        /*if (val > ){
                            SendAlert("High pressure");
                        }*/
                        break;
                    case "pm25":
                        val = (float)sensor.pm25;
                        /*if (val > ){
                            SendAlert("High pm2.5");
                        }*/
                        break;
                    case "pm10":
                        val = (float)sensor.pm10;
                        /*if (val > ){
                            SendAlert("High pm10");
                        }*/
                        break;
                    case "humidity":
                        val = (float)sensor.humidity;
                        /*if (val > ){
                            SendAlert("High humidity");
                        }*/
                        break;
                    case "CO2":
                        val = (float)sensor.CO2;
                        /*if (val > ){
                            SendAlert("High CO2");
                        }*/
                        break;
                    case "tvox":
                        val = (float)sensor.tvox;
                        /*if (val > ){
                            SendAlert("High tvox");
                        }*/
                        break;
                    case "salinity":
                        val = (float)sensor.salinity;
                        /*if (val > ){
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
        notification.Notify(message);
    }

    private GameObject FindObject(GameObject obj, string objToFind){
        return obj.transform.Find(objToFind).gameObject;
    }
}
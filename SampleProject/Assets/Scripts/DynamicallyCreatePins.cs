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

    void Start(){
        manager = GameObject.Find("Manager");
        jsonReader = manager.GetComponent<JSONReader>();
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
                var Cube = FindObject(Root, "Cube");
                var Stem = FindObject(Root, "MapPinStem");

                //Set color objects
                var mapPinRenderer = Cube.GetComponent<Renderer>();
                var stemRenderer = Stem.GetComponent<Renderer>();
                float val = 0f;
                /*switch (switch_on)
                {
                    case "temp":
                        val = (float)sensor.temp;
                        val = (val/40f);
                        break;
                    case "pressure":
                        val = (float)sensor.pressure;
                        break;
                    case "pm25":
                        val = (float)sensor.pm25;
                        break;
                    case "pm10":
                        val = (float)sensor.pm10;
                        break;
                    case "humidity":
                        val = (float)sensor.humidity;
                        break;
                    case "CO2":
                        val = (float)sensor.CO2;
                        break;
                    case "tvoc":
                        val = (float)sensor.tvoc;
                        break;
                    case "salinity":
                        val = (float)sensor.salinity;
                        break;
                    default:
                        Debug.Log("No important value found!")
                }*/
                Color lerpedColor = Color.Lerp(Color.red, Color.green, val);

                mapPinRenderer.material.color = lerpedColor;
                stemRenderer.material.color = lerpedColor;
            }
        }
    }

    public void ChangeColorPin(){

    }

    private GameObject FindObject(GameObject obj, string objToFind){
        return obj.transform.Find(objToFind).gameObject;
    }
}
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
            Color lerpedColor = Color.Lerp(Color.red, Color.green, sensor.temp);

            mapPinRenderer.material.color = lerpedColor;
            stemRenderer.material.color = lerpedColor;
            }
    }

    public void ChangeColorPin(){

    }

    private GameObject FindObject(GameObject obj, string objToFind){
        return obj.transform.Find(objToFind).gameObject;
    }
}
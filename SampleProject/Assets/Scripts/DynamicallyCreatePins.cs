using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;

public class Sensor{
    public LatLon position;
    public float value;
}

public class Sensors{
    public Sensor[] sensors;
}

public class DynamicallyCreatePins : MonoBehaviour
{
    public GameObject pinPrefab;
    public GameObject _Reader;
    public List<Sensor> sensors = new List<Sensor>();

    void Start(){
        PopulateData();
        int counter = 0; 
        foreach (Sensor sensor in sensors)
        {
            //Instantiate Pin & assign pin number
            var mapPin = Instantiate(pinPrefab);
            OnMousePin onMousePin = mapPin.GetComponent<OnMousePin>();
            onMousePin.sensorNumber = counter;
            counter += 1;

            //Set pin as child of map
            mapPin.transform.parent = gameObject.transform;
            var mapPinComponent = mapPin.GetComponent<MapPin>();
            mapPinComponent.Location = sensor.position;
                
            //Get object
            var Root = FindObject(mapPin, "Root");
            var Cube = FindObject(Root, "Cube");
            var Stem = FindObject(Root, "MapPinStem");

            //Set color objects
            var mapPinRenderer = Cube.GetComponent<Renderer>();
            var stemRenderer = Stem.GetComponent<Renderer>();
            Color lerpedColor = Color.Lerp(Color.red, Color.green, sensor.value);

            mapPinRenderer.material.color = lerpedColor;
            stemRenderer.material.color = lerpedColor;
            }
    }

    private GameObject FindObject(GameObject obj, string objToFind){
        return obj.transform.Find(objToFind).gameObject;
    }

    private void PopulateData(){
        AddComponent(new LatLon(-6.234412, 39.238479), 0.9f);
        AddComponent(new LatLon(-5.984683, 39.188133), 0.3f);
        AddComponent(new LatLon(-6.353185, 39.400993), 0.1f);
    }

    private void AddComponent(LatLon pos, float value){
        Sensor data = new Sensor();
        data.position = pos;
        data.value = value;
        sensors.Add(data);
    }
}
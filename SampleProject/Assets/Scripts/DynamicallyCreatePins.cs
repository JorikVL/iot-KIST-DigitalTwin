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
        foreach (Sensor sensor in sensors)
        {
            //Instantiate Pin
            var mapPin = Instantiate(pinPrefab);
            //Set pin as child of map
            mapPin.transform.parent = gameObject.transform;
            var mapPinComponent = mapPin.GetComponent<MapPin>();
            mapPinComponent.Location = sensor.position;
                
            //Get object
            var Root = mapPin.transform.Find("Root").gameObject;
            var Cube = Root.transform.Find("Cube").gameObject;
            var Stem = Root.transform.Find("MapPinStem").gameObject;

            //Set color objects
            var mapPinRenderer = Cube.GetComponent<Renderer>();
            var stemRenderer = Stem.GetComponent<Renderer>();
            Color lerpedColor = Color.Lerp(Color.red, Color.green, sensor.value);

            mapPinRenderer.material.color = lerpedColor;
            stemRenderer.material.color = lerpedColor;
            }
    }

    public void PopulateData(){
        _Reader = GameObject.Find("Reader");
        var _JSON = _Reader.GetComponent<JSONReader>();
        /*foreach (Sensor sensor in _JSON.mySensorList.sensor)
        {
            Debug.Log(sensor.position + "" + sensor.value);
        }*/
        AddComponent(new LatLon(-6.234412, 39.238479), 0.9f);
        AddComponent(new LatLon(-5.984683, 39.188133), 0.3f);
        AddComponent(new LatLon(-6.353185, 39.400993), 0.1f);
    }

    public void AddComponent(LatLon pos, float value){
        Sensor data = new Sensor();
        data.position = pos;
        data.value = value;
        sensors.Add(data);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;

public class Sensor{
    public LatLon position;
    public float value;
}

/*public class Sensors{
    public Sensor[] sensors;
}*/

public class DynamicallyCreatePins : MonoBehaviour
{
    public GameObject pinPrefab;
    public List<Sensor> sensors = new List<Sensor>();

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)){
            PopulateData();  
            foreach (Sensor sensor in sensors)
            {
                //Instantiate Pin
                var mapPin = Instantiate(pinPrefab);
                //Set pin as child of map
                mapPin.transform.parent = gameObject.transform;
                var mapPinComponent = mapPin.GetComponent<MapPin>();
                mapPinComponent.Location = sensor.position;
                
                //Get cube
                var Root = mapPin.transform.Find("Root").gameObject;
                var Cube = Root.transform.Find("Cube").gameObject;

                //Set color cube
                var mapPinRenderer = Cube.GetComponent<Renderer>();
                Color lerpedColor = Color.Lerp(Color.red, Color.green, sensor.value);

                mapPinRenderer.material.color = lerpedColor;
            }
        }
    }

    public void PopulateData(){
        Sensor data = new Sensor();
        data.position = new LatLon(-6.234412, 39.238479);
        data.value = 0.9f;
        sensors.Add(data);
        Sensor data2 = new Sensor();
        data2.position = new LatLon(-5.984683, 39.188133);
        data2.value = 0.2f;
        sensors.Add(data2);
        Sensor data3 = new Sensor();
        data3.position = new LatLon(-6.353185, 39.400993);
        data3.value = 0.5f;
        sensors.Add(data3);
    }
}
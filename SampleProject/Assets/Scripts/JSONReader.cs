using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Geospatial;

public class JSONReader : MonoBehaviour
{
    public TextAsset textJSON;

    [System.Serializable]
    public class Sensor
    {
        public LatLon position;
        public float value;
    }

    [System.Serializable]
    public class SensorList
    {
        public Sensor[] sensor;
    }

    public SensorList mySensorList = new SensorList();

    void Start()
    {
        mySensorList = JsonUtility.FromJson<SensorList>(textJSON.text);
        foreach (Sensor sensor in mySensorList.sensor)
        {
            Debug.Log(sensor.position + " " + sensor.value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

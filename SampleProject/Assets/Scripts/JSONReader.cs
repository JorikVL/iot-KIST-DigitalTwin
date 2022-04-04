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
        public string id;
        public string name;
        public string datetime;
        public int pm25;
        public int pm10;
        public int temp;
        public int pressure;
        public byte humidity;
        public int co2;
        public int tvoc;
        public byte salinity;
        public bool battery;
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
            //Debug.Log(sensor.position + " " + sensor.value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

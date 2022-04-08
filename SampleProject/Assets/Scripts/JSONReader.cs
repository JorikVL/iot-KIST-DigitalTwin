using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Geospatial;
using UnityEngine.UI;
using System;

public class JSONReader : MonoBehaviour
{
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
    }

    public List<Sensor> sensors = new List<Sensor>(); 

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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Geospatial;
using UnityEngine.UI;
using System;

public class JSONReader : MonoBehaviour
{
    public Text titleField;
    public Text textField;

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
        public DateTime time;
    }

    public List<Sensor> sensors = new List<Sensor>(); 

    public void AddSensor(string json){
        try{
        sensors.Add(JsonUtility.FromJson<Sensor>(json));
        Debug.Log("Sensor added to list");
        }
        catch {
            Debug.Log("Add sensor to list failed!");
        }
    }

    public void ShowSensor(int numberSensor){
        //titleField.text = myList[numberSensor].name;
        //textField.text = "ID: " + myList[numberSensor].id + "\nPM2.5: " + myList[numberSensor].pm25;
        }
}
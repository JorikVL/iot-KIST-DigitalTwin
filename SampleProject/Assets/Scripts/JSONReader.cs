using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Geospatial;
using UnityEngine.UI;

public class JSONReader : MonoBehaviour
{
    public TextAsset textJSON;
    public Text textField;

    [System.Serializable]
    public class Sensor
    {
        public int id;
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
        mySensorList = GetData();
    }

    private SensorList GetData(){
        return JsonUtility.FromJson<SensorList>(textJSON.text);
    }

    public void ShowSensor(int numberSensor){
        foreach (Sensor sensor in mySensorList.sensor){
            if (sensor.id == numberSensor){
                textField.text = "sensor: " + sensor.name;
            }
        }
    }
}

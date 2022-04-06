using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Geospatial;
using UnityEngine.UI;

public class JSONReader : MonoBehaviour
{
    public TextAsset textJSON;
    public Text titleField;
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

    List<Sensor> myList = new List<Sensor>();

    [System.Serializable]
    public class SensorList
    {
        public Sensor[] sensor;
    }

    public SensorList mySensorList = new SensorList();

    void Start()
    {
        mySensorList = GetData();
        foreach (Sensor sensor in mySensorList.sensor){
            myList.Add(sensor);
        }
    }

    private SensorList GetData(){
        return JsonUtility.FromJson<SensorList>(textJSON.text);
    }

    public void ShowSensor(int numberSensor){
        titleField.text = myList[numberSensor].name;
        textField.text = "ID: " + myList[numberSensor].id + "\nPM2.5: " + myList[numberSensor].pm25;
        }
    }


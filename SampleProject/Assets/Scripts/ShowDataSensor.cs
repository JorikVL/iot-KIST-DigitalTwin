using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDataSensor : MonoBehaviour
{
    public Text DataDisplay;
    public Text SensorName;

    private GameObject manager;
    private JSONReader jsonReader;

    void Start(){
        manager = GameObject.Find("Manager");
        jsonReader = manager.GetComponent<JSONReader>();
    }

    public void DisplayData(string id){
        foreach (JSONReader.Sensor sensor in jsonReader.sensors){
            if (sensor._id == id){
                SensorName.text = sensor.Name;
                DataDisplay.text = "ID: " + sensor._id + "\nBattery: " + sensor.battery + "\nCO2: " + sensor.CO2 + "\nHumidity: " + sensor.humidity + "\nPm10: " + sensor.pm10 + "\nPm2.5: " + sensor.pm25 + "\nPressure: " + sensor.pressure + "\nSalinity: " + sensor.salinity + "\ntemp= " + sensor.temp + "\nTvox: " + sensor.tvox + "\nDate: " + sensor.time;
            }
        }

    }
}

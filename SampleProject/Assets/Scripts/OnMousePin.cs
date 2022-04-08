using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnMousePin : MonoBehaviour
{
    public string sensorId;
    private GameObject manager;
    private ShowDataSensor showDataSensor;
    private JSONReader jsonReader;

    void Start(){
        //Get manager object and JSONReader component
        manager = GameObject.Find("Manager");
        jsonReader = manager.GetComponent<JSONReader>();
        showDataSensor = manager.GetComponent<ShowDataSensor>();
    }

    private void OnMouseDown() {
        showDataSensor.DisplayData(sensorId);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnMousePin : MonoBehaviour
{
    public string sensorId;
    private GameObject manager;
    private ShowDataSensor showDataSensor;

    void Start(){
        //Get manager object and JSONReader component
        manager = GameObject.Find("Manager");
        showDataSensor = manager.GetComponent<ShowDataSensor>();
    }

    private void OnMouseDown() {
        showDataSensor.DisplayData(sensorId);
    }
}

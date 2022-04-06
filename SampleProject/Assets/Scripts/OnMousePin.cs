using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnMousePin : MonoBehaviour
{
    public int sensorNumber;
    private GameObject manager;
    private JSONReader jsonReader;

    void Start(){
        manager = GameObject.Find("Manager");
        jsonReader = manager.GetComponent<JSONReader>();
    }

    private void OnMouseDown() {
        jsonReader.ShowSensor(sensorNumber);
    }
}

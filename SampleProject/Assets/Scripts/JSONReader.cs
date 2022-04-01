using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    public TextAsset textJSON;

    [System.Serializable]
    public class Sensor
    {
        public string position;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

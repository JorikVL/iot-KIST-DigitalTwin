using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;

public class DynamicallyCreatePins : MonoBehaviour
{
    public GameObject pinPrefab;
    public List<LatLon> testList = new List<LatLon>();

    void start(){
        testList.Add(new LatLon(-6.234412, 39.238479));
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)){
            //Instantiate Pin
            var mapPin = Instantiate(pinPrefab);
            //Set pin as child of map
            mapPin.transform.parent = gameObject.transform;
            var mapPinComponent = mapPin.GetComponent<MapPin>();
            LatLon testt = new LatLon(-6.234412, 39.238479);
            mapPinComponent.Location = testt;
        }
    }
}
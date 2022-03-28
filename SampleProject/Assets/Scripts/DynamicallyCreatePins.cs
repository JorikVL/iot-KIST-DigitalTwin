using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;

public class DynamicallyCreatePins : MonoBehaviour
{
    public GameObject pinPrefab;

    float colorValue = 0.9f;

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
            
            //Get cube
            var Root = mapPin.transform.Find("Root").gameObject;
            var Cube = Root.transform.Find("Cube").gameObject;

            //Set color cube
            var mapPinRenderer = Cube.GetComponent<Renderer>();
            Color lerpedColor = Color.Lerp(Color.red, Color.green, colorValue);

            mapPinRenderer.material.color = lerpedColor;
        }
    }
}
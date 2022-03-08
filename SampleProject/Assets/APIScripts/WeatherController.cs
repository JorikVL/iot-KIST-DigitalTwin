﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.IO;
using Assets;
using Assets.APIScripts;
using UnityEngine.Networking;
using UnityEngine.UI;


public class WeatherController : MonoBehaviour
{

    private const string API_KEY = "73cb75a2a3ef8b58e77c3581ec7b4726";
    public string cityInput;
    public static string CityId;
    private static string DEFAULT_URL;

    public WeatherController()
    {
        cityInput = null;
        DEFAULT_URL = $"api.openweathermap.org/data/2.5/weather?q=Brussel&APPID={API_KEY}&units=metric";
    }


    void Update()
    {
        
    }

    // Where to send our request
    
    string targetUrl = DEFAULT_URL;

    // Keep track of what we got back
    string recentData = "";
    void Awake()
    {
        this.StartCoroutine(this.RequestRoutine(this.targetUrl, this.ResponseCallback));
    }
    // Web requests are typially done asynchronously, so Unity's web request system
    // returns a yield instruction while it waits for the response.
    //
    private IEnumerator RequestRoutine(string url, Action<string> callback = null)
    {
        // Using the static constructor
        var request = UnityWebRequest.Get(url);

        // Wait for the response and then get our data
        yield return request.SendWebRequest();
        var data = request.downloadHandler.text;

        // This isn't required, but I prefer to pass in a callback so that I can
        // act on the response data outside of this function
        if (callback != null)
            callback(data);
    }
    // Callback to act on our response data
    private void ResponseCallback(string data)
    {
        Debug.Log(data);
        recentData = data;
    }
    void OnGUI()
    {
        this.targetUrl = GUI.TextArea(new Rect(0, 0, 500, 100), this.targetUrl);
        GUI.TextArea(new Rect(0, 100, 500, 300), this.recentData);
        if (GUI.Button(new Rect(0, 400, 500, 100), "Resend Request"))
        {
            this.StartCoroutine(this.RequestRoutine(targetUrl, this.ResponseCallback));
        }
    }

    public void ReadStringInput(string s)
    {
        cityInput = s;
        Debug.Log(cityInput);
    }

}
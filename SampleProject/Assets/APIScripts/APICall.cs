using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.IO;
using Assets;
using Assets.APIScripts;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.APIScripts
{
    internal class APICall : MonoBehaviour
    {
        private const string API_KEY = "73cb75a2a3ef8b58e77c3581ec7b4726";
        public static string cityInput = "Zanzibar"; 
        private static string DEFAULT_URL = $"api.openweathermap.org/data/2.5/weather?q={cityInput}&APPID={API_KEY}&units=metric";
        public Text DataDisplay;
        public Text CityName;
        string targetUrl = DEFAULT_URL;

        public void ReadStringInput(string s)
        {
            cityInput = s;
            DEFAULT_URL = $"api.openweathermap.org/data/2.5/weather?q="+ cityInput +"&APPID="+ API_KEY+ "&units=metric";
            Debug.Log(cityInput);
        }

        // Where to send our request

        void Update()
        {
            targetUrl = DEFAULT_URL;
        }

        // Keep track of what we got back
        private string recentData = "";

        private IEnumerator RequestRoutine(string url, Action<string> callback = null)
        {
            // Using the static constructor
            var request = UnityWebRequest.Get(url);

            // Wait for the response and then get our data
            yield return request.SendWebRequest();
            var data = request.downloadHandler.text;

            // This isn't required, but I prefer to pass in a callback so that I can
            // act on the response data outside of this function
            callback?.Invoke(data);
        }
        // Callback to act on our response data
        private void ResponseCallback(string data)
        {
            Debug.Log(data);
            recentData = data;
            // call formatjson 
            DataDisplay.text = recentData;
            CityName.text = cityInput;
        }
        public void ApiCall()
        {
            this.StartCoroutine(this.RequestRoutine(targetUrl, this.ResponseCallback));
        }

        public void Start()
        {
            ApiCall();
        }
    }
}

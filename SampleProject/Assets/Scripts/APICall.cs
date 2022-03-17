using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;


namespace Assets.APIScripts
{
    public class APICall : MonoBehaviour
    {
        private const string API_KEY = "73cb75a2a3ef8b58e77c3581ec7b4726";
        public static string cityInput = "Zanzibar";
        private static string DEFAULT_URL = $"api.openweathermap.org/data/2.5/weather?q={cityInput}&APPID={API_KEY}&units=metric";
        public Text DataDisplay;
        public Text CityName;
        string targetUrl = DEFAULT_URL;
        private string recentData = "";
        public void ReadStringInput(string s)
        {
            cityInput = s;
            DEFAULT_URL = $"api.openweathermap.org/data/2.5/weather?q=" + cityInput + "&APPID=" + API_KEY + "&units=metric";
            Debug.Log(cityInput);
        }
        

        public void Update()
        {
            targetUrl = DEFAULT_URL;
            CityName.text = cityInput;
        }
        private IEnumerator RequestRoutine(string url, Action<string> callback = null)
        {
            var request = UnityWebRequest.Get(url);


            yield return request.SendWebRequest();
            var data = request.downloadHandler.text;

            callback?.Invoke(data);
        }
        private void ResponseCallback(string data)
        {
            Debug.Log(data);
            recentData = data;
            DataDisplay.text = recentData;
            
        }
        public void ApiCall()
        {
            this.StartCoroutine(this.RequestRoutine(targetUrl, this.ResponseCallback));
        }

        public void Start()
        {
            ApiCall();
        }
        public void AreaOne()
        {
            cityInput = "Antwerpen";
            DEFAULT_URL = "localhost:1880/Area1";
        }
        public void AreaTwo()
        {
            cityInput = "CapeTown";
            DEFAULT_URL = "localhost:1880/Area2";
        }
        public void AreaTree()
        {
            cityInput = "Zanzibar";
            DEFAULT_URL = "localhost:1880/Area3";
        }

        public void endProgram()
        {
            Application.Quit();
            Debug.Log("Quit button works!");
        }

    }
}
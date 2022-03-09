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
        public string recievedData;

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
        private string recentData = "";

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
            recievedData = data;
            
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
            DEFAULT_URL = $"api.openweathermap.org/data/2.5/weather?q=" + cityInput + "&APPID=" + API_KEY + "&units=metric";
            ApiCall();
            
        }

    }
}
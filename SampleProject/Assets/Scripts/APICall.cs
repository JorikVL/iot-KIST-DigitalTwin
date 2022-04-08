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
        private JSONReader jsonReader;    

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
            if (data != null)
            {
                callback?.Invoke(data);
            } else {
                Debug.Log("API call returned null");
            }
        }

        private void ResponseCallback(string data)
        {
            jsonReader = this.GetComponent<JSONReader>();
            jsonReader.AddSensor(data);
            recentData = data;
        }

        public void ApiCall()
        {
            this.StartCoroutine(this.RequestRoutine(targetUrl, this.ResponseCallback));
        }

        public void Awake()
        {
            GetData();
        }
        
        public void AreaOne()
        {
            cityInput = "Antwerpen";
            DEFAULT_URL = "http://159.223.210.10:1880/Area1";
        }

        public void AreaTwo()
        {
            cityInput = "CapeTown";
            DEFAULT_URL = "http://159.223.210.10:1880/Area2";
        }

        public void AreaTree()
        {
            cityInput = "Zanzibar";
            DEFAULT_URL = "http://192.168.100.134:1880/Data1";
        }

        public void GetData(){
            Debug.Log("GetData");
            for (int i = 1; i <= 20; i++)
            {
                targetUrl = "http://192.168.100.134:1880/Data" + i;
                try
                {
                    ApiCall();
                }
                catch (System.Exception)
                {
                    Debug.Log("APICall failed");
                }
            }
        }

        public void endProgram()
        {
            Application.Quit();
            Debug.Log("Quit button works!");
        }
    }
}
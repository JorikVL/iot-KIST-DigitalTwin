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
        private static string DEFAULT_URL = "http://192.168.100.134:1880/Data1";
        string targetUrl = DEFAULT_URL;
        private string recentData = "";
        private JSONReader jsonReader;    

        private IEnumerator RequestRoutine(string url, Action<string> callback = null)
        {
            var request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            var data = request.downloadHandler.text;
            callback?.Invoke(data);
        }

        private void ResponseCallback(string data)
        {
            jsonReader = this.GetComponent<JSONReader>();
            jsonReader.AddSensor(data);
        }

        public void ApiCall()
        {
            this.StartCoroutine(this.RequestRoutine(targetUrl, this.ResponseCallback));
        }

        public void Start()
        {
            InvokeRepeating("GetData", 0, 300);
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
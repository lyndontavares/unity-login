using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Login : MonoBehaviour
{
    public void DoLogin() {

        Debug.Log("Iniciando login...");

         StartCoroutine(callLoginTeste());
        // A correct website page.
        //StartCoroutine(GetRequest("https://www.example.com"));
        // A non-existing page.
        //StartCoroutine(GetRequest("https://error.html"));
    }

    public IEnumerator callLoginTeste()
    {
        Debug.Log("Logando...");

        string bodyJsonString = "{\"username\":\"isa\",\"password\":\"123456\"}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);

        UnityWebRequest www = new UnityWebRequest("http://localhost:8080/api/auth/signin", "POST");
        www.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();

        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        if (www.error != null) {
            Debug.Log("Error: "+www.error);
        } else {
            Debug.Log("Response: "+www.downloadHandler.text);
        }

    }


    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }

}

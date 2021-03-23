using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Login : MonoBehaviour
{
    public void DoLogin() {

        // A correct website page.
        StartCoroutine(GetRequest("https://www.example.com"));

        // A non-existing page.
        StartCoroutine(GetRequest("https://error.html"));

        //callLoginTeste();

        /*Text mensagem = transform.Find("Mensagem").GetComponent<Text>();

        if (mensagem != null )
        {
             mensagem.text = "Logou...";
        }
        else {
            Debug.Log("Não encontrou objeto mensagem!");
        }*/
    }

    public IEnumerator callLoginTeste()
    {
        Debug.Log("Logando...");

        WWWForm form = new WWWForm();
        form.AddField("username","isa");
        form.AddField("password","123456");
        UnityWebRequest www = UnityWebRequest.Post("https://busanello.dataparerp.com/",form);
        yield return www.Send();
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

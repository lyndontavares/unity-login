using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Login : MonoBehaviour
{

    private InputField username;
    private InputField password;
    private Text mensagem;


    void Start()
    {
        username = GameObject.Find("Email").GetComponent<InputField>();
        password = GameObject.Find("Senha").GetComponent<InputField>();
        mensagem = GameObject.Find("Mensagem").GetComponent<Text>();
    }

    public void DoLogin()
    {
        Debug.Log("Iniciando login...");

        if ( LoginVerification() )
        {
            StartCoroutine(callLoginTeste());
        }
    }

    public IEnumerator callLoginTeste()
    {
        Debug.Log("Logando: "+username.text+"/"+password.text+"...");
        mensagem.text = "Verificando login para usuário: "+username.text;

        // Preparando JSON para o request
        string bodyJsonString = "{\"username\":\""+username.text+"\",\"password\":\""+password.text+"\"}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);

        UnityWebRequest www = new UnityWebRequest("http://localhost:8080/api/auth/signin", "POST");
        www.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();
        if (www.error != null) {
            Debug.Log("Error: "+www.error);
             mensagem.text = "Usuário não autorizado!";

        } else {
            Debug.Log("Response: "+www.downloadHandler.text);
            mensagem.text = "Bem-vindo "+username.text+" !";
        }
    }

    bool LoginVerification() {
        string aviso = "";
        if ( username.text == "" ) {
            aviso = aviso + "Usuário inválido / ";
        }
        if ( password.text == "" ) {
            aviso = aviso + "Senha inválida / ";
        }
        if ( aviso != "")
        {
            mensagem.text = aviso;
        }
        return  username.text != "" && password.text!= "";
    }


    // A correct website page.
    //StartCoroutine(GetRequest("https://www.example.com"));
    // A non-existing page.
    //StartCoroutine(GetRequest("https://error.html"));

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

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
            StartCoroutine(ProcessLogin());
        }
    }

    public IEnumerator ProcessLogin()
    {
        Debug.Log("Logando: "+username.text+"/"+password.text+"...");
        mensagem.text = "Verificando login para usuário: "+username.text;

        LoginRequest loginRequest = new LoginRequest();
        loginRequest.username=username.text;
        loginRequest.password=password.text;

        string bodyJsonString = JsonUtility.ToJson(loginRequest);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);

        UnityWebRequest request = new UnityWebRequest("http://localhost:8080/api/auth/signin", "POST");

        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        if (request.error != null) {
            Debug.Log("Error: "+request.error);
            mensagem.text = "Usuário não autorizado!";
        } else {
            Debug.Log("Response: "+request.downloadHandler.text);
            LoginSucess(request.downloadHandler.text);
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

    void LoginSucess(string json)
    {
        ResponseSuccess res = new ResponseSuccess();
        res = JsonUtility.FromJson<ResponseSuccess>(json);
        mensagem.text = "Bem-vindo "+res.username+" ("+res.email+") !";
    }

    [System.Serializable]
    public class ResponseSuccess
    {
        public int id;
        public string username;
        public string email;
        public string accessToken;
    }

    [System.Serializable]
    public class LoginRequest
    {
        public string username;
        public string password;
    }


}

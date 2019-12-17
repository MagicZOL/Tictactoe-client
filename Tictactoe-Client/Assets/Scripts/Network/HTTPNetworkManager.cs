using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class HTTPNetworkManager : MonoBehaviour
{
    static HTTPNetworkManager instance;

    public static HTTPNetworkManager Instance
    {
        get
        {
            if(!instance)
            {
                //유니티에서 할당된 객체가 있는지 찾아본다
                instance = GameObject.FindObjectOfType(typeof(HTTPNetworkManager)) as HTTPNetworkManager;
                if(!instance)
                {
                    //새로 만든다
                    GameObject container = new GameObject();
                    container.name = "HTTPNetworkManager";
                    instance = container.AddComponent(typeof(HTTPNetworkManager)) as HTTPNetworkManager;
                }
            }
            return instance;
        }
    }
    public void SignUp(string username, string password, string name, Action<HTTPResponse> success, Action fail)
    {
        HTTPRequestsSignUp signUpData = new HTTPRequestsSignUp(username, password, name);
        var postData = signUpData.GetJSON();

        StartCoroutine(SendPostRequest(postData, HTTPNetworkConstant.singUpRequestURL, success, fail));
    }

    public void SIgnIn(string username, string password, Action<HTTPResponse> success, Action fail)
    {
        HTTPRequestSignIn signIndata = new HTTPRequestSignIn (username, password);

        //받은 데이터를 Json으로 변경
        var postData = signIndata.GetJSON();

        //로그인
        StartCoroutine(SendPostRequest(postData, HTTPNetworkConstant.signInRequestURL, success, fail));
    }

    public void AddScore(int score, Action<HTTPResponse> success, Action fail)
    {
        HTTPRequestAddScore addScoreData = new HTTPRequestAddScore(score);

        var postData = addScoreData.GetJSON();

        StartCoroutine(SendPostRequest(postData, HTTPNetworkConstant.addScoreRequestURL, success, fail));
    }

    public void Info(Action<HTTPResponse> success, Action fail)
    {
        StartCoroutine(SendGetRequest(HTTPNetworkConstant.infoRequestURL, success, fail));
    }

    IEnumerator SendGetRequest(string requestURL, Action<HTTPResponse> success, Action fail)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(HTTPNetworkConstant.serverURL + requestURL))
        {
            yield return www.SendWebRequest();

            if(www.isNetworkError)
            {
                Debug.Log(www.error);
                fail();
            }
            else if(www.isHttpError)
            {
                long code = www.responseCode;

                switch (code)
                {
                    case 401:
                        PlayerPrefs.SetString("sid", "");
                        GameManager.Instance.ShowSignInPanel();
                        break;
                }
                fail();
            }
            else
            {
                Dictionary<string, string> header = www.GetResponseHeaders();// 헤더 정보, 세션아이디 포함

                string cookie = header["Set-Cookie"];

                long code = www.responseCode; //www.responseCode는 long 타입 , 응답에대한 코드
                string message = www.downloadHandler.text;

                HTTPResponse response = new HTTPResponse(code, message, header);
                success(response);
            }
        }
    }

    //Dectinary
    IEnumerator SendPostRequest(string data, string requestURL, Action<HTTPResponse> success, Action fail)
    {
        //서버와 약속한 건 post이지만 현재는 문제가 많아 put으로 선언
        //using 문으로 선언하여 동작하는 범위를 지정할 수 있다.
        using (UnityWebRequest www = UnityWebRequest.Put(HTTPNetworkConstant.serverURL + requestURL, data))
        {
            www.method = "Post";
            www.SetRequestHeader("Content-Type", "application/json");

            string sid = PlayerPrefs.GetString("sid", "");
            {
                if(sid != "")
                {
                    www.SetRequestHeader("Set-Cookie", sid);
                }
            }
            //보내기, sendWebRequest를 하면서 다른일을 하게 만들어줌
            yield return www.SendWebRequest();

            // 서버 > 클라이언트로 응답(Response) 메시지 도착
            if (www.isNetworkError)
            {
                Debug.Log(www.error);
                fail();
            }
            else if(www.isHttpError)
            {
                long code = www.responseCode;

                switch (code)
                {
                    case 401:
                        PlayerPrefs.SetString("sid", "");
                        GameManager.Instance.ShowSignInPanel();
                        break;
                }
                fail();
            }
            else
            {
                Dictionary<string, string> header = www.GetResponseHeaders();// 헤더 정보, 세션아이디 포함

                string cookie = header["Set-Cookie"];

                long code = www.responseCode; //www.responseCode는 long 타입 , 응답에대한 코드
                string message = www.downloadHandler.text; 

                HTTPResponse response = new HTTPResponse(code, message, header); 
                success(response);
            }
        }
    }
}

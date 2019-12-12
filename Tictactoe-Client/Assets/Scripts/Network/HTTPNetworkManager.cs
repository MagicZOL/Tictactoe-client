using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

    public void SIgnIn(string username, string password)
    {
        SignInData signIndata = new SignInData(username, password);

        //받은 데이터를 Json으로 변경
        var postData = JsonUtility.ToJson(signIndata);

        //로그인
        //StartCoroutine(SendSignInRequest(username, password));
        StartCoroutine(SendPostRequest(postData, HTTPNetworkConstant.signInRequestURL));
    }

    public void AddScore(int score)
    {
        AddScoreData addScoreData = new AddScoreData(score);

        var postData = JsonUtility.ToJson(addScoreData);

        StartCoroutine(SendPostRequest(postData, HTTPNetworkConstant.addScoreRequestURL));
    }

    public void SignUp(string username, string password, string name)
    {
        SignUpData signUpData = new SignUpData(username, password, name);

        var postData = JsonUtility.ToJson(signUpData);

        StartCoroutine(SendPostRequest(postData, HTTPNetworkConstant.singUpRequestURL));
    }

    IEnumerator SendPostRequest(string data, string requestURL)
    {
        //서버와 약속한 건 post이지만 현재는 문제가 많아 put으로 선언
        //using 문으로 선언하여 동작하는 범위를 지정할 수 있다.
        using (UnityWebRequest www = UnityWebRequest.Put(HTTPNetworkConstant.serverURL + requestURL, data))
        {
            www.method = "Post";
            www.SetRequestHeader("Content-Type", "application/json");

            //보내기, sendWebRequest를 하면서 다른일을 하게 만들어줌
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                var headers = www.GetResponseHeaders();
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
    //IEnumerator SendSignInRequest(string username, string password)
    //{
    //    SignIndata signIndata = new SignIndata();
    //    signIndata.username = username;
    //    signIndata.password = password;

    //    //받은 데이터를 Json으로 변경
    //    var postData = JsonUtility.ToJson(signIndata);

    //    //서버와 약속한 건 post이지만 현재는 문제가 많아 put으로 선언
    //    UnityWebRequest www = UnityWebRequest.Put("localhost:3000/users/signin", postData);
    //    www.method = "Post";
    //    www.SetRequestHeader("Content-Type", "application/json");

    //    //보내기, sendWebRequest를 하면서 다른일을 하게 만들어줌
    //    yield return www.SendWebRequest();

    //    if(www.isNetworkError || www.isHttpError)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        var headers = www.GetResponseHeaders();
    //        Debug.Log(www.downloadHandler.text);
    //    }
    //}

    //IEnumerator SendAddScoreRequest(int score)
    //{
    //    AddScoreData addScoreData = new AddScoreData();
    //    addScoreData.score = score;

    //    var postData = JsonUtility.ToJson(addScoreData);

    //    //서버와 약속한 건 post이지만 현재는 문제가 많아 put으로 선언
    //    UnityWebRequest www = UnityWebRequest.Put("localhost:3000/users/addScore", postData);
    //    www.method = "Post";
    //    www.SetRequestHeader("Content-Type", "application/json");

    //    //보내기, sendWebRequest를 하면서 다른일을 하게 만들어줌
    //    yield return www.SendWebRequest();

    //    if (www.isNetworkError || www.isHttpError)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        var headers = www.GetResponseHeaders();
    //        Debug.Log(www.downloadHandler.text);
    //    }
    //}
}

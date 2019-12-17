using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTTPRequest : MonoBehaviour
{

}

public class HTTPRequestsSingUp
{
    public string username, password, name;
    public HTTPRequestsSingUp(string username, string password, string name)
    {
        this.username = username;
        this.password = password;
        this.name = name;
    }
}

public class HTTPRequestSignIn
{
    public string username, password;
    public HTTPRequestSignIn(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}

public class HTTPRequestAddScore
{
    public int score;
    public HTTPRequestAddScore(int score)
    {
        this.score = score;
    }
}

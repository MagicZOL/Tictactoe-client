using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTTPNetworkConstant : MonoBehaviour
{
    //public const string serverURL = "https://tictactoczol.herokuapp.com";
    public const string serverURL = "localhost:3000";

    //POST
    public const string signInRequestURL = "/users/signin";
    public const string singUpRequestURL = "/users/signup";
    public const string addScoreRequestURL = "/users/addscore";
    public const string addMessageRequestURL = "/chat/add";

    //GET
    public const string infoRequestURL = "/users/info";
    public const string logoutURL = "/users/logout";
    public const string chatRequestURL = "/chat/";
}

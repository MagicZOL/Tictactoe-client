using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTTPNetworkConstant : MonoBehaviour
{
    public const string serverURL = "localhost:3000";

    public const string signInRequestURL = "/users/signin";
    public const string singUpRequestURL = "/users/signup";
    public const string addScoreRequestURL = "/users/addscore";
}

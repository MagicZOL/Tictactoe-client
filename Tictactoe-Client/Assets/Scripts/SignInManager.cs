using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SignInManager : MonoBehaviour
{
    [SerializeField] Text signInID;
    [SerializeField] Text singInPW;

    [SerializeField] Text addScore;

    [SerializeField] Text signUpID;
    [SerializeField] Text signUpPW;
    [SerializeField] Text signUpName;
    public void SignIn()
    {
        //HTTPNetworkManager.Instance.SIgnIn("magic", "kim1234");
        HTTPNetworkManager.Instance.SIgnIn(signInID.text, singInPW.text);
    }

    public void AddScore()
    {
        //HTTPNetworkManager.Instance.AddScore(12);
        addScore.text = "점수 추가 : 12점";
        HTTPNetworkManager.Instance.AddScore(12);
    }

    public void SignUp()
    {
        HTTPNetworkManager.Instance.SignUp(signUpID.text, signUpPW.text, signUpName.text);
    }
}

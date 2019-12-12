using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SignInData
{
    public string username;
    public string password;

    public SignInData(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}

public struct AddScoreData
{
    public int score;

    public AddScoreData(int score)
    {
        this.score = score;
    }
}

public struct SignUpData
{
    public string username;
    public string password;
    public string name;

    public SignUpData(string username, string password, string name)
    {
        this.username = username;
        this.password = password;
        this.name = name;
    }
}
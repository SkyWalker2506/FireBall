using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalValues 
{
    public static float HighestScore
    {
        get { return PlayerPrefs.GetFloat("HighScore", 0); }
        set { if(HighestScore<value) PlayerPrefs.SetFloat("HighScore", value); }
    }


}
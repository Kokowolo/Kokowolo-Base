using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DateTime now = DateTime.Now;
        string date = $"{DateTime.Now.ToString("MMMM")} {DateTime.Now.Day}, {DateTime.Now.Year}";


        Debug.Log($"{date}");
        Debug.Log($"{DateTime.Now.ToShortDateString()}");
    }
}

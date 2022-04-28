using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSingleton : MonoBehaviour
{
    //public static TestSingleton Singleton { get; private set; }

    private void Awake()
    {
        //Singleton<TestSingleton>.Instance;
    }

    protected void Start()
    {
        // Debug.Log(Singleton.name);
    }
}

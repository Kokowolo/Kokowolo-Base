using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSingletonRunner : MonoBehaviour
{
    [SerializeField] TestSingleton testSingleton;

    private void Start()
    {
        //ISingleton singleton = testSingleton;
        //Debug.Log(singleton);
        //singleton.SayHello();
    }
}

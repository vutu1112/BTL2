using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader1<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance1;
    public static T Instance1
    {
        get
        {
            if (instance1 == null)
            {
                instance1 = FindObjectOfType<T>();
            }
            else if (instance1 != FindObjectOfType<T>())
            {
                Destroy(FindObjectOfType<T>());
            }
            DontDestroyOnLoad(FindObjectOfType<T>());

            return instance1;
        }

    }
    //public GameObject manager1;

    //void Awake()
    //{
    //    if (Manager1.intance1 == null)
    //    {
    //        Instantiate(manager1);
    //    }
    //}
}

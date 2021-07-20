
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance;

    public static T Instance
    {
        get
        {

            if(instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if(instance == null)
                {
                    var obj = new GameObject();
                    instance = obj.AddComponent<T>();
                    obj.name = typeof(T).ToString();

                }
            }
            return instance;
        }
    }

}

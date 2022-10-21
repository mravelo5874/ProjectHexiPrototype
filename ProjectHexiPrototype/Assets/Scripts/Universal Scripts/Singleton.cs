using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 // https://answers.unity.com/questions/408518/dontdestroyonload-duplicate-object-in-a-singleton.html
 public class Singleton<Instance> : MonoBehaviour where Instance : Singleton<Instance>
 {
    public static Instance instance;
    public bool is_persistent;
    
    public virtual void Awake() 
    {
        if (is_persistent)
        {
            if (!instance)
            {
                instance = this as Instance;
            }
            else 
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            instance = this as Instance;
        }
    }
 }
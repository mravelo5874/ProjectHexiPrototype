using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneManager : MonoBehaviour
{
    public virtual void Awake()
    {
        GameManager.instance.StartScene();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
    public void Collected()
    {
        Debug.Log(gameObject.name + "被收集了啊！");
        gameObject.SetActive(false);
    }
}

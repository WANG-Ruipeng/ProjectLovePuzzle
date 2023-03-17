using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    public GameObject[] GOInPool => GOInChildren;
    GameObject[] GOInChildren;

    public T[] GetComponentInPool<T>()
    {
        T[] ret = new T[GOInChildren.Length];
        for (int i = 0; i < GOInChildren.Length; i++)
        {
            ret[i] = GOInChildren[i].GetComponent<T>();
        }
        return ret;
    }
    void Init()
    {
        int cnt = transform.childCount;
        for (int i = 0; i < cnt; i++)
        {
            GOInChildren[i] = transform.GetChild(i).gameObject;
        }
    }
    private void Awake()
    {
        Init();
    }
}

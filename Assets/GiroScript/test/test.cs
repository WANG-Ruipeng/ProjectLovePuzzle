using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HyperCasual.Core;
public class test : MonoBehaviour
{
    private void Start()
    {

        Archive.Recreate();
        Archive.WriteLevelProgress(3);

    }
}

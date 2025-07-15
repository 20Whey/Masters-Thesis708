using System;
using UnityEngine;

public class enemy_controller : MonoBehaviour
{
    // Update is called once per frame

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 45;
    }
}



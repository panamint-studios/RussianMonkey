using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour
{
    private void Start()
    {
        GameState.Instance.AlarmActivated += ChooseRandomExit;
    }

    private void ChooseRandomExit()
    {
        var exitIndex = UnityEngine.Random.Range(0, transform.childCount);
        var child = transform.GetChild(exitIndex);
        child.GetComponent<ExitDoor>().SetAsExit();
    }
}

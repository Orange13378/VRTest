using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Antena : MonoBehaviour
{
    [SerializeField]
    private GameObject point1, point2;
    [SerializeField]
    private TMP_Text mText;
    private Vector3 pos1, pos2;

    void Update()
    {
        if(!ActivateDevice.activated) return;

        pos1 = point1.transform.position;
        pos2 = point2.transform.position;
    }

    void FixedUpdate()
    {
        if(!ActivateDevice.activated) return;
        double distance = (pos1.x - pos2.x) * (pos1.x - pos2.x) + (pos1.y - pos2.y) * (pos1.y - pos2.y) + (pos1.z - pos2.z) * (pos1.z - pos2.z);
        distance = Math.Round(distance, 2);
        mText.text = distance.ToString();
    }
}

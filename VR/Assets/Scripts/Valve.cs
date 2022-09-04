using HTC.UnityPlugin.Vive;
using HTC.UnityPlugin.ColliderEvent;
using HTC.UnityPlugin.PoseTracker;
using System;
using UnityEngine;
using TMPro;

public class Valve : MonoBehaviour
{
    [SerializeField] private GameObject startPoint;

    [SerializeField] private GameObject leftHand, rightHand;

    [SerializeField] private TMP_Text mText;

    private bool grabbed = false, turned = false, turnedM = false;

    [SerializeField] float angleSum;

    private float startZ, startZAbs, endZ;
    float angle, angleAbs, angleSumStart, angleEnd, limit, procent;
    int krug;

    public float maxValue; // изменяемое значение

    void Start()
    {
        startZ = 0;
        endZ = 0;
        angleSum = 0;
        angleSumStart = 0;
        krug = 0;
    }


    void Update()
    {
        if (!grabbed) return;

        leftHand.GetComponent<PoseFreezer>().enabled = true; // Включение фризера для позиции z и верчения контроллера
        rightHand.GetComponent<PoseFreezer>().enabled = true;

        if(ViveInput.GetPressEx(HandRole.LeftHand, ControllerButton.Trigger))
        {
            angle = (int)Vector2.SignedAngle(VivePose.GetPoseEx(HandRole.LeftHand).pos, startPoint.transform.position); // Определение угла
        }

        if(ViveInput.GetPressEx(HandRole.RightHand, ControllerButton.Trigger))
        {
            angle = (int)Vector2.SignedAngle(VivePose.GetPoseEx(HandRole.RightHand).pos, startPoint.transform.position);
        }

        if (angle >= 0) angleAbs = angle;
        else if (angle < 0) angleAbs = 180 + (180 - (int)Math.Abs(angle));  //Замена значений от -180 до 180 на обычные углы от 0 до 360

        limit = angleSum - angleEnd; //Вычесления лимита для корректной работы при условиях ниже

        if (angleSum - angleSumStart <= 90 && angleSum - angleSumStart >= -90 && limit + angleAbs <= maxValue && angleSum + angleAbs > 0 && angleAbs <= maxValue) // Различные условия для поворота вентеля
        {
            transform.eulerAngles = new Vector3(0, 0, -angleAbs); //Поворот вентеля
            endZ = angle;
            angleEnd = angle;
        }

        if (angleEnd < 0) angleEnd = 180 + (180 - (int)Math.Abs(angleEnd));

        if (!turned && angleEnd < 90 && startZAbs > 270)                    // Проверка на то прошел ли один оборот
        {
            turned = true;
            krug++;
        }
        else if (turned && angleEnd > 270 && startZAbs < 270)
        {
            turned = false;
            krug--;
        }
        else if (!turnedM && angleEnd > 270 && startZAbs < 90)
        {
            turnedM = true;
            turned = false;
            krug--;
        }
        else if (turnedM && angleEnd < 90 && startZAbs < 90)
        {
            turnedM = false;
            krug++;
        }
    }

    void FixedUpdate()
    {
        if (!grabbed) return;

        angleSum = 360 * krug + angleEnd; //Сколько поворот в сумме сделан
        if (angleSum < 0) angleSum = 0;
        procent = (float)Math.Round((angleSum / maxValue), 2); // Процент от максимального угла поворота

        mText.text = procent.ToString(); 
    }

    public void Activate()
    {
        grabbed = true; //При взятии указываем что взяли

        startZ = endZ; // Записываем начальное положение при повороте

        if (startZ >= 0) startZAbs = startZ;
        else if (startZ < 0) startZAbs = 180 + (180 - (int)Math.Abs(startZ)); // Перевод в обычные углы

        if (startZAbs >= 180 && startZAbs < 270) 
        {
            turned = false;
            turnedM = false;
        }

        if (startZAbs <= 180 && startZAbs > 90)
        {
            turned = true;
            turnedM = false;
        }
    }

    public void Deactivate()
    {
        grabbed = false;
        leftHand.GetComponent<PoseFreezer>().enabled = false;
        rightHand.GetComponent<PoseFreezer>().enabled = false;
        angleSumStart = 360 * krug + angleEnd;
    }
}

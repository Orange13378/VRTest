using UnityEngine;
//using HTC.UnityPlugin.Utility;
using HTC.UnityPlugin.Vive;

public class ActivateDevice : MonoBehaviour
{
    private float timeLeft = 3f;
    private bool grabbed = false; 
    public static bool activated = false;

    [SerializeField]
    private GameObject activeObjects;

    void Update()
    {
        if (!grabbed) return;
        if (activated) return;

        if (ViveInput.GetPress(HandRole.LeftHand, ControllerButton.Grip))
        {
            if (ViveInput.GetPressDown(HandRole.LeftHand, ControllerButton.Grip))
            {
                timeLeft = 2.5f;
                Debug.Log("Reload");
            }

            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                activated = true;
                Debug.Log("Activate");
                activeObjects.SetActive(true);
            }
        }

        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Grip))
        {
            if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Grip))
            {
                timeLeft = 2.5f;
                Debug.Log("Reload");
            }

            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                activated = true;
                Debug.Log("Activate");
                activeObjects.SetActive(true);
            }
        }
    }

    public void Activate()
    {
        grabbed = true;
    }

    public void Deactivate()
    {
        grabbed = false;
    }
}
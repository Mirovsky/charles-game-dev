using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ONetworkMenuSpawner : MonoBehaviour
{
    [Header("2D Camera"), SerializeField]
    GameObject mobileCamera;

    [Header("VR Camera"), SerializeField]
    GameObject vrCamera;
    [Header("VR Hands"), SerializeField]
    GameObject leftHand;
    [SerializeField]
    GameObject rightHand;


    void Start()
    {
        SpawnCamera();   
    }

    void SpawnCamera()
    {
        SpawnMobileCamera();
        SpawnVRCamera();
    }

    void SpawnMobileCamera()
    {
        mobileCamera = Instantiate(mobileCamera);
    }

    void SpawnVRCamera()
    {
        vrCamera = Instantiate(vrCamera);

        leftHand = Instantiate(leftHand);
        rightHand = Instantiate(rightHand);

        leftHand.GetComponent<Grabber>().SetParentTransform(vrCamera.transform);
        rightHand.GetComponent<Grabber>().SetParentTransform(vrCamera.transform);
    }
}

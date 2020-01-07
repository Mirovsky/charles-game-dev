using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCameraMovement : MonoBehaviour
{
    [SerializeField]
    float minScale;
    [SerializeField]
    float maxScale;

    Transform cameraTransform;

    bool rHandPressed;
    bool lHandPressed;
    bool handsPressed;

    Transform leftHand;
    Transform rightHand;

    Vector3 center;
    Vector3 dir;
    Vector3 prevDir;
    Vector3 prevCenter;

    void Start()
    {
        cameraTransform = transform;

        leftHand = GameObject.Find("LeftHandAnchor").GetComponent<Transform>();
        rightHand = GameObject.Find("RightHandAnchor").GetComponent<Transform>();
    }

    void Update()
    {
        QueryHandsButtons();

        if (!handsPressed) {
            prevCenter = (rightHand.localPosition + leftHand.localPosition) / 2;
            prevDir = rightHand.localPosition - leftHand.localPosition;
            return;
        }

        center = (rightHand.localPosition + leftHand.localPosition) / 2;
        dir = rightHand.localPosition - leftHand.localPosition;

        Move();
        Scale();

        prevCenter = center;
        prevDir = dir;
    }

    void Move()
    {
        var delta = prevCenter - center;

        var position = cameraTransform.position;
        position += delta * 15f;
        cameraTransform.position = position;
    }

    void Rotate()
    {

    }

    void Scale()
    {
        var scaleDelta = prevDir.magnitude - dir.magnitude;
        var newScale = cameraTransform.localScale + (Vector3.one * scaleDelta * 15f);

        ScaleAround(cameraTransform, transform.TransformPoint(center), newScale);
    }

    void ScaleAround(Transform target, Vector3 pivot, Vector3 newScale)
    {
        var a = target.localPosition;
        var b = pivot;

        var c = a - b;

        float resultScale = newScale.x / target.transform.localScale.x;

        Vector3 finalPosition = b + c * resultScale;

        target.localScale = newScale;
        target.localPosition = finalPosition;
    }

    Vector3 Vector3Clamp(Vector3 v, float min, float max)
    {
        v.x = Mathf.Clamp(v.x, min, max);
        v.y = Mathf.Clamp(v.y, min, max);
        v.z = Mathf.Clamp(v.z, min, max);

        return v;
    }

    void QueryHandsButtons()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
            lHandPressed = true;
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
            rHandPressed = true;

        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
            lHandPressed = false;
        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
            rHandPressed = false;

        handsPressed = lHandPressed && rHandPressed;
    }
}

using UnityEngine;

public class SyncTransformPosition : MonoBehaviour
{
    [SerializeField]
    Vector3Variable positionVariable;

    [SerializeField]
    [HideInInspector]
    Transform tr;

    void LateUpdate()
    {
        tr.position = positionVariable;
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        tr = GetComponent<Transform>();
    }
#endif
}

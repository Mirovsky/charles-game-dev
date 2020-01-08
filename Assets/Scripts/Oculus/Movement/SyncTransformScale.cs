using UnityEngine;


[RequireComponent(typeof(Transform))]
public class SyncTransformScale : MonoBehaviour
{
    [SerializeField]
    Vector3Variable scaleVariable;

    [SerializeField]
    [HideInInspector]
    Transform tr;

    void LateUpdate()
    {
        tr.localScale = scaleVariable;
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        tr = GetComponent<Transform>();
    }
#endif
}

using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class AxisLineDrawer : MonoBehaviour
{
    [SerializeField]
    VectorGrabbable grabbable;
    [SerializeField]
    LineRenderer axisLineRenderer;


    void Start()
    {
        axisLineRenderer.SetPositions(new Vector3[] { grabbable.clampLeft, grabbable.clampRight });    
    }


#if UNITY_EDITOR
    void OnValidate()
    {
        grabbable = GetComponentInParent<VectorGrabbable>();
        axisLineRenderer = GetComponent<LineRenderer>();
    }
#endif
}

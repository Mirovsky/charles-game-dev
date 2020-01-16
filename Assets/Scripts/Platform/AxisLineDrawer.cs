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
        var left = grabbable.clampLeft;
        var right = grabbable.clampRight;

        axisLineRenderer.positionCount = 5;
        axisLineRenderer.SetPositions(new Vector3[] {
            left,
            Vector3.Lerp(left, right, 0.25f),
            Vector3.Lerp(left, right, 0.5f),
            Vector3.Lerp(left, right, 0.7f),
            right
        });    
    }


#if UNITY_EDITOR
    void OnValidate()
    {
        grabbable = GetComponentInParent<VectorGrabbable>();
        axisLineRenderer = GetComponent<LineRenderer>();
    }
#endif
}

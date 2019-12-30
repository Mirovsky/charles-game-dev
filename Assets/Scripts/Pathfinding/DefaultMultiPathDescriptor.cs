using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MultiPathDescriptor))]
public class DefaultMultiPathDescriptor : MonoBehaviour
{
    [SerializeField, Help("Starting MultiPathDescriptor for level")]
    MultiPathDescriptor defaultMultiPath;

    public MultiPathDescriptor DefaultMultiPath => defaultMultiPath;

#if UNITY_EDITOR
    void OnValidate()
    {
        defaultMultiPath = GetComponent<MultiPathDescriptor>();
    }
#endif
}

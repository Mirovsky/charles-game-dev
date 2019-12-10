using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMultiPathDescriptor : MonoBehaviour
{
    [SerializeField, Help("Starting MultiPathDescriptor for level")]
    MultiPathDescriptor defaultMultiPath;

    public MultiPathDescriptor DefaultMultiPath => defaultMultiPath;
}

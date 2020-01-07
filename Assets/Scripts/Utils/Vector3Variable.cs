using UnityEngine;


[CreateAssetMenu(menuName = "OOO/Variables/Vector3 Variable")]
public class Vector3Variable : ScriptableObject
{
    public Vector3 value;


    public static implicit operator Vector3(Vector3Variable v) => v.value;
}

using UnityEngine;


[CreateAssetMenu(menuName = "OOO/Variables/Float")]
public class FloatVariable : ScriptableObject
{
    public float value;

    public static implicit operator float(FloatVariable f) => f.value;
}

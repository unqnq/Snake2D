using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyData", menuName = "ScriptableObjects/DifficultyData", order = 1)]
public class DifficultyData : ScriptableObject
{
    [Range(0.05f, 1f)]
    public float stepRate = 0.3f;
    
    [Range(1, 10)]
    public int startingTailLength = 2;
}

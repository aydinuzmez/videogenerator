using UnityEngine;

[CreateAssetMenu(fileName = "MyData", menuName = "ScriptableObjects/MyData", order = 1)]
public class MyData : ScriptableObject
{
    public string text;
    public int number;
}
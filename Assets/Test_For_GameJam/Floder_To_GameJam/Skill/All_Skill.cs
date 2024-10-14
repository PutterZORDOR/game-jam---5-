using UnityEngine;

[CreateAssetMenu(fileName = "All_Skill", menuName = "Scriptable Objects/All_Skill")]
public class All_Skill : ScriptableObject
{
    public string skillname;

    [TextArea(3, 10)]
    public string Description;

    [Header("Game Prefab")]
    public GameObject gamePrefab;

    [Header("Sprite")]
    public Sprite sprite;

    [Header("Typr Skill")]
    public Type_Skill Type;

    [Header("CD")]
    public float cd;

}
public enum Type_Skill { Passive, Active }

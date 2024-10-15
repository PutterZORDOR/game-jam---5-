using UnityEngine;

[CreateAssetMenu(fileName = "All_Skill", menuName = "Scriptable Objects/All_Skill")]
public class All_Skill : ScriptableObject
{
    public string skillname;

    [TextArea(3, 10)]
    public string Description;

    [Header("Sprite")]
    public Sprite sprite;

    [Header("Typr Skill")]
    public Type_Skill Type;

    [Header("Skill Name")]
    public Skill_Ability Ability;


    [Header("CD")]
    public float cd;

    [Header("Ability Duration")]
    public float duration_ability;

}
public enum Type_Skill { Passive, Active }

public enum Skill_Ability { Dig, Dash, Double_Jump, Increase_Hp, Increase_Dmg, Increase_Speed, Giant}

using UnityEngine;

[CreateAssetMenu(fileName = "AnimSet", menuName = "Characters/Animation Set", order = 0)]
public class AnimSet : ScriptableObject
{
    [Header("Base")]
    public AnimationClip Idle;
    public AnimationClip Attack;
    public AnimationClip Hit;

    [Header("Extra estados")]
    public AnimationClip Victory;
    public AnimationClip Defeat;
}

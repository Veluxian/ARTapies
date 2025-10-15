using UnityEngine;

[CreateAssetMenu(fileName = "AnimSet", menuName = "Characters/Animation Set", order = 0)]
public class AnimSet : ScriptableObject
{
    public AnimationClip Idle;
    public AnimationClip Attack;
    public AnimationClip Hit;
}

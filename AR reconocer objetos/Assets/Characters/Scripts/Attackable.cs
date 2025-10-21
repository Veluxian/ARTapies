using UnityEngine;

[RequireComponent(typeof(AnimBinder))]
public class Attackable : MonoBehaviour
{
    private AnimBinder _binder;

    void Awake() => _binder = GetComponent<AnimBinder>();

    public void Attack() => _binder.PlayAttack();
    public void ReceiveHit() => _binder.PlayHit();
    public void Victory() => _binder.PlayVictory();
    public void Defeat() => _binder.PlayDefeat();
}

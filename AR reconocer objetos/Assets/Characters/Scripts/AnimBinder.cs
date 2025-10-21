using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class AnimBinder : MonoBehaviour
{
    [Header("Set de animaciones propio de este modelo")]
    public AnimSet Animations;

    private Animator _animator;

    private static readonly int TrAttack = Animator.StringToHash("Tr_Attack");
    private static readonly int TrHit = Animator.StringToHash("Tr_Hit");
    private static readonly int TrVictory = Animator.StringToHash("Tr_Victory");
    private static readonly int TrDefeat = Animator.StringToHash("Tr_Defeat");

    void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator.runtimeAnimatorController == null)
        {
            Debug.LogError($"[{name}] Falta asignar AC_BaseCharacter al Animator.");
            return;
        }
        if (Animations == null)
        {
            Debug.LogError($"[{name}] Falta asignar un AnimSet.");
            return;
        }

        var overrideCtrl = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        var pairs = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        overrideCtrl.GetOverrides(pairs);

        ReplaceClip(pairs, "Idle", Animations.Idle);
        ReplaceClip(pairs, "Attack", Animations.Attack);
        ReplaceClip(pairs, "Hit", Animations.Hit);
        ReplaceClip(pairs, "Victory", Animations.Victory);
        ReplaceClip(pairs, "Defeat", Animations.Defeat);

        overrideCtrl.ApplyOverrides(pairs);
        _animator.runtimeAnimatorController = overrideCtrl;
    }

    private void ReplaceClip(List<KeyValuePair<AnimationClip, AnimationClip>> list, string baseName, AnimationClip newClip)
    {
        if (newClip == null) return;
        for (int i = 0; i < list.Count; i++)
        {
            var original = list[i].Key;
            if (original != null && original.name == baseName)
            {
                list[i] = new KeyValuePair<AnimationClip, AnimationClip>(original, newClip);
                return;
            }
        }
        Debug.LogWarning($"[{name}] No encontré clip base '{baseName}' en el controller.");
    }

    // API pública
    public void PlayAttack() => _animator.SetTrigger(TrAttack);
    public void PlayHit() => _animator.SetTrigger(TrHit);
    public void PlayVictory() => _animator.SetTrigger(TrVictory);
    public void PlayDefeat() => _animator.SetTrigger(TrDefeat);

    public void ForceIdle()
    {
        _animator.ResetTrigger(TrAttack);
        _animator.ResetTrigger(TrHit);
        _animator.ResetTrigger(TrVictory);
        _animator.ResetTrigger(TrDefeat);
        _animator.CrossFadeInFixedTime("Idle", 0.1f);
    }
}

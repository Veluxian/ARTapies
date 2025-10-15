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

        // Crea un Override a partir del controller base y reemplaza clips por los del AnimSet
        var overrideCtrl = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        var pairs = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        overrideCtrl.GetOverrides(pairs);

        ReplaceClip(pairs, "Idle", Animations.Idle);
        ReplaceClip(pairs, "Attack", Animations.Attack);
        ReplaceClip(pairs, "Hit", Animations.Hit);

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

    public void PlayAttack() => _animator.SetTrigger(TrAttack);
    public void PlayHit() => _animator.SetTrigger(TrHit);

    public void ForceIdle()
    {
        _animator.ResetTrigger(TrAttack);
        _animator.ResetTrigger(TrHit);
        _animator.CrossFadeInFixedTime("Idle", 0.1f);
    }
}

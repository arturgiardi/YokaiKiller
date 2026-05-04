using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Subweapons/Punch")]
public class PunchSub : SubweaponBase
{
    public override void EvaluateCombo(NewController controller, SubweaponCallback callback, NewController.ComboState comboState)
    {
        if (controller.attackCoroutine != null)
            controller.StopCoroutine(controller.attackCoroutine);

        switch(comboState)
        {
            case NewController.ComboState.Grounded:
                controller.attackCoroutine = _ExcecuteCombo(combo.sequence[0], controller, callback); //Punch only has one move in combo data
            break;
            case NewController.ComboState.Air:
                controller.attackCoroutine = _ExcecuteCombo(combo.airSequence[0], controller, callback); //Punch only has one move in combo data
            break;
            case NewController.ComboState.AirMoving:
                controller.attackCoroutine = _ExcecuteCombo(combo.airMovingSequence[0], controller, callback); //Punch only has one move in combo data
            break;
            case NewController.ComboState.Crouching:
                controller.attackCoroutine = _ExcecuteCombo(combo.crouchingSequence[0], controller, callback); //Punch only has one move in combo data
            break;
        }
        controller.StartCoroutine(controller.attackCoroutine);
    }

    IEnumerator _ExcecuteCombo(Combo comboData, NewController controller, SubweaponCallback callback)
    {
        //controller.aPlayer.PlayAudio(swingSFX);
        controller.animator.Play(comboData.animationName, 1, 0);
        yield return new WaitForSeconds(comboData.minTime);
        if (controller.dodgeState == NewController.DodgeState.Attacking)
            controller.dodgeState = NewController.DodgeState.None;
        controller.attackState = NewController.AttackState.Attacking;
        yield return new WaitForSeconds(comboData.nextChain / controller.SpecialSpeed - comboData.minTime / controller.SpecialSpeed);
        controller.attackState = NewController.AttackState.None;
        yield return new WaitForSeconds(comboData.maxTime / controller.SpecialSpeed - comboData.nextChain / controller.SpecialSpeed - comboData.minTime / controller.SpecialSpeed);
        if (controller.moveState == NewController.MoveState.Attacking)
            controller.moveState = NewController.MoveState.Enabled;
        callback();
    }

    public override void ApplySpecialEffect(StateController target)
    {
        if (target != null)
            target.Stutter(0.3f);
    }

    public override DamageInfo GenerateSubweaponDamage()
    {
        DamageInfo playerBaseDI = PlayerStatsController.instance.baseStats.damage;

        DamageInfo finalDamage = DamageInfo.CreateInstance<DamageInfo>();
        finalDamage.power = playerBaseDI.power;
        finalDamage.contactTime = contactTime;
        finalDamage.positiveVariation = positiveVariation;
        finalDamage.negativeVariation = negativeVariation;
        finalDamage.intensity = intensity;
        finalDamage.criticalChance = criticalChance;
        finalDamage.criticalMultiplier = criticalMultiplier;
        finalDamage.elementalInfo = elementalInfo;

        return finalDamage;
    }
}

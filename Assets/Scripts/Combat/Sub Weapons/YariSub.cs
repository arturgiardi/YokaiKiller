using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Subweapons/Yari")]
public class YariSub : SubweaponBase
{
    private NewController newController;
    private NewController.MoveState defaultMS;
    private NewController.JumpState defaultJS;

    public override void EvaluateCombo(NewController controller, SubweaponCallback callback, NewController.ComboState comboState)
    {
        newController = controller;

        if (controller.attackCoroutine != null)
            controller.StopCoroutine(controller.attackCoroutine);

        switch(comboState)
        {
            case NewController.ComboState.Grounded:
                controller.attackCoroutine = _ExcecuteCombo(combo.sequence[0], controller, callback); //Yari only has one move in combo data
            break;
            case NewController.ComboState.Air:
                controller.attackCoroutine = _ExcecuteCombo(combo.airSequence[0], controller, callback); //Yari only has one move in combo data
            break;
            case NewController.ComboState.AirMoving:
                controller.attackCoroutine = _ExcecuteCombo(combo.airSequence[0], controller, callback); //Yari only has one move in combo data
            break;
            case NewController.ComboState.Crouching:
                controller.attackCoroutine = _ExcecuteCombo(combo.crouchingSequence[0], controller, callback); //Yari only has one move in combo data
            break;
        }
        controller.StartCoroutine(controller.attackCoroutine);
    }

    IEnumerator _ExcecuteCombo(Combo comboData, NewController controller, SubweaponCallback callback)
    {
        PlayerStats.instance.OnDamage += TookDamage;

        //controller.aPlayer.PlayAudio(swingSFX);

        defaultMS = controller.moveState;

        controller.jumpState = NewController.JumpState.NoGravity;
        controller.moveState = NewController.MoveState.Attacking;

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

        if(!controller.CheckGrounded())
            controller.jumpState = NewController.JumpState.Jumping;
        else
            controller.jumpState = NewController.JumpState.None;
        controller.moveState = defaultMS;

        PlayerStats.instance.OnDamage -= TookDamage;

        callback();
    }

    public override void ApplySpecialEffect(StateController target)
    {
        //
    }

    public override DamageInfo GenerateSubweaponDamage()
    {
        DamageInfo playerCurrentDI = PlayerStatsController.instance.stats.damage;
        DamageInfo finalDamage = DamageInfo.CreateInstance<DamageInfo>();
        finalDamage.power = (playerCurrentDI.power/6) + power;
        Debug.Log(finalDamage.power);
        finalDamage.contactTime = contactTime;
        finalDamage.positiveVariation = positiveVariation;
        finalDamage.negativeVariation = negativeVariation;
        finalDamage.intensity = intensity;
        finalDamage.criticalChance = criticalChance;
        finalDamage.criticalMultiplier = criticalMultiplier;
        finalDamage.elementalInfo = elementalInfo;

        return finalDamage;
    }

    public void TookDamage(GameObject instigator, DamageInfo damage, Stats attacker)
	{
        Debug.Log("AQUI");

        if (newController.attackCoroutine != null)
            newController.StopCoroutine(newController.attackCoroutine);

		newController.animator.Play("No Combo", 1);

        newController.jumpState = defaultJS;
        
        if (newController.moveState == NewController.MoveState.Attacking)
            newController.moveState = NewController.MoveState.Enabled;

        newController.attackState = NewController.AttackState.None;

        PlayerStats.instance.OnDamage -= TookDamage;
	}


}

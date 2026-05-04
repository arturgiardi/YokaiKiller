using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    public Image healthbar;
    public Image healthbarShadow;
    public Transform target;
    public Animator animator;

    float targetFill = 1;


    void Update()
    {
        if(target != null)
            transform.position = target.position + Vector3.up/1.5f;
        healthbarShadow.fillAmount = Mathf.MoveTowards(healthbarShadow.fillAmount, targetFill, .4f*Time.deltaTime);
    }

    public void SetTargetPosition(Transform newTarget)
    {
        target = newTarget;
    }
    public void ChangeHP(float currentHP, float maxHP)
    {
        if(currentHP > 0)
            animator.SetTrigger("Trigger");
        else
            animator.SetTrigger("Kill");
        targetFill = currentHP/maxHP;
        healthbar.fillAmount = targetFill;
    }
    public void SetCurrentFill(float ammount)
    {
        healthbar.fillAmount = ammount;
        healthbarShadow.fillAmount = ammount;
    }
}
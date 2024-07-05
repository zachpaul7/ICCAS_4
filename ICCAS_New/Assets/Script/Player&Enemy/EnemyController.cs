using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator animator;

    #region ½ºÅÝ
    public int maxHp;
    public int dmg;
    public int curHp;
    #endregion

    #region ÇÃ·¡±×
    public bool isDead = false;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetStat(int index)
    {
        maxHp = DataBase.instance.enemyInfos[index].maxHp;
        dmg = DataBase.instance.enemyInfos[index].damage;

        curHp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;

        if (curHp <= 0)
        {
            isDead = true;
        }
    }

    public void Attack(int selectCharacter)
    {
        animator.SetTrigger("Attack");
        GameManager.instance.pc[selectCharacter].GetComponent<PlayerController>().TakeDamage(dmg);
    }

    public void Dead()
    {
        animator.SetTrigger("Dead");
    }

    public void ClearStage()
    {
        UIManager.instance.exUI.StageClearSuccess();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;

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
        maxHp = DataBase.instance.characterInfos[index].maxHp[DataBase.instance.characterData.level[index]];
        dmg = DataBase.instance.characterInfos[index].damage[DataBase.instance.characterData.level[index]];

        curHp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;

        if(curHp <= 0)
        {
            isDead = true;
            Debug.Log("»ç¸Á");
        }
    }

    public void Attack(int stageNum, float multi)
    {
        animator.SetTrigger("Attack");
        GameManager.instance.ec[stageNum].GetComponent<EnemyController>().TakeDamage((int)((dmg / 3) + (dmg * multi)));
    }

    public void DamageText()
    {
        UIManager.instance.exUI.ShowDamageText(0, dmg);
    }

    public void Dead()
    {
        animator.SetTrigger("Dead");
    }

    public void FailedStage()
    {
        UIManager.instance.exUI.StageClear(1);
    }
}

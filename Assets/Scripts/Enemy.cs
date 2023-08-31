using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackKinds
{
    Normal,
}

public class Enemy : MonoBehaviour
{
    [SerializeField] float hpAmount;
    [SerializeField] float attackPower;
    [SerializeField] Stage stage;

    public float HpAmount { get => hpAmount; }

    [SerializeField] Player player;

    private void Start()
    {
        StartCoroutine("action");
    }

    //ダメージ計算を行うメソッド
    public void damage(float damageAmount)
    {
        hpAmount -= damageAmount;

        if (hpAmount <= 0f)
        {
            Debug.Log("プレイヤーに倒された");
            stage.gameOver();
            StopAllCoroutines();    //スクリプト内のすべてのコルーチン終了
        }
    }

    //Playerに攻撃を行うメソッド
    private IEnumerator attack(Player target, AttackKinds attackKinds = AttackKinds.Normal)
    {
        switch (attackKinds)
        {
            case AttackKinds.Normal:
                Debug.Log("プレイヤーにNormal Attack");
                float damageAmount = attackPower;
                target.damage(damageAmount);
                break;
        }

        yield break;
    }

    //行動を管理するコルーチン
    private IEnumerator action()
    {
        while (player.HpAmount > 0f && hpAmount > 0f)
        {
            yield return new WaitForSeconds(5f);
            yield return attack(player, AttackKinds.Normal);
        }
        yield break;
    }
}

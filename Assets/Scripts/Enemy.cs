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

    //�_���[�W�v�Z���s�����\�b�h
    public void damage(float damageAmount)
    {
        hpAmount -= damageAmount;

        if (hpAmount <= 0f)
        {
            Debug.Log("�v���C���[�ɓ|���ꂽ");
            stage.gameOver();
            StopAllCoroutines();    //�X�N���v�g���̂��ׂẴR���[�`���I��
        }
    }

    //Player�ɍU�����s�����\�b�h
    private IEnumerator attack(Player target, AttackKinds attackKinds = AttackKinds.Normal)
    {
        switch (attackKinds)
        {
            case AttackKinds.Normal:
                Debug.Log("�v���C���[��Normal Attack");
                float damageAmount = attackPower;
                target.damage(damageAmount);
                break;
        }

        yield break;
    }

    //�s�����Ǘ�����R���[�`��
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public enum TextKinds
    {
        PlayerHP,
        EnemyHP,
    }
    public class BattleUIManager : MonoBehaviour
    {
        [SerializeField] Text playerHpText;
        [SerializeField] Text enemyHpText;

        //UI�e�L�X�g�̕ύX���s��
        public void textUpdate(TextKinds textKinds, float value)
        {
            switch (textKinds)
            {
                case TextKinds.PlayerHP:
                    playerHpText.text = "HP(Player)�F" + value.ToString(".0");
                    break;
                case TextKinds.EnemyHP:
                    enemyHpText.text = "HP(Enemy)�F" + value.ToString(".0");
                    break;
            }
        }
    }

}
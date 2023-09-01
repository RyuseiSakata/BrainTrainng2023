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

        //UIテキストの変更を行う
        public void textUpdate(TextKinds textKinds, float value)
        {
            switch (textKinds)
            {
                case TextKinds.PlayerHP:
                    playerHpText.text = "HP(Player)：" + value.ToString(".0");
                    break;
                case TextKinds.EnemyHP:
                    enemyHpText.text = "HP(Enemy)：" + value.ToString(".0");
                    break;
            }
        }
    }

}
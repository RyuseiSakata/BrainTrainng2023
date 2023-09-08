using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public enum UIKinds
    {
        PlayerHP,
        EnemyHP,
        AttackChargedTurn,
    }
    public class BattleUIManager : MonoBehaviour
    {
        [SerializeField] Text playerHpText;
        [SerializeField] Text enemyHpText;
        [SerializeField] Text chargeTurnText;
        [SerializeField] Slider playerHpSlider;
        [SerializeField] Slider enemyHpSlider;

        //UIテキストの変更を行う
        public void uiUpdate(UIKinds uiKinds, float value)
        {
            switch (uiKinds)
            {
                case UIKinds.PlayerHP:
                    playerHpSlider.value = value / Player.maxHp;
                    break;
                case UIKinds.EnemyHP:
                    enemyHpSlider.value = value / Enemy.maxHp;
                    break;
                case UIKinds.AttackChargedTurn:
                    chargeTurnText.text = value.ToString("00");
                    break;
            }
        }

    }

}
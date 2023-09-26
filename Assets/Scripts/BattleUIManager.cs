using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public enum UIKinds
    {
        PlayerHP,
        EnemyHP,
        EnemyName,
        NextActionCount,
        GameTime,
        Fase,
    }
    public class BattleUIManager : MonoBehaviour
    {
        [SerializeField] Text enemyNameText;
        [SerializeField] Text chargeTurnText;
        [SerializeField] Slider playerHpSlider;
        [SerializeField] Slider enemyHpSlider;
        [SerializeField] Text faseText;

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
                case UIKinds.NextActionCount:
                    chargeTurnText.text = value.ToString("00");
                    break;
                case UIKinds.Fase:
                    faseText.text = "ばとる  " + (value+1).ToString("0") + " / 3";
                    break;
            }
        }

        //UIテキストの変更を行う
        public void uiUpdate(UIKinds uiKinds, string value)
        {
            switch (uiKinds)
            {
                case UIKinds.EnemyName:
                    enemyNameText.text = value;
                    break;
            }
        }
    }

}
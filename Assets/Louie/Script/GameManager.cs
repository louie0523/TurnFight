using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Louie
{
    public class GameManager : MonoBehaviour
    {
        UIControl uIControl;
        PlayerControl playerControl;
        EnemyControl enemyControl;
        BattleSystem battleSystem;
        public int playerNum = 4;
        public int enemyNum = 4;

        private void Start()
        {
            GameObject objUIControl = new GameObject("UIControl");
            uIControl = objUIControl.AddComponent<UIControl>();

            GameObject objPlayerControl = new GameObject("PlayerControl");
            playerControl = objPlayerControl.AddComponent<PlayerControl>();
            playerControl.CreatPlayers(4);

            GameObject objEnemyControl = new GameObject("EnemyControl");
            enemyControl = objEnemyControl.AddComponent<EnemyControl>();
            enemyControl.CreatEnemy(4);

            GameObject objBattleSystem = new GameObject("BattleSystem");
            battleSystem = objBattleSystem.AddComponent<BattleSystem>();

            //battleSystem.StartBattle();
        }
    }
}

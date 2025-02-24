using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Louie
{
    public class EnemyControl : MonoBehaviour
    {
        private void Start()
        {

        }

        public void CreatEnemy(int num)
        {
            for (int i = 0; i < num; i++)
            {
                int Ran = Random.Range(0, GameManager.instance.Units.Length);
                GameObject objEnemy = GameObject.Instantiate(GameManager.instance.Units[1]);
                objEnemy.name = "Enemy " + i.ToString();
                objEnemy.tag = "Enemy";
                objEnemy.AddComponent<Unit>().unitName = "Enemy " + i;
                objEnemy.transform.position = GameManager.instance.Postions[1].position + new Vector3((i * 2f), 0, 0);

                Unit unit = objEnemy.GetComponent<Unit>();
                int RanStatus = Random.Range(GameManager.instance.Status.GetComponent<Status>().MinHP, GameManager.instance.Status.GetComponent<Status>().MaxHP);
                unit.maxHp = RanStatus;
                RanStatus = Random.Range(GameManager.instance.Status.GetComponent<Status>().MinattackDmg, GameManager.instance.Status.GetComponent<Status>().MaxattackDmg);
                unit.attackDmg = RanStatus;
                RanStatus = Random.Range(GameManager.instance.Status.GetComponent<Status>().MinskillDmg, GameManager.instance.Status.GetComponent<Status>().MaxskillDmg);
                unit.skillDmg = RanStatus;
                RanStatus = Random.Range(GameManager.instance.Status.GetComponent<Status>().Minspeed, GameManager.instance.Status.GetComponent<Status>().Maxspeed);
                unit.speed = RanStatus;

                unit.currentHp = unit.maxHp;
            }
        }
    }
}

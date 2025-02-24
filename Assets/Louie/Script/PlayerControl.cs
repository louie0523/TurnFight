using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Louie
{
    public class PlayerControl : MonoBehaviour
    {
        private void Start()
        {
             
        }

        public void CreatPlayers(int num)
        {
            for(int i = 0; i < num; i++)
            {
                int Ran = Random.Range(0, GameManager.instance.Units.Length);
                GameObject objPlayer = GameObject.Instantiate(GameManager.instance.Units[0]);
                objPlayer.name = "Player " + i.ToString();
                objPlayer.tag = "Player";
                objPlayer.AddComponent<Unit>().unitName = "Player " + i;
                objPlayer.transform.position = GameManager.instance.Postions[0].position + new Vector3((i * 2f), 0, 0);
                
                Unit unit = objPlayer.GetComponent<Unit>();
                int RanStatus = Random.Range(GameManager.instance.Status.GetComponent<Status>().MinHP, GameManager.instance.Status.GetComponent<Status>().MaxHP);
                unit.maxHp = RanStatus;
                RanStatus = Random.Range(GameManager.instance.Status.GetComponent<Status>().MinattackDmg, GameManager.instance.Status.GetComponent<Status>().MaxattackDmg);
                unit.attackDmg = RanStatus;
                RanStatus = Random.Range(GameManager.instance.Status.GetComponent<Status>().MinskillDmg, GameManager.instance.Status.GetComponent<Status>().MaxskillDmg);
                unit.skillDmg = RanStatus;
                RanStatus = Random.Range(GameManager.instance.Status.GetComponent<Status>().Minspeed, GameManager.instance.Status.GetComponent<Status>().Maxspeed);
                unit.speed= RanStatus;

                unit.currentHp = unit.maxHp;

            }
        }
    }
}

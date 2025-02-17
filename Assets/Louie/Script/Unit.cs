using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Louie
{
    public class Unit : MonoBehaviour
    {
        /// <summary>
        /// UI에 표시될 유닛 이름
        /// </summary>
        public string unitName;
        /// <summary>
        /// 최대 체력
        /// </summary>
        public int maxHp;
        /// <summary>
        /// 현재 체력
        /// </summary>
        public int currentHp;
        /// <summary>
        /// 현재 공격력 
        /// </summary>
        public int attackDmg;
        /// <summary>
        /// 현재 스킬 공격력
        /// </summary>
        public int skillDmg;
        /// <summary>
        /// 턴 순서에 영향을 미치는 속도
        /// </summary>
        public int speed;

        private void Start()
        {
            currentHp = maxHp;
        }

        public bool Damage(int damage)
        {
            currentHp -= damage;
            if(currentHp <= maxHp)
            {
                currentHp = 0;
                return true;
            }
            return false;   
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Louie
{
    public class Unit : MonoBehaviour
    {
        /// <summary>
        /// UI�� ǥ�õ� ���� �̸�
        /// </summary>
        public string unitName;
        /// <summary>
        /// �ִ� ü��
        /// </summary>
        public int maxHp;
        /// <summary>
        /// ���� ü��
        /// </summary>
        public int currentHp;
        /// <summary>
        /// ���� ���ݷ� 
        /// </summary>
        public int attackDmg;
        /// <summary>
        /// ���� ��ų ���ݷ�
        /// </summary>
        public int skillDmg;
        /// <summary>
        /// �� ������ ������ ��ġ�� �ӵ�
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

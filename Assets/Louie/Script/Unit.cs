using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


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

        Animator animator;


        public TextMeshPro HPText;
        
        private void Start()
        {
            currentHp = maxHp;
            animator = this.GetComponent<Animator>();
            HPText = transform.Find("ü���ؽ�Ʈ").GetComponent<TextMeshPro>();
        }

        private void Update()
        {
            HpTextUpdate();
        }


        void HpTextUpdate()
        {
            HPText.text = "HP : " + currentHp + "/" + maxHp;
        }

        public void AttackAni()
        {
            animator.SetBool("isAttack", true);
            StartCoroutine(Wait());
            animator.SetBool("isAttack", false);
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(0.5f);
        }

        public bool Damage(int damage)
        {
            currentHp -= damage;
            if(currentHp <= maxHp)
            {
                currentHp = 0;
                return true;
                animator.SetTrigger("Death");
            }
            return false;   
        }
    }
}

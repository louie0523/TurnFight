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

        Animator animator;


        public TextMeshPro HPText;
        
        private void Start()
        {
            currentHp = maxHp;
            animator = this.GetComponent<Animator>();
            HPText = transform.Find("체력텍스트").GetComponent<TextMeshPro>();
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

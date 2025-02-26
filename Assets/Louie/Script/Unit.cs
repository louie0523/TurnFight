using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Properties;


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

        public bool isDead = false;

        Animator animator;

        public GameObject TartGetObj;
        public bool isAttack = false;


        public TextMeshPro HPText;

        GameObject MyPostion;
        private void Start()
        {
            currentHp = maxHp;
            animator = this.GetComponent<Animator>();
            HPText = transform.Find("체력텍스트").GetComponent<TextMeshPro>();
            if(this.gameObject.tag == "Player")
            {
                this.transform.rotation = Quaternion.Euler(0, -180f, 0);
            }
            
        }

        private void Update()
        {
            HpTextUpdate();
        }


        void HpTextUpdate()
        {
            HPText.text = "HP : " + currentHp + "/" + maxHp;
        }

        public void SelectTarGet(GameObject Enemy)
        {
            TartGetObj = Enemy;
            isAttack = true;
            AttackAni();
        }


        IEnumerator Wait(float time)
        {
            yield return new WaitForSeconds(time);
        }

        void AttackAni()
        {
            if(isAttack)
            {
                this.transform.LookAt(TartGetObj.transform.position + new Vector3(0, 1, 0));
                StartCoroutine(Wait(0.3f));
                animator.SetTrigger("Attack");
                TartGetObj = null;
                isAttack = false;
            }
        }


        public bool Damage(int damage)
        {
            currentHp -= damage;
            if(currentHp <= 0)
            {
                currentHp = 0;
                return true;
            }
            return false;   
        }
    }
}

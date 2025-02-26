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
            HPText = transform.Find("ü���ؽ�Ʈ").GetComponent<TextMeshPro>();
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

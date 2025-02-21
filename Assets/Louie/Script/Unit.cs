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

        Animator animator;

        public GameObject TartGetObj;
        public bool isAttack = false;
        public bool isBack = false;


        public TextMeshPro HPText;

        GameObject MyPostion;
        private void Start()
        {
            currentHp = maxHp;
            animator = this.GetComponent<Animator>();
            HPText = transform.Find("ü���ؽ�Ʈ").GetComponent<TextMeshPro>();
            MyPostion = new GameObject(gameObject.name + "Postion");
            MyPostion.transform.position = this.transform.position;
            MyPostion.transform.rotation = this.transform.rotation;
            
        }

        private void Update()
        {
            HpTextUpdate();
            if(TartGetObj != null)
            {
                Move();
                Rotation();
            }
            if(isBack)
            {
                BackWalk();
            }
        }


        void HpTextUpdate()
        {
            HPText.text = "HP : " + currentHp + "/" + maxHp;
        }

        public void SelectTarGet(GameObject Enemy)
        {
            TartGetObj = Enemy;
            isAttack = true;
        }

        void Move()
        {
            if(Vector3.Distance(TartGetObj.transform.position, this.transform.position) > 1.5f && TartGetObj != null)
            {
                this.transform.Translate(this.transform.forward * 2f * Time.deltaTime);
            } else
            {
                AttackAni();
            }
        }

        void Rotation()
        {
            this.transform.LookAt(TartGetObj.transform.position + new Vector3(0, 1, 0));
        }

        void AttackAni()
        {
            if(isAttack)
            {
                animator.SetTrigger("Attack");
                TartGetObj = null;
                isAttack = false;
                isBack = true;
            }
        }

        void BackWalk()
        {
            if(Vector3.Distance(this.transform.position, MyPostion.transform.position) > 0.5f)
            {
                this.transform.Translate(-this.transform.forward * 2f * Time.deltaTime);
            } else
            {
                this.transform.position = MyPostion.transform.position;
                this.transform.rotation = MyPostion.transform.rotation;
                isBack = false;
            }
        }

        public bool Damage(int damage)
        {
            currentHp -= damage;
            if(currentHp <= 0)
            {
                currentHp = 0;
                animator.SetTrigger("Death");
                return true;
            }
            return false;   
        }
    }
}

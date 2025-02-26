using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Louie
{
    public class Player : MonoBehaviour
    {
        public float speed = 4f;
        public float CurrentTime = 0f;
        public float BattleTime = 10f;
        bool Walk = false;
        Animator animator;
        FllowCamera fllowCamera;
        private void Start()
        {
            animator = this.GetComponent<Animator>();
            fllowCamera = GameObject.Find("Main Camera").GetComponent<FllowCamera>();
        }
        private void Update()
        {
            Move();
            Rotation();
            CurrentTime += Time.deltaTime;
            if(CurrentTime >= BattleTime && !GameManager.instance.Battling)
            {
                CurrentTime = 0;
                EnemyBattle();
            }
        }

        void Move()
        {
            Walk = false;
            if (Input.GetKey(KeyCode.W))
            {
                this.transform.Translate(this.transform.forward * speed * Time.deltaTime);
                Walk = true;
            } else if (Input.GetKey(KeyCode.S))
            {
                this.transform.Translate(-this.transform.forward * speed * Time.deltaTime);
                Walk = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                this.transform.Translate(this.transform.right * speed * Time.deltaTime);
                Walk = true;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                this.transform.Translate(-this.transform.right * speed * Time.deltaTime);
                Walk = true;
            }

            if (Walk)
            {
                animator.SetBool("Walk", true);
            } else
            {
                animator.SetBool("Walk", false);
            }

        }

        void Rotation()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Plane plane = new Plane(Vector3.up, Vector3.zero);

            float rayLength;
            if (plane.Raycast(ray, out rayLength))
            {
                Vector3 mousePoint = ray.GetPoint(rayLength);

                this.transform.LookAt(new Vector3(mousePoint.x, this.transform.position.y, mousePoint.z));
            }
        }

        void EnemyBattle()
        {
            //GameManager.instance.StartBattles();
            //fllowCamera.target = GameObject.Find("BattleLook").transform;
            //this.gameObject.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

namespace Louie
{
    public class BattleSystem : MonoBehaviour
    {
        /// <summary>
        /// 배틀 시스템 상태값
        /// </summary>
        public BattleStatus status;
        /// <summary>
        /// 플레이어의 유닛들
        /// </summary>
        public List<Unit> players = new List<Unit>();
        /// <summary>
        /// 적의 유닛들
        /// </summary>
        public List<Unit> enemys = new List<Unit>();
        /// <summary>
        ///  모든 유닛들
        /// </summary>
        private List<Unit> units = new List<Unit> ();
        /// <summary>
        /// 유닛들의 순번 지정
        /// </summary>
        private List<Unit> turnQueue;
        /// <summary>
        /// 유닛들의 행동 저장
        /// </summary>
        private Dictionary<Unit, int> unitActions;
        /// <summary>
        /// UI 전체 컨트롤
        /// </summary>
        public UIControl uIControl;
        /// <summary>
        /// 현재 움직일 턴인 플레이어 유닛
        /// </summary>
        private Unit currentPlayerUnit;
        /// <summary>
        /// 공격시 선택된 적
        /// </summary>
        private Unit selectedTarget;
        /// <summary>
        /// 타겟을 선택하는 도중인지 확인
        /// </summary>
        public bool isTargeting = false;

        //public Transform CamTransfom;

        private void Awake()
        {
            uIControl = GameObject.Find("UIControl").GetComponent<UIControl>();
        }


        public void StartBattle()
        {
            StartCoroutine(SetBattle());
        }

        IEnumerator SetBattle()
        {
            //GameManager.instance.Battling = true;
            //Camera cam = Camera.main;
            //CamTransfom = cam.transform;
            //cam.transform.position = new Vector3(-2f, 6f, 0);
            //cam.transform.rotation = Quaternion.Euler(50f, 90f, 0);
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
            {
                players.Add(obj.GetComponent<Unit>());
                units.Add(obj.GetComponent<Unit>());
            }
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                enemys.Add(obj.GetComponent<Unit>());
                units.Add(obj.GetComponent<Unit>());
            }
            if (players.Count < 1)
            {
                Debug.LogError("Player가 존재 하지 않습니다.");
            }
            if (enemys.Count < 1)
            {
                Debug.LogError("Enemy가 존재 하지 않습니다.");
            }

            uIControl.CreatCanvas();
            uIControl.CreatHPUI();
            uIControl.SetTexting("야생의 몬스터들과 마주쳤다.");

            yield return new WaitForSeconds(1.0f);
            SetTurnQueue();
            SetTurnUI();
            ProcessTurn();
        }



        void SetTurnQueue()
        {
            turnQueue = new List<Unit>(units);
            SortTurnQueueBySpeed();
            //unitActions = new Dictionary<Unit, int>();

            //foreach(Unit unit in units)
            //{
            //    ADDUnitToQueue(unit);
            //}
        }


        void SortTurnQueueBySpeed()
        {
            turnQueue = turnQueue.Where(units => ! units.isDead).OrderByDescending(units => units.speed).ToList();
        }
        /// <summary>
        /// 각 유닛의 속도 값을 상대유닛의 속도로 나누어 행동한다.
        /// (속도가 많이차이나는 유닛이 한턴에 여러번 공격가능)
        /// </summary>
        /// <param name="unit"></param>
        //void ADDUnitToQueue(Unit unit)
        //{
        //    int actions = Mathf.Max(1, unit.speed / GetFastestSpeed());
        //    unitActions[unit] = actions;
        //    for(int i = 0; i < actions; i++)
        //    {
        //        turnQueue.Enqueue(unit);
        //    }
        //}
        ///// <summary>
        ///// 가장 빠른 속도를 가진 Unit 속도를 받아온다.
        ///// </summary>
        ///// <returns></returns>
        //int GetFastestSpeed()
        //{
        //    int maxSpeed = 1;
        //    foreach(Unit unit in units)
        //    {
        //        if(unit.speed > maxSpeed)
        //            maxSpeed = unit.speed;
        //    }
        //    return maxSpeed;
        //}
        /// <summary>
        /// 턴 넘김
        /// </summary>
        void ProcessTurn()
        {
            SortTurnQueueBySpeed();
            if(turnQueue.Count == 0)
            {
                SetTurnQueue();
            }
            SetTurnUI();
            //currentPlayerUnit = turnQueue.Dequeue();
            currentPlayerUnit = turnQueue[0];
            turnQueue.RemoveAt(0);
            // 플레이어들에서 람다식을 통해 현재유닛이 있는지 확인한다.
            if(players.Exists(p => p.GetComponent<Unit>() == currentPlayerUnit))
            {
                status = BattleStatus.Player_Turn;
                PlayerTurn(currentPlayerUnit);
            } else
            {
                status = BattleStatus.Enemy_Turn;
                StartCoroutine(EnemyTurn(currentPlayerUnit));
            }
        }

        void PlayerTurn(Unit unit)
        {
            uIControl.PlayerTurn(unit.unitName);
        }

        IEnumerator EnemyTurn(Unit unit)
        {
            uIControl.SetTexting(unit.unitName + "의 턴");
            yield return new WaitForSeconds(1.0f);

            Unit target = players[Random.Range(0, players.Count)].GetComponent<Unit>();
            int Count = 0;
            while(target.isDead)
            {
                target = players[Random.Range(0, players.Count)].GetComponent<Unit>();
                Count++;
                if(Count > 100)
                {
                    Debug.LogError("100번이상 반복할 만큼 문제가 생김");
                    break;
                }
            }
            unit.SelectTarGet(target.gameObject);
            yield return new WaitUntil(() => !unit.isAttack);
            bool isDead = target.Damage(unit.attackDmg);

           uIControl.SetTexting(unit.unitName + "가" + target.unitName + "를 공격");

            yield return new WaitForSeconds(1f);
            if(isDead)
            {
                uIControl.SetTexting(target.unitName + "가 사망!");
                target.isDead = true;
                target.GetComponent<BoxCollider>().enabled = false;
                target.GetComponent<Animator>().SetTrigger("Death");
                turnQueue.RemoveAll(unit => unit.isDead);
                //RemoveDeadUnitFromQueue();
            }
            uIControl.SetPlayerHP(target);
            if (!players.Exists(x => !x.isDead))
            {
                uIControl.SetTexting("적이 승리하였습니다.");
                yield return new WaitForSeconds(3f);
                GameManager.instance.Battling = false;
            }
            else
            {
                ProcessTurn();
            }
        }

        public void SelectTartget(Unit unit)
        {
            selectedTarget = unit;
            StartCoroutine("PlayerAttack");
        }

        
        public void EndTurn()
        {
            ProcessTurn();
        }

        void SetTurnUI()
        {
            List<string> turnOrder = new List<string>();
            foreach (Unit unit in turnQueue)
            {
                if (!unit.isDead)
                {
                    turnOrder.Add(unit.unitName);
                }
            }
            uIControl.SetTextTurnOrder("현재 턴 : " + string.Join(" -> ", turnOrder));
        }

        IEnumerator PlayerAttack()
        {
            
            uIControl.SetTexting(currentPlayerUnit.unitName + "가" + selectedTarget.unitName + "를 공격!");
            Debug.Log(currentPlayerUnit.attackDmg);
            currentPlayerUnit.SelectTarGet(selectedTarget.gameObject);
            yield return new WaitUntil(() => !currentPlayerUnit.isAttack);    
            bool isDead = selectedTarget.Damage(currentPlayerUnit.attackDmg);

            yield return new WaitForSeconds(0.7f);

            if(isDead)
            {
                uIControl.SetTexting(selectedTarget.unitName + "가 사망!");

                selectedTarget.isDead = true;
                selectedTarget.GetComponent<BoxCollider>().enabled = false;
                selectedTarget.GetComponent<Animator>().SetTrigger("Death");
                turnQueue.RemoveAll(unit => unit.isDead);
                //RemoveDeadUnitFromQueue();
            }
            uIControl.SetEnemyHP(selectedTarget);
            if (!enemys.Exists(x => !x.isDead))
            {
                uIControl.SetTexting("플레이어가 승리하였습니다.");
                yield return new WaitForSeconds(3f);
                GameManager.instance.Battling = false;
            } else
            {
                ProcessTurn();
            }

        }

        //void RemoveDeadUnitFromQueue()
        //{
        //    Queue<Unit> queue = new Queue<Unit>();
        //    foreach( Unit unit in turnQueue)
        //    {
        //        if (!unit.isDead)
        //        {
        //            queue.Enqueue(unit);
        //        }
        //    }

        //    turnQueue = queue;
        //}

        private void Update()
        {
            if(isTargeting)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if(Physics.Raycast(ray,out hit))
                    {
                        // Ray가 충돌한 오브젝트 출력
                        // Debug.Log("클릭한오브젝트: " + hit.collider.gameobject.name);
                        if(hit.transform.CompareTag("Enemy"))
                        {
                            SelectTartget(hit.collider.GetComponent<Unit>());
                        }
                    }
                }
            }
        }

        void BattelEnd()
        {
            Debug.Log("전투가 종료되었습니다.");
        }
    }
}

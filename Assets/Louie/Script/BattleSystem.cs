using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
        private List<Unit> players = new List<Unit>();
        /// <summary>
        /// 적의 유닛들
        /// </summary>
        private List<Unit> enemys = new List<Unit>();
        /// <summary>
        ///  모든 유닛들
        /// </summary>
        private List<Unit> units = new List<Unit> ();
        /// <summary>
        /// 유닛들의 순번 지정
        /// </summary>
        private Queue<Unit> turnQueue;
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

        private void Awake()
        {
            uIControl = GameObject.Find("UIControl").GetComponent<UIControl>();
        }

        private void Start()
        {
            status = BattleStatus.Start;
            Invoke("StartBattle", 2.0f);
        }

        public void StartBattle()
        {
            StartCoroutine(SetBattle());
        }

        IEnumerator SetBattle()
        {
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

            uIControl.SetTexting("야생의 몬스터들과 마주쳤다.");

            yield return new WaitForSeconds(1.0f);
            SetTurnQueue();
            SetTurnUI();
            ProcessTurn();
        }



        void SetTurnQueue()
        {
            turnQueue = new Queue<Unit>();
            unitActions = new Dictionary<Unit, int>();

            foreach(Unit unit in units)
            {
                ADDUnitToQueue(unit);
            }
        }
        /// <summary>
        /// 각 유닛의 속도 값을 상대유닛의 속도로 나누어 행동한다.
        /// (속도가 많이차이나는 유닛이 한턴에 여러번 공격가능)
        /// </summary>
        /// <param name="unit"></param>
        void ADDUnitToQueue(Unit unit)
        {
            int actions = Mathf.Max(1, unit.speed / GetFastestSpeed());
            unitActions[unit] = actions;
            for(int i = 0; i < actions; i++)
            {
                turnQueue.Enqueue(unit);
            }
        }
        /// <summary>
        /// 가장 빠른 속도를 가진 Unit 속도를 받아온다.
        /// </summary>
        /// <returns></returns>
        int GetFastestSpeed()
        {
            int maxSpeed = 1;
            foreach(Unit unit in units)
            {
                if(unit.speed > maxSpeed)
                    maxSpeed = unit.speed;
            }
            return maxSpeed;
        }
        /// <summary>
        /// 턴 넘김
        /// </summary>
        void ProcessTurn()
        {
            if(turnQueue.Count == 0)
            {
                SetTurnQueue();
            }
            SetTurnUI();
            currentPlayerUnit = turnQueue.Dequeue();

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
            bool isDead = target.Damage(unit.attackDmg);

           uIControl.SetTexting(unit.unitName + "가" + target.unitName + "를 공격");

            yield return new WaitForSeconds(1f);
            if(isDead)
            {
                uIControl.SetTexting(target.unitName + "가 사망!");
            }
            ProcessTurn();
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
                turnOrder.Add(unit.unitName);
            }
            uIControl.SetTextTurnOrder("현재 턴 : " + string.Join(" -> ", turnOrder));
        }

        IEnumerator PlayerAttack()
        {
            
            uIControl.SetTexting(currentPlayerUnit.unitName + "가" + selectedTarget.unitName + "를 공격!");
            currentPlayerUnit.AttackAni();
            yield return new WaitForSeconds(0.3f);
            bool isDead = selectedTarget.Damage(currentPlayerUnit.attackDmg);

            yield return new WaitForSeconds(0.7f);

            if(isDead)
            {
                uIControl.SetTexting(selectedTarget.unitName + "가 사망!");
            }

            ProcessTurn();
        }

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
                        SelectTartget(hit.collider.GetComponent<Unit>());
                    }
                }
            }
        }
    }
}

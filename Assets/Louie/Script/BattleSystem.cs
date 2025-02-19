using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Louie
{
    public class BattleSystem : MonoBehaviour
    {
        /// <summary>
        /// ��Ʋ �ý��� ���°�
        /// </summary>
        public BattleStatus status;
        /// <summary>
        /// �÷��̾��� ���ֵ�
        /// </summary>
        private List<Unit> players = new List<Unit>();
        /// <summary>
        /// ���� ���ֵ�
        /// </summary>
        private List<Unit> enemys = new List<Unit>();
        /// <summary>
        ///  ��� ���ֵ�
        /// </summary>
        private List<Unit> units = new List<Unit> ();
        /// <summary>
        /// ���ֵ��� ���� ����
        /// </summary>
        private Queue<Unit> turnQueue;
        /// <summary>
        /// ���ֵ��� �ൿ ����
        /// </summary>
        private Dictionary<Unit, int> unitActions;
        /// <summary>
        /// UI ��ü ��Ʈ��
        /// </summary>
        public UIControl uIControl;
        /// <summary>
        /// ���� ������ ���� �÷��̾� ����
        /// </summary>
        private Unit currentPlayerUnit;
        /// <summary>
        /// ���ݽ� ���õ� ��
        /// </summary>
        private Unit selectedTarget;
        /// <summary>
        /// Ÿ���� �����ϴ� �������� Ȯ��
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
                Debug.LogError("Player�� ���� ���� �ʽ��ϴ�.");
            }
            if (enemys.Count < 1)
            {
                Debug.LogError("Enemy�� ���� ���� �ʽ��ϴ�.");
            }

            uIControl.SetTexting("�߻��� ���͵�� �����ƴ�.");

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
        /// �� ������ �ӵ� ���� ��������� �ӵ��� ������ �ൿ�Ѵ�.
        /// (�ӵ��� �������̳��� ������ ���Ͽ� ������ ���ݰ���)
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
        /// ���� ���� �ӵ��� ���� Unit �ӵ��� �޾ƿ´�.
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
        /// �� �ѱ�
        /// </summary>
        void ProcessTurn()
        {
            if(turnQueue.Count == 0)
            {
                SetTurnQueue();
            }
            SetTurnUI();
            currentPlayerUnit = turnQueue.Dequeue();

            // �÷��̾�鿡�� ���ٽ��� ���� ���������� �ִ��� Ȯ���Ѵ�.
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
            uIControl.SetTexting(unit.unitName + "�� ��");

            yield return new WaitForSeconds(1.0f);

            Unit target = players[Random.Range(0, players.Count)].GetComponent<Unit>();
            bool isDead = target.Damage(unit.attackDmg);

           uIControl.SetTexting(unit.unitName + "��" + target.unitName + "�� ����");

            yield return new WaitForSeconds(1f);
            if(isDead)
            {
                uIControl.SetTexting(target.unitName + "�� ���!");
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
            uIControl.SetTextTurnOrder("���� �� : " + string.Join(" -> ", turnOrder));
        }

        IEnumerator PlayerAttack()
        {
            
            uIControl.SetTexting(currentPlayerUnit.unitName + "��" + selectedTarget.unitName + "�� ����!");
            currentPlayerUnit.AttackAni();
            yield return new WaitForSeconds(0.3f);
            bool isDead = selectedTarget.Damage(currentPlayerUnit.attackDmg);

            yield return new WaitForSeconds(0.7f);

            if(isDead)
            {
                uIControl.SetTexting(selectedTarget.unitName + "�� ���!");
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
                        // Ray�� �浹�� ������Ʈ ���
                        // Debug.Log("Ŭ���ѿ�����Ʈ: " + hit.collider.gameobject.name);
                        SelectTartget(hit.collider.GetComponent<Unit>());
                    }
                }
            }
        }
    }
}

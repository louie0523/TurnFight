using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Louie
{
    public class UIControl : MonoBehaviour
    {
        [SerializeField]
        private Text Texting;
        public Button BtnAttack;
        public Button BtnSkill;
        public Button BtnEndTurn;
        [SerializeField]
        private Text TextTurnOrder;
        /// <summary>
        /// ���̵� �۾��� �ð�
        /// </summary>
        private float fadeTime = 1.0f;

        Canvas canvas;
        BattleSystem battleSystem;

        GameObject PlayerUI;
        GameObject EnemyUI;
        public List<Slider> sliderPlayer = new List<Slider>();
        public List<Slider> sliderEnemy = new List<Slider>();


        private void Start()
        {
            battleSystem = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        }

        public void CreatCanvas()
        {
            // ĵ���� ����
            GameObject objCanvas = new GameObject("Canvas");
            canvas = objCanvas.AddComponent<Canvas>();
            objCanvas.AddComponent<CanvasScaler>();
            objCanvas.AddComponent<GraphicRaycaster>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            // �ؽ�Ʈ�� ���� ��Ʈ ����
            Font myFont = (Font)Resources.GetBuiltinResource(typeof(Font), "LegacyRuntime.ttf");

            // UI �Է������� �̺�Ʈ �ý���
            GameObject objEventSystem = new GameObject("EventSystem");
            objEventSystem.AddComponent<EventSystem>();
            objEventSystem.AddComponent<StandaloneInputModule>();

            //
            GameObject objTexting = new GameObject("TextIng");
            objTexting.transform.parent = canvas.transform;
            Texting = objTexting.AddComponent<Text>();
            Texting.font = myFont;
            Texting.text = "���� ����";
            Texting.fontSize = 20;
            Texting.alignment = TextAnchor.MiddleCenter;
            RectTransform RtTextIng = objTexting.GetComponent<RectTransform>();
            RtTextIng.anchorMin = new Vector2(0, 1);
            RtTextIng.anchorMax = new Vector2(1, 1);
            RtTextIng.anchoredPosition = new Vector2(0, -(RtTextIng.sizeDelta.y));
            RtTextIng.offsetMin = new Vector2(0, RtTextIng.offsetMin.y);
            RtTextIng.offsetMax = new Vector2(0, RtTextIng.offsetMax.y);
            RtTextIng.pivot = new Vector2(0.5F, 0.5F);

            // �� UI
            GameObject objTextTurnOrder = new GameObject("TexTurnOrder");
            objTextTurnOrder.transform.parent = canvas.transform;
            TextTurnOrder = objTextTurnOrder.AddComponent<Text>();
            TextTurnOrder.font = myFont;
            TextTurnOrder.text = "TextTurnOrder";
            TextTurnOrder.fontSize = 20;
            TextTurnOrder.alignment = TextAnchor.MiddleCenter;
            RectTransform RtTextTurnOrder = objTextTurnOrder.GetComponent<RectTransform>();
            RtTextTurnOrder.anchorMin = new Vector2(0, 1);
            RtTextTurnOrder.anchorMax = new Vector2(1, 1);
            RtTextTurnOrder.anchoredPosition = new Vector2(0, -(RtTextTurnOrder.sizeDelta.y / 2));
            RtTextTurnOrder.offsetMin = new Vector2(0, RtTextTurnOrder.offsetMin.y);
            RtTextTurnOrder.offsetMax = new Vector2(0, RtTextTurnOrder.offsetMax.y);
            RtTextTurnOrder.pivot = new Vector2(0.5F, 0.5F);

            GameObject objBtnAttack = new GameObject("TexTurnOrder");
            objBtnAttack.transform.SetParent(canvas.transform);
            objBtnAttack.AddComponent<Image>();
            BtnAttack = objBtnAttack.AddComponent<Button>();
            RectTransform RtBtnAttack = objBtnAttack.GetComponent<RectTransform>();
            RtBtnAttack.anchoredPosition = new Vector2(0, 140);
            RtBtnAttack.sizeDelta = new Vector2(200, 20);
            RtBtnAttack.pivot = new Vector2(0.5f, 0.5f);
            BtnAttack.onClick.AddListener(OnBtnAttack);
            BtnAttack.interactable = false;

            GameObject objBtnAttackText = new GameObject("BtnAttackText");
            objBtnAttackText.transform.parent = BtnAttack.transform;
            Text BtnAttackText = objBtnAttackText.AddComponent<Text>();
            BtnAttackText.font = myFont;
            BtnAttackText.text = "BtnAttackText";
            BtnAttackText.fontSize = 18;
            BtnAttackText.color = Color.black;
            BtnAttackText.alignment = TextAnchor.MiddleCenter;
            RectTransform RtBtnAttackText = objBtnAttackText.GetComponent<RectTransform>();
            RtBtnAttackText.anchorMin = new Vector2(0, 0);
            RtBtnAttackText.anchorMax = new Vector2(1, 1);
            RtBtnAttackText.anchoredPosition = Vector2.zero; // �θ� ������Ʈ�� ���缭 ���� Ǯ�� ä���
            RtBtnAttackText.sizeDelta = Vector2.zero;

            GameObject objBtnSkill = new GameObject("TexTurnOrder");
            objBtnSkill.transform.SetParent(canvas.transform);
            objBtnSkill.AddComponent<Image>();
            BtnSkill = objBtnSkill.AddComponent<Button>();
            RectTransform RtBtnSkill = objBtnSkill.GetComponent<RectTransform>();
            RtBtnSkill.anchoredPosition = new Vector2(0, 100);
            RtBtnSkill.sizeDelta = new Vector2(200, 20);
            RtBtnSkill.pivot = new Vector2(0.5f, 0.5f);
            BtnSkill.onClick.AddListener(OnBtnSkill);
            BtnSkill.interactable = false;

            GameObject objBtnSkillText = new GameObject("BtnSkillText");
            objBtnSkillText.transform.parent = BtnSkill.transform;
            Text BtnSkillText = objBtnSkillText.AddComponent<Text>();
            BtnSkillText.font = myFont;
            BtnSkillText.text = "BtnSkillText";
            BtnSkillText.fontSize = 18;
            BtnSkillText.color = Color.black;
            BtnSkillText.alignment = TextAnchor.MiddleCenter;
            RectTransform RtBtnSkillText = objBtnSkillText.GetComponent<RectTransform>();
            RtBtnSkillText.anchorMin = new Vector2(0, 0);
            RtBtnSkillText.anchorMax = new Vector2(1, 1);
            RtBtnSkillText.anchoredPosition = Vector2.zero;
            RtBtnSkillText.sizeDelta = Vector2.zero;

            GameObject objBtnEndTurn = new GameObject("TexTurnOrder");
            objBtnEndTurn.transform.SetParent(canvas.transform);
            objBtnEndTurn.AddComponent<Image>();
            BtnEndTurn = objBtnEndTurn.AddComponent<Button>();
            RectTransform RtBtnEndTurn = objBtnEndTurn.GetComponent<RectTransform>();
            RtBtnEndTurn.anchoredPosition = new Vector2(0, 60);
            RtBtnEndTurn.sizeDelta = new Vector2(200, 20);
            RtBtnEndTurn.pivot = new Vector2(0.5f, 0.5f);
            BtnEndTurn.onClick.AddListener(OnBtnEndTurn);
            BtnEndTurn.interactable = false;

            GameObject objBtnEndTurnText = new GameObject("BtnEndTurnText");
            objBtnEndTurnText.transform.parent = BtnEndTurn.transform;
            Text BtnEndTurnText = objBtnEndTurnText.AddComponent<Text>();
            BtnEndTurnText.font = myFont;
            BtnEndTurnText.text = "BtnEndTurnText";
            BtnEndTurnText.fontSize = 18;
            BtnEndTurnText.color = Color.black;
            BtnEndTurnText.alignment = TextAnchor.MiddleCenter;
            RectTransform RtBtnEndTurnText = objBtnEndTurnText.GetComponent<RectTransform>();
            RtBtnEndTurnText.anchorMin = new Vector2(0, 0);
            RtBtnEndTurnText.anchorMax = new Vector2(1, 1);
            RtBtnEndTurnText.anchoredPosition = Vector2.zero;
            RtBtnEndTurnText.sizeDelta = Vector2.zero;

        }

        public void CreatHPUI()
        {
            PlayerUI = CreatStatusUI(0, "LeftPlayer", 150f, -400f);
            PlayerUI.AddComponent<VerticalLayoutGroup>().childControlHeight = false;
            EnemyUI = CreatStatusUI(1, "RightEnemy", 150f, -400f);
            EnemyUI.AddComponent<VerticalLayoutGroup>().childControlHeight = false;

            GameObject HPPrefab = Resources.Load<GameObject>("Prefabs/HP");

            if(battleSystem == null)
            {
                battleSystem = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
            }

            for(int i = 0; i < battleSystem.players.Count; i++)
            {
                GameObject p = Instantiate(HPPrefab);
                p.transform.SetParent(PlayerUI.transform);
                p.name= battleSystem.players[i].unitName;
                p.transform.Find("HPText").GetComponent<Text>().text = battleSystem.players[i].unitName;
                sliderPlayer.Add(p.GetComponent<Slider>());
            }

            for (int i = 0; i < battleSystem.enemys.Count; i++)
            {
                GameObject p = Instantiate(HPPrefab);
                p.transform.SetParent(EnemyUI.transform);
                p.name = battleSystem.enemys[i].unitName;
                p.transform.Find("HPText").GetComponent<Text>().text = battleSystem.enemys[i].unitName;
                sliderEnemy.Add(p.GetComponent<Slider>());
            }
        }
        /// <summary>
        /// UI ����
        /// </summary>
        /// <param name="preset">0: ����, ���Ʒ��� full, 1:������, ���Ʒ��� full</param>
        /// <param name="name">������Ʈ �̸�</param>
        /// <param name="sizeX">UI with ��</param>
        /// <param name="posY">UI TOP ��</param>
        /// <returns></returns>
        public GameObject CreatStatusUI(int preset, string name, float sizeX = 0, float posY = 0)
        {
            GameObject objCreateUI = new GameObject(name);
            objCreateUI.transform.parent = canvas.transform;
            RectTransform RtPlayerStatus = objCreateUI.AddComponent<RectTransform>();
            switch(preset)
            {
                case 0:
                    RtPlayerStatus.anchorMin = new Vector2(0, 0);
                    RtPlayerStatus.anchorMax = new Vector2(0, 1);
                    RtPlayerStatus.sizeDelta = new Vector2(sizeX, posY);
                    RtPlayerStatus.anchoredPosition = new Vector2(sizeX / 2, 0);
                    break;
                case 1:
                    RtPlayerStatus.anchorMin = new Vector2(1, 0);
                    RtPlayerStatus.anchorMax = new Vector2(1, 1);
                    RtPlayerStatus.sizeDelta = new Vector2(sizeX,posY);
                    RtPlayerStatus.anchoredPosition = new Vector2(-sizeX / 2, 0);
                    break;
            }
            return objCreateUI;
        }

        public void PlayerTurn(string unitName)
        {
            Texting.text = unitName + "�� ��. �ൿ�� �����ϼ���.";
            // ����Ƽ ��ư�� Ȱ��ȭ�� ��Ȱ��ȭ�� �����ϴ� ����
            BtnAttack.interactable = true;
            BtnSkill.interactable = true;
            BtnEndTurn.interactable = true;
        }

        public void EnemyTurn(string unitName)
        {
            Texting.text = unitName + "�� ��. �ൿ�� �����ϼ���.";
            BtnAttack.interactable = false;
            BtnSkill.interactable = false;
            BtnEndTurn.interactable = false;
        }

        public void SetTexting(string message)
        {
            Texting.text = message;
        }

        public void SetTextTurnOrder(string message)
        {
            TextTurnOrder.text = message;
        }

        public void SetPlayerHP(Unit unit)
        {
            sliderPlayer.Find(x => x.name == unit.unitName).value= ((float)unit.currentHp / (float)unit.maxHp);
        }

        public void SetEnemyHP(Unit unit)
        {
            sliderEnemy.Find(x => x.name == unit.unitName).value = ((float)unit.currentHp / (float)unit.maxHp);
        }

        public void OnBtnAttack()
        {
            Debug.Log("���� �ߵ�");
            if(battleSystem.status != BattleStatus.Player_Turn)
            {
                return;
            }
            Texting.text = "����� �������ּ���";
            BtnAttack.interactable = false;
            BtnSkill.interactable = false;
            BtnEndTurn.interactable = false;

            battleSystem.isTargeting = true;
        }

        public void OnBtnSkill()
        {
            Debug.Log("��ų �ߵ�");
            if (battleSystem.status != BattleStatus.Player_Turn)
            {
                return;
            }
            Texting.text = "����� �������ּ���";
            BtnAttack.interactable = false;
            BtnSkill.interactable = false;
            BtnEndTurn.interactable = false;

            battleSystem.isTargeting = true;
        }

        public void OnBtnEndTurn()
        {
            BtnAttack.interactable = false;
            BtnSkill.interactable = false;
            BtnEndTurn.interactable = false;
            Debug.Log("������");
            battleSystem.EndTurn();
        }
    }
}

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
        /// 페이드 작업시 시간
        /// </summary>
        private float fadeTime = 1.0f;

        Canvas canvas;
        BattleSystem battleSystem;

        private void Start()
        {
            battleSystem = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
            CreatCanvas();
        }

        public void CreatCanvas()
        {
            // 캔버스 생성
            GameObject objCanvas = new GameObject("Canvas");
            canvas = objCanvas.AddComponent<Canvas>();
            objCanvas.AddComponent<CanvasScaler>();
            objCanvas.AddComponent<GraphicRaycaster>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            // 텍스트에 사용될 폰트 지정
            Font myFont = (Font)Resources.GetBuiltinResource(typeof(Font), "LegacyRuntime.ttf");

            // UI 입력을위한 이벤트 시스템
            GameObject objEventSystem = new GameObject("EventSystem");
            objEventSystem.AddComponent<EventSystem>();
            objEventSystem.AddComponent<StandaloneInputModule>();

            //
            GameObject objTexting = new GameObject("TextIng");
            objTexting.transform.parent = canvas.transform;
            Texting = objTexting.AddComponent<Text>();
            Texting.font = myFont;
            Texting.text = "현재 상태";
            Texting.fontSize = 20;
            Texting.alignment = TextAnchor.MiddleCenter;
            RectTransform RtTextIng = objTexting.GetComponent<RectTransform>();
            RtTextIng.anchorMin = new Vector2(0, 1);
            RtTextIng.anchorMax = new Vector2(1, 1);
            RtTextIng.anchoredPosition = new Vector2(0, -(RtTextIng.sizeDelta.y));
            RtTextIng.offsetMin = new Vector2(0, RtTextIng.offsetMin.y);
            RtTextIng.offsetMax = new Vector2(0, RtTextIng.offsetMax.y);
            RtTextIng.pivot = new Vector2(0.5F, 0.5F);

            // 턴 UI
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
            RtBtnAttackText.anchoredPosition = Vector2.zero; // 부모 오브젝트에 맞춰서 완전 풀로 채우기
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

        public void PlayerTurn(string unitName)
        {
            Texting.text = unitName + "의 턴. 행동을 선택하세요.";
            // 유니티 버튼의 활성화와 비활성화를 관리하는 변수
            BtnAttack.interactable = true;
            BtnSkill.interactable = true;
            BtnEndTurn.interactable = true;
        }

        public void EnemyTurn(string unitName)
        {
            Texting.text = unitName + "의 턴. 행동을 선택하세요.";
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

        public void OnBtnAttack()
        {
            Debug.Log("공격 발동");
            if(battleSystem.status != BattleStatus.Player_Turn)
            {
                return;
            }
            Texting.text = "대상을 선택해주세요";
            BtnAttack.interactable = false;
            BtnSkill.interactable = false;
            BtnEndTurn.interactable = false;

            battleSystem.isTargeting = true;
        }

        public void OnBtnSkill()
        {
            Debug.Log("스킬 발동");
            if (battleSystem.status != BattleStatus.Player_Turn)
            {
                return;
            }
            Texting.text = "대상을 선택해주세요";
            BtnAttack.interactable = false;
            BtnSkill.interactable = false;
            BtnEndTurn.interactable = false;

            battleSystem.isTargeting = true;
        }

        public void OnBtnEndTurn()
        {
            Debug.Log("턴종료");
            battleSystem.EndTurn();
        }
    }
}

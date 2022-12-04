using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace NextEdgeGames {
    public class UIManager : MonoBehaviour {
        public enum MenuEnum { None, MainMenu, Inventory, LevelSelection, Store }
        public enum DialogueEnum { QuitGame, NotEnoughCash }

        public static UIManager instance;

        private List<Menu> menus;
        private List<Dialogue> dialogues;
        private Stack<Menu> menusStack;

        [Header("Inspector Assigned")]
        [SerializeField] private Image image_BG;
        [SerializeField] private Header header;
        [SerializeField] private Transform menusContainer;
        [SerializeField] private Transform dialoguesContainer;

        [Header("Runtime Information")]
        [SerializeField] private Menu activeMenu;

        private void Awake() {
            instance = this;
            menusStack = new Stack<Menu>();
        }
        private void Start() {
            menus = menusContainer.GetComponentsInChildren<Menu>(true).ToList();
            dialogues = dialoguesContainer.GetComponentsInChildren<Dialogue>(true).ToList();
            OpenMenu(MenuEnum.MainMenu);
        }
        public void OpenMenu(MenuEnum type) {
            Menu m = menus.Find(x => x.Type == type);
            m.SetActive(true);
            image_BG.gameObject.SetActive(m.UseBGImage);
            if (activeMenu != null) {
                menusStack.Push(activeMenu);
                activeMenu.SetActive(false);
            }
            activeMenu = m;
        }
        public void SetActiveDialogue(DialogueEnum type, bool setActive) {
            Dialogue d = dialogues.Find(x => x.Type == type);
            d.SetActive(setActive);
        }
        public void Back() {
            if (menusStack.Count > 0) {
                activeMenu.SetActive(false);
                Menu m = menusStack.Pop();
                m.SetActive(true);
                image_BG.gameObject.SetActive(m.UseBGImage);
                activeMenu = m;
            } else {
                SetActiveDialogue(DialogueEnum.QuitGame, true);
            }
        }
    }
}
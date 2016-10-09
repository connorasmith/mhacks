namespace VRTK.Examples {

    using UnityEngine;
    using System.Collections;

    public class HideRadialMenu : MonoBehaviour {
        public GameObject radialMenu;

        // Use this for initialization
        void Start() {

            GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(showMenu);
            GetComponent<VRTK_ControllerEvents>().TriggerReleased += new ControllerInteractionEventHandler(hideMenu);


        }

        // Update is called once per frame
        void Update() {

        }

        private void showMenu(object sender, ControllerInteractionEventArgs e) {
            radialMenu.SetActive(true);
        }

        private void hideMenu(object sender, ControllerInteractionEventArgs e) {
            radialMenu.SetActive(false);

        }
    }
}
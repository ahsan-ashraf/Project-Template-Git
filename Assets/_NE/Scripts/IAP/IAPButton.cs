using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NextEdgeGames {
    public class IAPButton : MonoBehaviour {

        [System.Serializable] private class Event {
            public UnityEvent onSuccess;
            public UnityEvent onFailure;
        }

        private Button button;

        [SerializeField] private IAPProductID productID;
        [SerializeField] private Event events;

        private void Awake() {
            button = GetComponent<Button>();
        }
        private void Start() {
            button.onClick.AddListener(OnClickButton);
        }

        private void OnClickButton() {
            IAPManager.instance.PurchaseProduct(productID, OnPurchaseSuccess, OnPurchaseFailure);
        }

        private void OnPurchaseSuccess() {
            events.onSuccess.Invoke();
        }
        private void OnPurchaseFailure() {
            events.onFailure.Invoke();
        }
    }
}
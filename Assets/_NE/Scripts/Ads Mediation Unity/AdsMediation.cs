using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Mediation;

namespace NextEdgeGames {
    public class AdsMediation : MonoBehaviour {

        private IInterstitialAd ad_Interstitial;
        private string interstitialAdUnitID = "Interstitial_Android";


        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }
        private void Start() {
            InitServices();
        }
        public async void InitServices() {
            try {
                await UnityServices.InitializeAsync();
                InitializationComplete();
            } catch (Exception e) {
                InitializationFailed(e);
            }
        }
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                ShowInterstitialAd();
            }
        }
        private void InitializationComplete() {
            SetupAd();
            ad_Interstitial.LoadAsync();
        }
        private void InitializationFailed(Exception e) {
            Debug.LogError("Failed to Initialization Unity Ads Mediation with error " + e.Message);
        }
        private void SetupAd() {
            // Create
            ad_Interstitial = MediationService.Instance.CreateInterstitialAd(interstitialAdUnitID);

            // Subscribe to Events
            ad_Interstitial.OnLoaded += this.Ad_Interstitial_OnLoaded;
            ad_Interstitial.OnShowed += this.Ad_Interstitial_OnShowed;
            ad_Interstitial.OnClosed += this.Ad_Interstitial_OnClosed;
            ad_Interstitial.OnFailedLoad += this.Ad_Interstitial_OnFailedLoad;
            ad_Interstitial.OnFailedShow += this.Ad_Interstitial_OnFailedShow;

            // Impression Event
            MediationService.Instance.ImpressionEventPublisher.OnImpression += this.ImpressionEventPublisher_OnImpression;
        }

        public void ShowInterstitialAd() {
            if (ad_Interstitial.AdState == AdState.Loaded) {
                ad_Interstitial.ShowAsync();
            }
        }
        private void Ad_Interstitial_OnLoaded(object sender, EventArgs e) {
            Debug.Log("Interstitial Ad Loaded");
        }
        private void Ad_Interstitial_OnShowed(object sender, EventArgs e) {
            Debug.Log("Interstitial Ad Showed");
        }
        private void Ad_Interstitial_OnClosed(object sender, EventArgs e) {
            ad_Interstitial.LoadAsync();
            Debug.Log("Interstitial Ad Closed");
        }
        private void Ad_Interstitial_OnFailedLoad(object sender, LoadErrorEventArgs e) {
            Debug.Log("Interstitial Ad Failed to Load");
        }
        private void Ad_Interstitial_OnFailedShow(object sender, ShowErrorEventArgs e) {
            Debug.Log("Interstitial Ad Failed to Show");
        }
        private void ImpressionEventPublisher_OnImpression(object sender, ImpressionEventArgs e) {}
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour{
    [Serializable]
    public struct KitchenObjectSO_GameObject {
        public KitchenObjectsSO kitchenObjectsSO;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObjects plateKitchenObjects;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectsSOGameObjectList;

    private void Start() {
        plateKitchenObjects.OnIngredientAdded += PlateKitchenObjects_OnIngredientAdded;
        foreach (KitchenObjectSO_GameObject kitchenObjectSO_GameObject in kitchenObjectsSOGameObjectList) {
            kitchenObjectSO_GameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObjects_OnIngredientAdded(object sender, PlateKitchenObjects.OnIngredientAddedEventArgs e) {
        foreach (KitchenObjectSO_GameObject kitchenObjectSO_GameObject in kitchenObjectsSOGameObjectList) {
            if (kitchenObjectSO_GameObject.kitchenObjectsSO == e.kitchenObjectsSO) {
                kitchenObjectSO_GameObject.gameObject.SetActive(true);
            }
        }
    }
}

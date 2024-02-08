using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconUI : MonoBehaviour{
    [SerializeField] private PlateKitchenObjects plateKitchenObjects;
    [SerializeField] private Transform iconTemplate;
    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        plateKitchenObjects.OnIngredientAdded += PlateKitchenObjects_OnIngredientAdded;
    }

    private void PlateKitchenObjects_OnIngredientAdded(object sender, PlateKitchenObjects.OnIngredientAddedEventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        foreach (Transform child in transform) {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (KitchenObjectsSO kitchenObjectsSO in plateKitchenObjects.GetKitchenObjectsSOList()) {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(kitchenObjectsSO);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectsParent {
    public static event EventHandler OnAnyObjectPlacedHere;
    public static void ResetStaticData() {
        OnAnyObjectPlacedHere = null;
    }

    [SerializeField] private Transform counterTopPoint;

    private KitchenObjects kitchenObjects;

    public virtual void Interact(Player player) {

    }
    public virtual void InteractAlternate(Player player) {

    }
    public Transform GetKitchenObjectFollowTransform() {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObjects kitchenObjects) {
        this.kitchenObjects = kitchenObjects;
        if (kitchenObjects != null) {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObjects GetKitchenObjects() {
        return kitchenObjects;
    }

    public void ClearKitchenObject() {
        kitchenObjects = null;
    }

    public bool HasKitchenObject() {
        return kitchenObjects != null;
    }
}

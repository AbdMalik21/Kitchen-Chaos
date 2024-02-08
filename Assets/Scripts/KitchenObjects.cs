using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObjects : MonoBehaviour{
    [SerializeField] private KitchenObjectsSO kitchenObjectsSO;

    private IKitchenObjectsParent iKitchenObjectParent;

    public KitchenObjectsSO GetKitchenObjectsSO() {
        return kitchenObjectsSO; 
    }

    public void SetKitchenObjectParent(IKitchenObjectsParent iKitchenObjectParent) {
        if (this.iKitchenObjectParent != null) {
            this.iKitchenObjectParent.ClearKitchenObject();
        }

        this.iKitchenObjectParent = iKitchenObjectParent;
        if (iKitchenObjectParent.HasKitchenObject()) {
            Debug.LogError("Already been there");
        }
        iKitchenObjectParent.SetKitchenObject(this);
        transform.parent = iKitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectsParent GetKitchenObjectParent() {
        return iKitchenObjectParent;
    }
    public void DestroySelf() {
        iKitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObjects plateKitchenObjects) {
        if (this is PlateKitchenObjects) {
            plateKitchenObjects = this as PlateKitchenObjects;
            return true;
        } else {
            plateKitchenObjects = null;
            return false;
        }
    }

    public static KitchenObjects SpawnKitchenObjects(KitchenObjectsSO kitchenObjectsSO, IKitchenObjectsParent kitchenObjectsParent) {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectsSO.prefab);
        KitchenObjects kitchenObjects = kitchenObjectTransform.GetComponent<KitchenObjects>();
        kitchenObjects.SetKitchenObjectParent(kitchenObjectsParent);
        return kitchenObjects;
    }
}

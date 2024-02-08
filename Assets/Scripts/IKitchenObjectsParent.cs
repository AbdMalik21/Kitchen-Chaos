using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectsParent{
    public Transform GetKitchenObjectFollowTransform();

    public void SetKitchenObject(KitchenObjects kitchenObjects);

    public KitchenObjects GetKitchenObjects();

    public void ClearKitchenObject();

    public bool HasKitchenObject();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter {

    [SerializeField] private KitchenObjectsSO kitchenObjectsSO;

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                player.GetKitchenObjects().SetKitchenObjectParent(this);
            }
        } else {
            if (player.HasKitchenObject()) {
                if (player.GetKitchenObjects().TryGetPlate(out PlateKitchenObjects plateKitchenObjects)) {
                    if (plateKitchenObjects.TryAddIngredient(GetKitchenObjects().GetKitchenObjectsSO())) {
                        GetKitchenObjects().DestroySelf();
                    }
                } else {
                    if (GetKitchenObjects().TryGetPlate(out plateKitchenObjects)) {
                        if (plateKitchenObjects.TryAddIngredient(player.GetKitchenObjects().GetKitchenObjectsSO())) {
                            player.GetKitchenObjects().DestroySelf();
                        }
                    }
                }
            } else {
                GetKitchenObjects().SetKitchenObjectParent(player);
            }
            
        }
    } 
}

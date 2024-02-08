using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounter : BaseCounter{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    [SerializeField] private KitchenObjectsSO plateKitchenObjectSO;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int spawnPlateCounter;
    private int spawnPlateCounterMax = 4;

    private void Update() {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax) {
            //KitchenObjects.SpawnKitchenObjects(plateKitchenObjectSO, this); 
            spawnPlateTimer = 0f;
            if (GameManager.Instance.IsGamePlaying() && spawnPlateCounter < spawnPlateCounterMax) {
                spawnPlateCounter++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (!player.HasKitchenObject()) {
                if (spawnPlateCounter > 0) {
                    spawnPlateCounter--;
                    KitchenObjects.SpawnKitchenObjects(plateKitchenObjectSO, player);

                    OnPlateRemoved?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}

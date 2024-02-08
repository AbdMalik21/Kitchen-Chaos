using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private int successfulRecipeAmount;

    private void Awake() {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update() {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f) {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if(GameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipesMax ) {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObjects plateKitchenObjects) {
        for (int i = 0; i < waitingRecipeSOList.Count; i++) {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectsSOList.Count == plateKitchenObjects.GetKitchenObjectsSOList().Count) {
                bool plateContentMatchesRecipe = true;

                foreach (KitchenObjectsSO recipeKitchenObjectsSO in waitingRecipeSO.kitchenObjectsSOList) {
                    bool ingeredientFound = false;

                    foreach (KitchenObjectsSO plateKitchenObjectsSO in plateKitchenObjects.GetKitchenObjectsSOList()) {
                        
                        if (plateKitchenObjectsSO == recipeKitchenObjectsSO) {
                            ingeredientFound = true;
                            break;
                        }
                    }
                    if (!ingeredientFound) {
                        plateContentMatchesRecipe = false;
                    }
                }
                if (plateContentMatchesRecipe) {
                    successfulRecipeAmount++;
                    waitingRecipeSOList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList() {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipeAmount() {
        return successfulRecipeAmount;
    }
}

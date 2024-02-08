using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter,IHasProgress {
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler <OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs { 
        public State state;
    }

    public enum State {
        Idle,
        Frying,
        Fried,
        Burning,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start() {
        state = State.Idle;
    }

    private void Update() {
        if (HasKitchenObject()) {
            switch (state) {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                    //FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObjects().GetKitchenObjectsSO());
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
                        GetKitchenObjects().DestroySelf();
                        KitchenObjects.SpawnKitchenObjects(fryingRecipeSO.output, this);
                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObjects().GetKitchenObjectsSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state,
                        });
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });
                    //FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObjects().GetKitchenObjectsSO());
                    if (burningTimer > burningRecipeSO.burningTimerMax) {
                        GetKitchenObjects().DestroySelf();
                        KitchenObjects.SpawnKitchenObjects(burningRecipeSO.output, this);
                        state = State.Burning;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state,
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Burning:
                    break;
            }
        }
    }
    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                if (HasRecipeWithInput(player.GetKitchenObjects().GetKitchenObjectsSO())) {
                    player.GetKitchenObjects().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObjects().GetKitchenObjectsSO());
                    state = State.Frying;
                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                        state = state,
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                }
            }
        } else {
            if (player.HasKitchenObject()) {
                if (player.GetKitchenObjects().TryGetPlate(out PlateKitchenObjects plateKitchenObjects)) {
                    if (plateKitchenObjects.TryAddIngredient(GetKitchenObjects().GetKitchenObjectsSO())) {
                        GetKitchenObjects().DestroySelf();
                        state = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state,
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0f
                        });
                    }
                }
            } else {
                GetKitchenObjects().SetKitchenObjectParent(player);
                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                    state = state,
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = 0f
                });
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectsSO inputKitchenObjectsSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectsSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectsSO GetOutputForInput(KitchenObjectsSO inputKitchenObjectsSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectsSO);
        if (fryingRecipeSO != null) {
            return fryingRecipeSO.output;
        } else {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectsSO inputKitchenObjectsSO) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
            if (fryingRecipeSO.input == inputKitchenObjectsSO) {
                return fryingRecipeSO;
            }
        }
        return null;
    }
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectsSO inputKitchenObjectsSO) {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray) {
            if (burningRecipeSO.input == inputKitchenObjectsSO) {
                return burningRecipeSO;
            }
        }
        return null;
    }
    public bool IsFried() {
        return state == State.Fried;
    }
}

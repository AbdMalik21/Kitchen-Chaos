using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectsParent {

    public static Player Instance { get; private set; }
    public event EventHandler OnPickedSomething;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObjects kitchenObjects;

    public event EventHandler<OnSelectedPlayerChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedPlayerChangedEventArgs : EventArgs {
        public BaseCounter selectedCounter;
    }

    private void Awake() {
        if (Instance != null){
            Debug.LogError("More than 1 Player");
        }
        Instance = this;
    }

    private void Start() {
        gameInput.OnInteractionAction += GameInput_OnInteractionAction;
        gameInput.OnInteractionAlternateAction += GameInput_OnInteractionAlternateAction;
    }

    private void GameInput_OnInteractionAlternateAction(object sender, EventArgs e) {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null) {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractionAction(object sender, System.EventArgs e) {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    }

    private void Update(){
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if(Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, counterLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                //clearCounter.Interact();
                if (baseCounter != selectedCounter) {
                    SetSelectedCounter(baseCounter);
                }
            } else {
                SetSelectedCounter(null);
            }
        } else {
            SetSelectedCounter(null);
        }

        //Debug.Log(selectedCounter);
    }
    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float playerRadius = .7f;
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove) {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove) {
                moveDir = moveDirX;
            } else {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove) {
                    moveDir = moveDirZ;
                } else {

                }
            }
        }

        if (canMove) {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter) {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedPlayerChangedEventArgs {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObjects kitchenObjects) {
        this.kitchenObjects = kitchenObjects;
        if (kitchenObjects != null) {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
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

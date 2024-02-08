using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI keyMoveUpText;
    [SerializeField] private TextMeshProUGUI keyMoveDownText;
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyMoveRightText;
    [SerializeField] private TextMeshProUGUI keyInteractText;
    [SerializeField] private TextMeshProUGUI keyInteractAltText;
    [SerializeField] private TextMeshProUGUI keyPauseText;

    private void Start () {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        UpdateVisual();
        Show();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsCountdownToStartActive()) {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        keyMoveUpText.text = GameInput.Instance.GetBindingString(GameInput.Binding.Move_Up);
        keyMoveDownText.text = GameInput.Instance.GetBindingString(GameInput.Binding.Move_Down);
        keyMoveLeftText.text = GameInput.Instance.GetBindingString(GameInput.Binding.Move_Left);
        keyMoveRightText.text = GameInput.Instance.GetBindingString(GameInput.Binding.Move_Right);
        keyInteractText.text = GameInput.Instance.GetBindingString(GameInput.Binding.Interact);
        keyInteractAltText.text = GameInput.Instance.GetBindingString(GameInput.Binding.InteractAlt);
        keyPauseText.text = GameInput.Instance.GetBindingString(GameInput.Binding.Pause);
    }

    private void Show() {
        gameObject.SetActive(true);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.LowLevel;

public class AutoSwitchInputsIcons : MonoBehaviour
{
    public Sprite keyboardSprite;
    public Sprite gamepadSprite;

    public Image targetImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {
        InputSystem.onEvent += OnInputEvent;
    }

    private void OnDisable()
    {
        InputSystem.onEvent -= OnInputEvent;
    }

    private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
        if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
            return;

        if (device is Gamepad)
        {
            targetImage.sprite = gamepadSprite;
        }
        else if (device is Keyboard || device is Mouse)
        {
            targetImage.sprite = keyboardSprite;
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

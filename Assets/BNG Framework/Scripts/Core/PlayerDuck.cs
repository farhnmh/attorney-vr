using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDuck : MonoBehaviour
{
    public InputActionReference PlayerDuckAction;
    public float defaultHeight;
    public float duckHeight;
    public bool AllowDuckControl;
    public bool isDucking;

    BNGPlayerController player;
    bool isClicked;

    private void Start()
    {
        player = FindObjectOfType<BNGPlayerController>();
        defaultHeight = player.CharacterControllerYOffset;
    }

    private void Update()
    {
        if (AllowDuckControl &&
            PlayerDuckAction != null)
        {
            if (PlayerDuckAction.action.ReadValue<float>() > 0 &&
                !isClicked)
            {
                isClicked = true;
                player.CharacterControllerYOffset = isDucking ? defaultHeight : duckHeight;
                isDucking = !isDucking;
            }
            else if (PlayerDuckAction.action.ReadValue<float>() <= 0)
            {
                isClicked = false;
            }
        }
    }
}

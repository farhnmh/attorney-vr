using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class PlayerSpecificTeleport : MonoBehaviour
{
    [Tooltip("If FadeScreenOnTeleport = true, fade the screen at this speed.")]
    public float TeleportFadeSpeed = 10f;

    public delegate void OnBeforeTeleportFadeAction();
    public static event OnBeforeTeleportFadeAction OnBeforeTeleportFade;

    public delegate void OnBeforeTeleportAction();
    public static event OnBeforeTeleportAction OnBeforeTeleport;

    public delegate void OnAfterTeleportAction();
    public static event OnAfterTeleportAction OnAfterTeleport;

    CharacterController controller;
    BNGPlayerController playerController;
    Rigidbody playerRigid;
    InputBridge input;
    Transform cameraRig;
    ScreenFader fader;

    private void Awake()
    {
        input = InputBridge.Instance;
        playerController = GetComponent<BNGPlayerController>();
        playerRigid = GetComponent<Rigidbody>();
        controller = GetComponentInChildren<CharacterController>();
        cameraRig = playerController.CameraRig;
        fader = cameraRig.GetComponentInChildren<ScreenFader>();
    }

    public void DoTeleport(Transform targetTeleport)
    {
        // Call any events, fade screen, etc.
        BeforeTeleportFade();
        StartCoroutine(doTeleport(targetTeleport));
    }

    public virtual void BeforeTeleportFade()
    {
        // Call any Before Teleport Events
        OnBeforeTeleportFade?.Invoke();

        if (fader)
        {
            fader.FadeInSpeed = TeleportFadeSpeed;
            fader.DoFadeIn();
        }
    }

    public virtual void BeforeTeleport()
    {
        if (fader)
        {
            fader.FadeInSpeed = TeleportFadeSpeed;
            fader.DoFadeIn();
        }

        // Call any Before Teleport Events
        OnBeforeTeleport?.Invoke();
    }

    public virtual void AfterTeleport()
    {
        if (fader)
        {
            fader.DoFadeOut();
        }

        // Call any After Teleport Events
        OnAfterTeleport?.Invoke();
    }

    IEnumerator doTeleport(Transform playerDestination)
    {
        // Call pre-Teleport event
        BeforeTeleport();

        // How to Teleport a CharacterController object
        if (controller)
        {
            // Disable before teleport
            controller.enabled = false;

            // Calculate teleport offset as character may have been resized
            float yOffset = 1 + cameraRig.localPosition.y - playerController.CharacterControllerYOffset;

            // Apply Teleport before offset is applied
            controller.transform.position = playerDestination.position;
            
            // Apply offset
            controller.transform.localPosition -= new Vector3(0, yOffset, 0);
            controller.transform.eulerAngles = new Vector3(0, playerDestination.eulerAngles.y, 0);
        }
        else
        {
            // Otherwise just move the transform directly
            transform.position = playerDestination.position;
            transform.eulerAngles = new Vector3(0, playerDestination.eulerAngles.y, 0);
        }

        // Reset the player's velocity
        if (playerRigid)
        {
            playerRigid.velocity = Vector3.zero;
        }

        // Update last teleport time
        if (playerController)
        {
            playerController.LastTeleportTime = Time.time;
        }

        // Call events, etc.
        AfterTeleport();

        yield return new WaitForEndOfFrame();

        if (controller)
        {
            // Re-Enable the character controller so we can move again
            controller.enabled = true;
        }
    }
}

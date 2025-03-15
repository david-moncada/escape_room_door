using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CardReader : XRSocketInteractor
{
    [Header("CardReader ReaderOptions Data")]
    public float allowedUprightErrorRange = 0.2f;

    [Header("Success References")]
    public GameObject visualLockToHide;
    public DoorHandle handleToEnable; // Reference the DoorHandle directly

    private Vector3 m_HoverEntry;
    private bool m_SwipIsValid;
    private Transform m_KeycardTransform;

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return false;
    }

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return interactable is Keycard;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        m_KeycardTransform = args.interactableObject.transform;
        m_HoverEntry = m_KeycardTransform.position;
        m_SwipIsValid = true;
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);

        Vector3 entryToExit = m_KeycardTransform.position - m_HoverEntry;
        Debug.Log("Swipe exit delta: " + entryToExit + " (y delta: " + entryToExit.y + ")");

        if (m_SwipIsValid && entryToExit.y < -0.15f)
        {
            Debug.Log("Swipe valid! Unlocking door automatically.");
            visualLockToHide.SetActive(false);
            // Instead of just enabling the door handle, call OpenDoorAutomatically:
            DoorHandle doorHandle = handleToEnable;  // assuming handleToEnable is typed as DoorHandle
            doorHandle.OpenDoorAutomatically();
        }
        else
        {
            Debug.Log("Swipe invalid. m_SwipIsValid: " + m_SwipIsValid + ", y delta: " + entryToExit.y);
        }

        m_KeycardTransform = null;
    }

    private void Update()
    {
        if (m_KeycardTransform != null)
        {
            Vector3 keycardUp = m_KeycardTransform.forward;
            float dot = Vector3.Dot(keycardUp, Vector3.up);

            if (dot < 1 - allowedUprightErrorRange)
            {
                m_SwipIsValid = false;
            }
        }
    }
}
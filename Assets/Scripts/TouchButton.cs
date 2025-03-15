using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Serialization;
public class TouchButton : UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable
{
    [Header("Visuals")]
    public Material normalMaterial;
    public Material touchedMaterial;

    [Header("Button Data")]
    public int buttonNumber;
    public NumberPad linkedNumberpad;

    private int m_NumberOfInteractor = 0;
    private Renderer m_RendererToChange;

    private void Start()
    {
        m_RendererToChange = GetComponent<MeshRenderer>();
    }

    public override bool IsHoverableBy(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRHoverInteractor interactor)
    {
        return base.IsHoverableBy(interactor) && (interactor is UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor);
    }

    private IEnumerator ResetButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        m_RendererToChange.material = normalMaterial;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);

        if (m_NumberOfInteractor == 0)
        {
            m_RendererToChange.material = touchedMaterial;
            linkedNumberpad.ButtonPressed(buttonNumber);

            // Reset the button after a small delay
            StartCoroutine(ResetButtonAfterDelay(0.5f));
        }

        m_NumberOfInteractor += 1;
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);

        // Decrease the number of interactors
        m_NumberOfInteractor -= 1;

        // Ensure it doesn't go below 0
        m_NumberOfInteractor = Mathf.Max(m_NumberOfInteractor, 0);

        // Reset to normal material if no interactors are hovering
        if (m_NumberOfInteractor == 0)
        {
            m_RendererToChange.material = normalMaterial;
        }
    }
}
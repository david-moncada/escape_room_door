using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DoorHandle : XRBaseInteractable
{
    [Header("Door Handle Data")]
    public Transform draggedTransform;
    public Vector3 localDragDirection;
    public float dragDistance;
    public int doorWeight = 20;

    [Header("Visual References")]
    public LineRenderer handleToHandLine;
    public LineRenderer dragVectorLine;

    private Vector3 m_StartPosition;
    private Vector3 m_EndPosition;
    private Vector3 m_WorldDragDirection;

    private void Start()
    {
        m_WorldDragDirection = transform.TransformDirection(localDragDirection).normalized;
        m_StartPosition = draggedTransform.position;
        m_EndPosition = m_StartPosition + m_WorldDragDirection * dragDistance;

        handleToHandLine.gameObject.SetActive(false);
        dragVectorLine.gameObject.SetActive(false);

        // Ensure handle is initially disabled (if set from inspector)
        //this.enabled = false; // Initially disabled
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Fixed && isSelected)
        {
            var interactorTransform = firstInteractorSelecting.GetAttachTransform(this);
            Vector3 selfToInteractor = interactorTransform.position - transform.position;
            float forceInDirectionOfDrag = Vector3.Dot(selfToInteractor, m_WorldDragDirection);

            bool dragToEnd = forceInDirectionOfDrag > 0.0f;
            float absoluteForce = Mathf.Abs(forceInDirectionOfDrag);
            float speed = absoluteForce / Time.deltaTime / doorWeight;

            draggedTransform.position = Vector3.MoveTowards(draggedTransform.position,
                dragToEnd ? m_EndPosition : m_StartPosition,
                speed * Time.deltaTime);

            handleToHandLine.SetPosition(0, transform.position);
            handleToHandLine.SetPosition(1, interactorTransform.position);

            dragVectorLine.SetPosition(0, transform.position);
            dragVectorLine.SetPosition(1, transform.position + forceInDirectionOfDrag * m_WorldDragDirection);

            Debug.Log("Door being dragged: " + draggedTransform.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 worldDirection = transform.TransformDirection(localDragDirection).normalized;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + worldDirection * dragDistance);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        handleToHandLine.gameObject.SetActive(true);
        dragVectorLine.gameObject.SetActive(true);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        handleToHandLine.gameObject.SetActive(false);
        dragVectorLine.gameObject.SetActive(false);
    }
    public void OpenDoorAutomatically()
    {
        StartCoroutine(OpenDoorCoroutine());
    }

    private IEnumerator OpenDoorCoroutine()
    {
        float t = 0f;
        float duration = 2f; // door opens over 2 seconds
        Vector3 startPos = draggedTransform.position;

        while (t < duration)
        {
            t += Time.deltaTime;
            // Lerp from start to end position
            draggedTransform.position = Vector3.Lerp(startPos, m_EndPosition, t / duration);
            yield return null;
        }
        draggedTransform.position = m_EndPosition;
    }
}
using System;
using UnityEngine;
using Interfaces;
using System.Collections.Generic;

public class InteractionDetector : MonoBehaviour
{
    [SerializeField] private GameObject interactionKeyVisual;
        
    private readonly List<IInteractable> interactablesInRange = new List<IInteractable>();
    private IInteractable currentInteractable;

    private void Awake()
    {
        if (interactionKeyVisual != null)
        {
            interactionKeyVisual.SetActive(false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
            
        if (interactable != null && !interactablesInRange.Contains(interactable))
        {
            interactablesInRange.Add(interactable);
            UpdateCurrentInteractable();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
            
        if (interactable != null && interactablesInRange.Contains(interactable))
        {
            interactablesInRange.Remove(interactable);
            UpdateCurrentInteractable();
        }
    }
    
    private void UpdateCurrentInteractable()
    {
        currentInteractable = null;
            
        foreach (IInteractable interactable in interactablesInRange)
        {
            if (interactable.CanInteract())
            {
                currentInteractable = interactable;
                break;
            }
        }
            
        if (interactionKeyVisual != null)
        {
            interactionKeyVisual.SetActive(currentInteractable != null);
        }
    }

    public void TryInteract()
    {
        if (currentInteractable != null && currentInteractable.CanInteract())
        {
            currentInteractable.Interact();
            UpdateCurrentInteractable();
        }
    }
}

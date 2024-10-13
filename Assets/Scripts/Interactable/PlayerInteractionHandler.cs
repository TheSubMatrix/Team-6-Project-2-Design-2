using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteractionHandler : MonoBehaviour
{
    [Serializable]
    class InteractorInfo
    {
        public InteractorInfo(IInteractable interactable)
        {
            this.interactable = interactable;
        }
        public IInteractable interactable{get; private set;}
        public float rank = -1;
    }
    
    [SerializeField] List<InteractorInfo> possibleInteractors = new List<InteractorInfo>();
    IInteractable currentInteractor = null;
    void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        List<IInteractable>foundInteractor = possibleInteractors.Where(x => x.interactable == other.gameObject.GetComponent<IInteractable>()).Select(x => x.interactable).ToList();
        if(interactable != null && foundInteractor.Count <= 0)
        {
            possibleInteractors.Add(new InteractorInfo(interactable));
        }
    }
    void OnTriggerExit(Collider other)
    {
        List<IInteractable>foundInteractor = possibleInteractors.Where(x => x.interactable == other.gameObject.GetComponent<IInteractable>()).Select(x => x.interactable).ToList();
        possibleInteractors.RemoveAll(x => foundInteractor.Contains(x.interactable));
    }
    void Update()
    {
        for(int i = 0; i < possibleInteractors.Count; i++)
        {
            possibleInteractors[i].rank = Vector3.Dot(transform.forward, (possibleInteractors[i].interactable.transform.position - transform.position).normalized);
        }
        possibleInteractors.Sort((x,y) => x.rank.CompareTo(y.rank));
        if(currentInteractor != null)
        {
            currentInteractor.OnInteractionUpdate();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(currentInteractor == null)
            {
                if(possibleInteractors.Count > 0)
                {
                    currentInteractor = possibleInteractors[0].interactable;
                    currentInteractor.OnInteractionStart();
                }
            }else
            {
                if(currentInteractor.canCancelInteraction)
                {
                    currentInteractor.OnInteractionEnd();
                    currentInteractor = null;
                }
            }

        }
    }
}

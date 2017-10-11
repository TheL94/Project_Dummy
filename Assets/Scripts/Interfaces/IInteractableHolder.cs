using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Generic
{
    public interface IInteractableHolder
    {
        /// <summary>
        /// Actual IInteractable (where IsInteractable == true) contained
        /// </summary>
        List<IInteractable> InteractableAvailable { get; set; }
        /// <summary>
        /// IInteractable actually contained
        /// </summary>
        List<IInteractable> InteractableList { get; set; }
        /// <summary>
        /// Add a IDroppable
        /// </summary>
        /// <param name="_interactableToAdd"></param>
        IInteractable AddInteractable(IInteractable _interactableToAdd);
        /// <summary>
        /// Remove a held IInteractable
        /// </summary>
        /// <param name="_InteractableToRemove"></param>
        void RemoveInteractable(IInteractable _InteractableToRemove);
    }
}

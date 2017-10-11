using System.Collections;
using System.Collections.Generic;
using DumbProject.Generic;
using UnityEngine;

namespace DumbProject.GDR_System {
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
        /// <param name="_droppableToAdd"></param>
        IInteractable AddInteractable(IDroppable _droppableToAdd);
        /// <summary>
        /// Remove a held IInteractable
        /// </summary>
        /// <param name="_InteractableToRemove"></param>
        void RemoveInteractable(IInteractable _InteractableToRemove);
    }
}

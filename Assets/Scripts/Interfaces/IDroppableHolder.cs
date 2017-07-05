using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items {
    public interface IDroppableHolder
    {
        /// <summary>
        /// IDroppable actually contained
        /// </summary>
        List<IDroppable> DroppableList { get; set; }
        /// <summary>
        /// Add a IDroppable
        /// </summary>
        /// <param name="_droppableToAdd"></param>
        IDroppable AddDroppable(DroppableBaseData _droppableToAdd);
        /// <summary>
        /// Remove a held IDroppable
        /// </summary>
        /// <param name="_droppableToRemove"></param>
        void RemoveDroppable(IDroppable _droppableToRemove);
    }
}

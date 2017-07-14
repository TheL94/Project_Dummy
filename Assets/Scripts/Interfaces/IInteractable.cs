using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Generic
{
    public interface IInteractable
    {
        bool IsInteractable { get; set; }
        Transform Transf { get; }
        void Interact(AIActor _actor);
    }
}
using System.Collections;
using System.Collections.Generic;
using Framework.AI;
using UnityEngine;

namespace DumbProject.Generic
{
    public interface IInteractable
    {
        bool IsInteractable { get; set; }
        Transform Transf { get; }
        bool Interact(AI_Controller _controller);
    }
}
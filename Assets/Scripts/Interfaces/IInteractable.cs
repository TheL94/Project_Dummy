﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject
{
    public interface IInteractable
    {
        bool IsInteractable { get; set; }
        Transform Transf { get; }
        void Interact();
    }
}
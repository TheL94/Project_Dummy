﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.Items.ALB
{
    //classe da cui fai derivare le classi dei vari item. Come "Sward" e "Gattini"
    public class ItemGeneric : MonoBehaviour
    {
        public void Setup() { }                    
        public void Setup(ItemData _data) { }
        public void Init() { }

        //esempi di funzioni di cui potresti aver bisogno
        public void Interact() { }
        public void Use() { }
    }
}
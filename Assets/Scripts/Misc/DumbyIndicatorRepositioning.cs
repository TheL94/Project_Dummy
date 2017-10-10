using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DumbProject.UI
{
    public class DumbyIndicatorRepositioning : MonoBehaviour
    {
        DumbyIndicatorManager dumbyIndicatorMovement;

        public void Init(DumbyIndicatorManager _indicator)
        {
            dumbyIndicatorMovement = _indicator;
        }


        public void Repositioning()
        {
            /// Da le coordinate di dumby tramite dumby indicator ( a meno che il manager non conosce questa informazione)
            /// alla camera per riposizionarla sulla sua posizione
        }
    }
}
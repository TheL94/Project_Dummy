using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTurnFunction : MonoBehaviour {

    public delegate void NextTurnEvent();
    public static event NextTurnEvent OnNextTurn;

    public void CallNextTurn()
    {
        if (OnNextTurn != null)
            OnNextTurn();
    }

}

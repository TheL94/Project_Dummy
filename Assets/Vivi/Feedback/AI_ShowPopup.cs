using Framework.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AI/NewAction/ShowPopup")]
public class AI_ShowPopup : AI_Action
{
	
	protected override bool Act(AI_Controller _controller)
	{
		InfoComponent popUp = _controller.GetComponentInChildren<InfoComponent>();
		if (popUp)
		{	
			popUp.ShowStatePopUp(_controller.CurrentState);
			return true;
		}
		
		return false;
	}

	
}

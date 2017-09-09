
using System.Collections.Generic;
using UnityEngine;
using Framework.AI;

public class InfoComponent: MonoBehaviour {

    public List<SpriteRenderer> IconsPrefabs;
    public SpriteRenderer IconToGet;
 
    private void Awake()
    {    
        // per ora è nell'awake, forse c'è un evento di setup generale?
        Setup();
    }

    void Setup() {
       IconToGet = GetComponent<SpriteRenderer>();
       IconsPrefabs = GetIconsPrefabs();
    }

 
    public List<SpriteRenderer> GetIconsPrefabs() {

        SpriteRenderer[] allIconsFromResources = Resources.LoadAll<SpriteRenderer>("Icons");
        List<SpriteRenderer> newIconList = new List<SpriteRenderer>();
        foreach (SpriteRenderer iconPointer in allIconsFromResources)
        {
            newIconList.Add(iconPointer);
        }
        return newIconList;
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform.position);
    }

    public void ShowStatePopUp (AI_State _state)
    {
        foreach (var i in IconsPrefabs)
        {
            if (_state.name.StartsWith(i.name))
            {  
                if (IconToGet)
                {
					
                    IconToGet.sprite = i.sprite;
                }
            }
        } 
    }

}

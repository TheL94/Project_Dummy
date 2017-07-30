using DG.Tweening;
using System.Collections.Generic;
using Framework.AI;
using UnityEngine;
using DumbProject.Generic;
using System.Linq;

namespace DumbProject.AI
{
    [CreateAssetMenu(menuName = "AI/State/Action/PopUp")]
    public class PopUp : Action
    {
        public Sprite Icon;
        public float VerticalOffSet;
        public float OffSetToAdd;
        public float DisplayTime;

        List<User> actors = new List<User>();

        public override void Act(AIController _controller)
        {
            AIActor actor = _controller as AIActor;

            PopUpSetup(actor);
            ShowPopUp(actor);
        }

        void ShowPopUp(AIActor _actor)
        {
            User actor = null;
            //Search for the right instance of the actor
            if (actors != null && actors.Count > 0)
                foreach (User user in actors)
                    if (user.actualUser == _actor)
                        actor = user;

            if (actor != null)
                actor.floating = actor.actualPopUp.transform.DOMoveY(VerticalOffSet + OffSetToAdd, DisplayTime)
                    .OnComplete(() => { actor.actualPopUp.SetActive(false); });
        }

        void PopUpSetup(AIActor _actor)
        {
            User actor = null;
            //Search for the right instance of the actor
            if (actors != null && actors.Count > 0)
                foreach (User user in actors)
                    if (user.actualUser == _actor)
                        actor = user;

            //Se non dovessi trovarlo, né creo uno nuovo
            if(actor == null)
            {
                actor = new User(_actor);
                actors.Add(actor);
            }
            
            actor.actualPopUp.SetActive(true);

            actor.popUpRenderer.enabled = true;

            if (actor.floating != null)
                actor.floating.Complete();

            actor.popUpRenderer.sprite = Icon;
            actor.actualPopUp.transform.parent = _actor.transform;
            actor.actualPopUp.transform.position = _actor.transform.position + new Vector3(0, VerticalOffSet, 0);
        }

        class User
        {
            public AIActor actualUser;
            public Tween floating;
            public GameObject actualPopUp;
            public SpriteRenderer popUpRenderer;

            /// <summary>
            /// costruttore della classe User
            /// </summary>
            /// <param name="actor"></param>
            public User(AIActor actor)
            {
                actualUser = actor;

                List<SpriteRenderer> renderers = actor.gameObject.GetComponentsInChildren<SpriteRenderer>().ToList();
                //controlla che la lista degli spriteRenderer non sia nulla
                if (renderers != null && renderers.Count > 0)
                {
                    //In tal caso se trova uno sprite renderer apartenente a un gameObject "PopUp", prende quelle referenze
                    foreach (SpriteRenderer spriteRender in renderers)
                    {
                        if (spriteRender.gameObject.name == "PopUp")
                        {
                            actualPopUp = spriteRender.gameObject;
                            popUpRenderer = spriteRender;
                        }
                    }
                }
                else
                {
                    //Altrimenti né crea uno e assegna dei nuovi component
                    actualPopUp = new GameObject("PopUp");
                    actualPopUp.AddComponent<ForceFacingCamera>();
                    popUpRenderer = actualPopUp.AddComponent<SpriteRenderer>();
                }
            }
        }
    }
}

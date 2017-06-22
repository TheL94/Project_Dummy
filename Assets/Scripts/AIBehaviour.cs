using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.AI {
    public class AIBehaviour
    {
        AIUser user;
        GameObject userGameObj { get { return user.UserGameObject; } }

        public float MovingSpeed = 1;

        public AIBehaviour(AIUser _user)
        {
            user = _user;
        }

        public void WalkTrughPoints(List<Vector3> _points)
        {
            Vector3 currentObjective = _points[0];

            userGameObj.transform.DOPath(_points.ToArray(), _points.Count * MovingSpeed);
        }
    }

    public interface AIUser
    {
        GameObject UserGameObject { get; set; }
    }
}

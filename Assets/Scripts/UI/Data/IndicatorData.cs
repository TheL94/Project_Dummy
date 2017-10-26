using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbProject.UI
{
    [CreateAssetMenu(fileName = "Indicator Data", menuName = "UI/new Indicator Data")]
    public class IndicatorData : ScriptableObject
    {

        public Sprite WalkSprite;
        public Sprite FightSprite;
        public Sprite ChooseSprite;
        public Sprite InteractSprite;
        public Sprite ForDumbyIndicator;
        public Sprite GenericIndicator;
        public GameObject GameObj;

    }
}
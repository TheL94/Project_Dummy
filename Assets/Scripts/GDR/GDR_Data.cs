using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;
using DumbProject.Generic;
using Framework.AI;
using DumbProject.GDR_System;

namespace DumbProject.GDR
{
    [CreateAssetMenu(menuName = "GDR_Controller")]
    public class GDR_Data : ScriptableObject
    {
        public string ID;
        public GDR_Controller GDRPrefab;
        public float ExperienceForNextLevel;
        public int PlayerLevel;
        public InventoryController iC;
        public float PercentageToSpawn;

        List<float> ExpLevel = new List<float>();

        [SerializeField] float _maxLife;// rischio.
        public float MaxLife
        {
            get { return _maxLife; }
            set
            {
                _maxLife = value;
                Life = MaxLife;

            }
        }
      public float Life { get; private set; }

        [SerializeField] float _maxArmor;// rischio.
        public float MaxArmor
        {
            get { return _maxArmor; }
            set
            {
                _maxArmor = value;
                Armor = MaxArmor;
            }
        }
        public float Armor { get; private set; }

        public float Speed;
        public float Attack;

        public float ExperienceToGive;
        public float ExperienceMuliplier = 1;

        GDR_Controller target;

        public ExperienceType experienceType;

        private float experienceCounter;
        public float ExperienceCounter
        {
            get { return experienceCounter; }
            set
            {
                experienceCounter = value;
                GetExperience(experienceType);
                AddLevel();
                UpdateLevels();
            }
        }

        public bool GetDamage(float _damage)
        {
            _damage -= Armor;
            if(_damage > 0)
            {
                Life -= _damage;
                iC.BreakArmor();
                Debug.Log(Life);
                if (Life <= 0)
                {
                    Life = 0;
                    Debug.LogWarning(ID+" is dead");
                    return false;
                }
            }
            return true;
        }

        public void GetCure(float _cure)
        {
            Life += _cure;
            if (Life > MaxLife)
            {
                Life = MaxLife;
            }
        }

        /// <summary>
        /// Aggiunge un livello ogni volta che viene superata l'ExperienceForNextLevel
        /// </summary>
        public void AddLevel()
        {
            if (ExpLevel.Count == 0)
            {
                ExpLevel.Add(ExperienceForNextLevel);
            }
            else
            {
                ExpLevel.Add(ExpLevel[ExpLevel.Count - 1] * 1.2f);
            }
        }

        /// <summary>
        /// Aggiorna l'esperienza necessaria per il livello successivo
        /// </summary>
        void UpdateLevels()
        {
            for (int i = 0; i < ExpLevel.Count; i++)
            {
                if (ExpLevel[i] >= ExperienceCounter)
                {
                    ModifyPlayerStats(PlayerLevel, i);
                    PlayerLevel = i;
                }
            }
        }

        /// <summary>
        /// Modify the Player statistic each time he reach a new level.
        /// </summary>
        void ModifyPlayerStats(int _previousLevel, int _newLevel)
        {
            if (_previousLevel < _newLevel)
            {
                Speed = Speed * 1.05f;
                Attack = Attack + 0.25f;
            }
            if (_previousLevel > _newLevel)
            {
                Speed = Speed * (-1.05f);
                Attack = Attack + 0.25f;
            }
        }

        // <summary>
        // Get the Experience from the Difficulty of the Enemy
        // </summary>
        // <returns></returns>
        float GetExperienceMultiplierFromEnemy()
        {
            if (ExperienceMuliplier > 1)
            {
                ExperienceToGive = ExperienceToGive * ExperienceMuliplier;
                return ExperienceToGive;
            }
            return 1;
        }

        /// <summary>
        /// Get the target and attack it.
        /// </summary>
        /// <param name="_target"></param>
        public void AttackTarget(GDR_Controller _target)
        {
            if (target = null)
            {
                target = _target;
            }
            _target.Data.Life -= Attack;
        }

        /// <summary>
        /// Get the experience from the Enemy
        /// </summary>
        public void GetExperience(ExperienceType _experienceType, GDR.GDR_Data_Experience _data = null)
        {
            Dumby dumby = FindObjectOfType<Dumby>();
            switch (_experienceType)
            {
                case ExperienceType.Enemy:
                    if (target != null)
                    {
                        ExperienceCounter += GetExperienceMultiplierFromEnemy();
                        target = null;
                    }
                    break;
                case ExperienceType.None:
                    break;
                case ExperienceType.Item:
                    break;
                case ExperienceType.Trap:               
                case ExperienceType.TimeWaster:
                    ExperienceCounter += _data.ExperienceToGive;
                    break;
                case ExperienceType.Room:
                    ExperienceCounter++;
                    break;
            }
        }
        

    }

    public enum ExperienceType
    {
        None,
        Enemy,
        Trap,
        Room,
        Item,
        TimeWaster
    }
    
}



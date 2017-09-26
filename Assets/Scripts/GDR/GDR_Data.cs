using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Rooms;
using DumbProject.Generic;
using Framework.AI;
using DumbProject.Items;

namespace DumbProject.GDR
{
    [CreateAssetMenu(menuName = "GDR_Controller")]
    public class GDR_Data : ScriptableObject
    {
        public GDR_Controller GDRPrefab;
        public float ExperienceForNextLevel;
        public int PlayerLevel;
        public InventoryController iC;
        public AI_Controller ai_Controller;

        List<float> ExpLevel = new List<float>();

        float _maxLife;
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

        float _maxArmor;
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
        public Obstacle obstacleType;

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

        public void GetDamage(float _damage)
        {
            _damage -= Armor;
            if(_damage > 0)
            {
                Life -= _damage;
                iC.BreakArmor();
                if (Life <= 0)
                {
                    Life = 0;
                    Debug.LogWarning("Dumby is dead");
                }
            }
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
        public void GetExperience(ExperienceType _experienceType)
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
                case ExperienceType.Obstacle:
                    GetObstacleExperience(obstacleType);
                    break;
                case ExperienceType.Room:
                    GetExperienceFromNewRoom();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Get the Experience when the room status id Explored.
        /// </summary>
        /// <param name="_room"></param>
        void GetExperienceFromNewRoom()
        {
            ExperienceCounter++;
        }

        /// <summary>
        /// Get the experience from the Obstacles
        /// </summary>
        /// <param name="_obstacle"></param>
        void GetObstacleExperience(Obstacle _obstacle)
        {
            switch (_obstacle)
            {
                case Obstacle.BananaPeel:
                    experienceCounter += 0.5f;
                    break;
                case Obstacle.Kitties:
                    experienceCounter += 0.75f;
                    break;
                case Obstacle.ShirtPresss:
                    experienceCounter += 1;
                    break;
                default:
                    break;
            }
        }
    }

    public enum ExperienceType
    {
        None,
        Enemy,
        Obstacle,
        Room
    }

    public enum Obstacle
    {
        None,
        BananaPeel,
        Kitties,
        ShirtPresss
    }
}



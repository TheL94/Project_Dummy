using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Grid;
using DumbProject.Generic;
using DumbProject.Items;

namespace DumbProject.Rooms
{
    public class GDR_Controller : MonoBehaviour
    {
        #region Variables

        public int PlayerLevel;

        private float experienceCounter;
        public float ExperienceCounter
        {
            get { return experienceCounter; }
            set
            {
                experienceCounter = value;
                GetExperienceMultiplier();
                CheckExperience();
            }
        }

        public float ExperienceForNextLevel;

        public float Life;
        public float ActualLife;
        public float Speed;
        public float Attack;


        private float experienceToGive;
        public float ExperienceToGive
        {
            get { return experienceToGive; }
            set
            {
                experienceToGive = value;
                GetExperienceMultiplier();
            }
        }
        public float ExperienceMuliplier;

        GDR_Controller target;
        public ExperienceType experienceType;
        public Obstacle obstacleType;
        #endregion

        #region API
        /// <summary>
        /// Check the experience for the next Player Level
        /// </summary>
        public bool CheckExperience()
        {
            float tempExperienceForlastLevel = ExperienceForNextLevel / 1.2f;
            if (ExperienceCounter >= ExperienceForNextLevel)
            {
                PlayerLevel++;
                ExperienceCounter = ExperienceCounter - ExperienceForNextLevel;
                ExperienceForNextLevel = ExperienceForNextLevel * 1.2f;
                ModifyPlayerStats();
                return true;
            }
            if (ExperienceCounter < tempExperienceForlastLevel)
            {
                PlayerLevel--;
                ExperienceForNextLevel = tempExperienceForlastLevel;
                ModifyPlayerStats();
            }
            return false;
        }
        /// <summary>
        /// Modify the Player statistic each time he reach a new level.
        /// </summary>
        public void ModifyPlayerStats()
        {
            if (CheckExperience() == true)
            {
                Speed = Speed * 1.05f;
                Attack = Attack + 0.25f;
            }
            else
            {
                Speed = Speed * (-1.05f);
                Attack = Attack + 0.25f;
            }
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
            _target.Life -= Attack;
        }
        /// <summary>
        /// Get the Experience from the Difficulty of the Enemy
        /// </summary>
        /// <returns></returns>
        public float GetExperienceMultiplier()
        {
            ExperienceToGive = ExperienceToGive * ExperienceMuliplier;
            return ExperienceToGive;
        }

        #region GetExperience


        /// <summary>
        /// Get the experience from the Enemy
        /// </summary>
        public void GetExperience(ExperienceType _experienceType)
        {
            Dumby dumby = FindObjectOfType<Dumby>();
            switch (_experienceType)
            {
                case ExperienceType.Enemy:
                    ExperienceCounter += GetExperienceMultiplier();
                    if (target != null)
                    {
                        target = null;
                    }
                    break;
                case ExperienceType.Obstacle:
                    GetObstacleExperience(obstacleType);
                    break;
                case ExperienceType.Room:
                    GetExperienceFromNewRoom(dumby.CurrentRoom);
                    break;
                default:
                    break;
            }

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

        /// <summary>
        /// Get the Experience when the room status id Explored.
        /// </summary>
        /// <param name="_room"></param>
        void GetExperienceFromNewRoom(Room _room)
        {
            if (_room != null)
            {
                if (_room.StatusOfExploration == ExplorationStatus.Explored)
                {
                    ExperienceCounter++;
                }
            }
        }
    }
    #endregion
    #endregion

    public enum ExperienceType
    {
        Enemy,
        Obstacle,
        Room
    }

    public enum Obstacle
    {
        BananaPeel,
        Kitties,
        ShirtPresss
    }
}
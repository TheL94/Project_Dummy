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
        private float experienceCounter;
        public float ExperienceCounter
        {
            get { return experienceCounter; }
            set
            {
                experienceCounter = value;
                CheckExperience();
            }
        }

        public int PlayerLevel;

        public float ExperienceForNextLevel;
        #endregion

        #region API
        /// <summary>
        /// Check the experience for the next Player Level
        /// </summary>
        public void CheckExperience()
        {
            if (ExperienceCounter >= ExperienceForNextLevel)
            {
                PlayerLevel++;
                ExperienceCounter = ExperienceCounter - ExperienceForNextLevel;
                ExperienceForNextLevel = ExperienceForNextLevel * 1.2f;
                ModifyPlayerStats();
            }
        }
        /// <summary>
        /// Modify the Player statistic each time he reach a new level.
        /// </summary>
        public void ModifyPlayerStats()
        {
            //Dumby.Speed = Dumby.Speed * 1,05f;
            //Dumby.Attack = Dumby.Attack +0.25f:
        }

        #region GetExperience
        /// <summary>
        /// Get the experience from the Enemy
        /// </summary>
        public void GetEnemyExperience(/*Enemy enemy*/)
        {

            //if (Enemy != null)
            //{
            //    ExperienceCounter += enemy.experience;
            //}

        }

        /// <summary>
        /// Get the experience from the Obstacles
        /// </summary>
        /// <param name="_obstacle"></param>
        public void GetObstacleExperience(Obstacle _obstacle)
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
        public void GetExperienceFromNewRoom(Room _room)
        {
            if (_room != null)
            {
                if (_room.StatusOfExploration == ExplorationStatus.Explored)
                {
                    /*ExperienceCounter++*/
                }
            }
        }
    } 
    #endregion
    #endregion

    public enum Obstacle
    {
        BananaPeel,
        Kitties,
        ShirtPresss
    }
}
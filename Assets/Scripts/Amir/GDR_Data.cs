using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;
namespace DumbProject.Rooms
{
    [CreateAssetMenu(menuName = "GDR_Controller")]
    public class GDR_Data : ScriptableObject
    {
        public bool IsInGame;
        public GDR_Controller GDRPrefab;
        public float ExperienceForNextLevel;
        public int PlayerLevel;

        public float Life;
        public float ActualLife;
        public float Speed;
        public float Attack;
        bool isNextLevel;


        public float ExperienceToGive;

        public float ExperienceMuliplier;

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
                GetExperienceMultiplier();
                CheckExperience();
            }
        }

        float tempExpLevel_0;
        float tempExpLevel_1;
        float tempExpLevel_2;
        float tempExpLevel_3;
        float tempExpLevel_4;
        float tempExpLevel_5;
        float tempExpLevel_6;
        float tempExpLevel_7;

        public void SetExperienceForNextLevel()
        {

            ExperienceForNextLevel = 10;
            tempExpLevel_0 = ExperienceForNextLevel;
            tempExpLevel_1 = tempExpLevel_0 + ExperienceForNextLevel * 1.2f;
            tempExpLevel_2 = tempExpLevel_1 + tempExpLevel_1 * 1.2f;
            tempExpLevel_3 = tempExpLevel_2 + tempExpLevel_2 * 1.2f;
            tempExpLevel_4 = tempExpLevel_3 + tempExpLevel_3 * 1.2f;
            tempExpLevel_5 = tempExpLevel_4 + tempExpLevel_4 * 1.2f;
            tempExpLevel_6 = tempExpLevel_5 + tempExpLevel_5 * 1.2f;
            tempExpLevel_7 = tempExpLevel_6 + tempExpLevel_6 * 1.2f;

        }
        /// <summary>
        /// Check the experience for the next Player Level
        /// </summary>
        public void CheckExperience()
        {
            if (ExperienceCounter >= ExperienceForNextLevel)
            {
                PlayerLevel++;
                CheckLevel(PlayerLevel);
            }
            if (ExperienceCounter < tempExpLevel_0 && PlayerLevel != 0)
            {
                PlayerLevel--;
                CheckLevel(PlayerLevel);
            }
            if (ExperienceCounter < tempExpLevel_1 && PlayerLevel > 1)
            {
                PlayerLevel--;
                CheckLevel(PlayerLevel);
            }
            if (ExperienceCounter < tempExpLevel_2 && PlayerLevel > 2)
            {
                PlayerLevel--;
                CheckLevel(PlayerLevel);
            }
            if (ExperienceCounter < tempExpLevel_3 && PlayerLevel > 3)
            {
                PlayerLevel--;
                CheckLevel(PlayerLevel);
            }
            if (ExperienceCounter < tempExpLevel_4 && PlayerLevel > 4)
            {
                PlayerLevel--;
                CheckLevel(PlayerLevel);
            }
            if (ExperienceCounter < tempExpLevel_5 && PlayerLevel > 5)
            {
                PlayerLevel--;
                CheckLevel(PlayerLevel);
            }
            if (ExperienceCounter < tempExpLevel_6 && PlayerLevel > 6)
            {
                PlayerLevel--;
                CheckLevel(PlayerLevel);
            }
            if (ExperienceCounter < tempExpLevel_7 && PlayerLevel > 7)
            {
                PlayerLevel--;
                CheckLevel(PlayerLevel);
            }

        }


        public int CheckLevel(int LevelToReturn)
        {
            if (PlayerLevel < 0)
            {
                PlayerLevel = 0;
            }

            int tempLevel;
            //float tempExpLevel_0 = ExperienceForNextLevel;
            //float tempExpLevel_1 = tempExpLevel_0 + ExperienceForNextLevel * 1.2f;
            //float tempExpLevel_2 = tempExpLevel_1 + tempExpLevel_1 * 1.2f;
            //float tempExpLevel_3 = tempExpLevel_2 + tempExpLevel_2 * 1.2f;
            //float tempExpLevel_4 = tempExpLevel_3 + tempExpLevel_3 * 1.2f;
            //float tempExpLevel_5 = tempExpLevel_4 + tempExpLevel_4 * 1.2f;
            //float tempExpLevel_6 = tempExpLevel_5 + tempExpLevel_5 * 1.2f;
            //float tempExpLevel_7 = tempExpLevel_6 + tempExpLevel_6 * 1.2f;
            switch (LevelToReturn)
            {
                case 0:
                    ExperienceForNextLevel = tempExpLevel_0;
                    break;
                case 1:
                    ExperienceForNextLevel = tempExpLevel_1;
                    break;
                case 2:
                    ExperienceForNextLevel = tempExpLevel_2;
                    break;
                case 3:
                    ExperienceForNextLevel = tempExpLevel_3;
                    break;
                case 4:
                    ExperienceForNextLevel = tempExpLevel_4;
                    break;
                case 5:
                    ExperienceForNextLevel = tempExpLevel_5;
                    break;
                case 6:
                    ExperienceForNextLevel = tempExpLevel_6;
                    break;
                case 7:
                    ExperienceForNextLevel = tempExpLevel_7;
                    break;
                default:
                    break;

            }
            tempLevel = LevelToReturn;
            if (tempLevel == PlayerLevel && PlayerLevel != 0)
            {
                isNextLevel = true;
                ModifyPlayerStats();
            }
            if (tempLevel != PlayerLevel)
            {
                isNextLevel = false;
                ModifyPlayerStats();
            }
            PlayerLevel = LevelToReturn;
            tempLevel = 0;
            return PlayerLevel;
        }

        /// <summary>
        /// Modify the Player statistic each time he reach a new level.
        /// </summary>
        public void ModifyPlayerStats()
        {
            if (isNextLevel)
            {
                Speed = Speed * 1.05f;
                Attack = Attack + 0.25f;
            }
            if (!isNextLevel && PlayerLevel != 0)
            {
                Speed = Speed * (-1.05f);
                Attack = Attack + 0.25f;
            }
        }

        // <summary>
        // Get the Experience from the Difficulty of the Enemy
        // </summary>
        // <returns></returns>
        public float GetExperienceMultiplier()
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
            _target.Data.ActualLife -= Attack;
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
                        ExperienceCounter += GetExperienceMultiplier();
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



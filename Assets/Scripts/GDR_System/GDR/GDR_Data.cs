using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DumbProject.Generic;


namespace DumbProject.GDR_System
{
    [CreateAssetMenu(menuName = "GDR_Controller")]
    public class GDR_Data : ScriptableObject
    {
        public string ID;
        public float ExperienceForNextLevel;
        public int PlayerLevel;
        public InventoryController inventoryCtrl;
        public float PercentageToSpawn;
        public List<float> ExpLevel = new List<float>();
        public GDR_PopUp PopUp;

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
                if(PopUp != null)
                    PopUp.ShowText(MaxArmor.ToString(), Color.yellow);
                Armor = MaxArmor;
            }
        }
        public float Armor { get; private set; }

        public int CurrentLevel { get; private set; }

        public float Speed;
        public float Damage;

        public float ExperienceToGive;
        public float ExperienceMuliplier = 1;

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

        GDR_Controller _GDR_Ctrl;

        public void Init(GDR_Controller _GDR_Controller)
        {
            _GDR_Ctrl = _GDR_Controller;
            inventoryCtrl = new InventoryController();
            Life = MaxLife;
            Armor = MaxArmor;
            CurrentLevel = 0;
            if (PopUp != null)
            {
                PopUp = Instantiate(PopUp, _GDR_Ctrl.transform);
                PopUp.transform.position = _GDR_Ctrl.transform.position;
            }
        }

        public bool GetDamage(float _damage)
        {
            if (PopUp != null)
                PopUp.ShowText(_damage.ToString(), Color.red);
            float damageToLife = 0f;
            if (Armor > 0)
            {
                damageToLife = _damage - Armor;
                if (damageToLife > 0)
                {
                    Armor -= _damage;
                    Life -= damageToLife;
                    inventoryCtrl.BreakArmor();
                    if (Life <= 0)
                    {
                        return false;
                    }
                }
                else
                {
                    Armor -= _damage;
                }
            }
            else
            {
                Life -= _damage;
                if (Life <= 0)
                {
                    Life = 0;
                    return false;
                }
            }
            return true;
        }

        public void GetCure(float _cure)
        {
            if (PopUp != null)
                PopUp.ShowText(_cure.ToString(), Color.green);
            Life += _cure;
            if (Life > MaxLife)
            {
                Life = MaxLife;
            }
        }

        /// <summary>
        /// Get the target and attack it.
        /// </summary>
        /// <param name="_target"></param>
        public bool AttackTarget(GDR_Controller _target)
        {
            float weaponDamage = inventoryCtrl.GetEquippedWeaponDamage();
            float totalDamage = Damage;

            if (weaponDamage > 0)
                totalDamage += weaponDamage;

            return !_target.Data.GetDamage(totalDamage);
        }

        /// <summary>
        /// Get the experience from the Enemy
        /// </summary>
        public void GetExperience(ExperienceType _experienceType, GDR_Element_Generic_Data _data = null)
        {
            Dumby dumby = FindObjectOfType<Dumby>();
            switch (_experienceType)
            {
                case ExperienceType.None:
                    break;
                case ExperienceType.Enemy:
                    ExperienceCounter += GetExperienceMultiplierFromEnemy();
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
                Damage = Damage + 0.25f;
            }
            if (_previousLevel > _newLevel)
            {
                Speed = Speed * (-1.05f);
                Damage = Damage + 0.25f;
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



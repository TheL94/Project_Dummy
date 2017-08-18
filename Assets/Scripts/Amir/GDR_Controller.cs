using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GDR_Controller : MonoBehaviour {


    private float experienceCounter;
    public float ExperienceCounter
    {
        get { return experienceCounter; }
        set { experienceCounter = value; }
    }

    private int playerLevel;

    public int PlayerLevel
    {
        get { return playerLevel; }
        set { playerLevel = value; }
    }


    /// <summary>
    /// Check the experience fort the next Player Level
    /// </summary>
    public void  CheckExperience()
    {
        switch ((int)ExperienceCounter)
        {
            case 10:
                PlayerLevel++;
                ExperienceCounter = 0;
                break;
            case 20:
                PlayerLevel++;
                ExperienceCounter = 0;
                break;
            case 30:
                PlayerLevel++;
                ExperienceCounter = 0;
                break;
            default:
                break;
        }
    }

    public void SetPlayerLevel()
    {
        switch (PlayerLevel)
        {
            case 1:
                //Dumby.Speed = Dumby.Speed * (1,05);
                //Dumby.Attacck = Dumby.Attack +0.25:
                break;
            case 2:
                //Dumby.Speed = Dumby.Speed * (1,05);
                //Dumby.Attacck = Dumby.Attack +0.25:
                break;
            case 3:
                //Dumby.Speed = Dumby.Speed * (1,05);
                //Dumby.Attacck = Dumby.Attack +0.25:
                break;
            default:
                break;
        }
    }
}

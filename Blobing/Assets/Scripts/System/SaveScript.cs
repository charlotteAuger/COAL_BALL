using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveScript
{
    public void Save(int currentGold)
    {
        PlayerPrefs.SetInt("Gold", currentGold);
    }

    public int GetSavedLevel()
    {
        int level = 1;

        if (PlayerPrefs.HasKey("Level"))
        {
            level = PlayerPrefs.GetInt("Level");
        }

        return level;
    }

    public int GetSavedGold()
    {
        int gold = 0;

        if (PlayerPrefs.HasKey("Gold"))
        {
            gold = PlayerPrefs.GetInt("Gold");
        }

        return gold;
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey("Level");
    }
}

#pragma warning disable 0649
using UnityEngine;

[System.Serializable]
public class Skill {

    public string skillName = "UNNAMED";
    private int currentLvl;
    private int maxLvl;
    private float currentXp;
    private float targetXp;

    // Shared settings
    [SerializeField] private static float startingXp = 82;
    [SerializeField] private static float baseLvlIncrement = 32;
    [SerializeField] private static float perLvlMultiplier = .4f;

    public float GetCurrentXp() {
        return currentXp;
    }

    public int GetCurrentLevel() {
        return currentLvl;
    }

    public void AddXp(float amount) {
        currentXp += amount;

        // If the current xp amount exceeds the target xp we need to lvl up
        if (currentXp >= targetXp) {
            LevelUp();
        }
    }

    // Increments the skill when a target xp is reached
    void LevelUp() {
        currentLvl++;
        targetXp = startingXp + baseLvlIncrement + (baseLvlIncrement * (currentLvl * perLvlMultiplier));
        // Check if we overleveled
        if (currentXp >= targetXp) {
            LevelUp();
        }
    }


}

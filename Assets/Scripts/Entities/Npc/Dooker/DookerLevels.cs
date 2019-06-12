#pragma warning disable 0649
using UnityEngine;

namespace Doodie.NPC {

    public class DookerLevels : MonoBehaviour {

        public enum DOOKER_LEVELS {
            STR_LVL = 0,
            ATT_LVL = 1
        }

        [SerializeField] private Skill[] dookerSkills = new Skill[2];


        public void AddExperience(float amount, DOOKER_LEVELS levelToAdd) {
            try {
                dookerSkills[(int)levelToAdd].AddXp(amount);
            }
            catch (System.Exception e){
                Debug.LogException(e);
            }
        }

    }

}
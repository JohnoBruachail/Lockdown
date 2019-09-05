using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    The idea here is to create an singleton object containing the states
    of the players/bases/robots upgrades, the player will unlock access to these upgrades
    with data chips and power them up (enable them)
*/
public class UpgradeManager : MonoBehaviour
{

    private static UpgradeManager instance;

    public static UpgradeManager Instance {
        get {
                return instance;
            }
    }

    private void Awake(){
        if (instance != null && instance != this){
            
            Destroy(this.gameObject);
            
        }
        else {
            instance = this;
        }
    }

    //Player upgrades
    public bool jetpack = false;
    public bool personalShield = false;
    public bool sentry = false;

    //Robot upgrades
    public int movementSpeedModifier = 1;
    public int constructionSpeedModifier = 1;

    //turret upgrades
    public int turretRangeModifier = 1;
    public int turretDamageModifier = 1;
    public int turretAttackSpeedModifier = 1;

    //shield upgrades
    public int shieldContactDamage = 0;
    public int shieldHealthModifier = 1;

    //mine upgrades
    public int crystalProductionModifier = 1;




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display : MonoBehaviour{

    int health;
    int maxHealth;
    int eCrystals;
    int maxCrystals;
    
    public Image[] hearts;
    public Image[] crystals;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Sprite fullCrystal;
    public Sprite emptyCrystal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealth(int newHealth){

        health = newHealth;

        for(int i = 0; i < hearts.Length; i++){
            if(i < health){
                hearts[i].sprite = fullHeart;
            } else{
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    public void UpdateMaxHealth(int newMaxHealth){

        maxHealth = newMaxHealth;

        for(int i = 0; i < hearts.Length; i++){
            if(i < maxHealth){
                hearts[i].enabled = true;
            }else{
                hearts[i].enabled = false;
            }
        }
    }

    public void UpdateCrystals(int newECrystals){
        
        eCrystals = newECrystals;

        for(int i = 0; i < crystals.Length; i++){
            if(i < eCrystals){
                crystals[i].sprite = fullCrystal;
            } else{
                crystals[i].sprite = emptyCrystal;
            }
        }
    }

    public void UpdateMaxCrystals(int newMaxCrystals){
        
        maxCrystals = newMaxCrystals;
        
        for(int i = 0; i < crystals.Length; i++){
            if(i < maxCrystals){
                crystals[i].enabled = true;
            }else{
                crystals[i].enabled = false;
            }
        }
    }

}

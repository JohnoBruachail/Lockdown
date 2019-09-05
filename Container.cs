using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour, IInteractable
{
    public int quantity;
    public GameObject content;
    public GameObject interactionPrompt;

    public void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            Debug.Log("Player collided with Container");
            if(quantity > 0){
                interactionPrompt.SetActive(true);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            
			interactionPrompt.SetActive(false);
        }
    }

    public void Interact(){
        interactionPrompt.SetActive(false);
        for(int i = 0; i < quantity; i++){
                Instantiate(content, gameObject.transform.position, Quaternion.identity);
        }
        quantity = 0;
        //SHOULD I DESTROY THE OBJECT OR CHANCE IT STATE TO EMPTY HERE?
    }
}

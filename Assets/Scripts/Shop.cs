﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    [Header("UI")]
    public GameObject shopHolderObject;
    public TMP_Text itemName;
    public TMP_Text itemFlavourText;
    public TMP_Text itemStatText;
    public TMP_Text playerValueText;
    public Image itemImage;

    private PlayerInventory playerInventory;
    private PlayerCombat playerCombat;
    private int desiredIndex = 0;

    private void Awake() {
        playerInventory = FindObjectOfType<PlayerInventory>();
        playerCombat = FindObjectOfType<PlayerCombat>();
    }

    private void Update() {
        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.V)){
            StartShop();
        }
        #endif

        if(shopHolderObject.activeInHierarchy && playerInventory.itemList.Count != 0) {
            // Navigating the inventory
            if(Input.GetKeyDown(KeyCode.W)){
                if(desiredIndex == 0){
                    desiredIndex = playerInventory.itemList.Count-1;
                }
                else{
                    desiredIndex--;
                }

                UpdateCurrentItem();
            }
            if(Input.GetKeyDown(KeyCode.S)){
                if(desiredIndex < playerInventory.itemList.Count-1){
                    desiredIndex++;
                }
                else{
                    desiredIndex = 0;
                }

                UpdateCurrentItem();
            }
            if(Input.GetKeyDown(KeyCode.Space)){
                SellCurrentItem();
            }
        }
    }

    public void StartShop(){
        shopHolderObject.SetActive(true);

        if(playerInventory.itemList.Count != 0){
            desiredIndex = 0;
        }
        else{
            desiredIndex = -1;
        }

        UpdateCurrentItem();
    }

    public void SellCurrentItem(){
        if(desiredIndex == -1){
            return;
        }

        Item itemToSell = playerInventory.itemList[desiredIndex];

        // Transfer currency to the player
        playerCombat.HP += itemToSell.value;

        // Remove the item!
        playerInventory.itemList.Remove(itemToSell);
        Destroy(itemToSell.gameObject);


        // Update UI
        UpdateCurrentItem();
    }

    public void UpdateCurrentItem(){

        playerValueText.text = "You have: " + playerCombat.HP.ToString();

        if(playerInventory.itemList.Count == 0){
            itemName.text = "No item left!";

            itemFlavourText.text = "";
            itemImage.color = Color.clear;
            itemStatText.text = "";

            desiredIndex = -1;
            return;
        }

        Item _item = playerInventory.itemList[desiredIndex];
        itemName.text = _item.itemName;
        itemFlavourText.text = _item.flavourText;

        itemImage.sprite = _item.GetComponent<SpriteRenderer>().sprite;
        itemImage.color = Color.white;
        
        string statText = "Value: " + _item.value.ToString();
        if(_item.itemType == Item.ItemType.WEAPON){
            statText += "\nDamage: " + _item.damage.ToString();
        }
        else if(_item.itemType == Item.ItemType.ARMOR){
            statText += "\nArmor: " + _item.defense.ToString(); 
        }

        statText += "\nPress Space to sell!";

        itemStatText.text = statText;
    }

}
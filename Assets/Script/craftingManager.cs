using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class craftingManager : MonoBehaviour
{
    private item currentItem;
    public Image customcursor;
    public Slot[] slots;

    public List<item> itemList;
    public string[] recipe;
    public item[] reciperesults;
    public Slot resultSlot;

    public void Update()
    {
        if(Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if(currentItem != null)
            {
                // 1. Check if you actually assigned slots in the inspector
                if (slots == null || slots.Length == 0)
                {
                    Debug.LogError("No slots assigned to the Crafting Manager!");
                    currentItem = null; // Reset anyway so it doesn't get stuck
                    return;
                }

                Vector3 mousePosition = Mouse.current.position.ReadValue();
                customcursor.transform.position = mousePosition;

                Slot nearestSlot = null;
                float nearestDistance = float.MaxValue;

                foreach(Slot slot in slots)
                {
                    if (slot == null) continue; // Skip unassigned slots

                    float distance = Vector3.Distance(mousePosition, slot.transform.position);
                    if(distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestSlot = slot;
                    }
                }
                
                // 2. Safely check if we found a slot before using it
                if (nearestSlot != null)
                {
                    nearestSlot.gameObject.SetActive(true);
                    
                    Image slotImage = nearestSlot.GetComponent<Image>();
                    Image itemImage = currentItem.GetComponent<Image>();

                    if (slotImage != null && itemImage != null)
                    {
                        slotImage.sprite = itemImage.sprite;
                    }

                    nearestSlot.itemInSlot = currentItem;
                    itemList[nearestSlot.index] = currentItem; // Assuming slotIndex is a property of Slot
                }

                // This will now always run safely
                currentItem = null;
                customcursor.gameObject.SetActive(false); // Hide the cursor image since hand is empty
                CheckRecipe();
            }
        }
    }

    void CheckRecipe()
    {
        resultSlot.gameObject.SetActive(false);
        resultSlot.itemInSlot = null;

        string currentRecipe = "";
        foreach(item item in itemList)
        {
            if(item != null)
            {
                currentRecipe += item.itemName + ",";
            }else
            {
                currentRecipe += "null,";
            }
        }

        for (int i = 0; i < recipe.Length; i++)
        {
            if(currentRecipe == recipe[i])
            {
                resultSlot.gameObject.SetActive(true);
                resultSlot.GetComponent<Image>().sprite = reciperesults[i].GetComponent<Image>().sprite;
                resultSlot.itemInSlot = reciperesults[i];
                break;
            }
        }

    }

    public void OnClickSlot(int slotIndex) // Changed from Slot to int
    {
        // Make sure the index is valid within your array bounds
        if (slotIndex >= 0 && slotIndex < slots.Length)
        {
            Slot targetSlot = slots[slotIndex];

            if (targetSlot != null)
            {
                targetSlot.itemInSlot = null;
                targetSlot.GetComponent<Image>().sprite = null; // Clear the slot's image
            }
        }

        // Update your item list data tracking
        if (slotIndex >= 0 && slotIndex < itemList.Count)
        {
            itemList[slotIndex] = null;
        }

        CheckRecipe();
    }

    public void OnMouseDown(item item)
    {
        if(currentItem == null)
        {
            currentItem = item;
            customcursor.gameObject.SetActive(true);
            customcursor.sprite = currentItem.GetComponent<Image>().sprite;
            Debug.Log("Current item set to: " + currentItem.itemName);
        }
        else
        {
            Debug.Log("Current item is already set to: " + currentItem.itemName);
        }
    }
}

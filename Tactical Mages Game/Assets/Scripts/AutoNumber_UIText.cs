using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutoNumber_UIText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberTextObject;
    [SerializeField] private Transform layoutGroup;

    private void Awake()
    {
        //For all children in layout group, assign numbered text as i+1 to appropiate object.
        for (int i = 0; i < layoutGroup.childCount; i++)
        {
            if (layoutGroup.GetChild(i) == this.transform)
            {                
                numberTextObject.SetText((i+1).ToString());         
            }
        }
    }
}

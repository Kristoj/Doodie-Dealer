using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saleable : MonoBehaviour
{
    [SerializeField] private float itemValue = 1;

    public float SellValue {
        get{
            return itemValue;
        }

        set{
            itemValue = value;
        }
    }
}

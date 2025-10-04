using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class InventoryControl : MonoBehaviour
{
    [Serializable]
    public class Item
    {
        public Sprite Icon;
        public string Name;
        public bool IsCollected = false;
    }

    [SerializeField] Item[] items;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject buttonTemplate = transform.GetChild(0).gameObject;
        GameObject g;

        int N = items.Length;

        for (int i = 0; i < N; i++)
        {
            g = Instantiate(buttonTemplate, transform);
            g.transform.GetChild(0).GetComponent<Image>().sprite = items[i].Icon;
            g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i].Name;
            //if (PlayerPrefs.HasKey(items[i].Name))
            //{
            //    if (PlayerPrefs.GetInt(items[i].Name) == 1)
            //    {
            //        items[i].IsCollected = true;
            //    }
            //}
            //g.transform.GetChild(2).GetComponent<TextMeshProUGUI>().enabled = items[i].IsCollected;
        }

        Destroy(buttonTemplate);
    }
}


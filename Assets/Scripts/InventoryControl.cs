using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections.Generic;

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
    private int N;
    public Dictionary<string, bool> collected = new Dictionary<string, bool>();
    public bool startDone = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject buttonTemplate = transform.GetChild(0).gameObject;
        N = items.Length;

        for (int i = 0; i < N; i++)
        {
            GameObject g = Instantiate(buttonTemplate, transform);
            g.transform.GetChild(0).GetComponent<Image>().sprite = items[i].Icon;
            g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i].Name;
            if (!(collected.ContainsKey(items[i].Name) && collected[items[i].Name]))
            {
                g.transform.GetChild(0).GetComponent<Image>().enabled = false;
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().enabled = false;
            }
        }
        startDone = true;
        Destroy(buttonTemplate);
    }

    public void UpdateList()
    {
        for (int i = 0; i < N; i++)
        {
            Transform g = transform.GetChild(i);
            if (collected.ContainsKey(items[i].Name) && collected[items[i].Name])
            {
                g.transform.GetChild(0).GetComponent<Image>().enabled = true;
                g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().enabled = true;
            }
        }
    }
}


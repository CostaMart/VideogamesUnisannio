using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// This class converts the JSON file into a list of items
/// </summary>
public class ItemManager
{
    public int id;
    public List<AbstractEffect> effects;

    public static Dictionary<int, Item> globalItemPool = new Dictionary<int, Item>(); /// this contains all the items created by the game from the JSON file
    public static Item ComputeAnItem()
    {

        Debug.Log("ComputeAnItem called");

        // Leggi il JSON dal file
        string text = File.ReadAllText("/home/costamh/HeroDivers/ItemList.json");

        // Deserializza il JSON in ItemJson, che contiene la proprietà item
        ItemJson data = JsonConvert.DeserializeObject<ItemJson>(text);

        // stadio molto prototipale, hardcoded la crezione di questo specifico tipo di effetto
        // ma i parametri sono presi dinamicaente dal file JSON
        // Accesso ai dati
        Item i = null;

        foreach (var item in data.items)  // per ogni item json
        {
            i = new Item
            {
                effects = new List<AbstractEffect>()
            };

            i.name = item.name;
            i.id = item.id;
            int effectID = 0;

            foreach (var effect in item.effects) // per ogni effetto nella lista
            {
                var type = effect["effectType"].ToString();
                AbstractEffect e = null;
                switch (type)
                {
                    case "sa":
                        e = new SingleActivationEffect(effect, item.id, effectID);
                        break;

                    case "ot":
                        e = new OverTimeEffect(effect, item.id, effectID);
                        break;

                    default:
                        throw new Exception("Effect type '" + type + "' not recognized");
                }

                i.effects.Add(e);
                effectID++;
            }

            Debug.Log("item created: " + item.name);
            Debug.Log("item id: " + item.id);

            if (globalItemPool.ContainsKey(i.id))
            {
                throw new Exception("Item with ID " + i.id + " already exists in the global pool. Skipping creation.");
            }
            else
            {
                globalItemPool.Add(i.id, i);
            }
        }



        return i;
    }

    private class ItemJson
    {
        public List<ItemIncomplete> items;
    }

    private class ItemIncomplete
    {
        public int id;

        public string name;
        public List<Dictionary<string, string>> effects;
    }
}


public class Item
{
    public string name;
    public int id;
    public List<AbstractEffect> effects;

    public override string ToString()
    {
        string s = "Item: \n";
        foreach (var effect in effects)
        {
            s += effect.ToString() + "\n";
        }
        return s;
    }
}
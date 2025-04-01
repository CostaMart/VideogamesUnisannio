using System;
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
    public static Dictionary<string, int> statClassToIdRegistry;
    public bool added = false;
    public static Dictionary<int, Item> globalItemPool = new Dictionary<int, Item>(); /// this contains all the items created by the game from the JSON file

    static ItemManager()
    {
        // Initialize the statClass dictionary with some values
        statClassToIdRegistry = new Dictionary<string, int>
        {
            { "CharStats", 0},
            { "testUpdate", 1 },
            { "Ragdoller",  2 },
            { "PrimaryWeaponState", 3},
            {"SecondaryWeaponState", 4}
        };

        ComputeAllItems();
    }

    /// <summary>
    /// This method is called at the start of the game to create the item pool reading items form the JSON file
    /// 
    /// TODO: in questa fase viene chiamato dal dispatcher per prova
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static Item ComputeAllItems()
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
        try
        {
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
                            throw new Exception("Effect type object type: '" + type + "' not recognized for item: " + item.id);
                    }

                    i.effects.Add(e);
                    effectID++;
                }

                if (globalItemPool.ContainsKey(i.id))
                {
                    throw new Exception("Item with ID " + i.id + " already exists in the global pool. Skipping creation.");
                }
                else
                {
                    globalItemPool.Add(i.id, i);
                }
            }
        }

        catch (KeyNotFoundException e)
        {
            Debug.LogError("Error in Item manager unable to create an item: " + e.Message + " check the JSON item definition file");
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

}


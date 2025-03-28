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

    public static Item ComputeAnItem()
    {

        // Leggi il JSON dal file
        string text = File.ReadAllText("/home/costamh/HeroDivers/ItemList.json");

        // Deserializza il JSON in ItemJson, che contiene la proprietà item
        ItemJson data = JsonConvert.DeserializeObject<ItemJson>(text);

        // stadio molto prototipale, hardcoded la crezione di questo specifico tipo di effetto
        // ma i parametri sono presi dinamicaente dal file JSON
        // Accesso ai dati
        Item i = null;
        foreach (var item in data.items)  // Accedi alla lista di effetti
        {
            i = new Item();
            i.effects = new List<AbstractEffect>();

            foreach (var effect in item.effects)
            {
                var type = effect["effectType"].ToString();
                AbstractEffect e = null;
                switch (type)
                {
                    case "ot":
                        e = new SingleActivationEffect(effect);
                        break;
                }

                i.effects.Add(e);
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
        public List<Dictionary<string, string>> effects;
    }
}


public class Item
{
    public List<AbstractEffect> effects;
}
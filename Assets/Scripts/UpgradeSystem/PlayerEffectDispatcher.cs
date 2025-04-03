using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using Weapon.State;
using static ItemManager;

/// <summary>
/// This component is responsible for dispatching the effects to the correct classes, and serves as a
/// bridge between the upgrades and all the gameobject components which could be useful to implement effects
/// 
/// this class shall manage overTime effects activation too
/// </summary>
public class PlayerEffectDispatcher : EffectsDispatcher
{

    public List<EffectsDispatcher> dispatchers = new List<EffectsDispatcher>();
    private List<EffectsDispatcher> toRemovesDisp = new List<EffectsDispatcher>();


    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            ItemDispatch(collision.gameObject.GetComponent<ItemMono>().item);
        }

        if (collision.TryGetComponent<EffectsDispatcher>(out EffectsDispatcher disp))
        {
            dispatchers.Add(disp);
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<EffectsDispatcher>(out EffectsDispatcher disp))
        {
            dispatchers.Remove(disp);
        }
    }


    void Update()
    {
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                this.ItemDispatch(ItemManager.bulletPool[3]);
            }



            foreach (var effect in toExternalDispatchArea)
            {
                foreach (var disp in dispatchers)
                {
                    Debug.Log("dhispatcing to external source " + effect.ToString());
                    disp.DispatchFromExternalSource(effect);
                    toRemovesDisp.Add(disp);
                }
            }

            foreach (var disp in toRemovesDisp)
            {
                dispatchers.Remove(disp);
            }

            toRemovesDisp.Clear();
        }

    }


}


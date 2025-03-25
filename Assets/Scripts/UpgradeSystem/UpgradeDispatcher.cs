using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeDispatcher : MonoBehaviour
{

    [SerializeField] IUpgradable[] upgraders = new IUpgradable[3];
    [SerializeField] private ControlEventManager controlEventManager;

    private List<Upgrade> activeOvertime = new List<Upgrade>();
    // test
    void Awake()
    {
        FindComponentsInChildren<IUpgradable>(transform);
    }
    void Update()
    {
        for (int i = activeOvertime.Count - 1; i >= 0; i--)
        {
            Upgrade up = activeOvertime[i];

            if (Time.time >= up.nextTickTime)
            {
                up.nextTickTime = Time.time + up.tickTime;
                up.tickNumber++;

                if (up.tickNumber > up.durationTicks)
                {
                    activeOvertime.RemoveAt(i);
                    continue;
                }

                UpgradeActivate(up);
            }
        }

        // PROTO: solo per prototipazione, verranno eliminati 
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Dispatching upgrade");
            Upgrade up = new Upgrade();
            up.classTarget = 0;
            up.attributeTarget = 6;
            up.value = 200;
            up.operation = (float operand) => operand + 100;
            Item it = new Item();
            it.upgrades.Add(up);

            OnItemPickUp(it);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Dispatching upgrade");
            Upgrade up = new Upgrade();
            up.classTarget = 1;
            up.value = 200;
            up.operation = (float operand) => operand + 100;
            up.isOvertime = true;
            up.tickTime = 1;
            up.durationTicks = 20;
            Item it = new Item();
            it.upgrades.Add(up);

            OnItemPickUp(it);
        }
    }
    public void UpgradeActivate(Upgrade up)
    {
        upgraders[up.classTarget].Upgrade(up);
    }

    public void OnItemPickUp(Item item)
    {
        foreach (Upgrade up in item.upgrades)
        {
            if (up.isOvertime)
            {
                activeOvertime.Add(up);
            }

            else
            {
                UpgradeActivate(up);
            }

        }
    }

    void FindComponentsInChildren<T>(Transform parent) where T : IUpgradable
    {
        var components = parent.GetComponents<Component>();

        foreach (var component in components)
        {
            if (component is T upgradable)
            {
                Debug.Log("Found IUpgradable component: " + component.GetType().Name);
                upgraders[upgradable.ID] = upgradable;
            }
        }

        foreach (Transform child in parent)
        {
            FindComponentsInChildren<T>(child);  // Chiamata ricorsiva per ogni figlio
        }
    }

}

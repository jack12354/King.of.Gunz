using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Dropdown
        BodyDropdown, FiringDropdown, BarrelDropdown, StockDropdown, GripDropdown, ScopeDropdown,
        BodyModsDropdown, FiringModsDropdown, BarrelModsDropdown, StockModsDropdown, GripModsDropdown, ScopeModsDropdown;
    // Use this for initialization
    void Start()
    {
        BodyDropdown = GameObject.Find("Body").GetComponent<Dropdown>();
        FiringDropdown = GameObject.Find("Firing").GetComponent<Dropdown>();
        BarrelDropdown = GameObject.Find("Barrel").GetComponent<Dropdown>();
        StockDropdown = GameObject.Find("Stock").GetComponent<Dropdown>();
        GripDropdown = GameObject.Find("Grip").GetComponent<Dropdown>();
        ScopeDropdown = GameObject.Find("Scope").GetComponent<Dropdown>();
        BodyModsDropdown = GameObject.Find("Body Mods").GetComponent<Dropdown>();
        FiringModsDropdown = GameObject.Find("Firing Mods").GetComponent<Dropdown>();
        BarrelModsDropdown = GameObject.Find("Barrel Mods").GetComponent<Dropdown>();
        StockModsDropdown = GameObject.Find("Stock Mods").GetComponent<Dropdown>();
        GripModsDropdown = GameObject.Find("Grip Mods").GetComponent<Dropdown>();
        ScopeModsDropdown = GameObject.Find("Scope Mods").GetComponent<Dropdown>();

        Gun g = GetComponent<Gun>();
        BodyDropdown.onValueChanged.AddListener(index => g.SetComponent(((OptionDataExt) BodyDropdown.options[index]).ComponentID));
        FiringDropdown.onValueChanged.AddListener(index => g.SetComponent(((OptionDataExt) FiringDropdown.options[index]).ComponentID));
        BarrelDropdown.onValueChanged.AddListener(index => g.SetComponent(((OptionDataExt) BarrelDropdown.options[index]).ComponentID));
        StockDropdown.onValueChanged.AddListener(index => g.SetComponent(((OptionDataExt) StockDropdown.options[index]).ComponentID));
        GripDropdown.onValueChanged.AddListener(index => g.SetComponent(((OptionDataExt) GripDropdown.options[index]).ComponentID));
        ScopeDropdown.onValueChanged.AddListener(index => g.SetComponent(((OptionDataExt) ScopeDropdown.options[index]).ComponentID));
        BodyModsDropdown.onValueChanged.AddListener(index => g.SetComponent(((OptionDataExt) BodyModsDropdown.options[index]).ComponentID));
        FiringModsDropdown.onValueChanged.AddListener(index => g.SetComponent(((OptionDataExt) FiringModsDropdown.options[index]).ComponentID));
        BarrelModsDropdown.onValueChanged.AddListener(index => g.SetComponent(((OptionDataExt) BarrelModsDropdown.options[index]).ComponentID));
        StockModsDropdown.onValueChanged.AddListener(index => g.SetComponent(((OptionDataExt) StockModsDropdown.options[index]).ComponentID));
        GripModsDropdown.onValueChanged.AddListener(index => g.SetComponent(((OptionDataExt) GripModsDropdown.options[index]).ComponentID));
        ScopeModsDropdown.onValueChanged.AddListener(index => g.SetComponent(((OptionDataExt) ScopeModsDropdown.options[index]).ComponentID));

        ComponentStore.BodyComponents.ForEach(wc => BodyDropdown.options.Add(new OptionDataExt { text = wc.Name, image = null, ComponentID = wc.ID }));
        ComponentStore.FiringComponents.ForEach(wc => FiringDropdown.options.Add(new OptionDataExt { text = wc.Name, image = null, ComponentID = wc.ID }));
        ComponentStore.BarrelComponents.ForEach(wc => BarrelDropdown.options.Add(new OptionDataExt { text = wc.Name, image = null, ComponentID = wc.ID }));
        ComponentStore.StockComponents.ForEach(wc => StockDropdown.options.Add(new OptionDataExt { text = wc.Name, image = null, ComponentID = wc.ID }));
        ComponentStore.GripComponents.ForEach(wc => GripDropdown.options.Add(new OptionDataExt { text = wc.Name, image = null, ComponentID = wc.ID }));
        ComponentStore.ScopeComponents.ForEach(wc => ScopeDropdown.options.Add(new OptionDataExt { text = wc.Name, image = null, ComponentID = wc.ID }));
        ComponentStore.BodyMods.ForEach(wc => BodyModsDropdown.options.Add(new OptionDataExt { text = wc.Name, image = null, ComponentID = wc.ID }));
        ComponentStore.FiringMods.ForEach(wc => FiringModsDropdown.options.Add(new OptionDataExt { text = wc.Name, image = null, ComponentID = wc.ID }));
        ComponentStore.BarrelMods.ForEach(wc => BarrelModsDropdown.options.Add(new OptionDataExt { text = wc.Name, image = null, ComponentID = wc.ID }));
        ComponentStore.StockMods.ForEach(wc => StockModsDropdown.options.Add(new OptionDataExt { text = wc.Name, image = null, ComponentID = wc.ID }));
        ComponentStore.GripMods.ForEach(wc => GripModsDropdown.options.Add(new OptionDataExt { text = wc.Name, image = null, ComponentID = wc.ID }));
        ComponentStore.ScopeMods.ForEach(wc => ScopeModsDropdown.options.Add(new OptionDataExt { text = wc.Name, image = null, ComponentID = wc.ID }));
    }

    // Update is called once per frame
    void Update()
    {

    }
}

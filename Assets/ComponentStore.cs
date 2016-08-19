using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleJson;
using SimpleJSON;

public static class ComponentStore
{
    public static List<WeaponComponent> AllWeaponComponents = new List<WeaponComponent>();
    #region Specific Lists
    public static List<WeaponComponent> BodyComponents { get { return AllWeaponComponents.Where(x => x.ComponentType == ComponentType.Body && !x.Mod).ToList(); } }
    public static List<WeaponComponent> FiringComponents { get { return AllWeaponComponents.Where(x => x.ComponentType == ComponentType.Firing && !x.Mod).ToList(); } }
    public static List<WeaponComponent> BarrelComponents { get { return AllWeaponComponents.Where(x => x.ComponentType == ComponentType.Barrel && !x.Mod).ToList(); } }
    public static List<WeaponComponent> StockComponents { get { return AllWeaponComponents.Where(x => x.ComponentType == ComponentType.Stock && !x.Mod).ToList(); } }
    public static List<WeaponComponent> GripComponents { get { return AllWeaponComponents.Where(x => x.ComponentType == ComponentType.Grip && !x.Mod).ToList(); } }
    public static List<WeaponComponent> ScopeComponents { get { return AllWeaponComponents.Where(x => x.ComponentType == ComponentType.Scope && !x.Mod).ToList(); } }
    public static List<WeaponComponent> BodyMods { get { return AllWeaponComponents.Where(x => x.ComponentType == ComponentType.Body && x.Mod).ToList(); } }
    public static List<WeaponComponent> FiringMods { get { return AllWeaponComponents.Where(x => x.ComponentType == ComponentType.Firing && x.Mod).ToList(); } }
    public static List<WeaponComponent> BarrelMods { get { return AllWeaponComponents.Where(x => x.ComponentType == ComponentType.Barrel && x.Mod).ToList(); } }
    public static List<WeaponComponent> StockMods { get { return AllWeaponComponents.Where(x => x.ComponentType == ComponentType.Stock && x.Mod).ToList(); } }
    public static List<WeaponComponent> GripMods { get { return AllWeaponComponents.Where(x => x.ComponentType == ComponentType.Grip && x.Mod).ToList(); } }
    public static List<WeaponComponent> ScopeMods { get { return AllWeaponComponents.Where(x => x.ComponentType == ComponentType.Scope && x.Mod).ToList(); } }
    #endregion

    static ComponentStore()
    {
        Resources.LoadAll<TextAsset>("Components").ToList().ForEach(Parse);
    }

    private static void Parse(TextAsset inFile)
    {
       // Debug.Log("Parsing " + inFile.name);
        var json = JSON.Parse(inFile.text);
        ComponentType type = (ComponentType)Enum.Parse(typeof(ComponentType), json["ComponentType"].Value);
        bool isMod = json["Mod"].AsBool;
        foreach (var child in json["Components"].Children)
        {
            WeaponComponent component = new WeaponComponent { ComponentType = type };
            foreach (string key in child.Keys)
            {
                var field = typeof(WeaponComponent).GetField(key);
                switch (field.FieldType.Name)
                {
                    case "UInt32": field.SetValue(component, (uint)child[key].AsInt); continue;
                    case "Int32": field.SetValue(component, child[key].AsInt); continue;
                    case "String": field.SetValue(component, child[key].Value); continue;
                    case "Single": field.SetValue(component, child[key].AsFloat); continue;
                    case "Double": field.SetValue(component, child[key].AsDouble); continue;
                    case "Boolean": field.SetValue(component, child[key].AsBool); continue;
                    default: field.SetValue(component, Enum.Parse(field.FieldType, child[key].Value)); continue; // It's probably an enum
                }
            }
            component.Mod = isMod;
            component.ID += (1000 * (int)type) + (500 * (isMod ? 1 : 0));
            Debug.Log("Adding " + type + " " + component.ID + ": " + component.Name);

            for (int lvl = 0; lvl < (int)Quality.Count; lvl++)
            {
                component.Quality = (Quality) lvl;
                AllWeaponComponents.Add((WeaponComponent)component.Clone());
                component.ID += 0.1;
            }
        }
//        AllWeaponComponents.AddRange(components);
    }

    public static bool init = true; // Well, it is now that you've come and looked at it
}
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Gun : MonoBehaviour
{
    #region Fixed Stats

    public bool FullAuto;   // This weapon is fully automatic
    public int BurstSize;   // Size of Burst ( 1 for single shot weapons )
    public float BurstSpeed // Rounds Per Minute of shots in burst
    {
        get { return Mathf.Pow(rofBurst, -1) * 60.0f; }
        set { rofBurst = Mathf.Pow(value / 60.0f, -1); }
    }


    public float BulletDamage;  // How much damage one bullet does (sans modifiers)
    public int BulletsPerShot;  // How many bullets are fired per round ( >1 for shotguns )
    public float RateOfFire     // Rounds Per Minute (or Bursts)
    {
        get { return Mathf.Pow(rof, -1) * 60.0f; }
        set { rof = Mathf.Pow(value / 60.0f, -1); }
    }

    public float ReloadTime;    // Reload Speed
    public int MagSize;         // Size of a full magazine
    public int MaxAmmo;         // Maximum ammo carried

    public float RestingSpread;         // Standard spread
    public float MaxSpread;             // Maximum weapon spread
    public float AdsSpreadReduction;    // How much aiming down the sights reduces spread by
    public float RecoilStrength;        // Percent of maximum spread added per bullet (0-100)
    public float RecenterSpeed;         // Speed at which spread reduces

    public float AdsSpeed;      // Time it takes to aim down the sights
    public float AdsZoomLevel;  // Amount to zoom in when aimed down the sights (1 for no zooming)

    public bool Silenced;   // a silencer has been fitted

    // Art Resources
    public ParticleSystem BulletVisual;
    public ParticleSystem MuzzleFlashVisual;
    public Animation ReloadAnimation;

    #endregion

    #region Variable Stats

    public int Rounds;          // Current number of rounds in magazine
    public float RecoilPercent; // How much recoil is being applied (0-100)

    #endregion

    #region Private Stats

    private float last;
    private float rof; // Calculated delta between shot inputs allowed (Don't set this directly)
    private float rofBurst; // Calculated delta between shots in burst (Don't set this directly)
    private bool safety = true;

    #endregion

    #region Components

    private List<WeaponComponent> Components = new List<WeaponComponent>();
    private List<WeaponComponent> Mods = new List<WeaponComponent>();

    public WeaponComponent BodyComponent { get { return GetWeaponComponent(ComponentType.Body); } }
    public WeaponComponent FiringComponent { get { return GetWeaponComponent(ComponentType.Firing); } }
    public WeaponComponent BarrelComponent { get { return GetWeaponComponent(ComponentType.Barrel); } }
    public WeaponComponent StockComponent { get { return GetWeaponComponent(ComponentType.Stock); } }
    public WeaponComponent GripComponent { get { return GetWeaponComponent(ComponentType.Grip); } }
    public WeaponComponent ScopeComponent { get { return GetWeaponComponent(ComponentType.Scope); } }

    /* Oh what a wonderful world it would be
    public WeaponComponent BodyComponent => GetWeaponComponent(ComponentType.Body);
    public WeaponComponent FiringComponent => GetWeaponComponent(ComponentType.Firing);
    public WeaponComponent BarrelComponent => GetWeaponComponent(ComponentType.Barrel);
    public WeaponComponent StockComponent => GetWeaponComponent(ComponentType.Stock);
    public WeaponComponent GripComponent => GetWeaponComponent(ComponentType.Grip);
    public WeaponComponent ScopeComponent => GetWeaponComponent(ComponentType.Scope);
    */

    #endregion

    // Use this for initialization
    void Start()
    {
        bool i = ComponentStore.init;

        Rounds = 5;
        FullAuto = false;
        BurstSize = 3;
        BurstSpeed = 0.13f;
        RateOfFire = 60f;
        MagSize = 12;
        //fireAnimation = Resources.Load<ParticleSystem>("Particle System Name");

        gameObject.AddComponent<UIManager>();

        //Init("1,2,3,4,5", "501,1508,5502"); // Comma Separated Component IDs, Comma Separated Mod IDs
    }

    public void Init(string inComponents, string inMods)
    {
        Components.Clear();
        Mods.Clear();

        List<double> componentIDs = inComponents.Split(',').ToList().ConvertAll(i => double.Parse(i));
        Components.AddRange(ComponentStore.AllWeaponComponents.FindAll(cmp => componentIDs.Contains(cmp.ID)));

        List<double> modIDs = inMods.Split(',').ToList().ConvertAll(i => double.Parse(i));
        Mods.AddRange(ComponentStore.AllWeaponComponents.FindAll(cmp => modIDs.Contains(cmp.ID)));

        CalculateValues();
    }
    public void CalculateValues()
    {
        Components.ForEachNotNull(wc => wc.Attach(this));
        Components.ForEachNotNull(wc => wc.Init());
        Components.ForEachNotNull(wc => wc.SetBaseValues());
        Components.ForEachNotNull(wc => wc.ApplyModifiers());

        Mods.ForEachNotNull(m => m.Attach(this));
        Mods.ForEachNotNull(m => m.Init());
        Mods.ForEachNotNull(m => m.SetBaseValues());
        Mods.ForEachNotNull(m => m.ApplyModifiers());
    }

    // Update is called once per frame
    void Update()
    {
        if (!safety && FullAuto ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1") && rof + rof < Time.time)
            for (float i = 0f; i < BurstSize; i += 1.0f)
                Invoke("Fire", rofBurst * i);

        Components.ForEachNotNull(c => c.Update());
        Mods.ForEachNotNull(m => m.Update());
    }

    private void Fire()
    {
        if (Rounds <= 0)
        {
            Debug.LogFormat("%s out of ammo", gameObject.name);
            CancelInvoke();
            Reload();
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            Debug.LogFormat(name + " hit %s for %d damage", hit.collider.name, BulletDamage);
        }
        else
        {
            Debug.Log(name + " missed");
        }
        last = Time.time;
        Rounds--;
        // fireAnimation.Emit(1);
        // Play Sound
    }

    private void Reload()
    {
        Debug.Log(name + " reloading!");
        last = Time.time + ReloadTime;
        Rounds = MagSize;
    }

    public void SetComponent(double inID)
    {
        var component = ComponentStore.AllWeaponComponents.Find(cmp => cmp.ID == inID);

        if (inID % 1000 < 500)
        {
            Components.Remove(Components.Find(cmp => cmp.ComponentType == component.ComponentType));
            Components.Add(component);
        }
        else if (!Mods.Exists(c => c.ID == inID))
            Mods.Add(component);

        Debug.Log("Setting Component: " + component.Name +
            "\nComponents: " + GetComponentsNameString() + " Mods: " + GetComponentsNameString(true));

        CalculateValues();
    }

    public void RemoveComponent(double inID)
    {
        var component = ComponentStore.AllWeaponComponents.Find(cmp => Equals(cmp.ID, inID));

        if (inID % 1000 < 500)
        {
            Components.Remove(Components.Find(cmp => Equals(cmp.ID, inID)));
        }
        else if (Mods.Exists(c => Equals(c.ID, inID)))
            Mods.Remove(component);

        CalculateValues();
    }

    WeaponComponent GetWeaponComponent(ComponentType inType)
    {
        return Components.FirstOrDefault(wc => wc.ComponentType == inType);
    }

    string GetComponentsIDString(bool mod = false)
    {
        string s = "";
        (mod ? Mods : Components).ForEachNotNull(c => s += c.ID + ",");
        return s;
    }
    string GetComponentsNameString(bool mod = false)
    {
        string s = "";
        (mod ? Mods : Components).ForEachNotNull(c => s += c.Name + ", ");
        return s;
    }
}

public enum ComponentType
{
    Body, Firing, Barrel, Stock, Grip, Scope, Count
}

[Flags]
public enum BodyType
{
    Pistol = 1 << 0,
    Short = 1 << 1,
    Medium = 1 << 2,
    Long = 1 << 3,
    None = 0,
    All = Pistol | Short | Medium | Long
}
public enum Quality { Broken, Common, Special, Rare, Epic, Legendary, Count } // Component Stats are multiplied by .25 * quality
public enum ScopeType { Iron, Close, Mid, Far, VeryFar, Special }
public enum AmmoType { }
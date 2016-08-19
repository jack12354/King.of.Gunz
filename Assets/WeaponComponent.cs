using System;
using UnityEngine;
using System.Collections;

public partial class WeaponComponent
{
    #region Component Variables

    public double ID;                     // Unique ID (Per Part)
    public ComponentType ComponentType;
    public BodyType BodyType;           // What Types it's designed for
    public GameObject Model;            // A prefab representing the model of the Component
    public string Name;                 // The Pretty Name of the Component
    public string FullName              // The pretty name of the component with quality
    { get { return Quality  + Name; } } 
    public bool Mod;                    // Is this a weapon mod
    public Quality Quality;             // Component Quality

    public string ActivateButton;       // What keybinding Activates this?

    #endregion

    #region Base Stats

    public bool FullAuto = false;   // This weapon is fully automatic
    public int BurstSize = 1;       // Size of Burst ( 1 for single shot weapons )
    public float BurstSpeed = 1;    // Rounds Per Minute of shots in burst

    public float BulletDamage = 1;      // How much damage one bullet does (sans modifiers)
    public int BulletsPerShot = 1;  // How many bullets are fired per round ( >1 for shot		guns )
    public float RateOfFire = 1;    // Rounds Per Minute (or Bursts)

    public float ReloadTime = 2;    // Reload Speed
    public int MagSize = 12;        // Size of a full magazine
    public int MaxAmmo = 96;        // Maximum ammo carried

    public float RestingSpread = 5;         // Standard spread (degree deviation)
    public float MaxSpread = 30;            // Maximum weapon spread
    public float AdsSpreadReduction = 50;   // How much aiming down the sights reduces spread by (0-100)
    public float RecoilStrength = 10;       // Percent of maximum spread added per bullet (0-100)
    public float RecenterSpeed = 0.5f;      // Speed at which spread reduces ( percent per second)

    public float AdsSpeed = 0.25f;      // Time it takes to aim down the sights
    public float AdsZoomLevel = 1.1f;   // Amount to zoom in when aimed down the sights (1 for no zooming)

    public bool Silenced = false;       // a silencer has been fitted

    // Art Resources
    public ParticleSystem BulletVisual;
    public ParticleSystem MuzzleFlashVisual;
    public Animation ReloadAnimation;

    #endregion

    #region Stat Modifiers

    public int BurstSizeModifier = 0;           // Rounds added to the burst
    public float BurstSpeedModifier = 1.0f;     // Change to burst shot speed

    public float BulletDamageModifier = 1.0f;   // How much damage one bullet does
    public float RateOfFireModifier = 1.0f;     // Rounds Per Minute (or Bursts)

    public float ReloadTimeModifier = 1.0f;     // Reload Speed
    public int MagSizeModifier = 0;             // Rounds added to a full magazine
    public float MaxAmmoModifier = 1.0f;        // Maximum ammo carried

    public float RestingSpreadModifier = 1.0f;      // Standard spread
    public float MaxSpreadModifier = 1.0f;          // Maximum weapon spread
    public float AdsSpreadReductionModifier = 1.0f; // How much aiming down the sights reduces spread by (0-100)
    public float RecoilStrengthModifier = 1.0f;     // Percent of maximum spread added per bullet (0-100)
    public float RecenterSpeedModifier = 1.0f;      // Speed at which spread reduces

    public float AdsSpeedModifier = 1.0f;      // Time it takes to aim down the sights

    #endregion

    private Gun gun;
    private bool isBaseSet = false;
    private bool isApplied = false;

    private bool activates = false;
    private ActivationComponent activationComponent;
    // Use this for initialization

    public void Attach(Gun inGun)
    {
        gun = inGun;
    }

    public void SetBaseValues()
    {
        if ((gun == null || isApplied) || isBaseSet) return;
        isBaseSet = true;

        if (FullAuto != false) gun.FullAuto = FullAuto;
        if (BurstSize != 1) gun.BurstSize = BurstSize;
        if (BurstSpeed != 1) gun.BurstSpeed = BurstSpeed;
        if (BulletDamage != 1) gun.BulletDamage = BulletDamage;
        if (BulletsPerShot != 1) gun.BulletsPerShot = BulletsPerShot;
        if (RateOfFire != 1) gun.RateOfFire = RateOfFire;
        if (ReloadTime != 2) gun.ReloadTime = ReloadTime;
        if (MagSize != 12) gun.MagSize = MagSize;
        if (MaxAmmo != 96) gun.MaxAmmo = MaxAmmo;
        if (RestingSpread != 5) gun.RestingSpread = RestingSpread;
        if (MaxSpread != 30) gun.MaxSpread = MaxSpread;
        if (AdsSpreadReduction != 50) gun.AdsSpreadReduction = AdsSpreadReduction;
        if (RecoilStrength != 10) gun.RecoilStrength = RecoilStrength;
        if (RecenterSpeed != 0.5f) gun.RecenterSpeed = RecenterSpeed;
        if (AdsSpeed != 0.25f) gun.AdsSpeed = AdsSpeed;
        if (AdsZoomLevel != 1.1f) gun.AdsZoomLevel = AdsZoomLevel;
        if (Silenced != false) gun.Silenced = Silenced;

    }

    public void ApplyModifiers()
    {
        if ((gun == null || isApplied) || !isBaseSet) return;
        isApplied = true;

        if (BurstSizeModifier == 0) gun.BurstSize += BurstSizeModifier;
        if (BurstSpeedModifier == 1.0f) gun.BurstSpeed *= BurstSpeedModifier;
        if (BulletDamageModifier == 1.0f) gun.BulletDamage *= BulletDamageModifier;
        if (RateOfFireModifier == 1.0f) gun.RateOfFire *= RateOfFireModifier;
        if (ReloadTimeModifier == 1.0f) gun.ReloadTime *= ReloadTimeModifier;
        if (MagSizeModifier == 0) gun.MagSize += MagSizeModifier;
        if (MaxAmmoModifier == 1.0f) gun.MaxAmmo += (int)(-gun.MaxAmmo + (gun.MaxAmmo * MaxAmmoModifier));
        if (RestingSpreadModifier == 1.0f) gun.RestingSpread *= RestingSpreadModifier;
        if (MaxSpreadModifier == 1.0f) gun.MaxSpread *= MaxSpreadModifier;
        if (AdsSpreadReductionModifier == 1.0f) gun.AdsSpreadReduction *= AdsSpreadReductionModifier;
        if (RecoilStrengthModifier == 1.0f) gun.RecoilStrength *= RecoilStrengthModifier;
        if (RecenterSpeedModifier == 1.0f) gun.RecenterSpeed *= RecenterSpeedModifier;
        if (AdsSpeedModifier == 1.0f) gun.AdsSpeed *= AdsSpeedModifier;
    }

    public void Activate()
    {
        activationComponent.gameObject.SetActive(!activationComponent.isActiveAndEnabled);
    }

    public void Init()
    {
        if (!string.IsNullOrEmpty(ActivateButton))
        {
            activates = true;
            activationComponent = Model.GetComponent<ActivationComponent>();
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (activates && Input.GetButtonDown(ActivateButton))
            Activate();
    }
}
public class ActivationComponent : MonoBehaviour { /* Dummy Component for tagging */ }

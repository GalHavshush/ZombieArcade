//using System;
//using System.Net.Http.Headers;
//using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HudManager : MonoBehaviour
{
    public static HudManager Instance { get; set; }

    [Header("Ammo")]
    [SerializeField] public TextMeshProUGUI magazineAmmoUI;
    [SerializeField] public TextMeshProUGUI totalAmmoUI;
    [SerializeField] public UnityEngine.UI.Image ammoTypeUI;

    [Header("Weapon")]
    [SerializeField] public UnityEngine.UI.Image activeWeaponUI;
    [SerializeField] public UnityEngine.UI.Image unActiveWeaponUI;

    [Header("Throwables")]
    [SerializeField] public UnityEngine.UI.Image lethalUI;
    [SerializeField] public TextMeshProUGUI lethalAmountUI;

    [SerializeField] public UnityEngine.UI.Image tacticalUI;
    [SerializeField] public TextMeshProUGUI tacticalAmountUI;


    public Sprite emptySlot;
    public Sprite greySlot;

    public GameObject middleDot;



    private void Awake()
    {
        if(Instance != null && Instance != this){
            Destroy(gameObject);
        }else{
            Instance = this;
        }
    }

    public void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = getUnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if(activeWeapon){
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft/activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";

            Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = getAmmoSprite(model);

            activeWeaponUI.sprite = getWeaponSprite(model);

            if(unActiveWeapon){
                unActiveWeaponUI.sprite = getWeaponSprite(unActiveWeapon.thisWeaponModel);
            }

        }else{
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;

            activeWeaponUI.sprite = emptySlot;
            unActiveWeaponUI.sprite = emptySlot;
        }

        if(WeaponManager.Instance.lethalsCount <= 0){
            lethalUI.sprite = greySlot;
        }

        if(WeaponManager.Instance.tacticalCount <= 0){
            tacticalUI.sprite = greySlot;
        }
    }

    private Sprite getWeaponSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Pistol:
                return Resources.Load<GameObject>("Pistol_Weapon").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.Rifle:
                return Resources.Load<GameObject>("Rifle_Weapon").GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }

    private Sprite getAmmoSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {

            case Weapon.WeaponModel.Pistol:
                return Resources.Load<GameObject>("Pistol_Ammo").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.Rifle:
                return Resources.Load<GameObject>("Rifle_Ammo").GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }

    private GameObject getUnActiveWeaponSlot()
    {
        foreach(GameObject weaponSlot in WeaponManager.Instance.weaponSlots){
            if(weaponSlot != WeaponManager.Instance.activeWeaponSlot){
                return weaponSlot;
            }
        }

        return null;
    }

    internal void UpdateThrowablesUI()
    {

        lethalAmountUI.text = $"{WeaponManager.Instance.lethalsCount}";
        tacticalAmountUI.text = $"{WeaponManager.Instance.tacticalCount}";

        switch(WeaponManager.Instance.equippedLethalType)
        {
            case Throwable.ThrowableType.Grenade:
                lethalUI.sprite = Resources.Load<GameObject>("Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }

        switch(WeaponManager.Instance.equippedTacticalType)
        {
            case Throwable.ThrowableType.Smoke_Grenade:
                tacticalUI.sprite = Resources.Load<GameObject>("Smoke_Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }
}

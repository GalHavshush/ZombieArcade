using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;
    public int weaponDamage;


    [Header("Shooting")]
    // Shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    [Header("Burst")]
    // Burst
    public int bulletsPerBurst = 3;
    public int burstBulletLeft;

    [Header("Spread")]
    // Spread
    public float spreadIntensity;
    public float hipSpreadIntensity;
    public float adsSpreadIntensity;

    [Header("Bullet")]
    // Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 3f;// seconds

    public GameObject muzzleEffect;

    internal Animator animator;

    [Header("Loading")]
    // Loading
    public float reloadTime;
    public int MagazineSize, bulletsLeft;
    public bool isReloading;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    bool isADS;

    public enum WeaponModel{
        Pistol,
        Rifle
    }

    public WeaponModel thisWeaponModel;



    public enum ShootingMode{
        Single,
        Burst,
        Auto
    }



    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = MagazineSize;

        spreadIntensity = hipSpreadIntensity;
    }


    void Update(){
        if (isActiveWeapon)
        {


            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("WeaponRender");
            }
            transform.gameObject.layer = LayerMask.NameToLayer("WeaponRender");

            if (Input.GetMouseButtonDown(1))
            {
                EnterADS();
            }

            if (Input.GetMouseButtonUp(1))
            {
                ExitADS();
            }



            GetComponent<Outline>().enabled = false;


            if (bulletsLeft == 0 && isShooting)
            {
                SoundManager.Instance.emptyMagazinePistol.Play();
            }



            if (currentShootingMode == ShootingMode.Auto)
            {
                // Holding down the left mouse button
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single)
            {
                // single mouse button click 
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }


            // Manual reload pressing R key
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < MagazineSize && !isReloading && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                Reload();
            }

            // // Auto reload
            // if(readyToShoot && !isShooting && !isReloading && bulletsLeft <= 0){
            //     Reload();
            // }



            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletLeft = bulletsPerBurst;
                FireWeapon();
            }



        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }
            
            transform.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    private void EnterADS()
    {
        animator.SetTrigger("enterADS");
        isADS = true;
        HudManager.Instance.middleDot.SetActive(false);
        spreadIntensity = adsSpreadIntensity;
    }
    private void ExitADS()
    {
        animator.SetTrigger("exitADS");
        isADS = false;
        HudManager.Instance.middleDot.SetActive(true);
        spreadIntensity = hipSpreadIntensity;
    }

    private void FireWeapon(){

        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();

        if (isADS)
        {
            animator.SetTrigger("RECOIL_ADS");

        }else{
            animator.SetTrigger("RECOIL");

        }

        SoundManager.Instance.PlayShootingSound(thisWeaponModel);


        readyToShoot = false;

        Vector3 shootingDirection = calculateDirectionAndSpread().normalized;


        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, quaternion.identity);

        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;


        // Point the bullet to face the shooting direction
        bullet.transform.forward = shootingDirection;

        // Shoot the bullet
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        // Destroy the bullet(after seconds)
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        // Check if done shooting
        if(allowReset){
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        // Burst mode
        if(currentShootingMode == ShootingMode.Burst && burstBulletLeft > 1){
            burstBulletLeft--;
            Invoke("FireWeapon", shootingDelay);
        }

    }

    private void Reload(){

        //SoundManager.Instance.reloadingSoundPistol.Play();
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);

        animator.SetTrigger("RELOAD");

        isReloading = true;
        Invoke("ReloadCompleted",reloadTime);
    }
    
    private void ReloadCompleted(){
        
        if(WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > MagazineSize){
            bulletsLeft = MagazineSize;
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }else{
            bulletsLeft = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }



        isReloading = false;
    }

    private void ResetShot(){
        readyToShoot = true;
        allowReset = true;
    }


    public Vector3 calculateDirectionAndSpread(){
        // Shooting from the middle of the screen to check where we are pointing at
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit)){
            // Hitting something
            targetPoint = hit.point;
        }else{
            // Shooting at the air
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;
        float Z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float Y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        // Returning the shooting direction and spread
        return direction + new Vector3(0,Y,Z);

    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay){
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }


}




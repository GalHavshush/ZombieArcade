using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }


    public AudioClip rifleShot;

    public AudioSource shootingSoundPistol;
    public AudioSource reloadingSoundPistol;
    public AudioSource emptyMagazinePistol;

    public AudioSource shootingSoundRifle;
    public AudioSource reloadingSoundRifle;

    public AudioSource throwablesChannel;
    public AudioClip grenadeSound;

    public AudioClip zombieWalking;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;
    public AudioClip zombieDeath;

    public AudioSource zombieChannel;
    public AudioSource zombieChannel2;

    public AudioSource playerChannel;
    public AudioClip playerHurt;
    public AudioClip playerDie;
    public AudioClip gameOverMusic;





    private void Awake()
    {
        if(Instance != null && Instance != this){
            Destroy(gameObject);
        }else{
            Instance = this;
        }
    }

    public void PlayShootingSound(WeaponModel weapon){
        if(weapon == WeaponModel.Pistol){
            shootingSoundPistol.Play();
        }else if(weapon == WeaponModel.Rifle){
            shootingSoundRifle.PlayOneShot(rifleShot); // so sounds wont override
        }
    }

    public void PlayReloadSound(WeaponModel weapon){
        if(weapon == WeaponModel.Pistol){
            reloadingSoundPistol.Play();
        }else if(weapon == WeaponModel.Rifle){
            reloadingSoundRifle.Play(); // same sound
        }
    }
}

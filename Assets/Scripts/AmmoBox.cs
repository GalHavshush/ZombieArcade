using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int ammoAmount = 0;
    public AmmoType ammoType;

    public enum AmmoType{
        RifleAmmo,
        PistolAmmo
    }
}

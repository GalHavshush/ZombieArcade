using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    public Weapon hoverWeapon = null;
    public AmmoBox hoveredAmmoBox = null;

    public Throwable hoveredThrowable = null;


    private void Awake()
    {
        if(Instance != null && Instance != this){
            Destroy(gameObject);
        }else{
            Instance = this;
        }
    }

    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit)){
            GameObject objectHitByRaycast = hit.transform.gameObject;

            if(objectHitByRaycast.GetComponent<Weapon>() && objectHitByRaycast.GetComponent<Weapon>().isActiveWeapon == false){

                if(hoverWeapon){
                    hoverWeapon.GetComponent<Outline>().enabled = false;
                }


                hoverWeapon = objectHitByRaycast.gameObject.GetComponent<Weapon>();
                hoverWeapon.GetComponent<Outline>().enabled = true;

                if(Input.GetKeyDown(KeyCode.F)){
                    WeaponManager.Instance.PickupWeapon(objectHitByRaycast.gameObject);
                }

            }else{
                if(hoverWeapon){
                    hoverWeapon.GetComponent<Outline>().enabled = false;
                }
            }


            // AmmoBox
            if(objectHitByRaycast.GetComponent<AmmoBox>()){

                if(hoveredAmmoBox){
                    hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                }

                hoveredAmmoBox = objectHitByRaycast.gameObject.GetComponent<AmmoBox>();
                hoveredAmmoBox.GetComponent<Outline>().enabled = true;

                if(Input.GetKeyDown(KeyCode.F)){
                    WeaponManager.Instance.PickupAmmo(hoveredAmmoBox);
                    Destroy(objectHitByRaycast.transform.parent.gameObject);
                }

            }else{
                if(hoveredAmmoBox){
                    hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                }
            }


            // Throwable
            if(objectHitByRaycast.GetComponent<Throwable>()){

                if(hoveredThrowable){
                    hoveredThrowable.GetComponent<Outline>().enabled = false;
                }
                

                hoveredThrowable = objectHitByRaycast.gameObject.GetComponent<Throwable>();
                hoveredThrowable.GetComponent<Outline>().enabled = true;

                if(Input.GetKeyDown(KeyCode.F)){
                    WeaponManager.Instance.PickupThrowable(hoveredThrowable);
                }

            }else{
                if(hoveredThrowable){
                    hoveredThrowable.GetComponent<Outline>().enabled = false;
                }
            }







        }
        
    }
}

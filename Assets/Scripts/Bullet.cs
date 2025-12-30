using System;
using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int bulletDamage;

    private void OnCollisionEnter(Collision objectWeHit)
    {
        if(objectWeHit.gameObject.CompareTag("Target")){
            print("hit " + objectWeHit.gameObject.name + "!");

            CreateBulletImpactEffect(objectWeHit);

            Destroy(gameObject);
        }

        if(objectWeHit.gameObject.CompareTag("Wall")){
            print("hit a wall!");

            CreateBulletImpactEffect(objectWeHit);

            Destroy(gameObject);
        }

        if(objectWeHit.gameObject.CompareTag("BeerBottle")){
            print("hit a beer bottle!");

            objectWeHit.gameObject.GetComponent<BeerBottle>().Shatter();

            //bullet will be destroyed using its lifetime
        }

        if(objectWeHit.gameObject.CompareTag("Enemy")){

            if(objectWeHit.gameObject.GetComponent<Enemy>().isDead == false)
            {
                objectWeHit.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
            }

            CreateBloodSprayEffect(objectWeHit);

            Destroy(gameObject);
        }
    }

    private void CreateBloodSprayEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        GameObject bloodSprayPrefab = Instantiate(
            GlobalRefrences.Instance.bloodSprayEffect,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );

        bloodSprayPrefab.transform.SetParent(objectWeHit.gameObject.transform);
    }
    

    void CreateBulletImpactEffect(Collision objectWeHit){
        ContactPoint contact = objectWeHit.contacts[0];

        GameObject hole = Instantiate(
            GlobalRefrences.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );

        hole.transform.SetParent(objectWeHit.gameObject.transform);
    }

}

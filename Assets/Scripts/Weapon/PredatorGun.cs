﻿using UnityEngine;

public class PredatorGun : WeaponRangedProjectile
{
    private float thrust = 12f;

    public void FireAtTarget(Character character, Transform target)
    {
        this.fireFrame = Time.frameCount;

        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();

        Quaternion rot = Quaternion.LookRotation(dir);

        GameObject projectile = GameObject.Instantiate(projectilePrefab, projectileEjector.transform.position, rot);
        projectile.GetComponent<Projectile>().Damage = damage;
        Rigidbody rigidBody = projectile.GetComponent<Rigidbody>();

        rigidBody.AddForce(dir * thrust, ForceMode.Impulse);

        Destroy(projectile, 3f);
    }


}
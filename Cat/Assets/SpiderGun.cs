using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderGun : MonoBehaviour
{
    public GameObject bullet;

    public float shootForce;
    public float fireRate, spread, reloadTime, timeBetweenShots;
    public int magSize, bulletsPerTap;
    public bool fullAuto;
    

    int bulletsLeft, bulletsShot;

    bool readyToShoot, reloading;

    public GameObject target;
    public Transform attackPoint;
    public GameObject muzzleFlash;

    public GameObject barrel;
    public float rotateBarrel = 0;
    public float OriginalRotate = 50;

    public bool invokeAllowed = true;

    float timeSinceLastShot;
    bool attack = false;

    public GameObject shell;
    public GameObject shellEjectPosition;

    void start()
    {
        readyToShoot = true;
        reloading = false;
        
        bulletsLeft = magSize;
    }


    void Update()
    {
        if (GameManager.instance.timeManager.GetComponent<timeManager>().isPaused ) return;
        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot > timeBetweenShots) readyToShoot = true;
        if (readyToShoot && !reloading && bulletsLeft > 0)
        { 
            Shoot();
        }
        if (bulletsLeft <= 0) Reload();
        if (!reloading) RotateGun();
        else stopRotation();
    }

    public void Shoot()
    {
        readyToShoot = false;

        Vector3 targetPoint = target.transform.position;

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        float xSpread = Random.Range(-spread, spread);
        float ySpread = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(xSpread, ySpread);

        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;

        currentBullet.SetActive(true);

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        if (muzzleFlash != null)
        {
            GameObject currentMuzzleFlash = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
            currentMuzzleFlash.transform.SetParent(attackPoint.transform, true);
            currentMuzzleFlash.transform.rotation = new Quaternion(0, 0, 0, 0);
            Destroy(currentMuzzleFlash, 0.1f);
        }


        bulletsLeft--;
        bulletsShot++;
        timeSinceLastShot = 0;

        if (shell != null)
        {
            float randomized = Random.Range(5, 12);
            GameObject currentShell = Instantiate(shell, shellEjectPosition.transform.position, Quaternion.identity);
            currentShell.active = true;
            currentShell.GetComponent<Rigidbody>().AddForce(-shellEjectPosition.transform.right.normalized * randomized, ForceMode.Impulse);
            Destroy(currentShell, 20f);
        }
        if (invokeAllowed)
        {
            Invoke("ResetShot", timeBetweenShots);

            invokeAllowed = false;
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        invokeAllowed = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magSize;
        reloading = false;
    }

    private void RotateGun()
    {
        if (rotateBarrel > OriginalRotate) rotateBarrel = OriginalRotate;
        else rotateBarrel++;
        barrel.transform.Rotate(0, 0, rotateBarrel);
    }
    private void stopRotation()
    {
        if (rotateBarrel < 0) rotateBarrel = 0;
        else rotateBarrel -= 1 * Time.deltaTime * 20f;
        barrel.transform.Rotate(0, 0, rotateBarrel);
    }

    public void isAttacking(bool isattack) { attack = isattack; }
}

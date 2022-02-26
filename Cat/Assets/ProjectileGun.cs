using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProjectileGun : MonoBehaviour
{

    public GameObject bullet;

    public float shootForce, upwardForce;
    public float fireRate, spread, reloadTime, timeBetweenShots;
    public int magSize, bulletsPerTap;
    public bool fullAuto;

    int bulletsLeft, bulletsShot;

    bool shooting, readyToShoot, reloading;

    public Rigidbody playerRB;
    public float recoilForce;

    public Camera cam;
    public Transform attackPoint;
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    public bool invokeAllowed = true;

    void Awake()
    {
        readyToShoot = true;
        bulletsLeft = magSize;
    }


    void Update()
    {
        PlayerInput();

        if(ammunitionDisplay != null)
        {
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magSize / bulletsPerTap);
        }
    }

    public void PlayerInput()
    {
        if (fullAuto) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if((Input.GetKey(KeyCode.R) && bulletsLeft < magSize && !reloading) || (readyToShoot && shooting && !reloading && bulletsLeft <= 0)) Reload();

        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;

            Shoot();
        }
    }

    public void Shoot()
    {
        readyToShoot = false;


        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))targetPoint = hit.point;
        else targetPoint = ray.GetPoint(75);
        

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        float xSpread = Random.Range(-spread, spread);
        float ySpread = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(xSpread, ySpread);

        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;
        currentBullet.SetActive(true);

        playerRB.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(cam.transform.up * upwardForce, ForceMode.Impulse);

        if (muzzleFlash != null) Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);



        bulletsLeft--;
        bulletsShot++;

        if (invokeAllowed)
        {
            Invoke("ResetShot", timeBetweenShots);

            invokeAllowed = false;
        }

        if(bulletsShot < bulletsPerTap && bulletsLeft > 0)
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
}

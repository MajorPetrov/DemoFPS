using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Weapon : MonoBehaviour
{
    public int ammoPerMag = 30; // Nombre de balles par chargeur
    public int ammoLeft = 120; // Munitions en réserve
    public int currentAmmo = 30; // Nombre de munitions actuellement dans le chargeur
    public float fireRate = 0.1f; // Temps entre chaque tir
    public float range = 100.0f; // Portée du tir
    public float damage = 20.0f;
    public enum ShootMode { Auto, Semi }
    public ShootMode shootMode;
    public Transform shootPoint; // point de départ du tir
    public GameObject hitParticles;
    public GameObject bulletImpact;
    public ParticleSystem muzzleFlash;
    public AudioClip shootSound;
    public AudioClip magout;
    public AudioClip magin;
    public AudioClip foldback;
    public AudioClip draw;
    public GameObject HUD;
    public GameObject UI_HUD;

    private float _fireTimer; // Temps avant un nouveau tir
    private bool _isReloading;
    private bool _shootInput;
    private Animator _animator;
    private AudioSource _audioSource;
    private FirstPersonController _firstPersonController;
    private GameObject _HUDInstance;
    private GameObject _UI_HUD;
    private Text _ammoText;

    // Use this for initialization
    void Start()
    {
        currentAmmo = ammoPerMag;
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _firstPersonController = transform.parent.transform.parent.transform.parent.GetComponent<FirstPersonController>();

        // Instantiation de l'ATH pour le joueur
        CrosshairActivation();
    }

    // Update is called once per frame
    void Update()
    {
        switch (shootMode)
        {
            case ShootMode.Auto:
                _shootInput = Input.GetButton("Fire1");
                break;
            case ShootMode.Semi:
                _shootInput = Input.GetButtonDown("Fire1");
                break;
        }

        // Tirer
        if (_shootInput && !Input.GetButton("Sprint") && !Input.GetButton("Reload") && currentAmmo > 0 && !_isReloading)
            Fire();

        // Contrôle de la cadence de tir
        if (_fireTimer < fireRate)
            _fireTimer += Time.deltaTime;

        // Rechargement manuel
        if (Input.GetButtonDown("Reload") && !Input.GetButton("Sprint") && !Input.GetButton("Fire1") && currentAmmo < ammoPerMag && ammoLeft > 0)
            DoReload();

        // Rechargement automatique si on tire mais que le chargeur est vide
        if (Input.GetButton("Fire1") && !Input.GetButton("Sprint") && !Input.GetButton("Reload") && currentAmmo <= 0 && ammoLeft > 0 && !_isReloading)
            DoReload();

        // Visée
        if (Input.GetButtonDown("Aim") && !Input.GetButton("Sprint") && !Input.GetButton("Reload") && !_isReloading)
            Aim();

        // S'accroupir
        if (Input.GetButtonDown("Crouch"))
            Crouch();
    }

    private void FixedUpdate()
    {
        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0); // 0 pour le premier layer
        _isReloading = info.IsName("Reload");

        if (info.IsName("Fire"))
            _animator.SetBool("Fire", false);

        if (info.IsName("Aiming"))
            _animator.SetBool("Aiming", false);
    }

    private void OnEnable()
    {
        _UI_HUD = Instantiate(UI_HUD);
        _UI_HUD.name = UI_HUD.name;
        _ammoText = _UI_HUD.GetComponentInChildren<Text>();
        UpdateAmmoText();
    }

    private void OnDisable()
    {
        Destroy(_UI_HUD);
    }

    private void Fire()
    {
        if (_fireTimer < fireRate)
            return;

        RaycastHit hit;

        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name + " trouvé");

            GameObject hitParticleEffect = Instantiate(hitParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            GameObject bulletHole = Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

            // J'attache les particules et les impacts à leur element parent, se sorte à ne pas avoir des trucs qui flottent dans le vide
            hitParticleEffect.transform.SetParent(hit.transform);
            bulletHole.transform.SetParent(hit.transform);

            // Destruction des traces des particules et des traces d'impact pour économiser les ressources
            Destroy(hitParticleEffect, 1.0f);
            Destroy(bulletHole, 2.0f);

            if (hit.transform.GetComponent<HealthController>())
                hit.transform.GetComponent<HealthController>().ApplyDamage(damage);
        }

        if (_animator.GetBool("Aiming"))
            _animator.CrossFadeInFixedTime("FireAiming", 0.01f);
        else
            _animator.CrossFadeInFixedTime("Fire", 0.01f);

        muzzleFlash.Play();
        _audioSource.PlayOneShot(shootSound);
        currentAmmo--;

        if (currentAmmo < 0)
            currentAmmo = 0;

        UpdateAmmoText();
        _fireTimer = 0.0f;
    }

    public void Reload()
    {
        int ammoToLoad = ammoPerMag - currentAmmo;
        int ammoToDeduct = (ammoLeft >= ammoToLoad) ? ammoToLoad : ammoLeft;

        ammoLeft -= ammoToDeduct;
        currentAmmo += ammoToDeduct;

        CrosshairActivation();
        UpdateAmmoText();
    }

    private void DoReload()
    {
        // si on est déjà en train de recharger, on stoppe la méthode
        if (_isReloading)
            return;

        _animator.SetBool("Aiming", false);
        Destroy(_HUDInstance);
        _animator.CrossFadeInFixedTime("Reload", 0.01f);
    }

    private void Aim()
    {
        if (_animator.GetBool("Aiming"))
        {
            CrosshairActivation();
            _animator.SetBool("Aiming", false);
            _firstPersonController.WalkSpeed = 5.0f;
        }

        else
        {
            Destroy(_HUDInstance);
            _animator.SetBool("Aiming", true);
            _firstPersonController.WalkSpeed = 3.0f;
        }
    }

    private void Crouch()
    {
        if (_animator.GetBool("Crouch"))
            _animator.SetBool("Crouch", false);
        else
            _animator.SetBool("Crouch", true);
    }

    private void UpdateAmmoText()
    {
        _ammoText.text = currentAmmo + " / " + ammoLeft;
    }

    private void CrosshairActivation()
    {
        _HUDInstance = Instantiate(HUD);
        _HUDInstance.name = HUD.name;
    }

    private void PlayMagoutSound()
    {
        _audioSource.PlayOneShot(magout);
    }

    private void PlayMaginSound()
    {
        _audioSource.PlayOneShot(magin);
    }

    private void PlayFoldbackSound()
    {
        _audioSource.PlayOneShot(foldback);
    }

    private void PlayDrawSound()
    {
        _audioSource.PlayOneShot(draw);
    }
}

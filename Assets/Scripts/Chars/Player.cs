﻿using UnityEngine;

public class Player : GoodGuy, IDamageable
{
    public enum State
    {
        BeingAwesome,
        Dead
    }

    public delegate void EventHandler();
    public delegate void EventHandlerWithHealth(float currentHealth, float maxHealth);
    public event EventHandler OnDamageReceived;
    public event EventHandler OnDeath;
    public event EventHandlerWithHealth OnDamageReceivedHealth;

    [SerializeField]
    private WeaponBase[] weapons;

    private Vector2 inputVec;
    private float yaw;
    private float pitch;

    private Animator _animator;

    public float Yaw { get { return yaw; } }
    public float Pitch { get { return pitch; } }
    public bool IsAiming { get; private set; }
    private WeaponBase CurrentActiveWeapon
    {
        get { return weapons[0]; }
    }

    public Vector3 ShootDirection
    {
        get { return _camera.transform.forward; }
    }

    public Transform backBone;
    private Camera _camera;

    private State currentState = State.BeingAwesome;



    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponentInChildren<Animator>();
        _camera = Camera.main;
    }

    private void Start()
    {
        yaw = transform.rotation.eulerAngles.y % 360;
    }

    protected override void Update()
    {
        PlayerControls();
        base.Update();
    }

    private void LateUpdate()
    {
        //backBone rotation
        Vector3 rot = _camera.transform.rotation.eulerAngles;
        rot.y += 180;
        rot.x *= -1;
        rot.x += 20;
        backBone.eulerAngles = rot;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();

        if (projectile != null && projectile.Source != this)
        {
            ApplyDamage(projectile.Damage);
            Instantiate(projectile.particle, projectile.transform.position, Quaternion.identity);
            Destroy(projectile.gameObject);
        }
    }

    private void PlayerControls()
    {
        if (currentState == State.Dead) return;

        if (Input.GetKey(KeyCode.A)) inputVec.x = -1;
        else if (Input.GetKey(KeyCode.D)) inputVec.x = 1;
        else inputVec.x = 0;

        if (Input.GetKey(KeyCode.S)) inputVec.y = -1;
        else if (Input.GetKey(KeyCode.W)) inputVec.y = 1;
        else inputVec.y = 0;

        if (inputVec.magnitude > 1) inputVec = inputVec.normalized;

        bool isRunningInput = inputVec.sqrMagnitude > float.Epsilon;
        _animator.SetBool("IsRunning", isRunningInput);

        yaw += (Input.GetAxisRaw("Mouse X") * GameSettings.MOUSE_SENSITIVITY);
        yaw %= 360f;

        pitch += (-Input.GetAxisRaw("Mouse Y") * GameSettings.MOUSE_SENSITIVITY);
        pitch = Mathf.Clamp(pitch, -30f, +30f);

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (CurrentActiveWeapon.CanFire())
            {
                GameUI.Instance.CrosshairUI.TriggerFX();
                CurrentActiveWeapon.Fire(this);
            }
        }

        //transform.Rotate(Vector3.up * yaw * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(0, yaw, 0);
        velocity += transform.localRotation * new Vector3(inputVec.x, 0, inputVec.y) * maxSpeed * Time.deltaTime;
    }

    public void ApplyDamage(float dmg)
    {
        if (currentState == State.Dead) return;
        health -= dmg;

        if (OnDamageReceived != null) OnDamageReceivedHealth.Invoke(this.health, this.maxHealth);

        if (health <= 0f) Die();
        if (OnDamageReceived != null) OnDamageReceived.Invoke();
    }

    public void Die()
    {
        currentState = State.Dead;
        _animator.SetBool("IsDead", true);
        GameManager.Instance.HeroDied();
        if (OnDeath != null) OnDeath.Invoke();
    }
}

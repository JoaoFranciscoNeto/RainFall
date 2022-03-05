using System.Collections;
using UnityEngine;

public class FloatyThing : MonoBehaviour
{
    public Transform MovementTarget;
    public Transform RotationTarget;

    public float MovementSpeed;
    public float RotationSpeed;

    public float ChanceToHit = .6f;
    public float MaxDispersion = 2f;

    public GameObject Projectile;

    public float TimeBetweenShots = 5;

    public float FireRate = 5f;
    public int NumberOfPewPews = 10;

    public float ProjectileSpeed = 10f;

    private float _timeToShoot;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _timeToShoot = Random.Range(1, TimeBetweenShots);
    }

    private void Update()
    {
        _timeToShoot -= Time.deltaTime;

        if (_timeToShoot <= 0)
        {
            _timeToShoot = TimeBetweenShots;
            StartCoroutine(Attack());
        }
    }


    private void FixedUpdate()
    {
        Rotate();
        //Move();
    }

    private void Rotate()
    {
        var dir = RotationTarget.position-transform.position;

        var rot = Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(dir, Vector3.up), Time.deltaTime * RotationSpeed);
        _rigidbody.MoveRotation(rot);
    }

    private void Move()
    {
        var dir = MovementTarget.position-transform.position;
        _rigidbody.AddForce(Mathf.Clamp01(dir.magnitude) * MovementSpeed * dir.normalized);
    }


    private IEnumerator Attack()
    {
        for (var i = 0; i < NumberOfPewPews; i++)
        {
            ShootProjectile();
            yield return new WaitForSeconds(1 / FireRate);
        }
    }

    private void ShootProjectile()
    {
        var dispersion = Random.Range(0f, 1f) < ChanceToHit ? Vector2.zero : Random.insideUnitCircle * MaxDispersion;
        var projection = transform.TransformDirection(new Vector3(dispersion.x, dispersion.y, 0));

        var direction = RotationTarget.transform.position+projection-transform.position;

        var projectile = Instantiate(Projectile, transform.position+transform.forward * 2, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().AddForce(direction.normalized * ProjectileSpeed, ForceMode.Impulse);
    }
}
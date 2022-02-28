using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField]
    protected float MaxHitPoints;

    protected float CurrentHitPoints;

    public void ApplyDamage(float amount)
    {
        CurrentHitPoints -= amount;

        if (CurrentHitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        CurrentHitPoints = MaxHitPoints;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}
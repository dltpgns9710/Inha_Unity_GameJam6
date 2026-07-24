using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public sealed class PetCloneTrigger : MonoBehaviour
{
    private static readonly HashSet<PetCloneTrigger> Instances = new HashSet<PetCloneTrigger>();

    private AnomalyPetClone _anomaly;
    private BoxCollider2D _collider;
    private bool _triggered;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _collider.isTrigger = true;
        _collider.enabled = false;
    }

    private void OnEnable() => Instances.Add(this);

    private void OnDisable()
    {
        Instances.Remove(this);
        Close();
    }

    public static int OpenAll(AnomalyPetClone anomaly)
    {
        int armedCount = 0;
        foreach (PetCloneTrigger trigger in Instances)
        {
            if (trigger == null || !trigger.isActiveAndEnabled) continue;
            trigger.Open(anomaly);
            armedCount++;
        }
        return armedCount;
    }

    public static void CloseAll(AnomalyPetClone anomaly)
    {
        foreach (PetCloneTrigger trigger in Instances)
        {
            if (trigger != null && trigger._anomaly == anomaly)
                trigger.Close();
        }
    }

    public void Open(AnomalyPetClone anomaly)
    {
        _anomaly = anomaly;
        _triggered = false;
        _collider.enabled = true;
    }

    public void Close()
    {
        _anomaly = null;
        _triggered = false;
        if (_collider != null)
            _collider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_triggered || _anomaly == null || !other.CompareTag("Player")) return;

        AnomalyPetClone anomaly = _anomaly;
        _triggered = true;
        CloseAll(anomaly);
        anomaly.ShowPets();
    }
}

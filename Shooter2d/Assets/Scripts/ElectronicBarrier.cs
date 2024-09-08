using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronicBarrier : Barrier
{
    public bool needsElectricity;
    public List<ElectricCore> cores;
    public GameObject screen;
    public BoxCollider2D screenCollider;

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (!HasEnergy())
        {
            hpSlider.value = 0;
            if (secondaryHpSlider != null)
            {
                secondaryHpSlider.value = 0;
            }
            Die();
        }
    }

    bool HasEnergy()
    {
        bool energy = false;
        foreach (ElectricCore core in cores)
        {
            if (core.isOn)
            {
                energy = true;
            }
        }
        return energy;
    }

    public override void Die()
    {
        screen.SetActive(false);
        screenCollider.enabled = false;
    }

    private void OnDrawGizmos()
    {
        screenCollider.size = screen.transform.localScale;
    }
}

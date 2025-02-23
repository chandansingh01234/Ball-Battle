using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyManager : MonoBehaviour
{
    public Slider attackerEnergyBar; // Attacker's energy bar
    public Slider defenderEnergyBar;  // Defender's energy bar

    public float energyRegenRate = 0.5f;  // Energy regeneration rate per second
    public float maxEnergy = 6f;         // Maximum energy points

    private float attackerEnergy;           // Attacker's current energy
    private float defenderEnergy;            // Defender's current energy

    public int Attacker_cost = 2;         // Attacker's cost
    public int Defender_cost = 3;         // Defender's cost

    void Start()
    {
        attackerEnergy = 0f;
        defenderEnergy = 0f;

        attackerEnergyBar.maxValue = maxEnergy;
        defenderEnergyBar.maxValue = maxEnergy;

        attackerEnergyBar.value = attackerEnergy;
        defenderEnergyBar.value = defenderEnergy;

    }

    void Update()
    {
        // Regenerate energy for both Attacker and Defender
        if (attackerEnergy < maxEnergy)
        {
            attackerEnergy += energyRegenRate * Time.deltaTime;
            attackerEnergy = Mathf.Clamp(attackerEnergy, 0f, maxEnergy); // Clamp energy to max
            attackerEnergyBar.value = attackerEnergy;
        }

        if (defenderEnergy < maxEnergy)
        {
            defenderEnergy += energyRegenRate * Time.deltaTime;
            defenderEnergy = Mathf.Clamp(defenderEnergy, 0f, maxEnergy); // Clamp energy to max
            defenderEnergyBar.value = defenderEnergy;
        }

    }

    public bool SpendEnergy(bool isAttacker)
    {
        if (isAttacker)
        {
            if (attackerEnergy >= Attacker_cost)
            {
                attackerEnergy -= Attacker_cost;
                attackerEnergyBar.value = attackerEnergy;
                return true;
            }
        }
        else
        {
            if (defenderEnergy >= Defender_cost)
            {
                defenderEnergy -= Defender_cost;
                defenderEnergyBar.value = defenderEnergy;
                return true;
            }
        }
        return false;
    }

  
}
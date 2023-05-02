using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum Skills
{
    AlwaysAvailable,
    Egsorcism,
    Speed,
    SpeedA,
    SpeedB,
    SpeedC,
    Dash,
    DashA,
    DashB,
    DashC,
    KillBoot,
    KillBoostA,
    KillBoostB,
    KillBoostC,
    Enlightenment,
    EnlightenmentA,
    EnlightenmentB,
    EnlightenmentC,
    HolyMissile,
    HolyMissileA,
    HolyMissileB,
    HolyMissileC,
    Conversion,
    ConversionA,
    ConversionB,
    ConversionC
}


public sealed class SkillsScript
{

    PlayerController player;

    List<Skills> unlockedSkills;

    public Dictionary<Skills, Action> skillUpgrades;

    public SkillsScript()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        unlockedSkills = new List<Skills>
        {
            Skills.AlwaysAvailable
        };

        skillUpgrades = new Dictionary<Skills, Action>
        {
            { Skills.Speed, Speed },
            { Skills.SpeedA, SpeedA },
            { Skills.SpeedB, SpeedB },
            { Skills.SpeedC, SpeedC },
            { Skills.Dash, Dash },
            { Skills.DashA, DashA },
            { Skills.DashB, DashB },
            { Skills.DashC, DashC },
            {Skills.KillBoot, KillBoost }
        };
    }

    public void UnlockSkill(Skills skill)
    {
        unlockedSkills.Add(skill);
        skillUpgrades[skill]();
    }

    public bool IsSkillUnlocked(Skills skill)
    {
        return unlockedSkills.Contains(skill);
    }

    void KillBoost()
    {

    }
    void Speed()
    {
        player.AddSpeed(1);
    }
    void SpeedA()
    {
        player.AddSpeed(0.5f);
    }
    void SpeedB()
    {
        player.AddSpeed(0.5f);
    }
    void SpeedC()
    {
        player.AddSpeed(0.3f);
    }

    void Dash()
    {

    }
    void DashA()
    {
        player.ChangeDashTime(0.15f);
    }
    void DashB()
    {
        player.ChangeDashCooldown(-0.15f);
    }
    void DashC()
    {
        player.ChangeDashTime(0.1f);
        player.ChangeDashCooldown(-0.1f);
    }
}

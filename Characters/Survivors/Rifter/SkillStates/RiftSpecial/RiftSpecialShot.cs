

using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using System;
using IL.RoR2.Skills;

namespace RifterMod.Survivors.Rifter.SkillStates
{
    public class RiftSpecial : MonoBehaviour
    {
        public static float procCoefficient = 1f;
        public static float specialBaseDuration = 2f;

        private float specialDuration;
        private bool secondShot;
        public bool thirdShot;


        //added vars for blastAttack
        private BlastAttack blastAttack = new BlastAttack();
        //Interface stuff
        public GameObject hitEffectPrefab = FireBarrage.hitEffectPrefab;

        public CharacterBody characterBody;
        public float radius;
        public Vector3 vector;

        //OnEnter() runs once at the start of the skill
        //All we do here is create a BulletAttack and fire it
        public void OnEnable()
        {


            blastAttack.attacker = base.gameObject;
            blastAttack.inflictor = base.gameObject;
            blastAttack.teamIndex = TeamIndex.None;
            blastAttack.radius = radius;
            blastAttack.falloffModel = BlastAttack.FalloffModel.None;
            blastAttack.baseDamage = characterBody.damage * 5f;
            blastAttack.crit = characterBody.RollCrit();
            blastAttack.procCoefficient = 1f;
            blastAttack.canRejectForce = false;
            blastAttack.position = vector;
            blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
            blastAttack.Fire();

            EffectData effectData2 = new EffectData();
            effectData2.origin = blastAttack.position;
            EffectManager.SpawnEffect(GlobalEventManager.CommonAssets.igniteOnKillExplosionEffectPrefab, effectData2, transmit: false);




        }

        private void FixedUpdate()
        {
            specialDuration += Time.fixedDeltaTime;
            if (specialDuration >= 1)
            {
                if (secondShot != true)
                {
                    blastAttack.Fire();
                    secondShot = true;
                }
                
            }
            if (specialDuration > specialBaseDuration)
            {
                if (thirdShot != true)
                {
                    blastAttack.Fire();
                    thirdShot = true;
                }
                if (thirdShot == true)
                {
                    Debug.Log("destroy");
                    Destroy(this);
                    return;
                }
            }
        }
    } 

    
}
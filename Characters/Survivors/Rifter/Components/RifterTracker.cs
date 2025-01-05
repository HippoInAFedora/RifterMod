using System;
using System.Collections.Generic;
using System.Linq;
using IL.RoR2.HudOverlay;
using Newtonsoft.Json.Linq;
using R2API.Utils;
using RifterMod.Survivors.Rifter;
using RoR2;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace RifterMod.Characters.Survivors.Rifter.Components
{
    public class RifterTracker : MonoBehaviour
    {
        public float maxTrackingDistance = 20f;

        public float maxTrackingAngle = 20f;

        public float trackerUpdateFrequency = 10f;

        private GameObject trackingTarget;

        private CharacterBody characterBody;

        private TeamComponent teamComponent;

        private InputBankTest inputBank;

        private float trackerUpdateStopwatch;

        private Indicator indicator;
        private Indicator indicatorMid;

        private Color fullColor = new Color(0.329f, 0.42f, 0.651f);
        private Color fullColorMid = new Color(0.518f, .329f, 0.651f);
        private Color nullColor = new Color(0, 0, 0, 0);

        public BullseyeSearch search = new BullseyeSearch();

        private void Awake()
        {
            indicator = new Indicator(base.gameObject, RifterAssets.riftIndicator);
            indicatorMid = new Indicator(base.gameObject, RifterAssets.riftIndicator);
        }

        private void Start()
        {
            characterBody = GetComponent<CharacterBody>();
            inputBank = GetComponent<InputBankTest>();
            teamComponent = GetComponent<TeamComponent>();
        }

        public GameObject GetTrackingTarget()
        {
            return trackingTarget;
        }

        private void OnEnable()
        {
            indicator.active = true;
            indicatorMid.active = true;
        }

        private void OnDisable()
        {
            indicator.active = false;
            indicatorMid.active = false;
        }

        private void FixedUpdate()
        {
            trackerUpdateStopwatch += Time.fixedDeltaTime;
            if (trackerUpdateStopwatch >= 1f / trackerUpdateFrequency)
            {
                trackerUpdateStopwatch -= 1f / trackerUpdateFrequency;
                if (indicator.active)
                {
                    _ = trackingTarget;
                    Ray aimRay = new Ray(inputBank.aimOrigin, inputBank.aimDirection);
                    Vector3 position = aimRay.GetPoint(RifterStaticValues.riftPrimaryDistance);
                    if (Physics.Raycast(aimRay, out var endPoint, RifterStaticValues.riftPrimaryDistance, LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal))
                    {
                        float hit = endPoint.distance;
                        if (hit > RifterStaticValues.riftSecondaryDistance + 5f)
                        {
                            position = aimRay.GetPoint(hit);
                        }
                    }
                    SearchForTarget(aimRay, position);
                    indicator.targetTransform = (trackingTarget ? trackingTarget.transform : null);
                    if (indicator.targetTransform != null)
                    {
                        float t = 0f;
                        float distance = Vector3.Distance(trackingTarget.transform.position, position);
                        if (distance > RifterStaticValues.blastRadius / 2)
                        {
                            float num = distance - RifterStaticValues.blastRadius / 2;
                            float num2 = ((RifterStaticValues.blastRadius + 10f) / 2) - distance;
                            t = Mathf.Lerp(0, 1, num / num2);
                        }
                        if (distance > (RifterStaticValues.blastRadius + 5f) / 2)
                        {
                            t = 1;
                        }
                        Color color = Lerp.Interpolate(fullColor, nullColor, t);
                        if (indicator.visualizerInstance)
                        {
                            indicator.visualizerInstance.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = color;
                            indicator.visualizerInstance.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
                            indicator.visualizerInstance.transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
                            indicator.visualizerInstance.transform.GetChild(1).transform.GetChild(2).transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
                            indicator.visualizerInstance.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().color = color;
                        }
                        
                    }
                }
                if (indicatorMid.active)
                {
                    _ = trackingTarget;
                    Ray aimRay = new Ray(inputBank.aimOrigin, inputBank.aimDirection);
                    Vector3 position = aimRay.GetPoint(RifterStaticValues.riftSecondaryDistance);
                    if (Physics.Raycast(aimRay, out var endPoint, RifterStaticValues.riftSecondaryDistance, LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal))
                    {
                        float hit = endPoint.distance;
                        position = aimRay.GetPoint(hit);
                    }
                    SearchForTarget(aimRay, position);
                    indicatorMid.targetTransform = (trackingTarget ? trackingTarget.transform : null);
                    if (indicatorMid.targetTransform != null)
                    {
                        float t = 0f;
                        float distance = Vector3.Distance(trackingTarget.transform.position, position);
                        if (distance > RifterStaticValues.blastRadius / 2)
                        {
                            float num = distance - RifterStaticValues.blastRadius / 2;
                            float num2 = ((RifterStaticValues.blastRadius + 10f) / 2) - distance;
                            t = Mathf.Lerp(0, 1, num / num2);
                        }
                        if (distance > (RifterStaticValues.blastRadius + 5f) / 2)
                        {
                            t = 1;
                        }
                        Color color = Lerp.Interpolate(fullColorMid, nullColor, t);
                        if (indicatorMid.visualizerInstance)
                        {
                            indicatorMid.visualizerInstance.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = color;
                            indicatorMid.visualizerInstance.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
                            indicatorMid.visualizerInstance.transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
                            indicatorMid.visualizerInstance.transform.GetChild(1).transform.GetChild(2).transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
                            indicatorMid.visualizerInstance.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().color = color;
                        }
                        
                    }
                }
            }

            void SearchForTarget(Ray aimRay, Vector3 position)
            {
                var allButNeutral = TeamMask.allButNeutral;
                allButNeutral.RemoveTeam(teamComponent.teamIndex);

                search.Reset();
                search.teamMaskFilter = allButNeutral;
                search.filterByLoS = true;
                search.searchOrigin = aimRay.origin;
                search.searchDirection = aimRay.direction;
                search.sortMode = BullseyeSearch.SortMode.Distance;
                search.maxDistanceFilter = 56.75f;
                search.maxAngleFilter = 5f;

                search.RefreshCandidates();
                search.FilterCandidatesByHealthFraction(Mathf.Epsilon);
                search.FilterOutGameObject(base.gameObject);

                HurtBox result = null;
                float distance = 0f;

                foreach (var hitbox in search.GetResults())
                {
                    var other = (hitbox.transform.position - position).sqrMagnitude;
                    if (distance < other)
                    {
                        result = hitbox;
                        distance = other;
                    }
                }
                trackingTarget = result ? result.gameObject : null;
            }
        }
    }
}

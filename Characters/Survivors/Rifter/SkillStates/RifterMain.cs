using RoR2;
using EntityStates;
using UnityEngine;
using RifterMod.Survivors.Rifter;
using JetBrains.Annotations;
using UnityEngine.Experimental.GlobalIllumination;

public class RifterMain : GenericCharacterMain
{

    

    private static GameObject distanceRenderPrefab = RifterAssets.distanceRenderer;

    private GameObject distanceRenderer;

    private LineRenderer lineComponent;

    private GameObject orb = RifterAssets.distanceOrb;

    private bool distanceAssist = RifterConfig.distanceAssist.Value;

    public override void OnEnter()
    {
        base.OnEnter();
        if ((bool)distanceRenderPrefab)
        {
            distanceRenderer = Object.Instantiate(distanceRenderPrefab, transform.position, transform.rotation);
            distanceRenderer.transform.parent = transform;
            lineComponent = distanceRenderer.GetComponent<LineRenderer>();
        }
    }
    public override void Update()
    {
        base.Update();

        if (!distanceRenderer || !lineComponent || distanceAssist == false)
        {
            return;
        }
        float num = RifterStaticValues.riftPrimaryDistance;
        Ray aimRay = GetAimRay();
        Vector3 position = transform.position;
        Vector3 point = aimRay.GetPoint(num);
        if (Physics.Raycast(aimRay, out var hitInfo, num, (int)LayerIndex.CommonMasks.bullet))
        {
            point = hitInfo.point;
        }
        lineComponent.SetPosition(0, position);
        lineComponent.SetPosition(1, point);
        lineComponent.startWidth = .1f;
        lineComponent.endWidth = .1f;
        orb.transform.position = point;
        EffectData effectData = new EffectData();
        effectData.scale = 5.0f;
        EffectManager.SpawnEffect(orb, effectData, false);

    }

    public override void OnExit()
    {
        base.OnExit();
        if ((bool)distanceRenderer)
        {
            EntityState.Destroy(distanceRenderer);
        }
    }
}


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

    private GameObject orbPrefab = RifterAssets.distanceOrb;

    private GameObject orb;

    private bool distanceAssist = RifterConfig.distanceAssist.Value;

    public override void OnEnter()
    {
        base.OnEnter();
        if ((bool)distanceRenderPrefab && distanceAssist == true)
        {
            distanceRenderer = Object.Instantiate(distanceRenderPrefab);
            distanceRenderer.transform.parent = transform;
            lineComponent = distanceRenderer.GetComponent<LineRenderer>();
        }
        //if ((bool)orbPrefab)
        //{
        //    orb = Object.Instantiate(orbPrefab);
        //    orb.transform.localScale = new Vector3(.25f, .25f, .25f);
        //}
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
        if (Physics.Raycast(aimRay, out var hitInfo, num, (int)LayerIndex.world.mask))
        {
            point = hitInfo.point;
        }
        lineComponent.SetPosition(0, position);
        lineComponent.SetPosition(1, point);
        lineComponent.startWidth = .1f;
        lineComponent.endWidth = .15f;
        //orb.transform.position = point;

    }

    public override void OnExit()
    {
        base.OnExit();
        if ((bool)distanceRenderer)
        {
            EntityState.Destroy(distanceRenderer);
        }
        //if ((bool)orb)
        //{
        //    EntityState.Destroy(orb);
        //}
    }
}


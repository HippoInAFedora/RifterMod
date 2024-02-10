
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

public class ModifiedTeleport : MonoBehaviour
{
    public CharacterBody body;
    public Vector3 targetFootPosition;

    public float teleportTimer;
    public float teleportWaitDuration = .5f;
    public bool teleportOut;

    

    private void OnEnable()
    { 
        if (!NetworkServer.active)
        {
            UnityEngine.Debug.Log("network server not active");
        }
        ModifiedTeleportBody(body, targetFootPosition);
        teleportTimer = 0f;
        
    }

    private void FixedUpdate()
    {
        if (NetworkServer.active)
        {
            
            teleportTimer += Time.fixedDeltaTime;
            if (teleportTimer >= teleportWaitDuration)
            {
                TeleportOut();
                teleportOut = true;
                enabled = false;
            }
        }
    }
    private void TeleportOut()
    {
        body.characterMotor.enabled = true;
        CharacterModel characterModel = body.modelLocator.modelTransform.GetComponent<CharacterModel>();
        if (characterModel == null)
        {
            UnityEngine.Debug.Log("null");
        }
        if (characterModel != null)
        {
            characterModel.invisibilityCount--;
            UnityEngine.Debug.Log("cmodelOut");
        }
        HurtBoxGroup hurtboxGroup = body.hurtBoxGroup;

        if (hurtboxGroup == null)
        {
            UnityEngine.Debug.Log("hboxnull");
        }
        if ((bool)hurtboxGroup)
        {
            HurtBoxGroup hurtBoxGroup = hurtboxGroup;
            int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter - 1;
            hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
            UnityEngine.Debug.Log("hurtBoxOut");
        }
    }
    
    private void ModifiedTeleportBody(CharacterBody body, Vector3 targetFootPosition)
    {

        if ((bool)body)
        {

            body.characterMotor.enabled = false;
            CharacterModel characterModel = body.modelLocator.modelTransform.GetComponent<CharacterModel>();
            if (characterModel == null)
            {
                UnityEngine.Debug.Log("null");
            }
            if (characterModel != null)
            {
                characterModel.invisibilityCount++;
                UnityEngine.Debug.Log("cmodel");
            }
            HurtBoxGroup hurtboxGroup = body.hurtBoxGroup;

            if (hurtboxGroup == null)
            {
                UnityEngine.Debug.Log("hboxnull");
            }
            if ((bool)hurtboxGroup)
            {
                HurtBoxGroup hurtBoxGroup = hurtboxGroup;
                int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter + 1;
                hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
                UnityEngine.Debug.Log("hurtBox");
            }
            TeleportHelper.TeleportBody(body, targetFootPosition);
        }
    }
}


using RifterMod.Modules;
using UnityEngine;

namespace RifterMod.Survivors.Rifter.Components
{
    internal class RifterWeaponComponent : MonoBehaviour
    {
        private void Awake()
        {
            RifterStep rifterStep = new RifterStep();
            //any funny custom behavior you want here
            //for example, enforcer uses a component like this to change his guns depending on selected skill
        }
    }
}
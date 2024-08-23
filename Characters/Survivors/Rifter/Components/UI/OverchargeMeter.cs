
using RoR2.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RifterMod.Characters.Survivors.Rifter.Components
{
    public class OverchargeMeter : MonoBehaviour
    {
        public OverchargeMeter instance;

        public RifterOverchargePassive passive;

        public static Image fill;

        public static Text counter;

        public float fillPercent;

        private void Awake()
        {
            instance = this;
            fill = GetComponent<Image>();
            counter = GetComponent<Text>();
        }

        private void FixedUpdate()
        {
            if (passive != null && fill != null)
            {
                fillPercent = Mathf.Clamp(passive.rifterOverchargePassive - 1, 0f, 5f);
                fill.fillAmount = fillPercent / 5f;
                counter.text = (passive.rifterOverchargePassive - 1 < 0 ? 0 : passive.rifterOverchargePassive - 1).ToString();
                //fill.fillAmount = 1f;
            }
        }
    }
}
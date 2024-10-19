using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Radial Bleed", menuName = "Scriptable Objects/Boons/Osiris/Radial Bleed")]
public class RadialBleed : BoonBase
{
    [SerializeField] float radius = 1.5f;
    StatusEffect effectToAdd;
    public override void OnBoonActivationEvent(PlayerBoonManager.BoonActivation boonActivation, PlayerBoonManager boonManager)
    {
        if(boonActivation == PlayerBoonManager.BoonActivation.OnAdded)
        {
            effectToAdd = Resources.Load("Bleed DOT") as StatusEffect;
        }
        if(boonActivation == PlayerBoonManager.BoonActivation.OnDashEnded)
        {
            RaycastHit[] hits = Physics.SphereCastAll(new Ray(boonManager.transform.position, boonManager.transform.forward), radius);
            if(hits.Length > 0)
            {
                foreach(RaycastHit hit in hits)
                {
                    hit.collider.gameObject.GetComponent<StatusManager>().ApplyStatus(effectToAdd);
                }
            }
        }
    }
}

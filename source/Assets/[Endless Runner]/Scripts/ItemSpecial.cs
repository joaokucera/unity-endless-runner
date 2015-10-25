using UnityEngine;

namespace EndlessRunner
{
    [AddComponentMenu("CUSTOM / Item Special")]
    public class ItemSpecial : ItemBase
    {
        public override int HitPoints { get { return 20; } }

        public override void Reset()
        {
            transform.localPosition = new Vector3(Random.Range(-5f, 5f), 0, 0);
        }

        protected override void OnCollisionEnter(Collision collision)
        {
            if (collision.IsPlayer())
            {
                GlobalVariables.Player.SetMaterialColors(CurrentMaterial);

                Hide();

                SoundManager.PlaySoundEffect("SpecialItemHit");

                GameDirector.AddScore(HitPoints);
            }
        }
    }
}
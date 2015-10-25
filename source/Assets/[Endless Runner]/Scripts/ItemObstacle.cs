using UnityEngine;

namespace EndlessRunner
{
    [AddComponentMenu("CUSTOM / Item Obstacle")]
    public class ItemObstacle : ItemBase
    {
        public override int HitPoints { get { return 10; } }

        public override void Reset()
        {
            transform.localScale = new Vector3(Random.Range(2.5f, 5f), Random.Range(1f, 2.5f), 1);

            transform.localPosition = new Vector3(RandomPositionX(), transform.localScale.y / 2 - transform.parent.localPosition.y, 0);
        }

        protected override void OnCollisionEnter(Collision collision)
        {
            if (collision.IsPlayer())
            {
                if (CurrentMaterialName == GlobalVariables.Player.CurrentMaterialName)
                {
                    GameDirector.DoEffects("ObstacleHit", CurrentMaterial);

                    Hide();

                    GameDirector.AddScore(HitPoints);
                }
                else
                {
                    GlobalVariables.Player.Death();
                }
            }
        }

        private float RandomPositionX()
        {
            GlobalVariables.SpawnAtPlayerDirection = !GlobalVariables.SpawnAtPlayerDirection;

            return GlobalVariables.SpawnAtPlayerDirection ? GlobalVariables.Player.transform.position.x : Random.Range(-5f, 5f);
        }
    }
}
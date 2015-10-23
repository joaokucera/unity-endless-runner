using UnityEngine;

namespace EndlessRunner
{
    [AddComponentMenu("CUSTOM / Item Obstacle")]
    public class ItemObstacle : ItemBase
    {
        protected override void Reload()
        {
            transform.localScale = new Vector3(Random.Range(1f, GroundScroll.WorldScale.x / 2), Random.Range(2.5f, 5f), 1);

            transform.localPosition = new Vector3(
                Random.Range(-GroundScroll.WorldScale.x + transform.localScale.x / 2, GroundScroll.WorldScale.x - transform.localScale.x / 2),
                transform.parent.localPosition.y, transform.parent.localPosition.z);

            Show();
        }
    }
}
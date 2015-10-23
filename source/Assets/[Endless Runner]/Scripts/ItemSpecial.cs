using UnityEngine;

namespace EndlessRunner
{
    [AddComponentMenu("CUSTOM / Item Special")]
    public class ItemSpecial : ItemBase
    {
        protected override void Reload()
        {
            transform.localPosition = new Vector3(transform.parent.localPosition.x, transform.parent.localPosition.y, transform.parent.localPosition.z);

            Show();
        }
    }
}
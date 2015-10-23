using UnityEngine;

namespace EndlessRunner
{
    public static class Extensions
    {
        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            T result = component.GetComponent<T>();

            if (result == null)
            {
                result = component.gameObject.AddComponent<T>();
            }

            return result;
        }

        public static bool IsGround(this Collision collision)
        {
            return collision.gameObject.name == "Ground";
        }

        public static bool IsObstacle(this Collision collision)
        {
            return collision.gameObject.CompareTag("Obstacle");
        }

        public static bool IsObstacle(this Collider collider)
        {
            return collider.CompareTag("Obstacle");
        }

        public static bool IsSpecialItem(this Collider collider)
        {
            return collider.CompareTag("SpecialItem");
        }

        public static bool IsExit(this Collider collider)
        {
            return collider.name == "Exit";
        }

        public static bool IsWater(this Collider collider)
        {
            return collider.name == "Water";
        }
    }
}
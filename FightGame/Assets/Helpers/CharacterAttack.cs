using UnityEngine;

namespace Assets.Helpers
{
    public class CharacterAttack
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public Vector3 Direction { get; set; }
        public int NeededStamina { get; set; }
    }
}

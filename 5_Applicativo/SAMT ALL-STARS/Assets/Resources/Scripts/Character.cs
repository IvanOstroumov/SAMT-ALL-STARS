namespace Resources.Scripts
{
    public class Character
    {
        private CharacterData data;
        private int currentHp;
        private Ability ability;
        private float duration;
        private float cooldown;

        public Character(CharacterData data, int currentHp, Ability ability, float duration, float cooldown)
        {
            this.data = data;
            this.currentHp = currentHp;
            this.ability = ability;
            this.duration = duration;
            this.cooldown = cooldown;
        }

        public CharacterData Data => data;

        public int CurrentHp => currentHp;

        public Ability Ability => ability;

        public float Duration => duration;

        public float Cooldown => cooldown;
    }
}
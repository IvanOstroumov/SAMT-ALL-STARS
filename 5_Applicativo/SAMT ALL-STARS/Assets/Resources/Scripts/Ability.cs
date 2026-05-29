namespace Resources.Scripts
{
    // Una skill / mossa speciale di un personaggio.
    // Per ora e' solo un contenitore di dati: nome, danno, durata e cooldown.
    // L'integrazione vera col gameplay arrivera' quando le mosse speciali
    // verranno collegate ai controller.
    public class Ability
    {
        private string name;
        private int damage;
        private int duration;
        private int cooldown;

        public Ability(string name, int damage, int duration, int cooldown)
        {
            this.name = name;
            this.damage = damage;
            this.duration = duration;
            this.cooldown = cooldown;
        }

        public string Name => name;
        public int Damage => damage;
        public int Duration => duration;
        public int Cooldown => cooldown;
    }
}

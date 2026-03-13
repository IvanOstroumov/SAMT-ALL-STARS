namespace Resources.Scripts
{
    public class CharacterData
    {
        private string name;
        private int maxHp;
        private float speed;
        private string description;
        private int winCount;
        private int gamesPlayed;

        public CharacterData(string name, int maxHp, float speed, string description, int winCount, int gamesPlayed)
        {
            this.name = name;
            this.maxHp = maxHp;
            this.speed = speed;
            this.description = description;
            this.winCount = winCount;
            this.gamesPlayed = gamesPlayed;
        }

        public string Name => name;

        public int MaxHp => maxHp;

        public float Speed => speed;

        public string Description => description;

        public int WinCount => winCount;

        public int GamesPlayed => gamesPlayed;
    }
}

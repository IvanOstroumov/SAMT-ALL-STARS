using UnityEngine;

namespace Resources.Scripts
{
    // Mappa selezionabile con nome + immagine preview
    public class Map
    {
        private string name;
        private Sprite background;
        private Sprite teil;

        public Map(string name, Sprite background, Sprite teil)
        {
            this.name = name;
            this.background = background;
            this.teil = teil;
        }

        public string Name => name;
        public Sprite Background => background;
        public Sprite Teil => teil;
    }
}
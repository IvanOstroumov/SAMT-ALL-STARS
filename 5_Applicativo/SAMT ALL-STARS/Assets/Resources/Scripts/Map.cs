using UnityEngine;

namespace Resources.Scripts
{
    // Una mappa selezionabile: nome + sprite di anteprima.
    public class Map
    {
        private string name;
<<<<<<< Updated upstream
        private Sprite background;
        private Sprite teil;
        
        public Map(string name, Sprite background, Sprite teil)
=======
        private Sprite image;

        public Map(string name, Sprite image)
>>>>>>> Stashed changes
        {
            this.name = name;
            this.background = background;
            this.teil = teil;
        }

        public string Name => name;
<<<<<<< Updated upstream

        public Sprite Background => background;
        public Sprite Teil => teil;
=======
        public Sprite Image => image;
>>>>>>> Stashed changes
    }
}

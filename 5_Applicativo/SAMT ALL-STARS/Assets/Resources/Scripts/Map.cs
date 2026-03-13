namespace Resources.Scripts
{
    public class Map
    {
        private string name;
        private string imagePath;
        
        public Map(string name, string imagePath)
        {
            this.name = name;
            this.imagePath = imagePath;
        }

        public string Name => name;

        public string ImagePath => imagePath;
    }
}
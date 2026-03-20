using System.Collections.Generic;
using NUnit.Framework;


namespace Resources.Scripts
{
    public class CharacterManager
    {
        List<Character> characters;

        public CharacterManager()
        {
            characters = new List<Character>();
        }

        public Character getCharByName(string name)
        {
            foreach (var character in characters)
            {
                if (character.Data.Name == name)
                {
                    return character;
                }
            }

            return null;
        }

        public List getCharsFromJSON()
        {
            return null;
        }
    }
}
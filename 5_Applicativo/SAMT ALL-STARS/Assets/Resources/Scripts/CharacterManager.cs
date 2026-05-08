using System;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts
{
    // Wrapper per JsonUtility — contiene l'array di CharacterData
    [Serializable]
    public class CharacterDataList
    {
        public CharacterData[] characters;
    }

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
                    return character;
            }
            return null;
        }

        public List<Character> getCharsFromJSON()
        {
            TextAsset jsonFile = UnityEngine.Resources.Load<TextAsset>("Scripts/characters");

            if (jsonFile == null)
            {
                Debug.LogError("characters.json non trovato in Assets/Resources/Scripts/");
                return null;
            }

            CharacterDataList jsonList = JsonUtility.FromJson<CharacterDataList>(jsonFile.text);

            characters = new List<Character>();

            foreach (CharacterData data in jsonList.characters)
            {
                Character character = new Character(data, data.MaxHp, null, data.duration, data.cooldown);
                characters.Add(character);
            }

            return characters;
        }
    }
}
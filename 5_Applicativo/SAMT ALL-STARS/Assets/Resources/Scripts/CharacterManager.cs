using System;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Scripts
{
    // Carica i personaggi dal JSON all'avvio e li tiene a portata di mano.
    public class CharacterManager
    {
        List<Character> characters;

        public CharacterManager()
        {
            try
            {
                characters = new List<Character>();
                getCharsFromJSON();
                LogManager.Info($"CharacterManager pronto, {characters.Count} personaggi caricati.");
            }
            catch (Exception e)
            {
                LogManager.Error("CharacterManager: errore in fase di costruzione", e);
                throw;
            }
        }
        
        public Character getCharByName(string name)
        {
            name = name.ToLower();
            foreach (var character in characters)
            {
                if (character.Data.Name == name)
                    return character;
            }
            return null;
        }

        // Legge characters.json da Resources/Scripts e popola la lista.
        public void getCharsFromJSON()
        {
            TextAsset jsonFile = UnityEngine.Resources.Load<TextAsset>("Scripts/characters");

            if (jsonFile == null)
            {
                LogManager.Error("characters.json non trovato in Assets/Resources/Scripts/");
                characters = new List<Character>();   // lista vuota, non null: evita NRE a cascata
                return;
            }

            CharacterDataList jsonList = JsonUtility.FromJson<CharacterDataList>(jsonFile.text);

            foreach (CharacterData data in jsonList.characters)
            {
<<<<<<< Updated upstream
                Character character = new Character(data, null, data.duration, data.cooldown,null,null,null);
=======
                Character character = new Character(data, data.MaxHp, null,
                    data.duration, data.cooldown, null, null);
>>>>>>> Stashed changes
                characters.Add(character);
            }
        }
        
        public void saveCharsToJSON()
        {
            CharacterDataList dataList = new CharacterDataList();

            List<CharacterData> datas = new List<CharacterData>();

            foreach (Character character in characters)
            {
                datas.Add(character.Data);
            }

            dataList.characters = datas.ToArray();

            string json = JsonUtility.ToJson(dataList, true);

            string path = Application.dataPath + "/Resources/Scripts/characters.json";

            System.IO.File.WriteAllText(path, json);

            Debug.Log("JSON salvato in: " + path);
        }


        [Serializable]
        public class CharacterDataList
        {
            public CharacterData[] characters;
        }
    }
}

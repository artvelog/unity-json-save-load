using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    //Verilerini Kaydetmek istediğimiz Karakterimizi çekiyoruz. Siz dilerseniz başka verileri kaydedebilirsiniz.
    public GameObject player;

    //Kayıt dosyamızın adı ve uzantısını yazıyoruz.
    public string fileName = "save.sav";

    //Kayıt edilecek dosya konumunu belirtiyoruz. "/Saves/" kısmını dilediğiniz gibi değiştirebilirsiniz.
    private static readonly string filePath = Application.persistentDataPath + "/Saves/";

    void Awake()
    {
        //Oyunun Data klosöründe 'Saves' adında klasor var mı diye kontrol ediyoruz, eğer yoksa oluşturuyoruz.
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
            Debug.Log(filePath);
        }
    }

    void Update()
    {
        //Klavyeden F7 basarak Kayıt gerçekleştiriyoruz.
        if (Input.GetKeyDown(KeyCode.F7))
        {
            Save();
        }

        //Kalvyeden F8 basarak Kayıtlı verileri çekiyoruz.
        if (Input.GetKeyDown(KeyCode.F8))
        {
            Load();
        }
    }


    public void Save()
    {
        SaveData saveData = new SaveData {
            //Kaydetmek istediğimiz değerleri class içerisine eklediğimiz değişkenlere eşitliyoruz.
            player_name = player.name,
            player_health = player.GetComponent<CharacterHealth>().playerHealth, //Karakterimizin Canını barındıran companentine erişerek değerimizi çekiyoruz.
            player_position = player.transform.position,
            player_rotation = player.transform.rotation,
        };

        //Tüm veriyi Json'a çeviriyoruz.
        string json_data = JsonUtility.ToJson(saveData, true);

        //Json'a çevirdiğimiz veriyi dosya olarak belirttiğimiz isimle, belirttiğimiz konuma export ediyoruz.
        File.WriteAllText(filePath + fileName, json_data);
    }

    public void Load()
    {
        //Save dosyamızdan verileri çekiyoruz
        string saveData = File.ReadAllText(filePath + fileName);
        SaveData saveObjects = JsonUtility.FromJson<SaveData>(saveData);

        //Dataları çekiyoruz
        player.transform.position = saveObjects.player_position;
        player.transform.rotation = saveObjects.player_rotation;
        player.GetComponent<CharacterHealth>().playerHealth = saveObjects.player_health;

        //Datayı Console'a çıktı alıyoruz
        Debug.Log("Player Name:" + saveObjects.player_name + 
        "Player Health:" + saveObjects.player_health + 
        "Player Position:" + saveObjects.player_position + 
        "Player Rotation:" + saveObjects.player_rotation);
    }

    private class SaveData
    {
        //Dilediğiniz Kader ekleme yapabilirsiniz.
        public string player_name;
        public float player_health;
        public Vector3 player_position;
        public Quaternion player_rotation;
    }
}

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ConfigManager
{
    [Serializable]
    public class Config
    {
        public int resolution = 1;
        public float sfxVolume = .5f;
        public float bgmVolume = .5f;
    }
    public static Config defaultConfig = new Config();
    public static Config currentConfig = new Config();

    public static void RemoveConfigData()
    {
        PlayerPrefs.DeleteKey("Config");
    }

    public static Config ReadConfigData()
    {
        string data;
        Config temp;

        try
        {
            // 문자열 데이터 로드
            data = PlayerPrefs.GetString("Config");

            // 빈 데이터가 아닌 경우
            if (!string.IsNullOrEmpty(data))
            {
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();

                // 문자열 데이터를 Byte 배열 형태로 변환
                ms = new MemoryStream(Convert.FromBase64String(data));
                temp = (Config)bf.Deserialize(ms);

                return temp;
            }
        }
        catch { }

        return null;
    }

    public static void SaveConfigData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();

        // 데이터를 Byte 배열 형태로 변환
        bf.Serialize(ms, currentConfig);
        // 문자열로 변환하여 저장
        PlayerPrefs.SetString("Config", Convert.ToBase64String(ms.GetBuffer()));
    }

    public static void LoadConfigData()
    {
        string data;

        try
        {
            // 문자열 데이터 불러옴
            data = PlayerPrefs.GetString("Config");

            // 빈 데이터가 아닌 경우
            if (!string.IsNullOrEmpty(data))
            {
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();

                // 문자열 데이터를 Byte 배열 형태로 변환
                ms = new MemoryStream(Convert.FromBase64String(data));
                currentConfig = (Config)bf.Deserialize(ms);
            }
        }
        catch { }
    }
}

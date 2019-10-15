using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions; //for Regex
using UnityEngine.UI; //for Text
using System.IO; //for FileInfo
/// <summary>
/// хранит тэги объектов, загружает из Resources звуки, машины, трэк
/// </summary>
public static class Globals
{
    public static int idxCar = 0;
    public static int idxTrack = 0;
    public static int idxStartPosition = 0;
    public static int numAICars = 8;
    public static List<GameObject> prefabCars = new List<GameObject>();
    public static List<GameObject> prefabTracks = new List<GameObject>();    

    public static string TagCar = "Car";
    public static string TagCheckpoint = "Checkpoint";
    public static string TagTerrain = "Terrain";
    public static string NameCheckpointHolder = "Checkpoints";
    public static string NamePrefabHolder = "PrefabHolder";
    public static string NameDecalTireDirt = "DecalTireDirt";
    public static string NameDecalTirePavement = "DecalTirePavement";
    public static string NameFollowPos = "FollowPos";
    public static string NameTerrain = "Terrain";
    public static string NameBarrier = "Barrier";
    public static int LayerCheckpoints = LayerMask.NameToLayer("Checkpoints");
    public static int LayerTerrain = LayerMask.NameToLayer("Terrain");
    /// <summary>
    /// загружает из папки Resources\Audio 
    /// </summary>
    /// <param name="gobj"></param>
    /// <param name="filenamenoextension"></param>
    /// <returns></returns>
    public static AudioSource CreateAudioSource(GameObject gobj, string filenamenoextension)
    {
        AudioSource audio = gobj.AddComponent<AudioSource>();
        audio.clip = Resources.Load<AudioClip>("Audio\\" + filenamenoextension);
        return audio;
    }
    public static int ExtractNumberValue(string name)
    {
        return System.Convert.ToInt32(Regex.Replace(name, "[^0-9]", ""));
    }
    public static GameObject LoadPrefab(string prefabname)
    {
        GameObject prefab = Resources.Load(prefabname) as GameObject;
        return prefab;
    }
    public static bool IsPositionInFront(Vector3 position, GameObject obj)
    {
        Vector3 relativePosition = obj.transform.InverseTransformPoint(position);
        if (relativePosition.z > 0.0)
        {
            return true;
        }
        return false;
    }
    public static float IsSidewayMotion(GameObject obj, Rigidbody rb)
    {
        return Mathf.Abs(obj.transform.InverseTransformDirection(rb.velocity).x);
    }
    public static void DebugScreen(string msg)
    {
        GameObject obj = GameObject.Find("txtDebug");
        if (obj != null)
        {
            Text txt = obj.GetComponent<Text>();
            if (txt != null)
            {
                txt.text = msg;
            }
        }
    }
    public static FileInfo[] GetResourcesFiles(string subFolder, string searchPattern)
    {
        string resourcesFolder = "\\Resources";
        if (subFolder.Length > 0)
            resourcesFolder += "\\" + subFolder;
        DirectoryInfo dirinfo = new DirectoryInfo(Application.dataPath + resourcesFolder);
        FileInfo[] files = dirinfo.GetFiles(searchPattern);
        return files;
    }
    /// <summary>
    /// получаю дочерний объект у родителя по имени
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject GetChildByName(GameObject parent, string name)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).name == name)
            {
                return parent.transform.GetChild(i).gameObject;
            }
        }
        ///если трэка нет возвращаю Ноль
        return null;
    }
    /// <summary>
    /// загружает машины из папки Resources\Cars
    /// </summary>
    public static void LoadPrefabCars()
    {
        prefabCars.Clear();
        FileInfo[] carfiles = GetResourcesFiles("Cars", "*.prefab");
        for (int i = 0; i < carfiles.Length; i++)
        {
            GameObject prefabCar = Resources.Load("Cars\\" + Path.GetFileNameWithoutExtension(carfiles[i].Name)) as GameObject;            
            prefabCars.Add(prefabCar);
        }
    }
    /// <summary>
    /// загружает гоночные трассы из папки Resources\Tracks
    /// </summary>
    public static void LoadPrefabTracks()
    {
        prefabTracks.Clear();
        FileInfo[] trackfiles = GetResourcesFiles("Tracks", "*.prefab");
        for (int i = 0; i < trackfiles.Length; i++)
        {
            GameObject prefabTrack = Resources.Load("Tracks\\" + Path.GetFileNameWithoutExtension(trackfiles[i].Name)) as GameObject;
            prefabTracks.Add(prefabTrack);
        }
    }
}

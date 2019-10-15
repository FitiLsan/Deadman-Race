using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text; //for StringBuilder
using System.IO; //for FileInfo
using UnityEngine.UI; //for Text
using UnityEngine.SceneManagement; //for scenemanagement
/// <summary>
/// расставляет тачки по позициям в гараже, как в префабе "Garage", выбирает тачки, выбирает трэк
/// </summary>
public class GarageController : MonoBehaviour
{
    #region --- helpers ---
    private static class sndFiles
    {
        public static string MenuMusic = "menu music (race-in-space)";
        public static string MenuButton = "menu button";
        public static string HereWeGo = "HereWeGo";
        public static string ByeMan = "ByeMan";
    }
    private struct structSounds
    {
        public AudioSource MenuMusic;
        public AudioSource MenuButton;
        public AudioSource HereWeGo;
        public AudioSource ByeMan;
    }
    [System.Serializable]
    public struct structViews
    {
        public GameObject Garage;
        public GameObject GarageParkingSpots;
        public List<GameObject> MenuCars;
        public List<GameObject> MenuTracks;
    }
    #endregion

    public Text SelectCar = null;
    public Text SelectTrack = null;
    public structViews view;
    private structSounds snd; 

    public void Start()
    {
        DisableAllCamera();
        LoadResources();

        //load sounds
        snd.HereWeGo = Globals.CreateAudioSource(this.gameObject, sndFiles.HereWeGo);
        snd.MenuButton = Globals.CreateAudioSource(this.gameObject, sndFiles.MenuButton);
        snd.HereWeGo = Globals.CreateAudioSource(this.gameObject, sndFiles.HereWeGo);
        snd.ByeMan = Globals.CreateAudioSource(this.gameObject, sndFiles.ByeMan);
        snd.MenuMusic = Globals.CreateAudioSource(this.gameObject, sndFiles.MenuMusic);

        //use sounds
        snd.MenuMusic.loop = true;
        snd.MenuMusic.volume = 0.50f;
        snd.MenuMusic.Play();
                
        ButtonPickTrack();
    }
    public void LoadResources()
    {
        //clear
        GameObject clearGarage = Globals.GetChildByName(this.gameObject, "Garage");
        if (clearGarage != null)
        {
            DestroyImmediate(clearGarage);
        }
        GameObject clearCars = Globals.GetChildByName(this.gameObject, "Cars");
        if (clearCars != null)
        {
            DestroyImmediate(clearCars);
        }
        GameObject clearTracks = Globals.GetChildByName(this.gameObject, "Tracks");
        if (clearTracks != null)
        {
            DestroyImmediate(clearTracks);
        }

        //garage
        GameObject prefabGarage = Resources.Load("Garage") as GameObject;
        view.Garage = Instantiate(prefabGarage, this.transform);
        view.Garage.name = "Garage";
        view.GarageParkingSpots = Globals.GetChildByName(view.Garage, "ParkingSpots");

        //cars
        Globals.LoadPrefabCars();
        GameObject carHolder = new GameObject("Cars");
        carHolder.transform.parent = this.transform;
        view.MenuCars = new List<GameObject>();
        for (int i = 0; i < Globals.prefabCars.Count; i++)
        {
            GameObject car = Instantiate(Globals.prefabCars[i], carHolder.transform);
            car.name = Globals.prefabCars[i].name;
            car.transform.position = view.GarageParkingSpots.transform.GetChild(i).transform.position;
            car.transform.rotation = view.GarageParkingSpots.transform.GetChild(i).transform.rotation;
            view.MenuCars.Add(car);
        }
        
        //tracks
        Globals.LoadPrefabTracks();
        GameObject trackHolder = new GameObject("Tracks");
        trackHolder.transform.parent = this.transform;
        view.MenuTracks = new List<GameObject>();
        for (int i = 0; i < Globals.prefabTracks.Count; i++)
        {
            GameObject track = Instantiate(Globals.prefabTracks[i], trackHolder.transform);
            track.name = Globals.prefabTracks[i].name;
            Vector3 pos = new Vector3(500 * (i + 1), 0, 0);
            track.transform.position = pos;
            track.transform.rotation = Quaternion.identity;
            view.MenuTracks.Add(track);
        }        
    }
    private void GotoRace()
    {
        SceneManager.LoadScene("race");
    }
    public void ButtonRace()
    {
        snd.HereWeGo.Play();
        Invoke("GotoRace", snd.HereWeGo.clip.length);
    }
    public void ButtonPickCar()
    {
        //click
        snd.MenuButton.Play();

        //car increment
        parkCar(Globals.idxCar);
        Globals.idxCar += 1;
        if (Globals.idxCar >= view.MenuCars.Count)
        {
            Globals.idxCar = 0;
        }
        selectCar(Globals.idxCar);

        //garage camera
        Camera cam = view.Garage.transform.GetComponentInChildren<Camera>();
        DisableAllCamera(cam);
        cam.GetComponent<Camera>().enabled = true;
        cam.GetComponent<AudioListener>().enabled = true;        
    }
    public void ButtonPickTrack()
    {
        //click
        snd.MenuButton.Play();

        //track increment
        Globals.idxTrack += 1;
        if (Globals.idxTrack >= view.MenuTracks.Count)
        {
            Globals.idxTrack = 0;
        }

        //track camera
        GameObject track = view.MenuTracks[Globals.idxTrack];
        Camera cam = track.transform.GetComponentInChildren<Camera>();
        DisableAllCamera(cam);
        cam.GetComponent<Camera>().enabled = true;
        cam.GetComponent<AudioListener>().enabled = true;

        //display name
        SelectTrack.text = track.gameObject.name;
    }        
    private void parkCar(int idx)
    {
        Transform parkingspot = view.GarageParkingSpots.transform.GetChild(idx);
        view.MenuCars[idx].transform.position = parkingspot.position;
        view.MenuCars[idx].transform.rotation = parkingspot.rotation;
    }
    private void selectCar(int idx)
    {
        GameObject SelectedPos = Globals.GetChildByName(view.Garage, "SelectedPos");
        view.MenuCars[idx].transform.position = SelectedPos.transform.position;
        view.MenuCars[idx].transform.rotation = SelectedPos.transform.rotation;
        SelectCar.text = view.MenuCars[idx].name;
    }
    private void DisableAllCamera(Camera cam = null)
    {
        Camera[] cams = GameObject.FindObjectsOfType<Camera>();
        for (int i = 0; i < cams.Length; i++)
        {
            if (cams[i].Equals(cam) == true)
            {
                continue;
            }
            cams[i].GetComponent<Camera>().enabled = false;
            cams[i].GetComponent<AudioListener>().enabled = false;
        }
    }    
    public string EditorLabelText()
    {
        StringBuilder sb1 = new StringBuilder();

        if (view.GarageParkingSpots == null)
        {
            sb1.Append("Assign ParkingSpots Object");
        }
        else
        {
            sb1.Append("Parking Spots: " + view.GarageParkingSpots.transform.childCount.ToString() + "\n");
            sb1.Append("Cars: " + view.MenuCars.Count.ToString() + "\n");
            sb1.Append("Tracks: " + view.MenuTracks.Count.ToString() + "\n");
        }

        return sb1.ToString();
    }
}

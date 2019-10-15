using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// для участия в гонках -добавляет трэк, сопреников и игрока
/// </summary>
public class RaceController : MonoBehaviour
{
    private GameObject Track = null;
    private GameObject Cam = null;
    private GameObject StartingPositions = null;
    private GameObject Player = null;

    private void Start()
    {
        LoadAll();
    }
    private void OnValidate()
    {
        if (Globals.prefabCars.Count == 0)
        {
            Globals.LoadPrefabCars();
        }
        if (Globals.prefabTracks.Count == 0)
        {
            Globals.LoadPrefabTracks();
        }        
    }
    /// <summary>
    /// загружает машины и трэк
    /// </summary>
    public void LoadAll()
    {
        Globals.LoadPrefabTracks();
        Globals.LoadPrefabCars();
        
        hlprLoadTrack();
        hlprLoadAICars();
        hlprLoadPlayerCar();
    }
    /// <summary>
    /// удаляет текущий трэк и загружает актуальный, получает ссылки на камеру и стартовые позиции тачек, создает в RaceController, дочерний объект "TrackParent"
    /// </summary>
    private void hlprLoadTrack()
    {
        //находит Родительский трек по имени, если он есть на сцене - удаляет его
        GameObject trackParent = Globals.GetChildByName(this.gameObject, "TrackParent");
        if (trackParent != null)
        {
            DestroyImmediate(trackParent);
        }

        //создает новый игровой объект с именем "TrackParent"
        trackParent = new GameObject();
        trackParent.transform.parent = this.transform;
        trackParent.name = "TrackParent";
        
        //создаю трэк, запоминаю имя
        Track = Instantiate(Globals.prefabTracks[Globals.idxTrack], trackParent.transform);
        Track.name = Globals.prefabTracks[Globals.idxTrack].name;
        //получаю у префаба трэка, у дочернего компонента, ссылку на камеру (компонент "Camera")
        //"Camera" на сцене - это обычная камера с аудиоЛистенером
        Cam = Globals.GetChildByName(Track, "Camera");
        //получаю у префаба трэка, у дочернего компонента, ссылку на начальные позиции тачек (компонент "StartingPositions")
        //"StartingPositions" - это Родитель трансформ, и в нем дочерние с Именем: Startpos (0), Startpos (1) и т.д.
        StartingPositions = Globals.GetChildByName(Track, "StartingPositions");
    }
    /// <summary>
    /// удаляет игрока и добавляет его на актуальный трек, выбирает тип управления (клавиатура или другой), вешает на камеру скрипт слежения за игроком, создает в RaceController, дочерний объект "PlayerParent"
    /// </summary>
    private void hlprLoadPlayerCar()
    {
        //erase
        GameObject playerParent = Globals.GetChildByName(this.gameObject, "PlayerParent");
        if (playerParent != null)
        {
            DestroyImmediate(playerParent);
        }

        //playerParent
        playerParent = new GameObject();
        playerParent.transform.parent = this.transform;
        playerParent.name = "PlayerParent";

        //player
        Player = Instantiate(Globals.prefabCars[Globals.idxCar], playerParent.transform);
        Player.name = Globals.prefabCars[Globals.idxCar].name;
        Player.tag = "Player";
        //получает ссылку на стартовую позицию на гонке
        Transform playerTransform = StartingPositions.transform.GetChild(Globals.idxStartPosition).transform;
        //присваивает её игроку
        Player.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 1.0f, playerTransform.position.z);
        //Присваивает игроку угол поворота тачки
        Player.transform.rotation = playerTransform.rotation;
        //получает ссылку на компонент контроллера всех AI тачек
        CarController playerScript = Player.GetComponent<CarController>();
        //присваиваем тип контролера - для игрока (без AI, обычное управление)
        playerScript.CarType = CarController.enumCarType.Player;
        //загружаем этот контроллер
        playerScript.UpdateComponents();

        //виды управления тачкой игрока
        //если подключен джостик
        if (Input.GetJoystickNames().Length > 0)
        {
            //передаем управление тачкой - джостику
            Player.GetComponent<CarControl>().ControlScheme = CarControl.enumControlScheme.Joystick;
        }
        //если нет
        else
        {
            //то клавиатурой
            Player.GetComponent<CarControl>().ControlScheme = CarControl.enumControlScheme.Keyboard;
        }

        //добавляет на камеру скрипт CamFollo
        Cam.AddComponent<CamFollow>();
        //
        Cam.GetComponent<CamFollow>().Car = Player;
    }
    /// <summary>
    /// добавляет AI тачкам, которые участвуют в гонках, создает в RaceController, дочерний объект "AICarParent"
    /// </summary>
    private void hlprLoadAICars()
    {
        //erase
        GameObject aicarParent = Globals.GetChildByName(this.gameObject, "AICarParent");
        if (aicarParent != null)
        {
            DestroyImmediate(aicarParent);
        }

        //aicarParent
        aicarParent = new GameObject();
        aicarParent.transform.parent = this.transform;
        aicarParent.name = "AICarParent";

        //добавление всем тачкам противника AI - езда по чекпоинтам
        //TODO сделать отдельное добавление AI на тачки которые ездят в открытом мире
        for (int i = 0; i < Globals.numAICars; i++)
        {
            int R = Random.Range(0, Globals.prefabCars.Count - 1);
            GameObject aicar = Instantiate(Globals.prefabCars[R], aicarParent.transform);
            aicar.name = Globals.prefabCars[R].name + i.ToString();
            Transform aiTransform = StartingPositions.transform.GetChild(i).transform;
            aicar.transform.position = new Vector3(aiTransform.position.x, aiTransform.position.y + 1.0f, aiTransform.position.z);
            aicar.transform.rotation = aiTransform.rotation;
            CarController aiScript = aicar.GetComponent<CarController>();
            aiScript.CarType = CarController.enumCarType.AIControlCheckpoint;
            aiScript.UpdateComponents();
        }
    }
    public void ChangeTrack()
    {
        Globals.idxTrack = Globals.idxTrack + 1;
        if (Globals.idxTrack > Globals.prefabTracks.Count - 1)
        {
            Globals.idxTrack = 0;
        }
        LoadAll();
    }
    public void ChangePlayerCar()
    {
        Globals.idxCar = Globals.idxCar + 1;
        if (Globals.idxCar > Globals.prefabCars.Count - 1)
        {
            Globals.idxCar = 0;
        }
        LoadAll();
    }
    public string TrackName()
    {
        return Globals.prefabTracks[Globals.idxTrack].name;
    }
    public string PlayerCarName()
    {
        return Globals.prefabCars[Globals.idxCar].name;
    }
}

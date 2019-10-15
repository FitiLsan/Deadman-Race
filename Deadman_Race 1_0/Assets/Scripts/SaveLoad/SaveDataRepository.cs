using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DeadMan_Race
{
    //патерн репозиторий - прослойка между программой и поставщиком данных
    //если хотите сохранить 2 объекта то еужно создать структуру, содержащую эти 2 объекьа и к ней обращатся
    public class SaveDataRepository
	{
        //тот кто поставляет данные, (абстрактный, IData, потому что не знаем кто будет поставщиком данных) типа <SerializableGameObject> - формата сохранения данных
        private IData<SerializableGameObject> _data;
        //название создаваемой папки для сохранения
		private string _folderName = "dataSave";
        //формат файла с сохранением
		private string _fileName = "data.bat";
        //путь к сохранению
		private string _path;
        //конструктор
		public SaveDataRepository()
		{
            //вычислет в реальном времени платформу, куда будем сохранять данные (указываем, что это WebGLPlayer)
            if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
                // нашем поставщиком данных будет являтся класс PlayerPrefsData
                _data = new PlayerPrefsData();
			}
			else
			{
                //добавили новый файл, вместо <SerializableGameObject> -положили новый объект
                //в других случаях поставщиком данныых будет XMLData
                _data = new XMLData(); //- сохранить в XML формате
                
            }
            //в этом конструкторе вычисляем путь, где будут находится сохранения, 
            //Application.dataPath - делает так чтобы папка создалась в папке Assets, с названием как в _folderName
            _path = Path.Combine(Application.dataPath, _folderName);
			
		}
        /// <summary>
        /// сохранение данных
        /// </summary>
		public void Save()
		{
            //проверяем, существует ли путь сохранения
			if (!Directory.Exists(Path.Combine(_path)))
			{
                //если путь не существует, то создаем путь к файлу
				Directory.CreateDirectory(_path);
			}
            //указываем, что хотим сохранить
            var player = new SerializableGameObject
            {
                //айди сцены - айди уровня, на котором игрок
                SceneID = Application.loadedLevel,
               
                //назначаем имя
                Name = "Игрок",
                //берем позицию игрока из скрипта Main
                //Pos = Main.Instance.Player.position,
                // видимость true
                IsEnable = true
               
                
            };
            //говорим поставщику данных, что нужно сохранить объект player, по пути Path.Combine(_path, _fileName)
            _data.Save(player, Path.Combine(_path, _fileName));
		}
        /// <summary>
        /// загрузка данных
        /// </summary>
		public void Load()
		{
            //при загрузке склеивается путь
			var file = Path.Combine(_path, _fileName);
            //если путь не существует, дальше не идем
			if (!File.Exists(file)) return;
            //загружаются данные с помощью поставщика данных
			var newPlayer = _data.Load(file);

            //Application.LoadLevel(newPlayer.SceneID);
            //if (Main.Instance.IsSceneBeingLoaded)
            //{
                
            //    //персонажу выводятся все необходимые настройки
            //    //позиция
            //    Main.Instance.Player.position = newPlayer.Pos;
            //    //Main.Instance.PlayerController._motion.Instance.position = newPlayer.Pos;
            //    //имя
            //    Main.Instance.Player.name = newPlayer.Name;
            //    //активен ли наш элемент
            //    Main.Instance.Player.gameObject.SetActive(newPlayer.IsEnable);
            //    Main.Instance.IsSceneBeingLoaded = false;
            //}
            
            

            
            
            
            Debug.Log(newPlayer);
            

        }
	}
    //добавление элементов в базу данных
}
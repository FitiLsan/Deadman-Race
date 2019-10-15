using UnityEngine;

namespace DeadMan_Race
{
    //сохранение способом ключ/значение
    //сохраняет данные в реестр Windows - размер сохранения не более 2Мб
    //нужен для передачи данных между сценами (сколько жизней у игрока)
    //не забывать вызывать метод DeleteAll(); и чистить данные
    public class PlayerPrefsData : IData<SerializableGameObject>
	{
		public void Save(SerializableGameObject data, string path = null)
		{
            //сохранение сцены
            PlayerPrefs.SetFloat("SceneID", data.SceneID);
            //имя сохраняем в строку
            PlayerPrefs.SetString("Name", data.Name);
            //позицию по х сохраняем в числовое значение 0.0
			PlayerPrefs.SetFloat("PosX", data.Pos.X);
            //bool переменые тоже переводим и сохраняем в строку
			PlayerPrefs.SetString("IsEnable", data.IsEnable.ToString());
            
            //-----------------------------
            //если произошло резкое закрытие приложения произошло автосохранение
            PlayerPrefs.Save();
		}

		public SerializableGameObject Load(string path = null)
		{
			var result = new SerializableGameObject();

            var key = "SceneID";
            if (PlayerPrefs.HasKey(key))
            {
                result.SceneID = PlayerPrefs.GetString(key).TryInt();
            }
            key = "Name";
            //проверка существует ли данный ключ
			if (PlayerPrefs.HasKey(key))
			{
                //то мы можем получить значение по ключу
				result.Name = PlayerPrefs.GetString(key);
			}

			key = "PosX";
			if (PlayerPrefs.HasKey(key))
			{
				result.Pos.X = PlayerPrefs.GetFloat(key);
			}

			key = "IsEnable";
			if (PlayerPrefs.HasKey(key))
			{
				result.IsEnable = PlayerPrefs.GetString(key).TryBool();
			}
            

            return result;
		}

		public void Clear()
		{
            //можем удалить все ключи
			PlayerPrefs.DeleteAll();
            //либо удалить конкретный ключ с его значением
		}
	}
}
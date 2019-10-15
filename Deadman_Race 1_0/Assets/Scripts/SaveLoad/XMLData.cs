using System.IO;
using System.Xml;
using UnityEngine;

namespace DeadMan_Race
{
    //нужно четко знать что мы хоти сохранить
	public class XMLData : IData<SerializableGameObject>
	{
		public void Save(SerializableGameObject player, string path = "")
		{
            //создается новый XML документ
			var xmlDoc = new XmlDocument();
            //создается главный элемент и помещается в корень документа
			XmlNode rootNode = xmlDoc.CreateElement("Player");
            //все остальные объекты будут "детьми" объекта - родителя rootNode 
            xmlDoc.AppendChild(rootNode);

            var element = xmlDoc.CreateElement("SceneID");
            element.SetAttribute("value", player.SceneID.ToString());
            rootNode.AppendChild(element);
            //элемент "Имя" ("Name")
            element = xmlDoc.CreateElement("Name");
            //с атрибутом "Значение" ("value") и имя объекта  player.Name
            element.SetAttribute("value", player.Name);
			rootNode.AppendChild(element);
            //элемент "Имя" ("PosX")
            element = xmlDoc.CreateElement("PosX");
            //с атрибутом "Значение" ("value") и координата "Х" положения игрока
            element.SetAttribute("value", player.Pos.X.ToString());
            //либо атрибут "Х" (вместо "value" и координата "Х" положения игрока
            element.SetAttribute("X", player.Pos.X.ToString());
			rootNode.AppendChild(element);

			element = xmlDoc.CreateElement("PosY");
			element.SetAttribute("value", player.Pos.Y.ToString());
			rootNode.AppendChild(element);

			element = xmlDoc.CreateElement("PosZ");
			element.SetAttribute("value", player.Pos.Z.ToString());
			rootNode.AppendChild(element);

			element = xmlDoc.CreateElement("IsEnable");
			element.SetAttribute("value", player.IsEnable.ToString());
			rootNode.AppendChild(element);

            


            //альтернативный способ создания атрибутов и объектов
            XmlNode userNode = xmlDoc.CreateElement("Info");
			var attribute = xmlDoc.CreateAttribute("Unity");
            //показывает версию Юнити
			attribute.Value = Application.unityVersion;
            //показывает системный язык пользователя
			userNode.Attributes.Append(attribute);
			userNode.InnerText = "System Language: " +
			                     Application.systemLanguage;
            //добавляю в рут объект ноды
			rootNode.AppendChild(userNode);
            //сохраняю путь
			xmlDoc.Save(path);
		}
        //считывание данных (парсинг)
		public SerializableGameObject Load(string path = "")
		{
			var result = new SerializableGameObject();
			if (!File.Exists(path)) return result;
			using (var reader = new XmlTextReader(path))
			{
                //пробегаюсь по всем объекам
				while (reader.Read())
				{
                    var key = "SceneID";
                    if (reader.IsStartElement(key))
                    {
                        result.SceneID = reader.GetAttribute("value").TryInt();
                    }
                    //нахожу ключ (элемент с названием "Name")
                    key = "Name";
					if (reader.IsStartElement(key))
					{
                        //нахожу по данному ключу атрибут "value"
                        result.Name = reader.GetAttribute("value");
					}
					key = "PosX";
					if (reader.IsStartElement(key))
					{
						result.Pos.X = reader.GetAttribute("value").TrySingle(); // TrySingle - преобразует строку в float
                    }
					key = "PosY";
					if (reader.IsStartElement(key))
					{
						result.Pos.Y = reader.GetAttribute("value").TrySingle();
					}
					key = "PosZ";
					if (reader.IsStartElement(key))
					{
						result.Pos.Z = reader.GetAttribute("value").TrySingle();
					}
					key = "IsEnable";
					if (reader.IsStartElement(key))
					{
						result.IsEnable = reader.GetAttribute("value").TryBool();
					}
                    
                }
			}
		
			return result;
		}
	}
}


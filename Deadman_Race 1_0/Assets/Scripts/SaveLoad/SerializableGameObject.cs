using System;
using UnityEngine;

namespace DeadMan_Race
{
    /// <summary>
    /// формат сохранения данных
    /// </summary>
	[Serializable]
	public struct SerializableGameObject
	{
        //можем сохранять имя (с помощью поставщика данных и class SaveDataRepository)
        public string Name;
        //сохраняем айди сцены
        public int SceneID;
        //сохранять позицию
		public SerializableVector3 Pos;
        //поворот
		public SerializableQuaternion Rot;
        //размерность
		public SerializableVector3 Scale;
        //компоненты на GameObject е
        //SerializableXMLData не может сериализовать массив (поэтому при использовании его)
        //эту строчку коментируем
        //public Component[] Components; //эту
        //включен либо выключен объект
        public bool IsEnable;
        //можем сохранить как строку
        public override string ToString()
        {
            return $"Name = {Name}; IsEnable = {IsEnable}; Pos = {Pos}; SceneID = {SceneID};";
        }

    }//можем сохранить , как структуру
	[Serializable]
	public struct SerializableVector3
	{
		public float X;
		public float Y;
		public float Z;
		public SerializableVector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}
        //перегруженные операторы присваивания, implicit - говорит о том что данные могут потерятся
        public static implicit operator Vector3(SerializableVector3 value)
		{
			return new Vector3(value.X, value.Y, value.Z);
		}
		public static implicit operator SerializableVector3(Vector3 value)
		{
			return new SerializableVector3(value.x, value.y, value.z);
		}
        public override string ToString()
        {
            return $"X = {X}; Y = {Y}; Z = {Z};";
        }
    }
	[Serializable]
	public struct SerializableQuaternion
	{
		public float X;
		public float Y;
		public float Z;
		public float W;
		public SerializableQuaternion(float x, float y, float z, float w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}
		public static implicit operator Quaternion(SerializableQuaternion value)
		{
			return new Quaternion(value.X, value.Y, value.Z, value.W);
		}
		public static implicit operator SerializableQuaternion(Quaternion value)
		{
			return new SerializableQuaternion(value.x, value.y, value.z, value.w);
		}
        public override string ToString()
        {
            return $"X = {X}; Y = {Y}; Z = {Z}; W = {W};";
        }
    }
}
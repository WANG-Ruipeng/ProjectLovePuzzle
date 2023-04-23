using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

static public class Archive
{
	public static bool hasLoad = false;
	public const int collectibleTypeCount = 5;
	static string xmlFilePath = Application.streamingAssetsPath + "Archive.xml";
	const string leveleProgressName = "LevelProgress";
	const string collectibleArrayName = "CollectibleArray";
	const string collectibleName = "Collectible";
	public static int LevelProgress => levelProgress;
	static int levelProgress = 0;
	public static CollectibleSaveInfo[] CollectiblesInfo => collectiblesInfo;
	static CollectibleSaveInfo[] collectiblesInfo;

	/// <summary>
	/// 存档文件是否存在
	/// </summary>
	static public bool Exist { get => File.Exists(xmlFilePath); }

	/// <summary>
	/// 从本地存档中读取存档信息到Archive类中
	/// </summary>
	static public void Load()
	{
		if (!Exist)
		{
			Debug.Log("没有生成过存档！已生成新存档！");
			Recreate();
		}
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.Load(xmlFilePath);
		XmlNode root = xmlDoc.SelectSingleNode("Root");
		//载入关卡进度
		levelProgress = int.Parse(root[leveleProgressName].InnerText);
		//载入收藏品信息
		XmlNode collectiblesRoot = root.SelectSingleNode(collectibleArrayName);
		collectiblesInfo = new CollectibleSaveInfo[collectibleTypeCount];
		XmlNodeList collectiblesList = collectiblesRoot.SelectNodes(collectibleName);
		for (int i = 0; i < collectibleTypeCount; i++)
		{
			bool unlocked = bool.Parse(collectiblesList[i].Attributes["Unlocked"].InnerText);
			collectiblesInfo[i] = new CollectibleSaveInfo(i, unlocked);
		}
		hasLoad = true;
	}
	static public void WriteCollectibleInfo(int id, bool unlocked)
	{
		//修改已读入的存档信息
		collectiblesInfo[id].unlocked = unlocked;
		//修改本地存档
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.Load(xmlFilePath);
		XmlNode collectiblesRoot = xmlDoc.SelectSingleNode("Root")[collectibleArrayName];
		XmlNodeList collectiblesList = collectiblesRoot.SelectNodes(collectibleName);
		collectiblesList[id].InnerText = unlocked.ToString();

	}
	static public void WriteLevelProgress(int levelProgress)
	{
		//修改已读入的存档信息
		Archive.levelProgress = levelProgress;
		//修改本地存档
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.Load(xmlFilePath);
		XmlElement xmlElement = (XmlElement)xmlDoc.SelectSingleNode("Root").SelectSingleNode(leveleProgressName);
		xmlElement.InnerText = levelProgress.ToString();
		xmlDoc.Save(xmlFilePath);
	}
	/// <summary>
	/// 重新生成存档，覆盖一切游戏进度
	/// </summary>
	static public void Recreate()
	{
		XmlTextWriter writer = new XmlTextWriter(xmlFilePath, System.Text.Encoding.UTF8);
		writer.Formatting = Formatting.Indented;
		writer.WriteStartDocument();
		writer.WriteStartElement("Root");
		writer.WriteElementString(leveleProgressName, "0");
		writer.WriteStartElement(collectibleArrayName);
		for (int i = 0; i < collectibleTypeCount; i++)
		{
			writer.WriteStartElement(collectibleName);
			writer.WriteAttributeString("ID", i.ToString());
			writer.WriteAttributeString("Unlocked", false.ToString());
			writer.WriteEndElement();
		}

		writer.WriteEndElement();
		writer.Close();
	}


}
public class CollectibleSaveInfo
{
	public int id;
	public bool unlocked;

	public CollectibleSaveInfo(int id, bool unlocked)
	{
		this.id = id;
		this.unlocked = unlocked;
	}
	public void SetData(int id, bool unlocked)
	{
		this.id = id;
		this.unlocked = unlocked;
	}
}
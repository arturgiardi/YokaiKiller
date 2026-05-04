using System.Collections;
using System.Collections.Generic;
using UnityEngine;	
using UnityEditor;
using TMPro;

//[CustomEditor(typeof(ItemDbManager))]
public class DataBaseInspector : EditorWindow
{
	[MenuItem ("Tools/Database Editor")]
	public static void  ShowWindow () {
        EditorWindow.GetWindow(typeof(DataBaseInspector));
    }
	public ItemDatabase itemDatabase;
	public TMP_SpriteAsset iconAsset;
	public Vector2 scrollPosition;
    void OnGUI ()
    {
		GUILayout.Space(10);
		itemDatabase = EditorGUILayout.ObjectField(itemDatabase, typeof(ItemDatabase), false) as ItemDatabase;
		iconAsset = EditorGUILayout.ObjectField(iconAsset, typeof(TMP_SpriteAsset), false) as TMP_SpriteAsset;
		GUILayout.BeginVertical(GUILayout.MaxWidth(Screen.width));
		GUILayout.Space(20);
		//GUILayout.BeginArea(new Rect(30,30,Screen.width,400));
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width-5), GUILayout.Height(Screen.height-115));
			int LastRowNumber = 0;
			int MaximumPerRow = Screen.width / 230;
			if(itemDatabase != null)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Space(10);
				for(int i = 0; i < itemDatabase.items.Count; i++)
				{
					DrawItemCard(itemDatabase.items[i]);
					LastRowNumber ++;
					if((LastRowNumber + 1) > MaximumPerRow)
					{
						LastRowNumber = 0;
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
						GUILayout.Space(10);
					}
				}
				GUILayout.EndHorizontal();
			}
        GUILayout.EndScrollView();
		//GUILayout.EndArea();
		if(GUILayout.Button("+ADD ITEM+"))
		{
			CreateItem();
		}
		GUILayout.EndVertical();
    }


	void DrawItemCard(Item item)
	{
		GUILayout.BeginVertical(GUILayout.Width(150), GUILayout.MinHeight(235));
			GUILayout.BeginHorizontal();
				EditorGUILayout.ObjectField(item, typeof(Item), false);
				if(GUILayout.Button("X", GUILayout.MaxWidth(20)))
				{
					DeleteItem(item);
				}
			GUILayout.EndHorizontal();
			//ID and Delete
			GUILayout.BeginHorizontal();
				GUILayout.Label("Item ID: " + item.id);
			GUILayout.EndHorizontal();

			//Name
			GUILayout.BeginHorizontal();
				GUILayout.Label("Name:", GUILayout.MaxWidth(40));
				item.iName = GUILayout.TextField(item.iName);
			GUILayout.EndHorizontal();

			//Description
			GUILayout.Label("Description:");
			item.iDesc = GUILayout.TextArea(item.iDesc, GUILayout.MinHeight(80), GUILayout.MaxHeight(80));

			//Icon and Others?
			item.iIconID = EditorGUILayout.IntField("sID", item.iIconID);
			GUILayout.BeginHorizontal();
				GUILayout.BeginVertical(GUILayout.MaxWidth(70));
					//item.iIcon = EditorGUILayout.ObjectField(item.iIcon, typeof(Sprite), false) as Sprite;
					//if(item.iIconID != null)
					//{
					DrawOnGUISprite(item.iIconID);
					//}
				GUILayout.EndVertical();
				GUILayout.BeginVertical();
					item.iType = (ItemType)EditorGUILayout.EnumPopup(item.iType);
					GUILayout.Label("Overrider:");
					item.equipBehaviour = EditorGUILayout.ObjectField(item.equipBehaviour, typeof(OnEquipBehaviour), false) as OnEquipBehaviour;
				GUILayout.EndVertical();
			GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.Space(20);
	}


	void DrawOnGUISprite(int spriteID)
	{
		Sprite aSprite = iconAsset.spriteInfoList[spriteID].sprite;
		Rect c = aSprite.rect;
		float spriteW = c.width;
		float spriteH = c.height;
		Rect rect = GUILayoutUtility.GetRect(spriteW, spriteH, GUILayout.MaxWidth(60), GUILayout.MaxHeight(60));
		if (Event.current.type == EventType.Repaint)
		{
			var tex = aSprite.texture;
			c.xMin /= tex.width;
			c.xMax /= tex.width;
			c.yMin /= tex.height;
			c.yMax /= tex.height;
			GUI.DrawTextureWithTexCoords(rect, tex, c);
		}
	}

	void CreateItem()
	{
		if(itemDatabase != null)
		{
			int entry = itemDatabase.lastEntry + 1;
			while(!itemDatabase.SetLastEntry(entry))
			{
				entry ++;
			}
			Item newItem = ScriptableObject.CreateInstance("Item") as Item;
			newItem.Init("Item: " + entry.ToString("0000"), entry);
			itemDatabase.items.Add(newItem);
			AssetDatabase.CreateAsset (newItem, "Assets/ScriptableObjects/ItemDb/" + entry.ToString("0000") + ".asset");
			AssetDatabase.SaveAssets ();
        	AssetDatabase.Refresh();
		}
	}

	void DeleteItem(Item item)
	{
		if(itemDatabase != null)
		{
			int itemIndex = itemDatabase.items.IndexOf(item);
			itemDatabase.items.Remove(item);
			AssetDatabase.DeleteAsset("Assets/ScriptableObjects/ItemDb/" + item.id + ".asset");
			AssetDatabase.SaveAssets ();
        	AssetDatabase.Refresh();
		}
	}

}

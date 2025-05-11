using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagistracyGame.Merge
{
	[CreateAssetMenu(menuName = "MergeGame/MergeGameData")]
	public class MergeGameData : ScriptableObject
	{
		[field: SerializeField, Header("Доступные элементы")] 
		public List<string> AvailableElements { get; private set; }

		[field: SerializeField, Header("Правила слияния")]
		public List<MergeRule> MergeRules { get; private set; }

		[field: SerializeField, Header("Финальные элементы")]
		public List<string> FinalElements { get; private set; }

		[field: SerializeField, Header("Иконки элементов")]
		public List<ElementIcon> ElementIcons { get; private set; }
	}

	[Serializable]
	public struct MergeRule
	{
		[field: SerializeField] public string Element1 { get; private set; }
		[field: SerializeField] public string Element2 { get; private set; }
		[field: SerializeField] public string Result { get; private set; }
	}

	[Serializable]
	public struct ElementIcon
	{
		[field: SerializeField] public string ElementName { get; private set; }
		[field: SerializeField] public Sprite Icon { get; private set; }
	}
}
using Unity.Entities;
using UnityEngine;

namespace Game
{
	public sealed class UnitCatalogAuthoring : MonoBehaviour
	{
		[SerializeField]
		private UnitCardsCatalog _catalog;

		private sealed class Baker : Baker<UnitCatalogAuthoring>
		{
			public override void Bake(UnitCatalogAuthoring authoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.None);
				DynamicBuffer<UnitCatalogItem> items = AddBuffer<UnitCatalogItem>(entity);

				DependsOn(authoring._catalog);

				if (authoring._catalog == null || authoring._catalog.Cards == null)
					return;

				foreach (UnitCardConfig card in authoring._catalog.Cards)
				{
					if (card == null || card.Prefab == null || card.Price < 0)
						continue;

					if (card.UnitType != CombatUnitType.Swordsman && card.UnitType != CombatUnitType.Archer)
						continue;

					bool duplicate = false;

					for (var i = 0; i < items.Length; i++)
					{
						if (items[i].unitType == card.UnitType)
						{
							duplicate = true;
							break;
						}
					}

					if (duplicate)
						continue;

					items.Add(new UnitCatalogItem
					{
						unitType = card.UnitType,
						prefab = GetEntity(card.Prefab, TransformUsageFlags.Dynamic),
						price = card.Price
					});
				}
			}
		}
	}
}

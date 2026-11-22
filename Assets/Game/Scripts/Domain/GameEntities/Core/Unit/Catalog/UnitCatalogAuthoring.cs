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

				foreach (UnitCardConfig card in authoring._catalog.Cards)
				{
					if (card.UnitType == UnitType.None)
						continue;

					if (items.TryGetItem(card.UnitType, out _))
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

using System.Collections.Generic;
using Content.Server.Atmos.Components;
using Content.Server.Atmos.EntitySystems;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using JetBrains.Annotations;
using Robust.Shared.GameObjects;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Set;

namespace Content.Server.Chemistry.ReagentEntityReactions
{
    [UsedImplicitly]
    public class ExtinguishReaction : ReagentEntityReaction
    {
        [DataField("reagents", true, customTypeSerializer:typeof(PrototypeIdHashSetSerializer<ReagentPrototype>))]
        // ReSharper disable once CollectionNeverUpdated.Local
        private readonly HashSet<string> _reagents = new ();

        protected override void React(EntityUid uid, ReagentPrototype reagent, FixedPoint2 volume, Solution? source, IEntityManager entityManager)
        {
            if (!entityManager.TryGetComponent(uid, out FlammableComponent? flammable) || !_reagents.Contains(reagent.ID)) return;

            var flammableSystem = EntitySystem.Get<FlammableSystem>();
            flammableSystem.Extinguish(uid, flammable);
            flammableSystem.AdjustFireStacks(uid, -1.5f, flammable);
        }
    }
}

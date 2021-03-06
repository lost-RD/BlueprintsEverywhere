using System;
using Verse;
using Verse.AI;
using RimWorld;
using CommunityCoreLibrary;

namespace BlueprintsEverywhere.Detour
{
	internal class _WorkGiver_ConstructDeliverResourcesToBlueprints : WorkGiver_ConstructDeliverResources
	{
		public override Job JobOnThing(Pawn pawn, Thing t)
		{
			//Log.Message("[BlueprintsEverywhere] JobOnThing: pawn: " + pawn.ToString() + ", thing: " + t.ToString());
			if (t.Faction != pawn.Faction) {
				return null;
			}
			Blueprint blueprint = t as Blueprint;
			if (blueprint == null) {
				return null;
			}
			Thing thing = blueprint.FirstBlockingThing(pawn, false);
			if (thing != null) {
				if (thing.def.category == ThingCategory.Plant) {
					if (pawn.CanReserveAndReach(thing, PathEndMode.ClosestTouch, pawn.NormalMaxDanger(), 1)) {
						return new Job(JobDefOf.CutPlant, thing);
					}
				}
				else {
					if (thing.def.category == ThingCategory.Item) {
						if (thing.def.EverHaulable) {
							return HaulAIUtility.HaulAsideJobFor(pawn, thing);
						}
						Log.ErrorOnce(string.Concat(new object[] {
							"Never haulable ",
							thing,
							" blocking ",
							t,
							" at ",
							t.Position
						}), 6429262);
					}
				}
				return null;
			}
			if (!GenConstruct.CanConstruct(blueprint, pawn)) {
				return null;
			}
			Job job = this._DeconstructExistingEdificeJob(pawn, blueprint);
			if (job != null) {
				return job;
			}
			Job job2 = base.ResourceDeliverJobFor(pawn, blueprint);
			if (job2 != null) {
				return job2;
			}
			Job job3 = this._NoCostFrameMakeJobFor(pawn, blueprint);
			if (job3 != null) {
				return job3;
			}
			return null;
		}
		
		internal Job _DeconstructExistingEdificeJob(Pawn pawn, Blueprint blue)
		{
			if (!blue.def.entityDefToBuild.IsEdifice())
			{
				return null;
			}
			Thing thing = null;
			CellRect cellRect = blue.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				int j = cellRect.minX;
				while (j <= cellRect.maxX)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					thing = c.GetEdifice();
					if (thing != null)
					{
						//Log.Message("[BlueprintsEverywhere] DeconstructExistingEdificeJob: Pawn: " + pawn.ToString() + ", Thing: " + thing.ToString() + ", Blueprint: " + blue.ToString());
						ThingDef thingDef = blue.def.entityDefToBuild as ThingDef;
						if (thingDef != null && thingDef.building.canPlaceOverWall && thing.def == ThingDefOf.Wall)
						{
							return null;
						}
						break;
					}
					else
					{
						j++;
					}
				}
				if (thing != null)
				{
					break;
				}
			}
			if (thing == null || !pawn.CanReserve(thing, 1))
			{
				return null;
			}
			
			string bluedef = blue.def.ToString();
			string tdef = thing.def.ToString();
			
			if (bluedef == "Wall_Blueprint" &&
				(
            		( tdef == "CollapsedRocks" )||
            		( tdef == "Sandstone" )||
            		( tdef == "Slate" )||
            		( tdef == "Marble" )||
            		( tdef == "Granite" )||
            		( tdef == "Limestone" )||
            		( tdef == "MineableSteel" )||
            		( tdef == "MineableSilver" )||
            		( tdef == "MineableGold" )||
            		( tdef == "MineableUranium" )||
            		( tdef == "MineablePlasteel" )||
            		( tdef == "MineableJade" )||
					( tdef == "MineableComponents" )
				)
			)
			{
				//Log.Message("[BlueprintsEverywhere] DeconstructExistingEdificeJob: blue.def is Wall_Blueprint and thing.def is stone, returning null");
				return null;
			}
			
			return new Job(JobDefOf.Deconstruct, thing)
			{
				ignoreDesignations = true
			};
		}
		
		internal Job _NoCostFrameMakeJobFor(Pawn pawn, IConstructible c)
		{
			if (c is Blueprint_Install) {
				return null;
			}
			if (c is Blueprint && c.MaterialsNeeded().Count == 0) {
				return new Job(JobDefOf.PlaceNoCostFrame) {
					targetA = (Thing)c
				};
			}
			return null;
		}
	}
}

using RimWorld;
using Verse;
using CommunityCoreLibrary;

namespace BlueprintsEverywhere.Detour
{

    internal static class _GenSpawn
    {
        
        // This method is to remove the hard-coded references allowing more flexibility in
        // building placements.  Specifically, it removes the steam geyser/geothermal generator
        // lock.
        internal static bool                _CanPlaceBlueprintOver( BuildableDef newDef, ThingDef oldDef )
        {
            if( oldDef.EverHaulable )
            {
                return true;
            }

            // Handle steam geysers in a mod friendly way (not geothermal exclusive)
            // By default, nothing can be placed on steam geysers without a place worker which allows it
            if( oldDef == ThingDefOf.SteamGeyser )
            {
                if( newDef.placeWorkers.NullOrEmpty() )
                {
                    // No place workers means nothing to allow it
                    return false;
                }
                if( newDef.placeWorkers.Contains( typeof( PlaceWorker_OnSteamGeyser ) ) )
                {
                    return true;
                }
                if( newDef.placeWorkers.Contains( typeof( PlaceWorker_OnlyOnThing ) ) )
                {
                    var Restrictions = newDef.RestrictedPlacement_Properties();
#if DEBUG
                    if( Restrictions == null )
                    {
                        CCL_Log.Error( "PlaceWorker_OnlyOnThing unable to get properties!", newDef.defName );
                        return false;
                    }
#endif
                    if( Restrictions.RestrictedThing.Contains( ThingDefOf.SteamGeyser ) )
                    {
                        return true;
                    }
                }
                return false;
            }

            ThingDef newThingDef = newDef as ThingDef;
            ThingDef oldThingDef = oldDef;
            BuildableDef buildableDef = GenSpawn.BuiltDefOf( oldDef );
            ThingDef resultThingDef = buildableDef as ThingDef;

            if(
                ( oldDef.category == ThingCategory.Plant )&&
                ( oldDef.passability == Traversability.Impassable )&&
                (
                    ( newThingDef != null )&&
                    ( newThingDef.category == ThingCategory.Building )
                )&&
                ( !newThingDef.building.canPlaceOverImpassablePlant )
            )
            {
                return false;
            }

            if(
                ( oldDef.category != ThingCategory.Building )&&
                ( !oldDef.IsBlueprint )&&
                ( !oldDef.IsFrame )
            )
            {
                return true;
            }


            if( newThingDef != null )
            {
                if( !EdificeUtility.IsEdifice( (BuildableDef) newThingDef ) )
                {
                    return
                        (
                            ( oldDef.building == null )||
                            ( oldDef.building.canBuildNonEdificesUnder )
                        )&&
                        (
                            ( !newThingDef.EverTransmitsPower )||
                            ( !oldDef.EverTransmitsPower )
                        );
                }
                if(
                    ( EdificeUtility.IsEdifice( (BuildableDef) newThingDef ) )&&
                    ( oldThingDef != null )&&
                    (
                        ( oldThingDef.category == ThingCategory.Building )&&
                        ( !EdificeUtility.IsEdifice( (BuildableDef) oldThingDef ) )
                    )
                )
                {
                    return
                        ( newThingDef.building == null )||
                        ( newThingDef.building.canBuildNonEdificesUnder );
                }
                if(
                    ( resultThingDef != null )&&
                    ( resultThingDef == ThingDefOf.Wall )&&
                    (
                        ( newThingDef.building != null )&&
                        ( newThingDef.building.canPlaceOverWall )
                    )||
                    ( newDef != ThingDefOf.PowerConduit )&&
                    ( buildableDef == ThingDefOf.PowerConduit )
                )
                {
                    return true;
                }
            }
            
            Log.Message("[BlueprintsEverywhere] Tried to place blueprint of " + newDef.ToString() + " onto " + oldDef.ToString() );
            
            return
                (
                    ( newDef is TerrainDef )&&
                    ( buildableDef is ThingDef )&&
                    ( ( (ThingDef) buildableDef ).CoexistsWithFloors )
                )||
                (
                    ( buildableDef is TerrainDef )&&
                    ( !( newDef is TerrainDef ) )
                )||
            	(
            		( newDef == ThingDefOf.Wall) &&
            		(
            			( oldDef.ToString() == "CollapsedRocks" )||
            			( oldDef.ToString() == "Sandstone" )||
            			( oldDef.ToString() == "Slate" )||
            			( oldDef.ToString() == "Marble" )||
            			( oldDef.ToString() == "Granite" )||
            			( oldDef.ToString() == "Limestone" )||
            			( oldDef.ToString() == "MineableSteel" )||
            			( oldDef.ToString() == "MineableSilver" )||
            			( oldDef.ToString() == "MineableGold" )||
            			( oldDef.ToString() == "MineableUranium" )||
            			( oldDef.ToString() == "MineablePlasteel" )||
            			( oldDef.ToString() == "MineableJade" )||
						( oldDef.ToString() == "MineableComponents" )
					)
				);
        }
        
        internal static bool _BlocksFramePlacement(Blueprint blue, Thing t)
		{
        	if (blue == t)
        	{
        		return false;
        	}
			if (t.def.category == ThingCategory.Plant) {
        		//Log.Message("[BlueprintsEverywhere] if (t.def.category == ThingCategory.Plant)");
				return t.def.plant.harvestWork > 200f;
			}
			if (blue.def.entityDefToBuild is TerrainDef || blue.def.entityDefToBuild.passability == Traversability.Standable) {
        		//Log.Message("[BlueprintsEverywhere] (blue.def.entityDefToBuild is TerrainDef || blue.def.entityDefToBuild.passability == Traversability.Standable)");
				return false;
			}
			if (blue.def.entityDefToBuild == ThingDefOf.GeothermalGenerator && t.def == ThingDefOf.SteamGeyser) {
        		//Log.Message("[BlueprintsEverywhere] (blue.def.entityDefToBuild == ThingDefOf.GeothermalGenerator && t.def == ThingDefOf.SteamGeyser)");
				return false;
			}
			ThingDef thingDef = blue.def.entityDefToBuild as ThingDef;
			if (thingDef != null) {
        		//Log.Message("[BlueprintsEverywhere] (thingDef != null)");
				if (thingDef.EverTransmitsPower && t.def == ThingDefOf.PowerConduit && thingDef != ThingDefOf.PowerConduit) {
        		//Log.Message("[BlueprintsEverywhere] (thingDef.EverTransmitsPower && t.def == ThingDefOf.PowerConduit && thingDef != ThingDefOf.PowerConduit)");
					return false;
				}
				if (t.def == ThingDefOf.Wall && thingDef.building != null && thingDef.building.canPlaceOverWall) {
        		//Log.Message("[BlueprintsEverywhere] (t.def == ThingDefOf.Wall && thingDef.building != null && thingDef.building.canPlaceOverWall)");
					return false;
				}
			}
			bool retval1 = t.def.IsEdifice();
			bool retval2 = thingDef.IsEdifice();
			bool retval3 = t.def.category == ThingCategory.Pawn;
			bool retval4 = t.def.category == ThingCategory.Item;
			bool retval5 = blue.def.entityDefToBuild.passability == Traversability.Impassable;
			bool retval6 = t.def.Fillage >= FillCategory.Partial;
			bool retval7 = thingDef.Fillage >= FillCategory.Partial;
			
			// so far I've had a prisoner wander into a wall as it was being built
			// he was Han Solo'd, RIP
			
			bool retbool =  (t.def.IsEdifice() && thingDef.IsEdifice()) || (t.def.category == ThingCategory.Pawn || (t.def.category == ThingCategory.Item && blue.def.entityDefToBuild.passability == Traversability.Impassable)) || (t.def.Fillage >= FillCategory.Partial && thingDef != null && thingDef.Fillage >= FillCategory.Partial);
			retbool = (retval1 && retval2) || (retval3 || (retval4 && retval5 )) || (retval6 && thingDef != null && retval7);
			
			//Log.Message("[BlueprintsEverywhere] _BlocksFramePlacement: "+t.ToString()+ (retbool ? " blocks" : " doesn't block") +" placement of "+blue.ToString());
			
			// above was the original code from Tynan, below will override it if the blueprint is a wall and the thing is rock
			
			string bluedef = blue.def.ToString();
			string tdef = t.def.ToString();
			
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
				//Log.Message("[BlueprintsEverywhere] Just building a wall on stone, don't mind me, nothing to see here...");
				retbool = false;
			}
			
			return retbool;
		}

    }

}

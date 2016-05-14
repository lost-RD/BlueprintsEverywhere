using System;
using System.Linq;
using System.Reflection;
using CommunityCoreLibrary;
using Verse;
using RimWorld;

namespace BlueprintsEverywhere
{

    public class DetourInjector : SpecialInjector
    {

        public override bool Inject()
        {
            // Detour Verse.GenSpawn.CanPlaceBlueprintOver
            MethodInfo Verse_GenSpawn_CanPlaceBlueprintOver = typeof( GenSpawn ).GetMethod( "CanPlaceBlueprintOver", BindingFlags.Static | BindingFlags.Public );
            MethodInfo BE_GenSpawn_CanPlaceBlueprintOver = typeof( Detour._GenSpawn ).GetMethod( "_CanPlaceBlueprintOver", BindingFlags.Static | BindingFlags.NonPublic );
            if( !Detours.TryDetourFromTo( Verse_GenSpawn_CanPlaceBlueprintOver, BE_GenSpawn_CanPlaceBlueprintOver ) )
			{
                return false;
			}
            
            // Detour RimWorld.GenConstruct.CanPlaceBlueprintAt
            MethodInfo RimWorld_GenConstruct_CanPlaceBlueprintAt = typeof( GenConstruct ).GetMethod( "CanPlaceBlueprintAt", BindingFlags.Static | BindingFlags.Public );
            MethodInfo BE_GenConstruct_CanPlaceBlueprintAt = typeof( Detour._GenConstruct ).GetMethod( "_CanPlaceBlueprintAt", BindingFlags.Static | BindingFlags.NonPublic );
            if( !Detours.TryDetourFromTo( RimWorld_GenConstruct_CanPlaceBlueprintAt, BE_GenConstruct_CanPlaceBlueprintAt ) )
			{
                return false;
			}
            
            // Detour RimWorld.GenConstruct.CanConstruct
            MethodInfo RimWorld_GenConstruct_CanConstruct = typeof( GenConstruct ).GetMethod( "CanConstruct", BindingFlags.Static | BindingFlags.Public );
            MethodInfo BE_GenConstruct_CanConstruct = typeof( Detour._GenConstruct ).GetMethod( "_CanConstruct", BindingFlags.Static | BindingFlags.NonPublic );
            if( !Detours.TryDetourFromTo( RimWorld_GenConstruct_CanConstruct, BE_GenConstruct_CanConstruct ) )
			{
                return false;
			}
            
            // Detour Verse.GenSpawn.BlocksFramePlacement
            MethodInfo Verse_GenSpawn_BlocksFramePlacement = typeof( GenSpawn ).GetMethod( "BlocksFramePlacement", BindingFlags.Static | BindingFlags.Public );
            MethodInfo BE_GenSpawn_BlocksFramePlacement = typeof( Detour._GenSpawn ).GetMethod( "_BlocksFramePlacement", BindingFlags.Static | BindingFlags.NonPublic );
            if( !Detours.TryDetourFromTo( Verse_GenSpawn_BlocksFramePlacement, BE_GenSpawn_BlocksFramePlacement ) )
			{
                return false;
			}

            // Detour RimWorld.WorkGiver_ConstructDeliverResourcesToBlueprints.JobOnThing
            MethodInfo RimWorld_WorkGiver_ConstructDeliverResourcesToBlueprints_JobOnThing = typeof( WorkGiver_ConstructDeliverResourcesToBlueprints ).GetMethod( "JobOnThing", BindingFlags.Instance | BindingFlags.Public );
            MethodInfo BE_WorkGiver_ConstructDeliverResourcesToBlueprints_JobOnThing = typeof( Detour._WorkGiver_ConstructDeliverResourcesToBlueprints ).GetMethod( "JobOnThing", BindingFlags.Instance | BindingFlags.Public );
            if( !Detours.TryDetourFromTo( RimWorld_WorkGiver_ConstructDeliverResourcesToBlueprints_JobOnThing, BE_WorkGiver_ConstructDeliverResourcesToBlueprints_JobOnThing ) )
			{
                return false;
			}
            /*
            // Detour RimWorld.WorkGiver_ConstructDeliverResourcesToBlueprints.DeconstructExistingEdificeJob
            MethodInfo RimWorld_WorkGiver_ConstructDeliverResourcesToBlueprints_DeconstructExistingEdificeJob = typeof( WorkGiver_ConstructDeliverResourcesToBlueprints ).GetMethod( "DeconstructExistingEdificeJob", BindingFlags.Public );
            MethodInfo BE_WorkGiver_ConstructDeliverResourcesToBlueprints_DeconstructExistingEdificeJob = typeof( Detour._WorkGiver_ConstructDeliverResourcesToBlueprints ).GetMethod( "_DeconstructExistingEdificeJob", BindingFlags.NonPublic );
            if( !Detours.TryDetourFromTo( RimWorld_WorkGiver_ConstructDeliverResourcesToBlueprints_DeconstructExistingEdificeJob, BE_WorkGiver_ConstructDeliverResourcesToBlueprints_DeconstructExistingEdificeJob ) )
			{
                return false;
			}
            */
            return true;
		}
	}
}
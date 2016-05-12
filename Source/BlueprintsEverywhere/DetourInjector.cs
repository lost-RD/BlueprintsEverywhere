using System;
using System.Linq;
using System.Reflection;
using CommunityCoreLibrary;
using Verse;

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
            
            return true;
		}
	}
}
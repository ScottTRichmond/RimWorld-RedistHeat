﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RedistHeat
{
    public class PlaceWorker_ExhaustPort : PlaceWorker
    {
        public override void DrawGhost( ThingDef def, IntVec3 center, Rot4 rot )
        {
            var vecNorth = center + IntVec3.North.RotatedBy( rot );
            if (!vecNorth.InBounds(base.Map))
            {
                return;
            }

            GenDraw.DrawFieldEdges( new List< IntVec3 >() {vecNorth}, Color.white );
            var room = vecNorth.GetRoom(base.Map);
            if (room == null || room.UsesOutdoorTemperature)
            {
                return;
            }
            GenDraw.DrawFieldEdges( room.Cells.ToList(), GenTemperature.ColorRoomHot );
        }

        public override AcceptanceReport AllowsPlacing( BuildableDef def, IntVec3 center, Rot4 rot, Thing thingToIgnore = null)
        {
            var vecNorth = center + IntVec3.North.RotatedBy( rot );
            var vecSouth = center + IntVec3.South.RotatedBy( rot );
            if (!vecSouth.InBounds(base.Map) || !vecNorth.InBounds(base.Map))
            {
                return false;
            }
            if (vecNorth.Impassable(base.Map))
            {
                return ResourceBank.ExposeHot;
            }

            var edifice = vecSouth.GetEdifice(base.Map);
            if (edifice == null || edifice.def != ThingDef.Named( "RedistHeat_IndustrialCooler" ))
            {
                return ResourceBank.AttachToCooler;
            }

            return true;
        }
    }
}
﻿using System;
using System.Threading.Tasks;
using Noskito.Logging;
using Noskito.World.Game;
using Noskito.World.Packet.Client.Player;
using Noskito.World.Packet.Server.Entities;
using Noskito.World.Processor.Extension;

namespace Noskito.World.Processor.Player
{
    public class WalkProcessor : PacketProcessor<Walk>
    {

        protected override async Task Process(WorldSession session, Walk packet)
        {
            var character = session.Character;

            if (character.Speed != packet.Speed)
            {
                Log.Debug("Incorrect character speed");
                return;
            }

            var distance = character.Position.GetDistance(new Position
            {
                X = packet.X,
                Y = packet.Y
            });

            if (distance > character.Speed / 2)
            {
                Log.Debug("Incorrect distance");
                return;
            }

            if ((packet.X + packet.Y) % 3 % 2 != packet.Checksum)
            {
                Log.Debug("Incorrect walk checksum");
                await session.Disconnect();
                return;
            }

            var travelTime = 2500 / packet.Speed * distance;
            if (travelTime > 1000 * 1.5)
            {
                Log.Debug("Incorrect travel time");
                await session.Disconnect();
                return;
            }

            character.Position = new Position
            {
                X = packet.X,
                Y = packet.Y
            };

            await character.Map.BroadcastMv(character);
        }
    }
}
import bf2
import host
import mm_utils
import math
import socket
import string
import bf2.PlayerManager
import datetime
import os
from bf2.stats.constants import *

__version__ = "0.0.1"

# Set the required module versions here
__required_modules__ = {
	'modmanager': 1.0
}
__supported_games__ = {
	'bf2': True
}

__supports_reload__ = True

__description__ = "Streams events to ptOS indirectly"

__author__ = "ArmedGuy"
class ptOS:
    def __init__(self, modManager):
        self._file = open("ptos_stream.log", "w")
        self.mm = modManager
        self.__state = 0
    
    def onPlayerConnect(self, player):
        self.writeEvent("joined", player, "")
    
    def onPlayerDisconnect(self, player):
        self.writeEvent("left", player, "")
    
    def onChat(self, playerId, text, channel, flags):
        if playerId != -1:
            player = bf2.playerManager.getPlayerByIndex(playerId)
            text = text.replace("HUD_CHAT_DEADPREFIX", "")
            if(channel == "Global"):
                self.writeEvent("sayall", player, "text=%s" % text)
            if(channel == "Team"):
                text = text.replace("HUD_TEXT_CHAT_TEAM", "")
                self.writeEvent("sayteam", player, "text=%s" % text)
            if(channel == "Squad"):
                text = text.replace("HUD_TEXT_CHAT_SQUAD", "")
                self.writeEvent("saysquad", player, "text=%s" % text)
        else:
            if(channel == "ServerMessage"):
                pass # todo
            
    
    def onPlayerKicked(self, player):
        self.writeEvent("gotkicked", player, "reason=Unspecified&admin=Server")
    def onPlayerBanned(self, player):
        self.writeEvent("gotbanned", player, "reason=Unspecified&admin=Server")
    
    def onPlayerKilled(self, victim, attacker, weapon, assists, victimSoldier):
        if not victim or not attacker or victim == attacker:
            return
        if victim.getTeam() == attacker.getTeam():
            self.writeEvent("gotteamkilled", victim, "client=%s" % attacker.getName())
            self.writeEvent("teamkilled", attacker, "target=%s" % victim.getName())
        else:
            self.writeEvent("gotkilled", victim, "client=%s" % attacker.getName())
            self.writeEvent("killed", attacker, "target=%s" % victim.getName())
    
    def init (self):
        if(self.__state == 0):
            host.registerHandler( 'PlayerConnect', self.onPlayerConnect, 1)
            host.registerHandler( 'PlayerDisconnect', self.onPlayerDisconnect, 1)
            host.registerHandler( 'ChatMessage', self.onChat, 1)
            host.registerHandler( 'PlayerKicked', self.onPlayerKicked, 1)
            host.registerHandler( 'PlayerBanned', self.onPlayerBanned, 1)
            host.registerHandler( 'PlayerKilled', self.onPlayerKilled)
            self.mm.debug(1, "ptOS inited")
        self.__state = 1
    def shutdown(self):
        self._file.close()
        self.__state == 2
    def update(self):
        pass
        
    def writeEvent(self, evt, player, args):
        self.mm.debug(1, "%s-;-%s-;-%s-;-%s-;-%s\n" % (evt, player.getProfileId(), player.getName(), player.getAddress(), args))
        self._file.write("%s-;-%s-;-%s-;-%s-;-%s\n" % (evt, player.getProfileId(), player.getName(), player.getAddress(), args))
        self._file.flush()
def mm_load(modManager):
    return ptOS(modManager)
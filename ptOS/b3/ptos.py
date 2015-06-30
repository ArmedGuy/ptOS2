
__author__ = "ArmedGuy"
__version__ = "0.1"
import os, sys, re, time, thread
import urllib, urllib2
from threading import Thread
import b3
import b3.events
class PtosPlugin( b3.plugin.Plugin ):
    def onStartup(self):
        self._adminPlugin = self.console.getPlugin('admin')
        if not self._adminPlugin:
          # something is wrong, can't start without admin plugin
          self.error('Could not find admin plugin')
          return False
        # XML config
        self._ptosLocation = self.config.get('ptos', 'url')
        self._serverGuid = self.config.get('ptos', 'server_guid')
        self._serverKey = self.config.get('ptos', 'server_key')
        self.debug("url " + self._ptosLocation)
        # listen for client events
        self.registerEvent(b3.events.EVT_CLIENT_SAY)
        self.registerEvent(b3.events.EVT_CLIENT_TEAM_SAY)
        self.registerEvent(b3.events.EVT_CLIENT_PRIVATE_SAY)
        self.registerEvent(b3.events.EVT_CLIENT_CONNECT)
        self.registerEvent(b3.events.EVT_CLIENT_DISCONNECT)
        self.registerEvent(b3.events.EVT_CLIENT_KICK)
        self.registerEvent(b3.events.EVT_CLIENT_BAN)
        
    def onEvent(self, event):
        if not event.client or event.client.cid == None:
            return
        if event.type == b3.events.EVT_CLIENT_SAY:
			self.send_event('sayall', event.client, { 'text': event.data })
        if event.type == b3.events.EVT_CLIENT_TEAM_SAY:
            self.send_event('sayteam', event.client, { 'text': event.data })
        if event.type == b3.events.EVT_CLIENT_CONNECT:
            self.send_event('joined', event.client)
        if event.type == b3.events.EVT_CLIENT_DISCONNECT:
            self.send_event('left', event.client)
        if event.type == b3.events.EVT_CLIENT_KICK:
            self.send_event('gotkicked', event.client, { 'reason': event.data })
        if event.type == b3.events.EVT_CLIENT_BAN:
            self.send_event('gotbanned', event.client, { 'reason': event.data['reason'], 'admin': event.data['admin'].name })
        
    def send_event(self, type, client, data={}):
        params = []
        self.debug(client)
        params.append(('ServerKey', self._serverKey))
        params.append(('ServerGuid', self._serverGuid))
        params.append(('PlayerName', client.name))
        params.append(('PlayerIp', client.ip))
        params.append(('PlayerGuid', client.guid))
        params.append(('EventType', type))
        i = 0
        for key in data:
            params.append(('EventData['+str(i)+'].Key', key))
            params.append(('EventData['+str(i)+'].Value', data[key]))
            i = i + 1
        
        rawData = urllib.urlencode(params)
        self.debug(rawData)
        req = urllib2.Request(self._ptosLocation, rawData)
        req.add_header("Content-type", "application/x-www-form-urlencoded")
        response = urllib2.urlopen(req)
        self.debug(response.read())
        response.close()
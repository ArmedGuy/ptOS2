import os, sys
import urllib, urllib2
from urlparse import parse_qs
class Client:
    guid = ""
    name = ""
    ip = ""
    def __init__(self, guid, name, ip):
        self.guid = guid
        self.name = name
        self.ip = ip
    
    def __str__(self):
        return "{Client: %s, %s, %s}" % (self.guid, self.name, self.ip)

class PtosStreamer:
    def __init__(self, location, guid, key, file):
        self._ptosLocation = location
        self._serverGuid = guid
        self._serverKey = key
        self.input = open(file, 'r')
        self.input.seek(0, os.SEEK_END)
        self._running = True
        
    
    def start(self):
        print "Starting stream loop"
        while self._running:
            lines = self.read()
            for line in lines:
                pt = line.split("-;-")
                e = pt[0]
                c = Client(pt[1], pt[2], pt[3])
                u = parse_qs(pt[4])
                
                print "Sending event %s with client %s, data: %s" % (e, str(c), str(u))
                self.send_event(e, c, u)
    
    def read(self):
        filestats = os.fstat(self.input.fileno())
        
        if self.input.tell() > filestats.st_size:
            self.input.seek(0, os.SEEK_END)
            print "Log has rotated, seeking to new end"
        return self.input.readlines()
     
    def send_event(self, type, client, data={}):
        params = []
        params.append(('ServerKey', self._serverKey))
        params.append(('ServerGuid', self._serverGuid))
        if client != None:
            params.append(('PlayerName', client.name))
            params.append(('PlayerIp', client.ip))
            params.append(('PlayerGuid', client.guid))
        params.append(('EventType', type))
        i = 0
        for key in data:
            params.append(('EventData['+str(i)+'].Key', key))
            params.append(('EventData['+str(i)+'].Value', data[key][0].strip()))
            i = i + 1
        
        rawData = urllib.urlencode(params)
        req = urllib2.Request(self._ptosLocation, rawData)
        req.add_header("Content-type", "application/x-www-form-urlencoded")
        response = urllib2.urlopen(req)
        response.close()

location = sys.argv[1]
guid = sys.argv[2]
key = sys.argv[3]
file = sys.argv[4]
s = PtosStreamer(location, guid, key, file)
s.start()
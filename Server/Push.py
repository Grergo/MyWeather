import requests
from ReadConfig import ReadConfig
import json
class Push():
    def __init__(self):
        config=ReadConfig.Read()
        self.AccessToken=None
        self.AppKey = config["Push"]["APPKey"]
        self.SID = config["Push"]["SID"]
        self.url = "https://login.live.com/accesstoken.srf"

    def Authorization(self):
        header={
            "Content-Type": "application/x-www-form-urlencoded"
        }
        data={
            "grant_type": "client_credentials",
            "client_id":self.SID,
            "client_secret":self.AppKey,
            "scope": "notify.windows.com"
        }
        AS=json.loads(requests.post(url=self.url,data=data,headers=header).text)
        return AS

    def PushTo(self,ChannelUrl, AS, Content):
        header={
            "Authorization": AS["token_type"]+" "+AS["access_token"],
            "X-WNS-Type": "wns/toast",
            "Content-Type":"text/xml",
            "Content-Length":len(Content)
        }
        return requests.post(ChannelUrl,data=Content,headers=header).status_code

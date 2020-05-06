import requests
import json
from ReadConfig import ReadConfig
import datetime
from DBUtil import SqlModel
from Push import Push
# 用于磁贴
class NowWeatherGet():
    def __init__(self):
        config = ReadConfig.Read()
        self.WeatherRoot = None
        self.Forecast = None
        self.NowURL = config["URL"]["NowURL"]
        self.ForecastURL = config["URL"]["ForecastURL"]
        self.key = config["Key"]
    def GetWeather(self,location):
        '''
            生成磁贴数据
        '''
        weekday = ["星期一", "星期二", "星期三", "星期四", "星期五", "星期六","星期日"]
        payload={
            "location":location,
            "key":self.key
        }
        data_now=json.loads(requests.get(self.NowURL,params=payload).text)
        data_forecast = json.loads(requests.get(self.ForecastURL, params=payload).text)
        with open("./template/Root.json",'r')as f:
            root=json.load(f)
        root["backimage"] =NowWeatherGet.getbackground(data_now["HeWeather6"][0]["now"]["cond_txt"])
        root["today"]=weekday[datetime.datetime.now().weekday()]
        root["now_weatherIcon"] = "ms-appx:///Resource/WeatherIcon/256/" + \
            data_now["HeWeather6"][0]["now"]["cond_code"]+".png"
        root["now_tmp"] = data_now["HeWeather6"][0]["now"]["tmp"]+"℃"
        root["today_maxtmp"] = data_forecast["HeWeather6"][0]["daily_forecast"][0]["tmp_max"]+"℃"
        root["today_mintmp"] = data_forecast["HeWeather6"][0]["daily_forecast"][0]["tmp_min"]+"℃"
        for i in data_forecast["HeWeather6"][0]["daily_forecast"]:
            Forecast={
                "day": weekday[datetime.datetime.strptime(i["date"], '%Y-%m-%d').weekday()],
                "weathericon": "ms-appx:///Resource/WeatherIcon/256/"+i["cond_code_d"]+".png",
                "tmp_max": i["tmp_max"]+"℃",
                "tmp_min": i["tmp_min"]+"℃"
            }
            root["forecast"].append(Forecast)
        root["today_tmp"] = data_forecast["HeWeather6"][0]["daily_forecast"][0]["tmp_max"] + \
            "℃ / " + \
            data_forecast["HeWeather6"][0]["daily_forecast"][0]["tmp_min"]+"℃"
        root["today_Rain"] = data_forecast["HeWeather6"][0]["daily_forecast"][0]["pop"]+"%的降水概率"
        root["today_wind"] = data_forecast["HeWeather6"][0]["daily_forecast"][0]["wind_dir"] + \
            ","+data_forecast["HeWeather6"][0]["daily_forecast"][0]["wind_sc"]+"级"
        root["location"] = data_now["HeWeather6"][0]["basic"]["location"]
        return json.dumps(root)
    @staticmethod
    def getbackground(des):
        '''
            根据天气确定背景图片，参数是天气描述
        '''
        if "雨" in des:
            if 6 < datetime.datetime.now().hour < 14:
                return "ms-appx:///Resource/background/201.jpg"
            elif 14 < datetime.datetime.now().hour < 19:
                return "ms-appx:///Resource/background/202.jpg"
            elif datetime.datetime.now().hour > 19 or datetime.datetime.now().hour  < 6:
                return "ms-appx:///Resource/background/203.jpg"
        elif "雪" in des:
            if 6 < datetime.datetime.now().hour < 14:
                return "ms-appx:///Resource/background/301.jpg"
            elif 14 < datetime.datetime.now().hour < 19:
                return "ms-appx:///Resource/background/302.jpg"
            elif datetime.datetime.now().hour > 19 or datetime.datetime.now().hour < 6:
                return "ms-appx:///Resource/background/303.jpg"
        else:
            if 6 < datetime.datetime.now().hour  < 14:
                return "ms-appx:///Resource/background/101.jpg"
            elif 14 < datetime.datetime.now().hour < 19:
                return "ms-appx:///Resource/background/102.jpg"
            elif datetime.datetime.now().hour > 19 or datetime.datetime.now().hour < 6:
                return "ms-appx:///Resource/background/103.jpg"
# 用于未来3小时降雨提醒
class HourlyWeatherGet():
    def __init__(self):
        config = ReadConfig.Read()
        self.hourlyURL = config["URL"]["HourlyURL"]
        self.key = config["Key"]
        self.toast='<toast><visual><binding template="ToastGeneric"><text>降雨提醒</text><text>未来三小时内有降雨，出门要记得带伞！</text></binding></visual></toast>'.encode("utf-8")
    def HourlyWeather(self):
        sql=SqlModel()
        locations=set(sql.select_location(None))
        for l in locations:
            if l!="*":
                payload = {
                    "location": l,
                    "key": self.key
                }
                date=json.loads(requests.get(self.hourlyURL,params=payload).text)
                if ("雨" in date["HeWeather6"][0]["hourly"][0]["cond_txt"]) or ("雪" in date["HeWeather6"][0]["hourly"][0]["cond_txt"]):
                    pushurl=sql.select_URL(l)
                    P=Push()
                    AS=P.Authorization()
                    for u in pushurl:
                        P.PushTo(u,AS,self.toast)
class TomorrowWeatherGet():
    def __init__(self):
        config = ReadConfig.Read()
        self.ForecastURL = config["URL"]["ForecastURL"]
        self.key = config["Key"]
        self.toast ='<toast ><visual><binding template="ToastGeneric"><text>明日天气</text><text>{content}</text></binding></visual></toast>'
    def GetTomorrorweather(self):
        sql = SqlModel()
        locations = set(sql.select_location(None))
        for l in locations:
            if l!='*':
                payload = {
                    "location": l,
                    "key": self.key
                }
                date = json.loads(requests.get(self.ForecastURL, params=payload).text)
                if ("雨" in date["HeWeather6"][0]["daily_forecast"][1]["cond_txt_d"]) or ("雪" in date["HeWeather6"][0]["daily_forecast"][0]["cond_txt_d"]):
                    content = f"明天白天有{date['HeWeather6'][0]['daily_forecast'][1]['cond_txt_d']}，出门记得带伞，明天最高温度{date['HeWeather6'][0]['daily_forecast'][1]['tmp_max']}℃，最低温度{date['HeWeather6'][0]['daily_forecast'][1]['tmp_min']}℃"
                else:
                    content = f"明天白天{date['HeWeather6'][0]['daily_forecast'][1]['cond_txt_d']}，明天最高温度{date['HeWeather6'][0]['daily_forecast'][1]['tmp_max']}℃，最低温度{date['HeWeather6'][0]['daily_forecast'][1]['tmp_min']}℃"
                self.toast.format(content=content)
                pushurl = sql.select_URL(l)
                P = Push()
                AS = P.Authorization()
                for u in pushurl:
                    P.PushTo(u, AS, self.toast.encode("utf-8"))

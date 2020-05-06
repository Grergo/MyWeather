from flask import Flask, request, render_template,session, redirect, url_for, current_app
from DBUtil import SqlModel
from WeatherInfo import NowWeatherGet,TomorrowWeatherGet,HourlyWeatherGet
from apscheduler.schedulers.background import BackgroundScheduler
import multiprocessing
import base64
app=Flask(__name__)

@app.route('/UWP/Weather',methods=['GET'])
def Tile():
    location=request.args.get("location")
    N=NowWeatherGet()
    return N.GetWeather(location)


@app.route('/UWP/INFO',methods=['POST'])
def Updatelocation():
    url=request.form['noticeurl']
    location = request.form['location']
    DB=SqlModel()
    DB.update_location(url,location)
    return "OK"


@app.route('/UWP/URL',methods=['POST'])
def NoticeURL():
    DB=SqlModel()
    new_url = str(base64.b64decode(request.form['newurl']),encoding='utf-8')
    
    old_url = str(base64.b64decode(request.form['oldurl']), encoding='utf-8')
    location = request.form['location']
    if old_url=="*":
        DB.insert_User(new_url,location)
    else:
        DB.update_url(old_url,new_url)
    return "OK"

if __name__ == "__main__":
    scheduler = BackgroundScheduler()
    T=TomorrowWeatherGet()
    R=HourlyWeatherGet()
    scheduler.add_job(T.GetTomorrorweather, 'cron',day_of_week='0-6', hour='17', minute='0')
    scheduler.add_job(R.HourlyWeather, 'interval', minutes=30)
    scheduler.start()
    app.run(host='127.0.0.1',port=5001,debug=False)








import pymysql
from ReadConfig import ReadConfig
class SqlModel():
    def __init__(self):
        config=ReadConfig.Read()
        self.address = config["DataBase"]["address"]
        self.name=config["DataBase"]["name"]
        self.passwd = config["DataBase"]["passwd"]
        self.DBName = config["DataBase"]["DBName"]
    def insert_User(self,url,location):
        '''
            插入用户信息，url+location
        '''
        db=pymysql.connect(self.address,self.name,self.passwd,self.DBName)
        cursor=db.cursor()
        sql=f"insert into user_info(url,location) values('{url}','{location}')"
        try:
            cursor.execute(sql)
            print(sql)
            db.commit()
        except Exception as e:
            db.rollback()
            print(e)
        db.close()
    def select_location(self,url):
        '''
            查询位置信息，url为None时查询所有位置，以List形式返回
        '''
        db = pymysql.connect(self.address, self.name, self.passwd, self.DBName)
        cursor = db.cursor()
        if(url!=None):
            sql = f"select location from user_info where url='{url}'"
        else:
            sql = f"select location from user_info"
        cursor.execute(sql)
        results = cursor.fetchall()
        locations=[]
        for i in results:
            locations.append(i[0])
        db.close()
        return locations

    def select_URL(self, location):
        '''
            查询URL信息，location为None时查询所有位置，以List形式返回
        '''
        db = pymysql.connect(self.address, self.name, self.passwd, self.DBName)
        cursor = db.cursor()
        if(location != None):
            sql = f"select url from user_info where location='{location}'"
        else:
            sql = f"select url from user_info"
        cursor.execute(sql)
        results = cursor.fetchall()
        url = []
        for i in results:
            url.append(i[0])
        db.close()
        return url
    def update_url(self,old_url,new_url):
        '''
            更新url，old_url,new_url
        '''
        db = pymysql.connect(self.address, self.name, self.passwd, self.DBName)
        cursor = db.cursor()
        sql = f"update user_info set url = '{new_url}' where url = '{old_url}'"
        try:
            cursor.execute(sql)
            db.commit()
        except Exception as e:
            db.rollback()
            print(e)
        db.close()
    def update_location(self, url, new_location):
        '''
            更新位置信息，url，location
        '''
        db = pymysql.connect(self.address, self.name, self.passwd, self.DBName)
        cursor = db.cursor()
        sql = f"update user_info set location = '{new_location}' where url = '{url}'"
        try:
            cursor.execute(sql)
            db.commit()
        except Exception as e:
            db.rollback()
            print(e)
        db.close()

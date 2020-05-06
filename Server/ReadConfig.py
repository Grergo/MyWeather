import yaml
class ReadConfig():
    def __init__(self):
        pass
    @staticmethod
    def Read():
        '''
        读取配置文件
        '''
        with open("./config.yaml", 'r')as f:
            config = yaml.load(f.read(), Loader=yaml.SafeLoader)
        return config

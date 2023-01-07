import sqlite3

import matplotlib
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import seaborn as sns

from sklearn.model_selection import train_test_split
from sklearn.linear_model import LinearRegression
from sqlite3 import Error

# создание подключения
def create_connection(path):
    connection = None
    try:
        connection = sqlite3.connect(path)
        print("Connection to SQLite DB successful")
    except Error as e:
        print(f"The error '{e}' occurred")

    return connection

# подключение
connection = create_connection("C:\\Users\\nidro\\Projects\\UnityProjects\\Allusion-to-Adventure\\Database.db")

# получить таблицы базы данных
request = "SELECT * FROM [Time];"
time_df = pd.read_sql(request, connection)
request = "SELECT * FROM [World];"
world_df = pd.read_sql(request, connection)
request = "SELECT * FROM [WorldStocks];"
world_stocks_df = pd.read_sql(request, connection)

# получить базу данных
df = world_df.join(time_df.set_index("Id"), on="TimeID").join(world_stocks_df.set_index("Id"), on="WorldStocksID")
df = df.drop(["Id", "UserID", "TimeID", "WorldStocksID"], axis=1)
print("Dataframe:\n", df)

# построение и обучение модели
features = df[["Peaceful", "Swordsman", "Woodman", "Enemy", "Day", "Month", "Year", "Sword", "Armor", "HealingPotion",
               "Axe", "Logs", "Trees"]]
result = df["Coins"]

print("Features:\n", features)
print("Result:\n", result)

features_train, features_test, result_train, result_test = train_test_split(features, result, test_size=0.3)
model = LinearRegression()
model.fit(features_train, result_train)
predictions = model.predict(features_test)

# результат предсказания
print("То что должно получиться:\n", result_test)
print("То что получилось:\n", predictions)
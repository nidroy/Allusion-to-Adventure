import sqlite3
import pandas as pd
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
df = world_df.join(time_df.set_index("Id"), on = "TimeID").join(world_stocks_df.set_index("Id"), on = "WorldStocksID")
df = df.drop(["Id", "UserID", "TimeID", "WorldStocksID"], axis = 1)
print(df.to_string())

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExReaderPlus.Manage;
using ExReaderPlus.Manage.PassageManager;
//using exReader.PassageManager;
using ExReaderPlus.WordsManager;
using Microsoft.Data.Sqlite;

namespace ExReaderPlus.DatabaseManager
{
    class UserDataDB
    {
        public static UserDataDB instance;
        SqliteConnection db;
        public UserDataDB()
        {

            var tt = new SqliteConnectionStringBuilder("Data Source=:memory:;");
            //传递参数使之驻留在内存
            db = new SqliteConnection("filename=userdata.db");
            //db = new SqliteConnection("Data Source=:memory:;");
            db.Open();
            var command = new SqliteCommand();
            command.Connection = db;
            try
            {
                command.CommandText = "CREATE TABLE newword(word varchar(64), rmb integer)";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE UNIQUE INDEX newword_word ON newword(word)";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE articles(title text, content text)";
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {

            }
            //ttt
            /*
            var v = new Vocabulary();
            v.Word = "aaa";
            v.YesorNo = 1;
            List<Vocabulary> l = new List<Vocabulary>();
            l.Add(v);
            SaveWordBook(l);
            var ta = GetRmbStatus("aaa");
            */
        }
        public List<String> FetchWord()
        {
            List<String> words = new List<string>();

            var command = new SqliteCommand();
            command.Connection = db;

            command.CommandText = "SELECT word FROM newword";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                words.Add(reader.GetString(0));
            }
            reader.Close();
            return words;
        }
        public bool GetRmbStatus(String word)
        {
            var command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "SELECT rmb FROM newword WHERE word = '" + word + "'";
            var reader = command.ExecuteReader();
            bool r;
            if (reader.Read())
            {
                var s = reader.GetString(0);
                r = (s == "1");
            }
            else
            {
                r = false;
            }
            reader.Close();
            return r;

        }
        public void DeleteWord(String Word)
        {
            var command = new SqliteCommand
            {
                Connection = db,
                CommandText = "DELETE FROM newword WHERE word = '" + Word + "'"
            };
            command.ExecuteNonQuery();
        }
        public void SaveWordBook(List<Vocabulary> book)
        {
            var command = new SqliteCommand();
            command.Connection = db;
            foreach (Vocabulary v in book)
            {
                command.CommandText = "INSERT OR REPLACE INTO newword VALUES('" + v.Word + "', '" + v.YesorNo + "')";
                command.ExecuteNonQuery();
            }
        }
  
        public void SavaPassage(Passage passage)
        {
            var command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "INSERT INTO articles (title,content) VALUES ('" + passage.HeadName + "','" + passage.Content + "')";

            command.ExecuteNonQuery();
        }
        public List<Passage> LoadPassage()
        {
            List<Passage> myPassages = new List<Passage>();
            var command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "SELECT title FROM articles";

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Passage passage = new Passage();
                passage.HeadName = reader.GetString(0);
                myPassages.Add(passage);
            }

            // >>> TANG HAO ADD
            command.CommandText = "SELECT content FROM articles";
            var reader2 = command.ExecuteReader();
            int length = 0;
            while (reader2.Read() && length < myPassages.Count)
            {
                myPassages[length].Content = reader2.GetString(0);
                length++;
            }
            reader2.Close();
            // <<< TANG HAO ADD

            reader.Close();

            return myPassages;

        }

        //  public void ClearPassages() { }


        public void DeletePassage(string title)
        {
            var command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "DELETE FROM articles WHERE title = " + "'" + title + "'";
            command.ExecuteNonQuery();
        }
        
    }
}

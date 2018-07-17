using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExReaderPlus.WordsManager;
namespace ExReaderPlus.DataManage
{

        /* 单词属性规定

        public string Word { get; set; }            // "book"
        public string Translation { get; set; }     // "n. 书；v. 预订"
               //public string pronunciation { get; set; } //   音标待定  "/`boo:k/"
        public int Classification { get; set; }    // "CET4"
        public int YesorNo { get; set; }            // "0"

        */
        public class WordManage
        {
            public static char[] spc = new char[] { ' ', ',', '.', '?', '!', '\'', '\"', '=', '\\', '/' };
            public static WordManage instance;
            SQLiteConnection dbfile;
            SQLiteConnection db;
            public WordManage()
            {
                //文件词库数据库
                string path = Path.GetFullPath("db/dic.db");
                dbfile = new SQLiteConnection("Data Source=" + path + ";Version=3;");
                dbfile.Open();
                //内存词库数据库
                string str = "Data Source=:memory:;Version=3;New=True;";
                db = new SQLiteConnection(str);
                db.Open();
                //数据库读入内存
                dbfile.BackupDatabase(db, "main", "main", -1, null, 0);
                //command对象
                var command = new SQLiteCommand();
                command.Connection = db;
                //关闭文件词库数据库
                dbfile.Close();
                dbfile = null;
                //建立查询文章表
                command.CommandText = "CREATE TABLE wordset AS SELECT word FROM stardict WHERE \'1\' = \'2\'";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE UNIQUE INDEX wordset_word ON wordset(word)";
                command.ExecuteNonQuery();
                //建立缓存词表
                command.CommandText = "CREATE TABLE wordcache AS SELECT word FROM stardict WHERE \'1\' = \'2\'";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE UNIQUE INDEX wordcache_word ON wordcache(word)";
                command.ExecuteNonQuery();
                //ttt
                //QueryWord("have have have if if base base base go usage able technology","cet4");

                //CacheAddWord("master");
                //CacheAddText("a may may what what what science propaganda senior husband");
                //List<Vocabulary> l = VocabularyFiltCacheAnti("cet4 cet6 ky");
            }
            public void CacheAddList(List<Vocabulary> vocabularies)
            {
                var command = new SQLiteCommand();
                command.Connection = db;
                foreach (Vocabulary v in vocabularies)
                {
                    command.CommandText = "INSERT OR IGNORE INTO wordcache VALUES (\'" + v.Word + "\')";
                    command.ExecuteNonQuery();
                }
            }
            public void CacheAddList(List<String> book)
            {
                var command = new SQLiteCommand();
                command.Connection = db;
                foreach (String v in book)
                {
                    command.CommandText = "INSERT OR IGNORE INTO wordcache VALUES (\'" + v + "\')";
                    command.ExecuteNonQuery();
                }
            }
            public void CacheClear()
            {
                SQLiteCommand command = new SQLiteCommand();
                command.Connection = db;
                command.CommandText = "DELETE FROM wordcache WHERE \'1\' = \'1\'";
                command.ExecuteNonQuery();
            }
            public void CacheAddWord(String Word)
            {
                Word = Word.Split(spc)[0];
                SQLiteCommand command = new SQLiteCommand();
                command.Connection = db;
                command.CommandText = "INSERT OR IGNORE INTO wordcache VALUES (\'" + Word + "\')";
                command.ExecuteNonQuery();
            }
            public void CacheAddText(String Text)
            {
                String[] splitedtext = Text.Split(spc);
                SQLiteCommand command = new SQLiteCommand();
                command.Connection = db;
                //添加词语
                foreach (String aword in splitedtext)
                {
                    command.CommandText = "INSERT OR IGNORE INTO wordcache VALUES (\'" + aword + "\')";
                    command.ExecuteNonQuery();
                }
            }
            public List<Vocabulary> VocabularyFiltCache(String TypeSet)
            {
                var vocabularies = new List<Vocabulary>();
                String[] Types = TypeSet.Split(spc);
                SQLiteCommand command = new SQLiteCommand();
                command.Connection = db;

                command.CommandText =
                    "SELECT wordcache.word,stardict.translation FROM wordcache,stardict WHERE wordcache.word = stardict.word AND (\'1\'=\'2\'";
                foreach (String Type in Types)
                {
                    command.CommandText += " OR stardict.tag LIKE \'%" + Type + "%\'";
                }
                command.CommandText += ")";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var v = new Vocabulary();
                    v.Word = reader.GetString(0);
                    v.Translation = reader.GetString(1);
                    vocabularies.Add(v);
                }
                reader.Close();
                return vocabularies;
            }
            public List<Vocabulary> VocabularyFiltCacheAnti(String TypeSet)
            {
                var vocabularies = new List<Vocabulary>();
                String[] Types = TypeSet.Split(spc);
                SQLiteCommand command = new SQLiteCommand();
                command.Connection = db;

                command.CommandText =
                    "SELECT wordcache.word,stardict.translation FROM wordcache,stardict WHERE wordcache.word = stardict.word ";
                foreach (String Type in Types)
                {
                    command.CommandText += " AND NOT stardict.tag LIKE \'%" + Type + "%\'";
                }
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var v = new Vocabulary();
                    v.Word = reader.GetString(0);
                    v.Translation = reader.GetString(1);
                    vocabularies.Add(v);
                }
                reader.Close();
                return vocabularies;
            }

            public List<Vocabulary> QueryWord(String text, String Type)
            {
                //Type can be   zk gk cet4 cet6 toefl gre ielts ky
                //如果查询的类别不是这些考试类别之一则报错
                if (Type != "zk" && Type != "gk" && Type != "cet4" && Type != "cet6" && Type != "toefl" && Type != "gre" && Type != "ielts" && Type != "ky")
                {
                    throw new Exception("Test type " + Type + " not supported.");
                }
                String[] splitedtext = text.Split(spc);
                List<Vocabulary> vocabularies = new List<Vocabulary>();
                //开始添词
                SQLiteCommand command = new SQLiteCommand();
                command.Connection = db;
                foreach (String aword in splitedtext)
                {


                    command.CommandText = "INSERT OR IGNORE INTO wordset VALUES (\'" + aword + "\')";
                    command.ExecuteNonQuery();
                }
                //词语连接
                command.CommandText =
                    "SELECT wordset.word,stardict.translation FROM wordset,stardict WHERE wordset.word = stardict.word AND stardict.tag LIKE \'%" + Type + "%\'";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Vocabulary v = new Vocabulary();
                    v.Word = reader.GetString(0);
                    v.Translation = reader.GetString(1);
                    vocabularies.Add(v);
                }
                reader.Close();
                //最后删词
                command.CommandText = "DELETE FROM wordset WHERE \'1\'=\'1\'";
                command.ExecuteNonQuery();
                return vocabularies;
            }
        }

  }


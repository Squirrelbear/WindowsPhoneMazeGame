using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.IO;

// for Application.StartupPath
using System.Reflection;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO.IsolatedStorage;
namespace WindowsMazeGame
{
    public class DatabaseHandler
    {
        private List<HighScore> scores;
        private const string fileName = "HighScores.xml";

        public DatabaseHandler()
        {
            scores = LoadScores();
        }

        public void addScore(HighScore score)
        {
            int maxID = 0;
            foreach(HighScore s in scores)
            {
                if(s.getID() > maxID) 
                {
                    maxID = s.getID();
                }
            }

            if(maxID != 0)
                maxID++;
            score.setID(maxID);

            scores.Add(score);
            SaveScores(scores);
        }

        public List<HighScore> getScores(int mapID)
        {
            List<HighScore> result = new List<HighScore>();

            foreach (HighScore score in scores)
            {
                if (score.getMap() == mapID)
                {
                    result.Add(score);
                }
            }

            return result;
        }

        public void deleteScore(int scoreID)
        {
            HighScore removeS = null;
            foreach (HighScore score in scores)
            {
                if (score.getID() == scoreID)
                {
                    removeS = score;
                    break;
                }
            }
            if (removeS != null)
            {
                scores.Remove(removeS);
                SaveScores(scores);
            }
        }

        public void clearDatabase()
        {
            scores = new List<HighScore>();
            SaveScores(scores);
        }

        // http://blogs.msdn.com/b/dawate/archive/2010/08/31/windows-phone-7-xml-isolatedstorage-example.aspx
        private List<HighScore> LoadScores()
        {
            List<HighScore> scores = new List<HighScore>();
            TextReader reader = null;

            try
            {
                IsolatedStorageFile isoStorage = IsolatedStorageFile.GetUserStoreForApplication();
                IsolatedStorageFileStream file = isoStorage.OpenFile(fileName, FileMode.OpenOrCreate);
                reader = new StreamReader(file);

                XmlSerializer xs = new XmlSerializer(typeof(List<HighScore>));
                scores.AddRange((List<HighScore>)xs.Deserialize(reader));

                reader.Close();
            }
            catch
            {
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
            }

            return scores;

        }

        private void SaveScores(List<HighScore> scores)
        {
            TextWriter writer = null;

            try
            {
                IsolatedStorageFile isoStorage = IsolatedStorageFile.GetUserStoreForApplication();
                IsolatedStorageFileStream file = isoStorage.OpenFile(fileName, FileMode.Create);
                writer = new StreamWriter(file);

                XmlSerializer xs = new XmlSerializer(typeof(List<HighScore>));
                xs.Serialize(writer, scores);

                writer.Close();
            }
            catch
            {
            }
            finally
            {
                if (writer != null)

                    writer.Dispose();
            }

        }

    }
}

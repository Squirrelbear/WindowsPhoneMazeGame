using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsMazeGame
{
    public class HighScore
    {
        public int id;
        public float score;
        public String name;
        public int map;

        public HighScore()
        {
        }

        public HighScore(int id, int map, float score, String name)
        {
            this.id = id;
            this.score = score;
            this.name = name;
            this.map = map;
        }

        public HighScore(int map, float score, String name)
        {
            this.map = map;
            this.score = score;
            this.name = name;
        }

        public int getID()
        {
            return id;
        }

        public void setID(int id)
        {
            this.id = id;
        }

        public float getScore()
        {
            return score;
        }

        public void setScore(float score)
        {
            this.score = score;
        }

        public String getName()
        {
            return name;
        }

        public void setName(String name)
        {
            this.name = name;
        }

        public int getMap()
        {
            return map;
        }

        public void setMap(int map)
        {
            this.map = map;
        }
    }
}

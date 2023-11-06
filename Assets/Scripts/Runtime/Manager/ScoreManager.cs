using System;

namespace Runtime.Manager
{
    public class ScoreManager
    {
        public Action<int> OnPointGained;
        
        private int _score;

        public ScoreManager()
        {
            _score = 0;

            OnPointGained += IncreaseScore;
        }

        private void IncreaseScore(int point)
        {
            _score += point;
        }
    }
}
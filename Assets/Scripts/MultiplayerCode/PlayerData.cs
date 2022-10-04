using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MudPuppyGames.CardGame
{
    public class PlayerData : MonoBehaviour
    {
        private string _nickName = null;

        public string Nickname
        {
            get
            {
                if (string.IsNullOrEmpty(_nickName))
                    _nickName = RandomizeNickname();

                return _nickName;
            }

            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _nickName = RandomizeNickname();
                else
                    _nickName = value;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            var count = FindObjectsOfType<PlayerData>().Length;
            if(count > 1)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        public string RandomizeNickname()
        {
            return "Player" + Random.Range(1,1000);
        }
    }

}

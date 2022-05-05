using UnityEngine;

namespace Notion.Unity
{
    [CreateAssetMenu]
    public class Device : ScriptableObject
    {
        [SerializeField]
        private string _email ="simon.mcmanis@gmail.com";

        [SerializeField]
        private string _password = "SdM3wTu@&Tt^%4";

        [SerializeField]
        private string _deviceId = "11bd87018da80c924a01602bbf3bb209";

        public string Email => _email;
        public string Password => _password;
        public string DeviceId => _deviceId;

        public bool IsValid => 
            !string.IsNullOrEmpty(_email) && 
            !string.IsNullOrEmpty(_password) && 
            !string.IsNullOrEmpty(DeviceId);
    }
}
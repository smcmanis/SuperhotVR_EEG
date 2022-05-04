using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Notion.Unity;

namespace SuperhotVR_EEG
{
    public class NotionGodClass : MonoBehaviour
    {
        public Device _device;

        public FirebaseController _controller;
        public Notion.Unity.Notion _notion;

        public NotionGodClass() { }
        public Device getDevice()
        {
            if (_device == null)
            {
                _device = new Device();
            }
            return _device;
        }
        public void OnEnable()
        {
            if (_device == null)
            {
                _device = this.getDevice();
                Login();

                return;
            }
            if (!_device.IsValid)
            {
                return;
            } 

        }

        public async Task Login()
        {
            _controller = new FirebaseController();
            await _controller.Initialize();
            _notion = new Notion.Unity.Notion(_controller);
            await _notion.Login(_device);

        }

        public async void Logout()
        {
            await _notion.Logout();
            _controller = null;
            _notion = null;
        }

        public async void GetDevices()
        {
            if (!_notion.IsLoggedIn) return;
            var devices = await _notion.GetDevices();
        }

        public void GetStatus()
        {
            if (!_notion.IsLoggedIn)
            {
                return;
            }
        }

        public void SubscribeCalm()
        {
            if (!_notion.IsLoggedIn) return;
            _notion.Subscribe(new CalmHandler());
        }

        public void SubscribeFocus()
        {
            if (!_notion.IsLoggedIn) return;
            _notion.Subscribe(new FocusHandler());
        }

        public void SubscribeBrainwaves()
        {
            if (!_notion.IsLoggedIn) return;
            _notion.Subscribe(new BrainwavesRawHandler());
        }

        public void SubscribeAccelerometer()
        {
            if (!_notion.IsLoggedIn) return;
            _notion.Subscribe(new AccelerometerHandler());
        }

        /// <summary>
        /// Add kinesisLabel based on the thought you're training.
        /// For instance: leftArm, rightArm, leftIndexFinger, etc
        /// </summary>
        /// <param name="kinesisLabel"></param>
        public void SubscribeKinesis(string kinesisLabel)
        {
            if (!_notion.IsLoggedIn) return;

            _notion.Subscribe(new KinesisHandler
            {
                Label = kinesisLabel,
                
            });
        }

        private async void OnDisable()
        {
            if (_notion == null) return;
            if (!_notion.IsLoggedIn) return;

            // Wrapping because Logout is meant to be invoked and forgotten about for use in button callbacks.
            await Task.Run(() => Logout());
        }
    }
}

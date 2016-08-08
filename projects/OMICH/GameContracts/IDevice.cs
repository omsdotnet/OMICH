using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameContracts
{
    public interface IDevice
    {
        void Prepare();
        void PrepareContent(IEnumerable<ContentItem> items);
        void Draws(String colorHex, IEnumerable<ContentDrawable> items);
        void PlayBackgroundMusic(String name);
        bool IsPlayBackgroundMusic();
        void PrepareBackgroundMusic(String name);
        void PauseBackgroundMusic();
        void ResumeBackgroundMusic();
        void PlaySound(String name);
        void SaveSettings(String key, String value);
        bool LoadSettings(String key, ref String value);
        void SendEmail(String addr, String subj, String body);
        void Exit();

        InputState GetInputState();

    }
}

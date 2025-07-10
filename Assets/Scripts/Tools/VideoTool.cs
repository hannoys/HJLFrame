using HJLFrame;
using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VideoTool : UnitySingleton<VideoTool>
{
    public MediaPlayer curPlayer;
    bool isReady = false;
    public delegate void VideoReadyEventHandler();
    public event VideoReadyEventHandler videoReadyEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public bool IsPlayerReady()
    {
        return isReady;
    }
    /// <summary>
    /// 初始化mediaPlayer
    /// </summary>
    /// <param name="player"></param>
    public void InitPlayer(MediaPlayer player)
    {
        if (curPlayer != null)
        {
            curPlayer.Control.Stop();
            curPlayer.Events.RemoveListener(OnVideoEvent);
        }
        curPlayer = player;
       
    }
    /// <summary>
    /// 播放之前先InitPlayer 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="player"></param>
    public void PlayVideoByABSURL(string path,MediaPlayer player)
    {
        //MediaPlayer.FileLocation _location = MediaPlayer.FileLocation.AbsolutePathOrURL;
        //if (curPlayer != null)
        //{
        //    curPlayer.Control.Stop();
        //    curPlayer.Events.RemoveListener(OnVideoEvent);
        //}
        curPlayer = player;
        curPlayer.Events.AddListener(OnVideoEvent);
        curPlayer.OpenMedia(MediaPathType.AbsolutePathOrURL, path);
    }
    public void PauseVideo()
    {
        if (curPlayer)
        {
            curPlayer.Control.Pause();
        }
        
    }
    public void PlayVideo()
    {
        if (curPlayer)
        {
            curPlayer.Control.Play();
        }
    }
    public void StopVideo()
    {
        if (curPlayer)
        {
            curPlayer.Control.Stop();
        }
    }
    private bool _wasPlayingOnScrub;
    public Slider curVideoSlider;
    private float _setVideoSeekSliderValue;
    public void SetVideoSlider(Slider slider)
    {
        
        //if (curVideoSlider != null)
        //{
        //    eventTrigger = curVideoSlider.transform.GetComponent<EventTrigger>();
        //    for (int i = eventTrigger.triggers.Count - 1; i >= 0; i--)
        //    {
        //        eventTrigger.triggers.RemoveAt(i);
        //    }
        //}
        curVideoSlider = slider;
        EventTrigger eventTrigger = curVideoSlider.transform.GetComponent<EventTrigger>();
        EventTrigger.Entry beginDragEntry = new EventTrigger.Entry();
        beginDragEntry.eventID = EventTriggerType.BeginDrag;
        beginDragEntry.callback.AddListener((data) => { OnVideoSliderDown((PointerEventData)data); });
        EventTrigger.Entry endDragEntry = new EventTrigger.Entry();
        endDragEntry.eventID = EventTriggerType.EndDrag;
        endDragEntry.callback.AddListener((data) => { OnVideoSliderUp((PointerEventData)data); });
        eventTrigger.triggers.Add(endDragEntry);
        curVideoSlider.onValueChanged.AddListener((value) => {
            OnVideoSeekSlider();
        });

       // Debug.LogError("SetVideoSlider");
    }
    public void OnVideoSeekSlider()
    {
        if (curPlayer && curVideoSlider && curVideoSlider.value != _setVideoSeekSliderValue)
        {

            double length = curPlayer.Info.GetDuration();
            curPlayer.Control.Seek(curVideoSlider.value * length);

           
            //double curTime = curPlayer.Control.GetCurrentTime();
            //float curSliderValue = (float)(curTime / length);
            //_setVideoSeekSliderValue = curSliderValue;
            //curVideoSlider.value = curSliderValue;
        }
    }

    public void OnVideoSliderDown(PointerEventData eventData)
    {
       
        if (curPlayer)
        {
            _wasPlayingOnScrub = curPlayer.Control.IsPlaying();
            if (_wasPlayingOnScrub)
            {
                curPlayer.Control.Pause();
                //					SetButtonEnabled( "PauseButton", false );
                //					SetButtonEnabled( "PlayButton", true );
            }
            OnVideoSeekSlider();
        }
    }
    public void OnVideoSliderUp(PointerEventData eventData)
    {
        if (curPlayer && _wasPlayingOnScrub)
        {
            curPlayer.Control.Play();
            _wasPlayingOnScrub = false;

            //				SetButtonEnabled( "PlayButton", false );
            //				SetButtonEnabled( "PauseButton", true );
        }
    }

    public void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
    {
        switch (et)
        {
            case MediaPlayerEvent.EventType.ReadyToPlay:
               
                break;
            case MediaPlayerEvent.EventType.Started:
                break;
            case MediaPlayerEvent.EventType.FirstFrameReady:
                //isReady = true;
                videoReadyEvent?.Invoke();
                break;
            case MediaPlayerEvent.EventType.FinishedPlaying:
                //isReady = false;
                break;
        }
      
        //Debug.Log("Event: " + et.ToString());
    }
    private TimeRange GetTimelineRange()
    {
        if (curPlayer.Info != null)
        {
            return Helper.GetTimelineRange(curPlayer.Info.GetDuration(), curPlayer.Control.GetSeekableTimes());
        }
        return new TimeRange();
    }

    void Update()
    {
        if (curPlayer && curPlayer.Info != null && curPlayer.Info.GetDuration() > 0f)
        {
            //TimeRange timelineRange = GetTimelineRange();
            //Debug.Log($"总长度:{curPlayer.Info.GetDuration()}  当前进度:{curPlayer.Control.GetCurrentTime()}");
            double length = curPlayer.Info.GetDuration();
            double curTime = curPlayer.Control.GetCurrentTime();
            float curSliderValue = (float)(curTime / length);
            _setVideoSeekSliderValue = curSliderValue;
            curVideoSlider.value = curSliderValue;
            //Debug.Log("length " + length);
            //Debug.Log("curTime " + curTime);
            //Debug.Log("curSliderValue " + curSliderValue);
            //double curSliderValue = curPlayer.Info.GetDuration()/ curPlayer.Control.GetCurrentTime();
            //float d = Mathf.Clamp((float)curSliderValue, 0.0f, 1.0f);
            //Debug.Log(curSliderValue);
            //curVideoSlider.value = (float)curSliderValue;
            //double time = timelineRange.startTime + (_sliderTime.value * timelineRange.duration);
            //curPlayer.Control.Seek(time);

            // double durationInSeconds = curPlayer.Info.GetDuration();
            // // 转换为毫秒
            // double durationInMilliseconds = durationInSeconds * 1000;
            // float time = curPlayer.Info.GetDuration();
            // float duration = curPlayer.Info.GetDurationMs();
            // float d = Mathf.Clamp(time / duration, 0.0f, 1.0f);
            //// Debug.LogError(string.Format("time: {0}, duration: {1}, d: {2}", time, duration, d));

            // _setVideoSeekSliderValue = d;
            // curVideoSlider.value = d;

            //if (_bufferedSliderRect != null)
            //{
            //    if (mediaPlayer.Control.IsBuffering())
            //    {
            //        float t1 = 0f;
            //        float t2 = mediaPlayer.Control.GetBufferingProgress();
            //        if (t2 <= 0f)
            //        {
            //            if (mediaPlayer.Control.GetBufferedTimeRangeCount() > 0)
            //            {
            //                mediaPlayer.Control.GetBufferedTimeRange(0, ref t1, ref t2);
            //                t1 /= mediaPlayer.Info.GetDurationMs();
            //                t2 /= mediaPlayer.Info.GetDurationMs();
            //            }
            //        }

            //        Vector2 anchorMin = Vector2.zero;
            //        Vector2 anchorMax = Vector2.one;

            //        if (_bufferedSliderImage != null &&
            //            _bufferedSliderImage.type == Image.Type.Filled)
            //        {
            //            _bufferedSliderImage.fillAmount = d;
            //        }
            //        else
            //        {
            //            anchorMin[0] = t1;
            //            anchorMax[0] = t2;
            //        }

            //        _bufferedSliderRect.anchorMin = anchorMin;
            //        _bufferedSliderRect.anchorMax = anchorMax;
            //    }
            //}
        }
    }

}

# PowerBeatsMusicVideoPlayer
Music Video Player for PowerBeats VR

This mod is still in development and has not implemented all the required functionality such as:
• Ability to download videos.
• Ability to generate json files from in-game.
• Ability to swap between more placement settings in-game.
• Adjusting by different environments (Medieval, Desert, Space, etc) 

In the current state, loading the mod will generate the config files, custom songs folder, and allows you to play videos. A custom
json file "video.json" is placed in the song directory and used to indicate the video's detail and attributeds. An example of the json file is posted below. 

**Sample video.json**
```json
{
	"Title":"$100 Bills BEAT SABER Playthrough!",
	"Author":"Ruirize",
	"Description":"Come join me on https://twitch.tv/ruirize ! Mixed Reality filmed using LIV: https://liv.tv Played on the HTC Vive in Beat Saber. Twitch: ...",
	"Duration":"2:36",
	"URL":"/watch?v=NHD1utOvak8",
	"ThumbnailURL":"https://i.ytimg.com/vi/NHD1utOvak8/hqdefault.jpg?sqp=-oaymwEjCPYBEIoBSFryq4qpAxUIARUAAAAAGAElAADIQj0AgKJDeAE=&amp;rs=AOn4CLD48jnea5icsiDQiE0QL4tF8j2t7w",
	"Loop":false,
	"Offset":0,
	"VideoPath":"$100 Bills BEAT SABER Playthrough!.mp4"
}
```

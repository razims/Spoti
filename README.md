# Spoti - Libspotify SDK Wrapper for .Net
Spoti is a small and flexible wrapper around libspotify SDK. It allows to build own Spotify music streaming application.

You should start by inheriting the SessionBase class adding your own functionaly. DownloadSession class has been added as an example and reference. 

Here is a very simple example of using the DownloadSession class:

```C#
// you spotify app key. You should get yours from https://devaccount.spotify.com/my-account/keys/
var key = new byte[]{} 
using (_downloadSession = new DownloadSession(key, "<app_name>")) {
  if (_downloadSession.LogIn("<spotify_username>", "<spotify_password>"))
  {
    var link = new Link(turl, _downloadSession);
    var cts = new CancellationTokenSource();
    using (var track = new Track(link, _downloadSession))
    {
      link.Dispose();    
      var task = _downloadSession.TrackToFileAsync(track, "test.wave", cts);
    }
  }
}
```

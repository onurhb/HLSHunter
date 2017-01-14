# StreamHunter
A small program that uses selenium and fiddlecore to find m3u8 stream on a website by simulating a real browser.

# How to
You can either schedule a job that will run every given hour for that website or use FindStreamSource(string StreamWebsite, string StreamKeyword) on a single website <br />
StreamWebsite is the website which we want to analyze, StreamKeyword is a keyword identifying the m3u8 link. <br />
StreamKeyword is used to find the valid m3u8 link, normally just ".m3u8".
<br />

If you want to run a schedule job on multiple websites, use the Schedule(). <br />
First create a file in this format (json): <br />

```json
[
    {
        "StreamName": "Website1",
        "StreamWebsite": "http:/www.website1.com/livstream",
        "StreamSource": "",
        "StreamUniqueKeyword": ".m3u8",
        "StreamRefreshInterval": 24,
        "StreamLastRefresh": ""
    },
    {
        "StreamName": "Website2",
        "StreamWebsite": "http:/www.website2.com/livstream",
        "StreamSource": "",
        "StreamUniqueKeyword": "index_1500_av-b.m3u8",
        "StreamRefreshInterval": -1,
        "StreamLastRefresh": ""
    },
    {
        "StreamName": "Website3",
        "StreamWebsite": "http:/www.website3.com/livstream",
        "StreamSource": "",
        "StreamUniqueKeyword": ".m3u8?token",
        "StreamRefreshInterval": 1,
        "StreamLastRefresh": ""
    }
    ...
]
```
Each json object represents a website. Fill StreamName with a desered name (optional),
StreamWebsite with the URL to the webpage with the stream (needed),
the StreamUniqueKeyword (you can use chrome networking tab) (needed)
and StreamRefreshInterval for how fast it should renew the stream source (-1 if not needed). <br />

When the program is executed, it will open this file, parse it and finally start to find all m3u8 stream links on each website.
The file will then be updated including StreamSource. You can use this URL to stream from VLC or any media player supporting m3u8.

# HLSHunter
A small program written in C# that uses selenium and fiddlecore to find m3u8 stream on a website by simulating a real chrome browser.

# Background
If you're familir with HLS stream then you know that all HLS streams uses a standard common file called ".m3u8".  
This file includes small video segments which is put together consecutively as you're streaming.  
This file is not bound to a particular websites which means that any media player supporting HLS protocol can stream this file.  

However, many websites nowadays protects this file by adding a token to it. This token is renewed every given time.  
The only way to get this token is by running the javascript on the websites which then requests this ".m3u8" file.  
I wanted to solve this problem by creating a small program which can spitt out the stream files on any website. 

# Using
HLSHunter needs a json document formatted as following:  

```

[
    {
        "StreamName": "<something required>",
        "StreamWebsite": "http:/<website required>",
        "StreamSource": "<leave blank not required>",
        "StreamUniqueKeyword": "<a identifier for the m3u8 file, normally just .m3u8>",
        "StreamRefreshInterval": <refresh interval in hours (-1 for never)>,
        "StreamLastRefresh": "<leave blank not required>"
    },
    etc

```

> StreamName : Give this stream a name  
> StreamWebsite : The website to navigate to 
> StreamSource : When the program is done, this field will include the m3u8 link
> StreamUniqueKeyword : You need to find this manually (use chrome dev tools - network tab ). This is used to find the valid m3u8 file by checking if this keyword is index of the m3u8 link  
> StreamRefreshInterval : Set refresh interval. It will automatically renew the m3u8 link 
> StreamLastRefresh : A timestamp for when the stream link was added 

Use then Hunter class to start the job `Hunter Hunter = new Hunter("C:/Users/Onur/Google Drive/workspace/C++/TTV/Resources/playlist.tivi",
                "C:/Users/Onur/Google Drive/workspace/NET/TiviBackend/ext.crx");
            Hunter.Schedule();`  
            
> NB! if you want to block ads, simply add ext.crx file for the extension as second parameter. This can be downloaded from the internet.

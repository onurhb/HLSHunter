# HLSHunter
A small program written in C# that uses selenium and fiddlecore to find m3u8 stream on a website by simulating a real chrome browser.

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
    ...
]

```

> StreamName : Give this stream a name  
> StreamWebsite : The website to navigate to  
> StreamSource : When the program is done, this field will include the m3u8 link  
> StreamUniqueKeyword : You need to find this manually (use chrome dev tools - network tab ). This is used to find the valid m3u8 file by checking if this keyword is index of the m3u8 link  
> StreamRefreshInterval : Set refresh interval. It will automatically renew the m3u8 link 
> StreamLastRefresh : A timestamp for when the stream link was added 

Use then Hunter class to start the job: 

```
Hunter Hunter = new Hunter("C:/<path to the json document as shown above>", "<adblock_ext.crx>");
Hunter.Schedule();
```  
            
> NB! if you want to block ads, simply add ext.crx file for the extension as second parameter. This can be downloaded from the internet.

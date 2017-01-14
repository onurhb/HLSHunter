using System;
using System.Collections.Generic;
using Fiddler;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Proxy = OpenQA.Selenium.Proxy;
using System.Linq;
using System.Threading;

namespace StreamHunter
{
    public class Stream
    {
        public string StreamName { get; set; }
        public string StreamWebsite { get; set; }
        public string StreamSource { get; set; }
        public string StreamUniqueKeyword { get; set; }
        public int StreamRefreshInterval { get; set; }
        public DateTime StreamLastRefresh { get; set; }
    }

    public class Hunter
    {
        // - Member variables
        private Proxy Proxy;
        private IWebDriver Driver;

        // - Path to stream file
        private readonly string StreamFile;

        // - List of all Streams
        private List<Stream> Streams = new List<Stream>();

        // - Used to determine if Proxy found stream
        private string LastFoundStream;

        // - Used to determine if stream link is valid
        private string CurrentStreamKeyword;

        public Hunter(string StreamFile)
        {
            this.StreamFile = StreamFile;

            InitializeStreamsFromFile();
            InitializeProxy();
            InitializeBrowser();
        }

        ~Hunter()
        {
            Console.WriteLine("Application exits.");

            Driver.Quit();
            FiddlerApplication.Shutdown();
        }

        private void InitializeProxy()
        {
            Console.WriteLine("Initializing Proxy");

            FiddlerApplication.Startup(5000, true, false);
            FiddlerApplication.BeforeResponse += RequestCallback;

            // - Selenium Proxy
            Proxy = new Proxy
            {
                Kind = ProxyKind.Manual,
                IsAutoDetect = false,
                HttpProxy = "localhost:5000"
            };
        }

        private void InitializeBrowser()
        {
            Console.WriteLine("Initializing Browser");

            // - Create options for chrome
            var options = new ChromeOptions {Proxy = Proxy};
            options.AddArgument("ignore-certificate-errors");
            options.AddArgument("mute-audio");

            // - Initialize selenium for browser automation
            Driver = new ChromeDriver(options);
        }

        private void InitializeStreamsFromFile()
        {
            Console.WriteLine("Reading file: " + StreamFile);

            // - Read json file and parse Streams
            var json = System.IO.File.ReadAllText(StreamFile);
            Streams = JsonConvert.DeserializeObject<List<Stream>>(json);
        }

        private void UpdateStreamsFile()
        {
            Console.WriteLine("Writing to file: " + StreamFile);

            var json = JsonConvert.SerializeObject(Streams);
            System.IO.File.WriteAllText(StreamFile, json);
        }

        public void Schedule()
        {
            double timer = Streams.OrderByDescending(x => x.StreamRefreshInterval == -1)
                .ThenBy(x => x.StreamLastRefresh)
                .Last()
                .StreamRefreshInterval;

            // - Loop if only timer is > 0
            while (timer > 0)
            {
                foreach (Stream stream in Streams)
                {
                    // - Skip if already found and timer interval is null
                    if (stream.StreamRefreshInterval == -1 && !string.IsNullOrEmpty(stream.StreamSource)) continue;

                    var deltaTime = (DateTime.Now - stream.StreamLastRefresh).Hours;

                    if (deltaTime < stream.StreamRefreshInterval &&
                        !string.IsNullOrEmpty(stream.StreamSource)) continue;

                    stream.StreamSource = FindStreamSource(stream.StreamWebsite, stream.StreamUniqueKeyword);
                    stream.StreamLastRefresh = DateTime.Now;
                }

                UpdateStreamsFile();
                Driver.Navigate().GoToUrl("about:blank");
                Thread.Sleep((int) TimeSpan.FromHours(timer).TotalMilliseconds);
            }
        }

        public string FindStreamSource(string StreamWebsite, string StreamKeyword)
        {
            // - Reset LastFoundStream so we can use this to see if callback found a stream
            LastFoundStream = "";

            // - Used to find the stream
            CurrentStreamKeyword = StreamKeyword;

            // - Navigate
            Console.WriteLine("Navigating to: " + StreamWebsite);
            Driver.Navigate().GoToUrl(StreamWebsite);

            var time = DateTime.Now;

            // - Wait until callback has found the stream
            while (string.IsNullOrWhiteSpace(LastFoundStream)) if ((DateTime.Now - time).Minutes > 1) return "";

            Console.WriteLine("Found stream: " + LastFoundStream);
            return LastFoundStream;
        }

        private void RequestCallback(Session s)
        {
            // - Skip if request does not contain stream keyword
            if (!s.fullUrl.Contains(CurrentStreamKeyword)) return;
            LastFoundStream = s.fullUrl;
        }
    }
}
using Xunit;

namespace TwitterQueue.Queue.Test
{
    public class BrowserTest
    {
        [Fact]
        public void Open()
        {          
            Browser.Open("http://google.com");
        }
    }
}
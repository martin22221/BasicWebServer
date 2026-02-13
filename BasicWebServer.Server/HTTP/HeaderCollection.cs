using System.Collections.Generic;

namespace BasicWebServer.Server.HTTP
{
    public class HeaderCollection : IEnumerable<Header>
    {
        private readonly Dictionary<string, Header> headers;

        public HeaderCollection()
        {
            this.headers = new Dictionary<string, Header>();
        }

        public int Count => this.headers.Count;

        public void Add(string location, Header header)
        {
            this.headers[header.Name] = header;
        }

        public IEnumerator<Header> GetEnumerator()
        {
           return headers.Values.GetEnumerator();
        }

        internal void Add(string location1, string location2)
        {
            throw new NotImplementedException();
        }

        internal void Add(Header header)
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
